#nullable enable

using System;
using Zenject;
using FCPercentage.FCPCore.Configuration;
using System.Collections.Generic;

namespace FCPercentage.FCPCore
{
	public class ScoreTracker : IInitializable, IDisposable
	{
		[InjectOptional] private GameplayCoreSceneSetupData sceneSetupData = null!;
		[Inject] private PlayerDataModel playerDataModel = null!;
		private readonly ScoreController scoreController;
		private readonly BeatmapObjectManager beatmapObjectManager;
		private readonly ScoreManager scoreManager;

		private Dictionary<GoodCutScoringElement, CutData> GoodCutCutData;

		private static readonly Func<int, int> MultiplierAtNoteCount = noteCount => noteCount > 13 ? OptimiseGetMultiplier() : (noteCount > 5 ? 4 : (noteCount > 1 ? 2 : 1));
		private static readonly Func<int, int> MultiplierAtMax = noteCount => 8;
		private static Func<int, int>? GetMultiplier;
		private static int OptimiseGetMultiplier() { GetMultiplier = MultiplierAtMax; return 8; }

		private int noteCount;

		public ScoreTracker([InjectOptional] ScoreController scoreController, BeatmapObjectManager beatmapObjectManager, ScoreManager scoreManager)
		{
			this.scoreManager = scoreManager;
			this.scoreController = scoreController;
			this.beatmapObjectManager = beatmapObjectManager;

			GoodCutCutData = new Dictionary<GoodCutScoringElement, CutData>();
			noteCount = 0;

			GetMultiplier = x => 1;
		}

		public void Initialize()
		{
			// Do not initialize if either of these are null
			if (playerDataModel == null || sceneSetupData == null)
				return;

			// Reset ScoreManager at level start
			scoreManager.ResetScoreManager(sceneSetupData.difficultyBeatmap, playerDataModel, sceneSetupData.colorScheme);

			// Set function for multiplier according to setting
			GetMultiplier = PluginConfig.Instance.IgnoreMultiplier ? MultiplierAtMax : MultiplierAtNoteCount;

			// Assign events
			if (scoreController != null)
			{
				scoreController.scoringForNoteStartedEvent += ScoreController_scoringForNoteStartedEvent;
				scoreController.scoringForNoteFinishedEvent += ScoreController_scoringForNoteFinishedEvent;
			}
			if (beatmapObjectManager != null)
			{
				beatmapObjectManager.noteWasMissedEvent += BeatmapObjectManager_noteWasMissedEvent;
			}
		}

		public void Dispose()
		{
			// Unassign events
			if (scoreController != null)
			{
				scoreController.scoringForNoteStartedEvent -= ScoreController_scoringForNoteStartedEvent;
				scoreController.scoringForNoteFinishedEvent -= ScoreController_scoringForNoteFinishedEvent;
			}
			if (beatmapObjectManager != null)
			{
				beatmapObjectManager.noteWasMissedEvent -= BeatmapObjectManager_noteWasMissedEvent;
			}
		}

		private void BeatmapObjectManager_noteWasMissedEvent(NoteController noteController)
		{
			// Ignore bombs
			if (noteController.noteData.colorType == ColorType.None)
				return;

			// But do count the missed blocks for proper application of the multiplier
			noteCount++;
		}

		private void ScoreController_scoringForNoteStartedEvent(ScoringElement scoringElement)
		{
			// Ignore bombs
			if (scoringElement.noteData.colorType == ColorType.None)
				return;

			// And ignore bad cuts. But do count them for proper application of the multiplier
			noteCount++;
			if (scoringElement is GoodCutScoringElement goodCutScoringElement)
			{
				int acc = goodCutScoringElement.cutScoreBuffer.centerDistanceCutScore;

				// Track cut data
				GoodCutCutData.Add(goodCutScoringElement, new CutData(goodCutScoringElement.noteData.colorType, acc, noteCount));

				// Add provisional score assuming it'll be a full swing to make it feel more responsive even though it may be temporarily incorrect
				//ScoreModel.RawScoreWithoutMultiplier(noteCutInfo.swingRatingCounter, noteCutInfo.centerDistanceCutScore, out _, out _, out int acc);
				scoreManager.AddScore(goodCutScoringElement.noteData.colorType, MaxPotentialScore(goodCutScoringElement), goodCutScoringElement.maxPossibleCutScore, GetMultiplier(noteCount));
			}
		}

		private void ScoreController_scoringForNoteFinishedEvent(ScoringElement scoringElement)
		{
			if (scoringElement is GoodCutScoringElement goodCutScoringElement)
			if (GoodCutCutData.TryGetValue(goodCutScoringElement, out CutData cutData))
			{
				// Calculate difference between previously applied score and actual score
				int diffAngleCutScore = DifferenceFromProvisionalScore(goodCutScoringElement);

				// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
				if (diffAngleCutScore > 0)
					scoreManager.SubtractScore(cutData.colorType, diffAngleCutScore, GetMultiplier(cutData.noteCount));

				// Remove cut data since it won't be needed again.
				GoodCutCutData.Remove(goodCutScoringElement);
			}
			else
				Plugin.Log.Error("ScoreTracker, ScoreController_scoringForNoteFinishedEvent : Failed to get cutData from GoodCutCutData!");
		}

		private int DifferenceFromProvisionalScore(GoodCutScoringElement goodCutScoringElement)
		{
			int maxAngleCutScore = goodCutScoringElement.cutScoreBuffer.noteScoreDefinition.maxBeforeCutScore + goodCutScoringElement.cutScoreBuffer.noteScoreDefinition.maxAfterCutScore;
			int ratingAngleCutScore = goodCutScoringElement.cutScoreBuffer.beforeCutScore + goodCutScoringElement.cutScoreBuffer.afterCutScore;

			return maxAngleCutScore - ratingAngleCutScore;
		}


		private struct CutData
		{
			public ColorType colorType { get; private set; }
			public float centerDistanceCutScore { get; private set; }
			public int noteCount { get; private set; }

			public CutData(ColorType colorType, float centerDistanceCutScore, int noteCount)
			{
				this.colorType = colorType;
				this.centerDistanceCutScore = centerDistanceCutScore;
				this.noteCount = noteCount;
			}
		}



		// This method assumes that the ScoringType has been checked so it only checks for cuts that have a variable centerDistanceCutScore.
		private int MaxPotentialScore(GoodCutScoringElement scoringElement) => scoringElement.maxPossibleCutScore - MissedCenterDistanceCutScore(scoringElement);
		private int MissedCenterDistanceCutScore(GoodCutScoringElement scoringElement) => scoringElement.cutScoreBuffer.noteScoreDefinition.maxCenterDistanceCutScore - scoringElement.cutScoreBuffer.centerDistanceCutScore;
	}
}

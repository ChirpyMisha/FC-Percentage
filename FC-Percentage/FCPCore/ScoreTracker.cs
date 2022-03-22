using System;
using Zenject;
using FCPercentage.FCPCore.Configuration;
using System.Collections.Generic;

namespace FCPercentage.FCPCore
{
	public class ScoreTracker : IInitializable, IDisposable, ISaberSwingRatingCounterDidFinishReceiver
	{
		private int MaxScoreIfFinished(int acc) => acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;

		[InjectOptional] private GameplayCoreSceneSetupData sceneSetupData = null!;
		[Inject] private PlayerDataModel playerDataModel = null!;
		private readonly ScoreController scoreController;
		private readonly ScoreManager scoreManager;

		private Dictionary<ISaberSwingRatingCounter, CutData> swingCounterCutData;

		private static readonly Func<int, int> MultiplierAtNoteCount = noteCount => noteCount > 13 ? OptimiseGetMultiplier() : (noteCount > 5 ? 4 : (noteCount > 1 ? 2 : 1));
		private static readonly Func<int, int> MultiplierAtMax = noteCount => 8;
		private static Func<int, int>? GetMultiplier;
		private static int OptimiseGetMultiplier() { GetMultiplier = MultiplierAtMax; return 8; }

		//private readonly int badCutThreshold;
		private int noteCount;

		public ScoreTracker([InjectOptional] ScoreController scoreController, ScoreManager scoreManager)
		{
			this.scoreManager = scoreManager;
			this.scoreController = scoreController;

			swingCounterCutData = new Dictionary<ISaberSwingRatingCounter, CutData>();
			noteCount = 0;

			GetMultiplier = x => 1;

			//badCutThreshold = PluginConfig.Instance.BadCutThreshold;
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
				scoreController.noteWasMissedEvent += ScoreController_noteWasMissedEvent;
				scoreController.noteWasCutEvent += ScoreController_noteWasCutEvent;
			}
		}

		public void Dispose()
		{
			// Unassign events
			if (scoreController != null)
			{
				scoreController.noteWasMissedEvent -= ScoreController_noteWasMissedEvent;
				scoreController.noteWasCutEvent -= ScoreController_noteWasCutEvent;
			}
		}

		private void ScoreController_noteWasMissedEvent(NoteData noteData, int _)
		{
			// Ignore bombs
			if (noteData.colorType == ColorType.None)
				return;

			// But do count the missed blocks for proper application of the multiplier
			noteCount++;
		}

		private void ScoreController_noteWasCutEvent(NoteData noteData, in NoteCutInfo noteCutInfo, int _)
		{
			// Ignore bombs
			if (noteData.colorType == ColorType.None)
				return;

			// And ignore bad cuts. But do count them for proper application of the multiplier
			noteCount++;
			if (noteCutInfo.allIsOK)
			{
				// Track cut data
				swingCounterCutData.Add(noteCutInfo.swingRatingCounter, new CutData(noteData.colorType, noteCutInfo.cutDistanceToCenter, noteCount));
				noteCutInfo.swingRatingCounter.RegisterDidFinishReceiver(this);

				// Add provisional score assuming it'll be a full swing to make it feel more responsive even though it may be temporarily incorrect
				ScoreModel.RawScoreWithoutMultiplier(noteCutInfo.swingRatingCounter, noteCutInfo.cutDistanceToCenter, out _, out _, out int acc);
				scoreManager.AddScore(noteData.colorType, MaxScoreIfFinished(acc), GetMultiplier(noteCount));
			}
		}

		public void HandleSaberSwingRatingCounterDidFinish(ISaberSwingRatingCounter saberSwingRatingCounter)
		{
			if (swingCounterCutData.TryGetValue(saberSwingRatingCounter, out CutData cutData))
			{
				// Calculate difference between previously applied score and actual score
				int diffAngleCutScore = DifferenceFromProvisionalScore(saberSwingRatingCounter, cutData.cutDistanceToCenter);

				// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
				if (diffAngleCutScore > 0)
					scoreManager.SubtractScore(cutData.colorType, diffAngleCutScore, GetMultiplier(cutData.noteCount));

				// Remove cut data since it won't be needed again.
				swingCounterCutData.Remove(saberSwingRatingCounter);
			}
			else
				Plugin.Log.Error("ScoreTracker, HandleSaberSwingRatingCounterDidFinish : Failed to get cutData from swingCounterCutData!");

			// Unregister saber swing rating counter.
			saberSwingRatingCounter.UnregisterDidFinishReceiver(this);
		}

		private int DifferenceFromProvisionalScore(ISaberSwingRatingCounter saberSwingRatingCounter, float cutDistanceToCenter)
		{
			// note: Accuracy won't change over time, therefore it can be ignored in the calculation since it'll just cancel out.
			ScoreModel.RawScoreWithoutMultiplier(saberSwingRatingCounter, cutDistanceToCenter, out int preCut, out int postCut, out _);

			int maxAngleCutScore = ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
			int ratingAngleCutScore = preCut + postCut;

			return maxAngleCutScore - ratingAngleCutScore;
		}


		private struct CutData
		{
			public ColorType colorType { get; private set; }
			public float cutDistanceToCenter { get; private set; }
			public int noteCount { get; private set; }

			public CutData(ColorType colorType, float cutDistanceToCenter, int noteCount)
			{
				this.colorType = colorType;
				this.cutDistanceToCenter = cutDistanceToCenter;
				this.noteCount = noteCount;
			}
		}

		// Disabled implementation of the badCutThreshold setting.
		/*
		private void NoteTracker_OnNoteRatingFinished(object s, NoteRatingUpdateEventArgs e)
		{
			NoteRating rating = e.NoteRating;

			// Check if score is below the bad cut threshold
			int ratingAngleCutScore = rating.beforeCut + rating.afterCut;
			int cutScore = ratingAngleCutScore + rating.acc;
			if (cutScore <= badCutThreshold)
			{
				scoreManager.SetBadCutThresholdBroken();

				int maxScoreIfFinished = e.NoteRating.acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
				scoreManager.SubtractScore(e.NoteData.colorType, maxScoreIfFinished, GetMultiplier(e.NoteRating.noteCount), true);

				Plugin.Log.Notice($"Cut Threshold has been broken. Score = {cutScore}, BlockColor = {e.NoteData.colorType}, Multiplier = {GetMultiplier(e.NoteRating.noteCount)}, NoteCount = {e.NoteRating.noteCount}");
			}
			else
			{
				// Calculate difference between previously applied score and actual score
				int maxAngleCutScore = ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
				int diffAngleCutScore = maxAngleCutScore - ratingAngleCutScore;

				// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
				if (diffAngleCutScore > 0)
				{
					scoreManager.SubtractScore(e.NoteData.colorType, diffAngleCutScore, GetMultiplier(e.NoteRating.noteCount));
				}
			}
		}
		*/
	}
}

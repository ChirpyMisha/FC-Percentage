using System;
using Zenject;
using FCPercentage.FCPCore.Configuration;

namespace FCPercentage.FCPCore
{
	public class ScoreTracker : IInitializable, IDisposable
	{
		[InjectOptional] private GameplayCoreSceneSetupData sceneSetupData = null!;
		[Inject] private PlayerDataModel playerDataModel = null!;

		private readonly Func<int, int> MultiplierAtNoteCount = noteCount => noteCount > 13 ? 8 : (noteCount > 5 ? 4 : (noteCount > 1 ? 2 : 1));
		private readonly Func<int, int> MultiplierAtMax = noteCount => 8;
		private Func<int, int> GetMultiplier;

		//private readonly int badCutThreshold;

		private readonly NoteRatingTracker noteRatingTracker;
		private readonly ScoreManager scoreManager;

		public ScoreTracker(ScoreManager scoreManager, NoteRatingTracker noteRatingTracker)
		{
			this.scoreManager = scoreManager;
			this.noteRatingTracker = noteRatingTracker;

			// Set function for multiplier according to setting
			GetMultiplier = PluginConfig.Instance.IgnoreMultiplier ? MultiplierAtMax : MultiplierAtNoteCount;
			//badCutThreshold = PluginConfig.Instance.BadCutThreshold;
		}

		public void Initialize()
		{
			if (HasNullReferences())
				return;

			// Reset ScoreManager at level start
			scoreManager.ResetScoreManager(sceneSetupData.difficultyBeatmap, playerDataModel, sceneSetupData.colorScheme);

			// Assign events
			noteRatingTracker.OnRatingAdded += NoteRatingTracker_OnNoteRatingAdded;
			noteRatingTracker.OnRatingFinished += NoteTracker_OnNoteRatingFinished;
		}

		public void Dispose()
		{
			// Unassign events
			noteRatingTracker.OnRatingAdded -= NoteRatingTracker_OnNoteRatingAdded;
			noteRatingTracker.OnRatingFinished -= NoteTracker_OnNoteRatingFinished;
		}

		public bool HasNullReferences()
		{
			if (scoreManager == null || noteRatingTracker == null || sceneSetupData == null || playerDataModel == null)
			{
				Plugin.Log.Error("FCPercentage : ScoreTracker has a null reference and cannot initialize! Please notify ChirpyMisha about this bug.");
				Plugin.Log.Error("The following objects are null:");
				if (scoreManager == null)
					Plugin.Log.Error("- scoreManager");
				if (noteRatingTracker == null)
					Plugin.Log.Error("- noteRatingTracker");
				if (sceneSetupData == null)
					Plugin.Log.Error("- sceneSetupData");
				if (playerDataModel == null)
					Plugin.Log.Error("- playerDataModel");

				return true;
			}

			return false;
		}
		

		private void NoteRatingTracker_OnNoteRatingAdded(object s, NoteRatingUpdateEventArgs e)
		{
			int maxScoreIfFinished = e.NoteRating.acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
			scoreManager.AddScore(e.NoteData.colorType, maxScoreIfFinished, GetMultiplier(e.NoteRating.noteCount));
		}

		private void NoteTracker_OnNoteRatingFinished(object s, NoteRatingUpdateEventArgs e)
		{
			NoteRating rating = e.NoteRating;

			// Calculate difference between previously applied score and actual score
			int maxAngleCutScore = ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
			int ratingAngleCutScore = rating.beforeCut + rating.afterCut;
			int diffAngleCutScore = maxAngleCutScore - ratingAngleCutScore;

			// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
			if (diffAngleCutScore > 0)
				scoreManager.SubtractScore(e.NoteData.colorType, diffAngleCutScore, GetMultiplier(e.NoteRating.noteCount));
		}


		// Disabled implementation of the badCutThreshold setting.

		//private void NoteTracker_OnNoteRatingFinished(object s, NoteRatingUpdateEventArgs e)
		//{
		//	NoteRating rating = e.NoteRating;

		//	// Check if score is below the bad cut threshold
		//	int ratingAngleCutScore = rating.beforeCut + rating.afterCut;
		//	int cutScore = ratingAngleCutScore + rating.acc;
		//	if (cutScore <= badCutThreshold)
		//	{
		//		scoreManager.SetBadCutThresholdBroken();

		//		int maxScoreIfFinished = e.NoteRating.acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
		//		scoreManager.SubtractScore(e.NoteData.colorType, maxScoreIfFinished, GetMultiplier(e.NoteRating.noteCount), true);

		//		Plugin.Log.Notice($"Cut Threshold has been broken. Score = {cutScore}, BlockColor = {e.NoteData.colorType}, Multiplier = {GetMultiplier(e.NoteRating.noteCount)}, NoteCount = {e.NoteRating.noteCount}");
		//	}
		//	else
		//	{
		//		// Calculate difference between previously applied score and actual score
		//		int maxAngleCutScore = ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
		//		int diffAngleCutScore = maxAngleCutScore - ratingAngleCutScore;

		//		// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
		//		if (diffAngleCutScore > 0)
		//		{
		//			scoreManager.SubtractScore(e.NoteData.colorType, diffAngleCutScore, GetMultiplier(e.NoteRating.noteCount));
		//		}
		//	}
		//}
	}
}

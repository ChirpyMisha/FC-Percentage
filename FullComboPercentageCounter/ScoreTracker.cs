using FullComboPercentageCounter.Configuration;
using System;
using Zenject;

namespace FullComboPercentageCounter
{
	public class ScoreTracker : IInitializable, IDisposable
	{
		private readonly Func<int, int> MultiplierAtNoteCount = noteCount => (noteCount > 13 ? 8 : (noteCount > 5 ? 4 : (noteCount > 1 ? 2 : 1)));
		private readonly Func<int, int> MultiplierAtMax = noteCount => 8;
		private Func<int, int> GetMultiplier;

		private readonly NoteRatingTracker noteRatingTracker;
		private readonly ScoreManager scoreManager;

		public ScoreTracker(ScoreManager scoreManager, NoteRatingTracker noteRatingTracker)
		{
			this.scoreManager = scoreManager;
			this.noteRatingTracker = noteRatingTracker;
		}

		public void Initialize()
		{
			// Set function for multiplier according to setting
			GetMultiplier = PluginConfig.Instance.IgnoreMultiplier ? MultiplierAtMax : MultiplierAtNoteCount;

			// Reset the stored score back to 0
			scoreManager.ResetScore();

			// Assign events
			noteRatingTracker.OnRatingAdded += NoteRatingTracker_OnNoteRatingAdded;
			noteRatingTracker.OnRatingFinished += NoteTracker_OnNoteRatingFinished;
			noteRatingTracker.OnNoteMissed += NoteTracker_OnNoteMissed;
			noteRatingTracker.OnComboBreak += NoteTracker_OnComboBreak;
		}

		public void Dispose()
		{
			// Unassign events
			noteRatingTracker.OnRatingAdded -= NoteRatingTracker_OnNoteRatingAdded;
			noteRatingTracker.OnRatingFinished -= NoteTracker_OnNoteRatingFinished;
			noteRatingTracker.OnNoteMissed -= NoteTracker_OnNoteMissed;
			noteRatingTracker.OnComboBreak -= NoteTracker_OnComboBreak;
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
			{
				scoreManager.SubtractScore(e.NoteData.colorType, diffAngleCutScore, GetMultiplier(e.NoteRating.noteCount));
			}
		}

		private void NoteTracker_OnNoteMissed(object s, NoteMissedEventArgs e)
		{
			scoreManager.AddMissedScore(e.NoteData.colorType, ScoreModel.kMaxCutRawScore, GetMultiplier(e.NoteCount));
		}

		private void NoteTracker_OnComboBreak()
		{
			scoreManager.ComboBroke();
		}
	}
}

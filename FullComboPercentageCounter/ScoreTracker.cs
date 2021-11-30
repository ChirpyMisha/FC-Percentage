using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullComboPercentageCounter
{
	class ScoreTracker : IDisposable
	{
		public event EventHandler<ScoreUpdateEventArgs> OnScoreUpdate;

		private int currentScoreA, currentScoreB;
		private int currentMaxScoreA, currentMaxScoreB;

		private readonly Func<int, int> MultiplierAtNoteCount = noteCount => (noteCount > 13 ? 8 : (noteCount > 5 ? 4 : (noteCount > 1 ? 2 : 1)));

		private NoteRatingTracker noteRatingTracker;

		public ScoreTracker(NoteRatingTracker noteRatingTracker)
		{
			// Assign variables
			this.noteRatingTracker = noteRatingTracker;
			
			// init variables
			currentScoreA = currentScoreB = 0;
			currentMaxScoreA = currentMaxScoreB = 0;

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

		private void NoteRatingTracker_OnNoteRatingAdded(object s, NoteRatingUpdateEventArgs e)
		{
			NoteRating rating = e.NoteRating;
			int maxScoreIfFinishedMultiplied = (rating.acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore) * rating.multiplier;

			// Update score for left or right saber
			if (e.NoteData.colorType == ColorType.ColorA)
			{
				currentScoreA += maxScoreIfFinishedMultiplied;
				currentMaxScoreA += ScoreModel.kMaxCutRawScore * rating.multiplier;
			}
			else if (e.NoteData.colorType == ColorType.ColorB)
			{
				currentScoreB += maxScoreIfFinishedMultiplied;
				currentMaxScoreB += ScoreModel.kMaxCutRawScore * rating.multiplier;
			}

			InvokeScoreUpdate();
		}

		private void NoteTracker_OnNoteRatingFinished(object s, NoteRatingUpdateEventArgs e)
		{
			NoteRating rating = e.NoteRating;

			// Calculate difference between previously applied score and actual score
			int maxAngleCutScoreMultiplied = (ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore) * rating.multiplier;
			int ratingAngleCutScoreMultiplied = (rating.beforeCut + rating.afterCut) * rating.multiplier;
			int diffAngleCutScoreMultiplied = maxAngleCutScoreMultiplied - ratingAngleCutScoreMultiplied;

			// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
			if (diffAngleCutScoreMultiplied > 0)
			{
				if (e.NoteData.colorType == ColorType.ColorA)
					currentScoreA -= diffAngleCutScoreMultiplied;
				else if (e.NoteData.colorType == ColorType.ColorB)
					currentScoreB -= diffAngleCutScoreMultiplied;

				InvokeScoreUpdate();
			}
		}

		protected virtual void InvokeScoreUpdate()
		{
			// Create event handler
			EventHandler<ScoreUpdateEventArgs> handler = OnScoreUpdate;
			if (handler != null)
			{
				// Assign event args
				ScoreUpdateEventArgs scoreUpdateEventArgs = new ScoreUpdateEventArgs();
				scoreUpdateEventArgs.CurrentScoreA = currentScoreA;
				scoreUpdateEventArgs.CurrentMaxScoreA = currentMaxScoreA;
				scoreUpdateEventArgs.CurrentScoreB = currentScoreB;
				scoreUpdateEventArgs.CurrentMaxScoreB = currentMaxScoreB;

				// Invoke event
				handler(this, scoreUpdateEventArgs);
			}
		}

		
	}

	public class ScoreUpdateEventArgs : EventArgs
	{
		public int CurrentScoreA { get; set; }
		public int CurrentMaxScoreA { get; set; }
		public int CurrentScoreB { get; set; }
		public int CurrentMaxScoreB { get; set; }
		public int CurrentScoreTotal { get { return CurrentScoreA + CurrentScoreB; } }
		public int CurrentMaxScoreTotal { get { return CurrentMaxScoreA + CurrentMaxScoreB; } }
	}
}

//using FullComboPercentageCounter.Configuration;
using System;
using System.Collections.Generic;
using Zenject;

// I copied a part of PikminBloom's homework and changed a few things so it isn't obvious.

namespace FullComboPercentageCounter
{
	public class ScoreTracker : IInitializable, IDisposable, ISaberSwingRatingCounterDidChangeReceiver, ISaberSwingRatingCounterDidFinishReceiver
	{
		public event EventHandler<ScoreUpdateEventArgs> OnScoreUpdate;

		private readonly ScoreController scoreController;
		private Dictionary<NoteData, Rating> noteRatings;
		private Dictionary<ISaberSwingRatingCounter, NoteCutInfo> swingCounterCutInfo;
		private Dictionary<NoteCutInfo, NoteData> noteCutInfoData;

		private int noteCount;
		private int currentScore, currentMaxScore;

		private readonly Func<int, int> MultiplierAtNoteCount = noteCount => (noteCount > 13 ? 8 : noteCount > 5 ? 4 : noteCount > 1 ? 2 : 1);

		public ScoreTracker(ScoreController scoreController)
		{
			this.scoreController = scoreController;
		}

		public void Initialize()
		{
			Plugin.Log.Notice("Initializing ScoreTracker");

			scoreController.noteWasMissedEvent += ScoreController_noteWasMissedEvent;
			scoreController.noteWasCutEvent += ScoreController_noteWasCutEvent;

			noteRatings = new Dictionary<NoteData, Rating>();
			swingCounterCutInfo = new Dictionary<ISaberSwingRatingCounter, NoteCutInfo>();
			noteCutInfoData = new Dictionary<NoteCutInfo, NoteData>();

			noteCount = 0;
			currentScore = currentMaxScore = 0;
		}

		public void Dispose()
		{
			scoreController.noteWasMissedEvent -= ScoreController_noteWasMissedEvent;
			scoreController.noteWasCutEvent -= ScoreController_noteWasCutEvent;
		}

		private void ScoreController_noteWasMissedEvent(NoteData noteData, int _)
		{
			noteCount++;
		}

		private void ScoreController_noteWasCutEvent(NoteData noteData, in NoteCutInfo noteCutInfo, int multiplier)
		{
			noteCount++;
			if (noteData.colorType != ColorType.None && noteCutInfo.allIsOK)
			{
				swingCounterCutInfo.Add(noteCutInfo.swingRatingCounter, noteCutInfo);
				noteCutInfoData.Add(noteCutInfo, noteData);
				noteCutInfo.swingRatingCounter.RegisterDidChangeReceiver(this);
				noteCutInfo.swingRatingCounter.RegisterDidFinishReceiver(this);

				int beforeCutRawScore, afterCutRawScore, accRawScore;
				ScoreModel.RawScoreWithoutMultiplier(noteCutInfo.swingRatingCounter, noteCutInfo.cutDistanceToCenter, out beforeCutRawScore, out afterCutRawScore, out accRawScore);
				Rating rating = new Rating(noteData, beforeCutRawScore, afterCutRawScore, accRawScore, MultiplierAtNoteCount(noteCount));
				noteRatings.Add(noteData, rating);

				UpdateScoreUnfinished(rating);
			}
		}

		public void HandleSaberSwingRatingCounterDidChange(ISaberSwingRatingCounter saberSwingRatingCounter, float rating)
		{
			NoteCutInfo noteCutInfo;
			if (swingCounterCutInfo.TryGetValue(saberSwingRatingCounter, out noteCutInfo))
			{
				NoteData noteData;
				if (noteCutInfoData.TryGetValue(noteCutInfo, out noteData))
				{
					int beforeCutRawScore, afterCutRawScore, accRawScore;
					ScoreModel.RawScoreWithoutMultiplier(saberSwingRatingCounter, noteCutInfo.cutDistanceToCenter, out beforeCutRawScore, out afterCutRawScore, out accRawScore);

					Rating previousRating = noteRatings[noteData];
					Rating updatedRating = new Rating(noteData, beforeCutRawScore, afterCutRawScore, accRawScore, previousRating.multiplier);
					noteRatings[noteData] = updatedRating;
				}
				else
					Plugin.Log.Error("ScoreTracker, HandleSaberSwingRatingCounterDidChange : Failed to get NoteData from noteCutInfoData!");
			}
			else
				Plugin.Log.Error("ScoreTracker, HandleSaberSwingRatingCounterDidChange : Failed to get NoteCutInfo from swingCounterCutInfo!");
		}

		public void HandleSaberSwingRatingCounterDidFinish(ISaberSwingRatingCounter saberSwingRatingCounter)
		{
			NoteCutInfo noteCutInfo;
			if (swingCounterCutInfo.TryGetValue(saberSwingRatingCounter, out noteCutInfo))
			{
				NoteData noteData;
				if (noteCutInfoData.TryGetValue(noteCutInfo, out noteData))
				{
					UpdateScoreFinished(noteRatings[noteData]);
					noteRatings.Remove(noteData);
				}
				else
					Plugin.Log.Error("ScoreTracker, HandleSaberSwingRatingCounterDidFinish : Failed to get NoteData from noteCutInfoData!");

				swingCounterCutInfo.Remove(saberSwingRatingCounter);
			}
			else
				Plugin.Log.Error("ScoreTracker, HandleSaberSwingRatingCounterDidFinish : Failed to get NoteCutInfo from swingCounterCutInfo!");

			saberSwingRatingCounter.UnregisterDidChangeReceiver(this);
			saberSwingRatingCounter.UnregisterDidFinishReceiver(this);
		}

		private void UpdateScoreUnfinished(Rating rating)
		{
			int maxScoreIfFinishedMultiplied = (rating.acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore) * rating.multiplier;

			currentScore += maxScoreIfFinishedMultiplied;
			currentMaxScore += ScoreModel.kMaxCutRawScore * rating.multiplier;

			InvokeScoreUpdate();
		}

		private void UpdateScoreFinished(Rating rating)
		{
			int maxAngleCutScoreMultiplied = (ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore) * rating.multiplier;
			int ratingAngleCutScoreMultiplied = (rating.beforeCut + rating.afterCut) * rating.multiplier;
			int diffAngleCutScoreMultiplied = maxAngleCutScoreMultiplied - ratingAngleCutScoreMultiplied;

			if (diffAngleCutScoreMultiplied > 0)
			{
				currentScore -= diffAngleCutScoreMultiplied;

				InvokeScoreUpdate();
			}
		}

		protected virtual void InvokeScoreUpdate()
		{
			EventHandler<ScoreUpdateEventArgs> handler = OnScoreUpdate;
			if (handler != null)
			{
				ScoreUpdateEventArgs scoreUpdateEventArgs = new ScoreUpdateEventArgs();
				scoreUpdateEventArgs.CurrentScore = currentScore;
				scoreUpdateEventArgs.CurrentMaxScore = currentMaxScore;

				handler(this, scoreUpdateEventArgs);
			}
		}


		public struct Rating
		{
			public NoteData noteData;
			public int multiplier, beforeCut, afterCut, acc;

			public Rating(NoteData noteData, int beforeCutRaw, int afterCutRaw, int accRaw, int multiplier)
			{
				this.noteData = noteData;
				this.multiplier = multiplier;
				this.beforeCut = beforeCutRaw;
				this.afterCut = afterCutRaw;
				this.acc = accRaw;
			}
		}
	}

	public class ScoreUpdateEventArgs : EventArgs
	{
		public int CurrentScore { get; set; }
		public int CurrentMaxScore { get; set; }
	}
}


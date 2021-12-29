#nullable enable
using System;
using System.Collections.Generic;
using Zenject;

// I copied a part of PikminBloom's homework and changed a few things so it isn't obvious.
namespace FCPercentage
{
	public class NoteRatingTracker : IInitializable, IDisposable, ISaberSwingRatingCounterDidChangeReceiver, ISaberSwingRatingCounterDidFinishReceiver
	{
		public event EventHandler<NoteRatingUpdateEventArgs>? OnRatingAdded;
		public event EventHandler<NoteRatingUpdateEventArgs>? OnRatingFinished;

		private readonly ScoreController scoreController;

		private Dictionary<NoteData, NoteRating> noteRatings;
		private Dictionary<ISaberSwingRatingCounter, NoteCutInfo> swingCounterCutInfo;
		private Dictionary<NoteCutInfo, NoteData> noteCutInfoData;

		private int noteCount;

		public NoteRatingTracker([InjectOptional]ScoreController scoreController)
		{
			this.scoreController = scoreController;

			noteRatings = new Dictionary<NoteData, NoteRating>();
			swingCounterCutInfo = new Dictionary<ISaberSwingRatingCounter, NoteCutInfo>();
			noteCutInfoData = new Dictionary<NoteCutInfo, NoteData>();

			noteCount = 0;
		}

		public void Initialize()
		{
			if (scoreController != null)
			{
				scoreController.noteWasMissedEvent += ScoreController_noteWasMissedEvent;
				scoreController.noteWasCutEvent += ScoreController_noteWasCutEvent;
			}
		}

		public void Dispose()
		{
			if (scoreController != null)
			{
				scoreController.noteWasMissedEvent -= ScoreController_noteWasMissedEvent;
				scoreController.noteWasCutEvent -= ScoreController_noteWasCutEvent;
			}
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
				NoteRating noteRating = new NoteRating(beforeCutRawScore, afterCutRawScore, accRawScore, multiplier, noteCount);
				noteRatings.Add(noteData, noteRating);

				InvokeRatingAdded(noteData, noteRating);
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

					noteRatings[noteData].UpdateRating(beforeCutRawScore, afterCutRawScore, accRawScore);
				}
				else
					Plugin.Log.Error("noteRatingTracker, HandleSaberSwingRatingCounterDidChange : Failed to get NoteData from noteCutInfoData!");
			}
			else
				Plugin.Log.Error("noteRatingTracker, HandleSaberSwingRatingCounterDidChange : Failed to get NoteCutInfo from swingCounterCutInfo!");
		}

		public void HandleSaberSwingRatingCounterDidFinish(ISaberSwingRatingCounter saberSwingRatingCounter)
		{
			NoteCutInfo noteCutInfo;
			if (swingCounterCutInfo.TryGetValue(saberSwingRatingCounter, out noteCutInfo))
			{
				NoteData noteData;
				if (noteCutInfoData.TryGetValue(noteCutInfo, out noteData))
				{
					NoteRating noteRating = noteRatings[noteData];
					InvokeRatingFinished(noteData, noteRating);
					noteRatings.Remove(noteData);
				}
				else
					Plugin.Log.Error("noteRatingTracker, HandleSaberSwingRatingCounterDidFinish : Failed to get NoteData from noteCutInfoData!");

				swingCounterCutInfo.Remove(saberSwingRatingCounter);
			}
			else
				Plugin.Log.Error("noteRatingTracker, HandleSaberSwingRatingCounterDidFinish : Failed to get NoteCutInfo from swingCounterCutInfo!");

			saberSwingRatingCounter.UnregisterDidChangeReceiver(this);
			saberSwingRatingCounter.UnregisterDidFinishReceiver(this);
		}

		protected virtual void InvokeRatingAdded(NoteData noteData, NoteRating noteRating)
		{
			EventHandler<NoteRatingUpdateEventArgs>? handler = OnRatingAdded;
			if (handler != null)
			{
				NoteRatingUpdateEventArgs eventArgs = new NoteRatingUpdateEventArgs(noteData, noteRating);

				handler(this, eventArgs);
			}
		}
		protected virtual void InvokeRatingFinished(NoteData noteData, NoteRating noteRating)
		{
			EventHandler<NoteRatingUpdateEventArgs>? handler = OnRatingFinished;
			if (handler != null)
			{
				NoteRatingUpdateEventArgs eventArgs = new NoteRatingUpdateEventArgs(noteData, noteRating);

				handler(this, eventArgs);
			}
		}
	}

	public class NoteRatingUpdateEventArgs : EventArgs
	{
		public NoteData NoteData { get; set; }
		public NoteRating NoteRating { get; set; }
		public NoteRatingUpdateEventArgs(NoteData noteData, NoteRating noteRating)
		{
			NoteData = noteData;
			NoteRating = noteRating;
		}
	}

	public class NoteRating
	{
		public int beforeCut, afterCut, acc, multiplier, noteCount;

		public NoteRating(int beforeCutRaw, int afterCutRaw, int accRaw, int multiplier, int noteCount)
		{
			this.beforeCut = beforeCutRaw;
			this.afterCut = afterCutRaw;
			this.acc = accRaw;
			this.multiplier = multiplier;
			this.noteCount = noteCount;
		}

		public void UpdateRating(int beforeCutRaw, int afterCutRaw, int accRaw)
		{
			beforeCut = beforeCutRaw;
			afterCut = afterCutRaw;
			acc = accRaw;
		}
	}
}


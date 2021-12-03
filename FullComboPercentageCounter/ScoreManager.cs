using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace FullComboPercentageCounter
{
	public class ScoreManager : IInitializable, IDisposable
	{
		public event EventHandler OnScoreUpdate;

		public int ScoreTotal { get { return ScoreA + ScoreB; } }
		public int ScoreA { get; private set; }
		public int ScoreB { get; private set; }
		public int MaxScoreTotal { get { return MaxScoreA + MaxScoreB; } }
		public int MaxScoreA { get; private set; }
		public int MaxScoreB { get; private set; }

		public void Initialize()
		{
			ResetScore();
		}

		public void Dispose()
		{
			return;
		}

		public void ResetScore()
		{
			ScoreA = 0;
			ScoreB = 0;
			MaxScoreA = 0;
			MaxScoreB = 0;
		}

		public void AddScore(ColorType colorType, int score, int multiplier)
		{
			// Update score for left or right saber
			if (colorType == ColorType.ColorA)
			{
				ScoreA += score * multiplier;
				MaxScoreA += ScoreModel.kMaxCutRawScore * multiplier;
			}
			else if (colorType == ColorType.ColorB)
			{
				ScoreB += score * multiplier;
				MaxScoreB += ScoreModel.kMaxCutRawScore * multiplier;
			}
			else
			{
				Plugin.Log.Warn($"scoreManager, AddScore: Failed to add score of [score={score}, multiplier={multiplier}]. Reason: colorType is invalid [colorType={colorType}].");
			}

			// Inform listeners that the score has updated
			InvokeScoreUpdate();
		}

		public void SubtractScore(ColorType colorType, int score, int multiplier, bool subtractFromMaxScore = false)
		{
			// Update score for left or right saber
			if (colorType == ColorType.ColorA)
			{
				ScoreA -= score * multiplier;
				if (subtractFromMaxScore) MaxScoreA -= ScoreModel.kMaxCutRawScore * multiplier;
			}
			else if (colorType == ColorType.ColorB)
			{
				ScoreB -= score * multiplier;
				if (subtractFromMaxScore) MaxScoreB -= ScoreModel.kMaxCutRawScore * multiplier;
			}
			else
			{
				Plugin.Log.Warn($"scoreManager, AddScore: Failed to subtract score of [score={score}, multiplier={multiplier}]. Reason: colorType is invalid [colorType={colorType}].");
			}

			// Inform listeners that the score has updated
			InvokeScoreUpdate();
		}

		protected virtual void InvokeScoreUpdate()
		{
			Plugin.Log.Notice($"Score Has Updated - currentScore = {ScoreTotal}, currentMaxScore = {MaxScoreTotal}");

			// Create event handler
			EventHandler handler = OnScoreUpdate;
			if (handler != null)
			{
				// Invoke event
				handler(this, EventArgs.Empty);
			}
		}
	}
}

using FullComboPercentageCounter.Configuration;
using System;
using System.Globalization;
using Zenject;

namespace FullComboPercentageCounter
{
	public class ScoreManager : IInitializable, IDisposable
	{
		public event EventHandler OnScoreUpdate;

		private static double defaultPercentage = 100;

		private string percentageStringFormat;

		public double Percentage => CalculatePercentage(ScoreTotal, MaxScoreTotal, PluginConfig.Instance.DecimalPrecision);
		public int ScoreTotal => ScoreA + ScoreB;
		public int ScoreA { get; private set; }
		public int ScoreB { get; private set; }
		public int MaxScoreTotal => MaxScoreA + MaxScoreB;
		public int MaxScoreA { get; private set; }
		public int MaxScoreB { get; private set; }
		public int ScoreAtCurrentPercentage => CalculateScoreFromCurrentPercentage();

		private int CalculateScoreFromCurrentPercentage()
		{
			if (MaxScoreTotal == 0)
				return 0;

			double currentRatio = CalculateRatio(ScoreTotal, MaxScoreTotal);
			return (int)Math.Round(currentRatio * MaxScoreAtLevelStart);
		}

		public int HighscoreAtLevelStart { get; private set; }
		public int MaxScoreAtLevelStart { get; private set; }
		public double HighscoreAtLevelStartPercentage => CalculatePercentage(HighscoreAtLevelStart, MaxScoreAtLevelStart, PluginConfig.Instance.DecimalPrecision);
		

		public void Initialize()
		{
			ResetScore();
			InitPercentageStringFormat();
		}

		private void InitPercentageStringFormat()
		{
			percentageStringFormat = "0";
			if (PluginConfig.Instance.DecimalPrecision > 0)
				percentageStringFormat += "." + new string('0', PluginConfig.Instance.DecimalPrecision);
		}

		public void Dispose()
		{
			return;
		}

		private void ResetScore()
		{
			ScoreA = 0;
			ScoreB = 0;
			MaxScoreA = 0;
			MaxScoreB = 0;
		}

		internal void ResetScoreManager(IDifficultyBeatmap beatmap, PlayerDataModel playerDataModel)
		{
			ResetScore();
			InitPercentageStringFormat();

			PlayerLevelStatsData stats = playerDataModel.playerData.GetPlayerLevelStatsData(beatmap);
			HighscoreAtLevelStart = stats.highScore;
			MaxScoreAtLevelStart = CalculateMaxScore(beatmap.beatmapData.cuttableNotesCount);

			Plugin.Log.Notice("Highscore at level start = " + HighscoreAtLevelStart + "/" + MaxScoreAtLevelStart);
		}

		internal void AddScore(ColorType colorType, int score, int multiplier)
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

			// Inform listeners that the score has updated
			InvokeScoreUpdate();
		}

		internal void SubtractScore(ColorType colorType, int score, int multiplier, bool subtractFromMaxScore = false)
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

			// Inform listeners that the score has updated
			InvokeScoreUpdate();
		}

		private int CalculateMaxScore(int noteCount)
		{
			if (noteCount < 14)
			{
				if (noteCount == 1)
					return 115;
				else if (noteCount < 5)
					return (noteCount - 1) * 230 + 115;
				else
					return (noteCount - 5) * 460 + 1035;
			}
			else
				return (noteCount - 13) * 920 + 4715;
		}

		private double CalculatePercentage(int val, int maxVal, int decimalPrecision)
		{
			return Math.Round(CalculateRatio(val, maxVal) * 100, decimalPrecision);
		}

		private double CalculateRatio(int val, int maxVal)
		{
			if (maxVal == 0)
				return defaultPercentage / 100;
			return (double)val / (double)maxVal;
		}

		public string PercentageToString(double percentage)
		{
			return percentage.ToString(percentageStringFormat) + "%";
		}

		public string ScoreToString(int score)
		{
			// Format the score to norwegian notation (which uses spaces as seperator in large numbers) and then remove the decimal characters ",00" from the end.
			string scoreString = score.ToString("n", new CultureInfo("nb-NO"));
			return scoreString.Remove(scoreString.Length - 3);
		}

		protected virtual void InvokeScoreUpdate()
		{
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

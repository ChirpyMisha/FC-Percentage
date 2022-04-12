#nullable enable

using System;
using System.Globalization;
using UnityEngine;
using Zenject;

namespace FCPercentage.FCPCore
{
	public class ScoreManager : IInitializable, IDisposable
	{
		public event EventHandler? OnScoreUpdate;

		private static double defaultPercentageAtStart = 100;
		private static double defaultPercentageAtEnd = 0;
		private double defaultPercentage;

		public double PercentageTotal => CalculatePercentage(ScoreTotal, MaxScoreTotal);
		public double PercentageA => CalculatePercentage(ScoreA, MaxScoreA);
		public double PercentageB => CalculatePercentage(ScoreB, MaxScoreB);
		public int ScoreTotal => ScoreA + ScoreB;
		public int ScoreA { get; private set; }
		public int ScoreB { get; private set; }
		public int MaxScoreTotal => MaxScoreA + MaxScoreB;
		public int MaxScoreA { get; private set; }
		public int MaxScoreB { get; private set; }
		public int ScoreAtCurrentPercentage => CalculateScoreFromCurrentPercentage();

		public string SaberAColor { get; private set; } = "#FFFFFF";
		public string SaberBColor { get; private set; } = "#FFFFFF";

		public int Highscore { get; private set; }
		public int HighscoreAtLevelStart { get; internal set; }
		public int MaxScoreAtLevelStart { get; private set; }
		public double HighscorePercentage => CalculatePercentage(Highscore, MaxScoreAtLevelStart);
		public double HighscorePercentageAtLevelStart => CalculatePercentage(HighscoreAtLevelStart, MaxScoreAtLevelStart);
		

		private int CalculateScoreFromCurrentPercentage()
		{
			if (MaxScoreTotal == 0)
				return 0;

			double currentRatio = CalculateRatio(ScoreTotal, MaxScoreTotal);
			return (int)Math.Round(currentRatio * MaxScoreAtLevelStart);
		}
		
		public ScoreManager()
		{
			ResetScore();
		}

		public void Initialize()
		{
			return;
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
			Highscore = 0;
			HighscoreAtLevelStart = 0;
			MaxScoreAtLevelStart = 0;
			defaultPercentage = defaultPercentageAtStart;
		}

		internal void NotifyOfSongEnded(int levelResultScoreModified)
		{
			defaultPercentage = defaultPercentageAtEnd;
			UpdateHighscore(levelResultScoreModified);
		}

		private void UpdateHighscore(int levelResultScoreModified)
		{
			if (levelResultScoreModified > Highscore)
				Highscore = levelResultScoreModified;
		}

		internal void ResetScoreManager(PlayerLevelStatsData stats, IReadonlyBeatmapData transformedBeatmapData, ColorScheme colorScheme)
		{
			ResetScore();

			Highscore = stats.highScore;
			HighscoreAtLevelStart = stats.highScore;
			MaxScoreAtLevelStart = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(transformedBeatmapData);

			SaberAColor = "#" + ColorUtility.ToHtmlStringRGB(colorScheme.saberAColor);
			SaberBColor = "#" + ColorUtility.ToHtmlStringRGB(colorScheme.saberBColor);

			InvokeScoreUpdate();
		}

		internal void AddScore(ColorType colorType, int score, int maxScore, int multiplier)
		{
			// Update score for left or right saber
			if (colorType == ColorType.ColorA)
			{
				ScoreA += score * multiplier;
				MaxScoreA += maxScore * multiplier;
			}
			else if (colorType == ColorType.ColorB)
			{
				ScoreB += score * multiplier;
				MaxScoreB += maxScore * multiplier;
			}

			// Inform listeners that the score has updated
			InvokeScoreUpdate();
		}

		internal void SubtractScore(ColorType colorType, int score, int multiplier)
		{
			SubtractScore(colorType, score, 0, multiplier);
		}
		internal void SubtractScore(ColorType colorType, int score, int maxScore, int multiplier, bool subtractFromMaxScore = false)
		{
			// Update score for left or right saber
			if (colorType == ColorType.ColorA)
			{
				ScoreA -= score * multiplier;
				if (subtractFromMaxScore) MaxScoreA -= maxScore * multiplier;
			}
			else if (colorType == ColorType.ColorB)
			{
				ScoreB -= score * multiplier;
				if (subtractFromMaxScore) MaxScoreB -= maxScore * multiplier;
			}

			// Inform listeners that the score has updated
			InvokeScoreUpdate();
		}

		private double CalculatePercentage(int val, int maxVal) =>  CalculateRatio(val, maxVal) * 100;
		private double CalculateRatio(int val, int maxVal) => maxVal != 0 ? ((double)val / (double)maxVal) : (defaultPercentage / 100);

		public string PercentageToString(double percentage, string decimalFormat, bool keepTrailingZeros)
		{
			int decimalPrecision = 0;
			if (decimalFormat.Length >= 3) // A length smaller than 3 means that it contains less than 1 decimal.
				decimalPrecision = decimalFormat.Length - 2;

			percentage = Math.Round(percentage, decimalPrecision);

			string result;
			if (keepTrailingZeros)
				result = percentage.ToString(decimalFormat);
			else
				result = percentage.ToString();

			result += "%";
			return result;
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
			EventHandler? handler = OnScoreUpdate;
			if (handler != null)
			{
				// Invoke event
				handler(this, EventArgs.Empty);
			}
		}

		public string CreatePercentageStringFormat(int decimalPrecision)
		{
			string percentageStringFormat = "0";
			if (decimalPrecision > 0)
				percentageStringFormat += "." + new string('0', decimalPrecision);
			return percentageStringFormat;
		}
	}
}

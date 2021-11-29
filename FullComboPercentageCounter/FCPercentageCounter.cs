using CountersPlus.Counters.Interfaces;
using CountersPlus.Custom;
using CountersPlus.Utils;
using Zenject;
using TMPro;
using FullComboPercentageCounter.Configuration;
using System;

namespace FullComboPercentageCounter
{
	public class FCPercentageCounter : ICounter
	{
		//The icon doesn't work for some unknown reason.

		private static double DefaultPercentage = 100.0;

		private TMP_Text counterText;
		private string counterFormat;

		private readonly ScoreTracker scoreTracker;
		private readonly FCPercentageConfigModel counterConfig;

		// Can this be changed to be the same as CanvasUtility and Settings? Need to test this after testing the previous changes.
		public FCPercentageCounter([InjectOptional] ScoreTracker scoreTracker, [InjectOptional] FCPercentageConfigModel counterConfig)
		{
			this.scoreTracker = scoreTracker;
			this.counterConfig = counterConfig;
		}

		[Inject] protected CanvasUtility CanvasUtility;
		[Inject] protected CustomConfigModel Settings;

		public void CounterInit()
		{
			Plugin.Log.Info("Starting NoShitmissPercentageCounter Init");

			if (scoreTracker == null)
			{
				Plugin.Log.Error("ERROR: scoreTracker == null");
				return;
			}
			if (counterConfig == null)
			{
				Plugin.Log.Error("ERROR: counterConfig == null");
				return;
			}

			InitCounterText();

			scoreTracker.OnScoreUpdate += OnScoreUpdateHandler;
		}

		private void InitCounterText()
		{
			counterFormat = CreateCounterFormat();

			counterText = CanvasUtility.CreateTextFromSettings(Settings);
			counterText.text = $"{DefaultPercentage.ToString(counterFormat)}%";
		}

		private string CreateCounterFormat()
		{
			string format = "0";
			if (counterConfig.DecimalPrecision > 0)
				format += ".";

			for (int i = 0; i < counterConfig.DecimalPrecision; i++)
				format += "0";

			return format;
		}

		public void CounterDestroy()
		{
			scoreTracker.OnScoreUpdate -= OnScoreUpdateHandler;
		}

		private void OnScoreUpdateHandler(object s, ScoreUpdateEventArgs e)
		{
			double percent = PercentageOf(e.CurrentScore, e.CurrentMaxScore, counterConfig.DecimalPrecision);
			counterText.text = $"{percent.ToString(counterFormat)}%";
		}

		private double PercentageOf(double part, double total, int decimalPrecision)
		{
			return Math.Round(PercentageOf(part, total), decimalPrecision);
		}
		private double PercentageOf(double part, double total)
		{
			return (part / total) * 100;
		}
	}
}


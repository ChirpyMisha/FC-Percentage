using CountersPlus.Counters.Interfaces;
using CountersPlus.ConfigModels;
using CountersPlus.Custom;
using CountersPlus.Utils;
using Zenject;
using TMPro;
using UnityEngine;
using FullComboPercentageCounter.Configuration;
using IPA.Utilities;
using System;
using FullComboPercentageCounter.Installers;
using SiraUtil.Zenject;
//using static CountersPlus.Utils.Accessors;

namespace FullComboPercentageCounter
{
	public class FCPercentageCounter : ICounter
	{
		//I got an idea to make it feel more responsive.
		//Im going to assume that it is a full swing and update the score as such.
		//But if it turns out it is not a full swing I will subtract those points when the swing is finished.

		//The icon doesn't work for some unknown reason.

		//The default counter position may need to be changed?

		//Changing the # of digits in the settings doesn't work. It also doesn't get saved?

		private TMP_Text counterText;
		private string counterFormat;

		private readonly ScoreTracker scoreTracker;
		FCPercentageConfigModel counterConfig;

		public FCPercentageCounter([InjectOptional] ScoreTracker scoreTracker, [InjectOptional] FCPercentageConfigModel counterConfig)
		{
			this.scoreTracker = scoreTracker;
			this.counterConfig = counterConfig;
		}

		/// <summary>
		/// Helper class for creating text within Counters+'s system.
		/// Not recommended for creating text belonging outside of Counters+.
		/// </summary>
		[Inject] protected CanvasUtility CanvasUtility;

		/// <summary>
		/// The <see cref="CustomConfigModel"/> for your Custom Counter.
		/// Use it to help position your text with <see cref="CanvasUtility"/>.
		/// </summary>
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
			double defaultPercentage = 100.0;
			counterFormat = CreateCounterFormat();

			counterText = CanvasUtility.CreateTextFromSettings(Settings);
			counterText.text = $"{defaultPercentage.ToString(counterFormat)}%";
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
			return;
		}

		private void OnScoreUpdateHandler(object s, ScoreUpdateEventArgs e)
		{
			double percent = PercentageOf(e.CurrentScore, e.CurrentMaxScore, counterConfig.DecimalPrecision);
			counterText.text = $"{percent.ToString(counterFormat)}%";
		}

		private double PercentageOf(double value1, double value2, int decimalPrecision)
		{
			return Math.Round(PercentageOf(value1, value2), decimalPrecision);
		}
		private double PercentageOf(double value1, double value2)
		{
			return (value1 / value2) * 100;
		}
	}
}


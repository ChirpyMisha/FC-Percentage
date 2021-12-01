using CountersPlus.Counters.Interfaces;
using CountersPlus.Custom;
using CountersPlus.Utils;
using Zenject;
using TMPro;
using FullComboPercentageCounter.Configuration;
using System;
using UnityEngine;

namespace FullComboPercentageCounter
{
	public class FCPercentageCounter : ICounter
	{
		private static double DefaultPercentage = 100.0;

		private TMP_Text counterText;
		private TMP_Text counterNameText;
		private string counterFormat;
		private string counterPrefix;

		private readonly NoteRatingTracker noteTracker;
		private readonly PluginConfig counterConfig;
		private ScoreTracker scoreTracker;

		// Can this be changed to be the same as CanvasUtility and Settings? Need to test this after testing the previous changes.
		public FCPercentageCounter([InjectOptional] NoteRatingTracker noteTracker)
		{
			this.noteTracker = noteTracker;
			counterConfig = PluginConfig.Instance;
		}

		[Inject] protected CanvasUtility CanvasUtility;
		[Inject] protected CustomConfigModel Settings;

		public void CounterInit()
		{
			Plugin.Log.Info("Starting NoShitmissPercentageCounter Init");

			if (noteTracker == null)
			{
				Plugin.Log.Error("ERROR: scoreTracker == null");
				return;
			}
			if (counterConfig == null)
			{
				Plugin.Log.Error("ERROR: counterConfig == null");
				return;
			}

			scoreTracker = new ScoreTracker(noteTracker);

			

			InitCounterText();

			scoreTracker.OnScoreUpdate += OnScoreUpdateHandler;
		}

		private void InitCounterText()
		{
			counterPrefix = "";
			if (counterConfig.EnableLabel)
			{
				if (counterConfig.LabelAboveCount)
				{
					counterNameText = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(0.0f, counterConfig.LabelOffsetAboveCount, 0.0f));
					counterNameText.text = counterConfig.LabelTextAboveCount;
					counterNameText.fontSize *= counterConfig.LabelSizeAboveCount;
				}
				else
				{
					counterPrefix = counterConfig.LabelTextPrefix;
				}
			}

			counterFormat = CreateCounterFormat();
			counterText = CanvasUtility.CreateTextFromSettings(Settings);

			counterText.fontSize *= counterConfig.PercentageSize;
			if (counterConfig.EnableLabel && !counterConfig.LabelAboveCount)
				counterText.fontSize -= 0.15f;
			counterText.text = $"{counterPrefix}{DefaultPercentage.ToString(counterFormat)}%";
		}

		private string CreateCounterFormat()
		{
			if (counterConfig.DecimalPrecision > 0)
				return "0." + new string('0', counterConfig.DecimalPrecision);
			else
				return "0";
		}

		public void CounterDestroy()
		{
			scoreTracker.OnScoreUpdate -= OnScoreUpdateHandler;
		}

		private void OnScoreUpdateHandler(object s, ScoreUpdateEventArgs e)
		{
			double percent = PercentageOf(e.CurrentScoreTotal, e.CurrentMaxScoreTotal, counterConfig.DecimalPrecision);
			counterText.text = $"{counterPrefix}{percent.ToString(counterFormat)}%";
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


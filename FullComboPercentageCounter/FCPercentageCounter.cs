//#nullable enable
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
		private TMP_Text counterText = null!;
		private TMP_Text counterNameText = null!;
		private string counterPrefix = "";
		private string percentageStringFormat = "";

		private PluginConfig config;
		
		private readonly ScoreManager scoreManager;
		private readonly CanvasUtility canvasUtility;
		private readonly CustomConfigModel settings;

		public FCPercentageCounter(ScoreManager scoreManager, CanvasUtility canvasUtility, CustomConfigModel settings)
		{
			this.scoreManager = scoreManager;
			this.canvasUtility = canvasUtility;
			this.settings = settings;

			config = PluginConfig.Instance;

			counterPrefix = config.EnableLabel_Counter && !config.LabelAboveCount ? config.CounterLabelTextPrefix : "";

			if (!HasNullReferences())
				percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision_Counter);
		}

		public void CounterInit()
		{
			if (HasNullReferences())
				return;

			InitCounterText();

			scoreManager.OnScoreUpdate += OnScoreUpdateHandler;
		}

		private void InitCounterText()
		{
			if (config.EnableLabel_Counter && config.LabelAboveCount)
			{
				counterNameText = canvasUtility.CreateTextFromSettings(settings, new Vector3(0.0f, config.CounterLabelOffsetAboveCount, 0.0f));
				counterNameText.text = config.CounterLabelTextAboveCount;
				counterNameText.fontSize *= config.CounterLabelSizeAboveCount;
			}

			counterText = canvasUtility.CreateTextFromSettings(settings);
			counterText.fontSize *= config.PercentageSize;

			RefreshCounterText();
		}

		public void CounterDestroy()
		{
			scoreManager.OnScoreUpdate -= OnScoreUpdateHandler;
		}

		public bool HasNullReferences()
		{
			if (scoreManager == null || canvasUtility == null || settings == null)
			{
				Plugin.Log.Error("FullComboPercentageCounter : FCPercentageCounter has a null reference and cannot initialize! Please notify ChirpyMisha about this bug.");
				Plugin.Log.Error("The following objects are null:");
				if (scoreManager == null)
					Plugin.Log.Error("- ScoreManager");
				if (canvasUtility == null)
					Plugin.Log.Error("- CanvasUtility");
				if (settings == null)
					Plugin.Log.Error("- Settings");

				return true;
			}

			return false;
		}

		private void OnScoreUpdateHandler(object s, EventArgs e)
		{
			RefreshCounterText();
		}

		private void RefreshCounterText()
		{
			counterText.text = $"{counterPrefix}{scoreManager.PercentageToString(scoreManager.Percentage, percentageStringFormat, config.KeepTrailingZeros_Counter)}";
		}
	}
}


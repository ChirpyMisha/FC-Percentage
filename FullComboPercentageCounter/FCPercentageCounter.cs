//#nullable enable
using CountersPlus.Counters.Interfaces;
using CountersPlus.Custom;
using CountersPlus.Utils;
using TMPro;
using FullComboPercentageCounter.Configuration;
using System;
using UnityEngine;
using System.Drawing;

namespace FullComboPercentageCounter
{
	public class FCPercentageCounter : ICounter
	{
		private TMP_Text counterText = null!;
		private TMP_Text counterNameText = null!;
		private readonly string counterPrefix;
		private readonly string counterSeparator = "";
		private readonly string counterColorTagLeft = "";
		private readonly string counterColorTagRight = "";
		private readonly string percentageStringFormat = "";

		private PluginConfig config;
		
		private readonly ScoreManager scoreManager;
		private readonly CanvasUtility canvasUtility;
		private readonly CustomConfigModel settings;
		private readonly ColorScheme colorScheme;

		public FCPercentageCounter(ScoreManager scoreManager, CanvasUtility canvasUtility, CustomConfigModel settings, GameplayCoreSceneSetupData sceneSetupData)
		{
			this.scoreManager = scoreManager;
			this.canvasUtility = canvasUtility;
			this.settings = settings;
			this.colorScheme = sceneSetupData.colorScheme;

			config = PluginConfig.Instance;

			counterPrefix = config.EnableLabel_Counter && !config.LabelAboveCount ? config.CounterLabelTextPrefix : "";

			if (!HasNullReferences())
			{
				if (config.SplitPercentage_Counter)
				{
					counterSeparator = config.SplitPercentageSeparatorText_Counter;
					if (config.UseSaberColorScheme_Counter)
					{
						counterSeparator = $"<color=#FFFFFF>{counterSeparator}";
						counterColorTagLeft = $"<color=#{ColorUtility.ToHtmlStringRGB(colorScheme.saberAColor)}>";
						counterColorTagRight = $"<color=#{ColorUtility.ToHtmlStringRGB(colorScheme.saberBColor)}>";
					}
				}

				percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision_Counter);
			}
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
			if (scoreManager == null || canvasUtility == null || settings == null || colorScheme == null)
			{
				Plugin.Log.Error("FullComboPercentageCounter : FCPercentageCounter has a null reference and cannot initialize! Please notify ChirpyMisha about this bug.");
				Plugin.Log.Error("The following objects are null:");
				if (scoreManager == null)
					Plugin.Log.Error("- ScoreManager");
				if (canvasUtility == null)
					Plugin.Log.Error("- CanvasUtility");
				if (settings == null)
					Plugin.Log.Error("- Settings");
				if (colorScheme == null)
					Plugin.Log.Error("- ColorScheme");

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
			counterText.text = config.SplitPercentage_Counter
			?
				config.UseSaberColorScheme_Counter
				?
					$"{counterPrefix}{counterColorTagLeft}{PercentageToString(scoreManager.PercentageA)}" +
						$"{counterSeparator}{counterColorTagRight}{PercentageToString(scoreManager.PercentageB)}"
				:
					$"{counterPrefix}{PercentageToString(scoreManager.PercentageA)}{counterSeparator}{PercentageToString(scoreManager.PercentageB)}"
			:
				 $"{counterPrefix}{PercentageToString(scoreManager.PercentageTotal)}";
		}

		private string PercentageToString(double percent)
		{
			return scoreManager.PercentageToString(percent, percentageStringFormat, config.KeepTrailingZeros_Counter);
		}
	}
}


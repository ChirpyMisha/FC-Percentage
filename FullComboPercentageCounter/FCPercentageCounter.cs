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
		private TMP_Text counterPercentageText = null!;
		private TMP_Text counterNameText = null!;
		private readonly string percentagePrefix;
		private string percentageSeparator = "";
		private string percentageColorTagLeft = "";
		private string percentageColorTagRight = "";
		private string percentageStringFormat = "";

		private PluginConfig config;
		
		private readonly ScoreManager scoreManager;
		private readonly CanvasUtility canvasUtility;
		private readonly CustomConfigModel settings;

		public FCPercentageCounter(ScoreManager scoreManager, CanvasUtility canvasUtility, CustomConfigModel settings, GameplayCoreSceneSetupData sceneSetupData)
		{
			this.scoreManager = scoreManager;
			this.canvasUtility = canvasUtility;
			this.settings = settings;

			config = PluginConfig.Instance;

			percentagePrefix = config.EnableLabel_Counter && !config.LabelAboveCount ? config.CounterLabelTextPrefix : "";

			if (!HasNullReferences())
			{
				if (config.SplitPercentage_Counter)
				{
					percentageSeparator = config.SplitPercentageSeparatorText_Counter;
					if (config.UseSaberColorScheme_Counter)
					{
						percentageSeparator = $"<color=#FFFFFF>{percentageSeparator}";
						percentageColorTagLeft = $"<color=#{ColorUtility.ToHtmlStringRGB(sceneSetupData.colorScheme.saberAColor)}>";
						percentageColorTagRight = $"<color=#{ColorUtility.ToHtmlStringRGB(sceneSetupData.colorScheme.saberBColor)}>";
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

			scoreManager.OnScoreUpdate += ScoreManager_OnScoreUpdate;
		}

		private void InitCounterText()
		{
			if (config.EnableLabel_Counter && config.LabelAboveCount)
			{
				counterNameText = canvasUtility.CreateTextFromSettings(settings, new Vector3(0.0f, config.CounterLabelOffsetAboveCount, 0.0f));
				counterNameText.text = config.CounterLabelTextAboveCount;
				counterNameText.fontSize *= config.CounterLabelSizeAboveCount;
			}

			counterPercentageText = canvasUtility.CreateTextFromSettings(settings);
			counterPercentageText.fontSize *= config.PercentageSize;

			RefreshCounterText();
		}

		public void CounterDestroy()
		{
			scoreManager.OnScoreUpdate -= ScoreManager_OnScoreUpdate;
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

		private void ScoreManager_OnScoreUpdate(object sender, EventArgs e)
		{
			RefreshCounterText();
		}

		private void RefreshCounterText()
		{
			counterPercentageText.text = config.SplitPercentage_Counter
			?
				config.UseSaberColorScheme_Counter
				?
					$"{percentagePrefix}{percentageColorTagLeft}{PercentageToString(scoreManager.PercentageA)}" +
						$"{percentageSeparator}{percentageColorTagRight}{PercentageToString(scoreManager.PercentageB)}"
				:
					$"{percentagePrefix}{PercentageToString(scoreManager.PercentageA)}{percentageSeparator}{PercentageToString(scoreManager.PercentageB)}"
			:
				 $"{percentagePrefix}{PercentageToString(scoreManager.PercentageTotal)}";
		}

		private string PercentageToString(double percent)
		{
			return scoreManager.PercentageToString(percent, percentageStringFormat, config.KeepTrailingZeros_Counter);
		}
	}
}


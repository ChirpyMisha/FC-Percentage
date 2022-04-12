using System;
using UnityEngine;
using TMPro;
using CountersPlus.Counters.Interfaces;
using CountersPlus.Custom;
using CountersPlus.Utils;
using FCPercentage.FCPCounter.Configuration;
using FCPercentage.FCPCore;
using FCPercentage.FCPCore.Configuration;
using SiraUtil.Logging;

namespace FCPercentage.FCPCounter
{
	public class FCPCounterController : ICounter
	{
		private TMP_Text counterPercentageText = null!;
		private TMP_Text labelAboveCounterText = null!;
		private readonly string percentagePrefix;
		private string percentageColorTagA = "";
		private string percentageColorTagB = "";
		private string percentageToStringFormat = "";

		private CounterSettings config;

		private readonly SiraLog logger;
		private readonly ScoreManager scoreManager;
		private readonly CanvasUtility canvasUtility;
		private readonly CustomConfigModel settings;

		public FCPCounterController(SiraLog logger, ScoreManager scoreManager, CanvasUtility canvasUtility, CustomConfigModel settings, GameplayCoreSceneSetupData sceneSetupData)
		{
			this.logger = logger;
			this.scoreManager = scoreManager;
			this.canvasUtility = canvasUtility;
			this.settings = settings;

			config = PluginConfig.Instance.CounterSettings;

			percentagePrefix = config.EnableLabel == CounterLabelOptions.AsPrefix ? config.Advanced.LabelPrefixText : "";

			if (!HasNullReferences())
			{
				if (config.PercentageMode == CounterPercentageModes.Split || config.PercentageMode == CounterPercentageModes.TotalAndSplit)
				{
					if (config.SplitPercentageUseSaberColorScheme)
					{
						percentageColorTagA = $"<color=#{ColorUtility.ToHtmlStringRGB(sceneSetupData.colorScheme.saberAColor)}>";
						percentageColorTagB = $"<color=#{ColorUtility.ToHtmlStringRGB(sceneSetupData.colorScheme.saberBColor)}>";
					}
				}

				percentageToStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision);
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
			if (config.EnableLabel == CounterLabelOptions.AboveCounter)
			{
				labelAboveCounterText = canvasUtility.CreateTextFromSettings(settings, new Vector3(0.0f, config.Advanced.LabelAboveCounterTextOffset + config.Advanced.CounterOffset, 0.0f));
				labelAboveCounterText.text = config.Advanced.LabelAboveCounterText;
				labelAboveCounterText.fontSize *= config.Advanced.LabelAboveCounterTextSize;
			}

			counterPercentageText = canvasUtility.CreateTextFromSettings(settings, new Vector3(0.0f, config.Advanced.CounterOffset, 0.0f));
			counterPercentageText.fontSize *= config.Advanced.PercentageSize;
			counterPercentageText.lineSpacing = config.Advanced.PercentageTotalAndSplitLineHeight * 100;
			

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
				logger.Error("FCPCounterController has a null reference and cannot initialize! Please notify ChirpyMisha about this bug.");
				logger.Error("The following objects are null:");
				if (scoreManager == null)
					logger.Error("- ScoreManager");
				if (canvasUtility == null)
					logger.Error("- CanvasUtility");
				if (settings == null)
					logger.Error("- Settings");

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
			counterPercentageText.text = percentagePrefix;
			if (config.PercentageMode == CounterPercentageModes.Total || config.PercentageMode == CounterPercentageModes.TotalAndSplit)
				counterPercentageText.text += GetPercentageTotalStringFormatted();
			if (config.PercentageMode == CounterPercentageModes.Split || config.PercentageMode == CounterPercentageModes.TotalAndSplit)
				counterPercentageText.text += GetPercentageSplitStringFormatted();
		}

		private string GetPercentageTotalStringFormatted()
		{
			return $"{PercentageToString(scoreManager.PercentageTotal)}\n";
		}

		private string GetPercentageSplitStringFormatted()
		{
			return $"{percentageColorTagA}{config.Advanced.PercentageSplitSaberAPrefixText}{PercentageToString(scoreManager.PercentageA)} " +
				   $"{percentageColorTagB}{config.Advanced.PercentageSplitSaberBPrefixText}{PercentageToString(scoreManager.PercentageB)}";
		}

		private string PercentageToString(double percent)
		{
			return scoreManager.PercentageToString(percent, percentageToStringFormat, config.KeepTrailingZeros);
		}
	}
}


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
		private string percentageColorTagA = "";
		private string percentageColorTagB = "";
		private string percentageStringFormat = "";

		private SettingsFCPercentageCounter config;
		
		private readonly ScoreManager scoreManager;
		private readonly CanvasUtility canvasUtility;
		private readonly CustomConfigModel settings;

		public FCPercentageCounter(ScoreManager scoreManager, CanvasUtility canvasUtility, CustomConfigModel settings, GameplayCoreSceneSetupData sceneSetupData)
		{
			this.scoreManager = scoreManager;
			this.canvasUtility = canvasUtility;
			this.settings = settings;

			config = PluginConfig.Instance.FcCounterSettings;

			percentagePrefix = config.EnableLabel == CounterLabelOptions.AsPrefix ? config.Formatting.LabelPrefixText : "";

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

				percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision);
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
				counterNameText = canvasUtility.CreateTextFromSettings(settings, new Vector3(0.0f, config.Formatting.LabelAboveCounterTextOffset, 0.0f));
				counterNameText.text = config.Formatting.LabelAboveCounterText;
				counterNameText.fontSize *= config.Formatting.LabelAboveCounterTextSize;
			}

			counterPercentageText = canvasUtility.CreateTextFromSettings(settings, new Vector3(0.0f, config.Formatting.PercentageTextOffset, 0.0f));
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
			if (config.PercentageMode == CounterPercentageModes.Total)
				counterPercentageText.text = $"{percentagePrefix}{PercentageToString(scoreManager.PercentageTotal)}";
			else if (config.PercentageMode == CounterPercentageModes.Split)
				counterPercentageText.text = $"{percentagePrefix}" +
											 $"{percentageColorTagA}{config.Formatting.PercentageSplitSaberAPrefixText}{PercentageToString(scoreManager.PercentageA)}" +
											 $"{percentageColorTagB}{config.Formatting.PercentageSplitSaberBPrefixText}{PercentageToString(scoreManager.PercentageB)}";
			else if (config.PercentageMode == CounterPercentageModes.TotalAndSplit)
				counterPercentageText.text = $"<line-height={config.Formatting.PercentageTotalAndSplitLineHeight}%>{percentagePrefix}{PercentageToString(scoreManager.PercentageTotal)}\n" +
											 $"{percentageColorTagA}{config.Formatting.PercentageSplitSaberAPrefixText}{PercentageToString(scoreManager.PercentageA)}" +
											 $"{percentageColorTagB}{config.Formatting.PercentageSplitSaberBPrefixText}{PercentageToString(scoreManager.PercentageB)}";
		}

		private string PercentageToString(double percent)
		{
			return scoreManager.PercentageToString(percent, percentageStringFormat, config.KeepTrailingZeros);
		}
	}
}


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
		private string counterPrefix;

		private PluginConfig config;
		
		[Inject] protected ScoreManager ScoreManager = null!;
		[Inject] protected CanvasUtility CanvasUtility = null!;
		[Inject] protected CustomConfigModel Settings = null!;

		public FCPercentageCounter()
		{
			config = PluginConfig.Instance;

			counterPrefix = config.EnableLabel_Counter && !config.LabelAboveCount ? config.CounterLabelTextPrefix : "";
		}

		public void CounterInit()
		{
			Plugin.Log.Info("Starting FCPercentageCounter Init");

			if (HasNullReferences())
				return;

			InitCounterText();

			ScoreManager.OnScoreUpdate += OnScoreUpdateHandler;
		}

		private void InitCounterText()
		{
			if (config.EnableLabel_Counter && config.LabelAboveCount)
			{
				counterNameText = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(0.0f, config.CounterLabelOffsetAboveCount, 0.0f));
				counterNameText.text = config.CounterLabelTextAboveCount;
				counterNameText.fontSize *= config.CounterLabelSizeAboveCount;
			}

			counterText = CanvasUtility.CreateTextFromSettings(Settings);
			counterText.fontSize *= config.PercentageSize;

			RefreshCounterText();
		}

		public void CounterDestroy()
		{
			ScoreManager.OnScoreUpdate -= OnScoreUpdateHandler;
		}

		public bool HasNullReferences()
		{
			if (ScoreManager == null || CanvasUtility == null || Settings == null)
			{
				Plugin.Log.Error("FullComboPercentageCounter : FCPercentageCounter has a null reference and cannot initialize! Please notify ChirpyMisha about this bug.");
				Plugin.Log.Error("The following objects are null:");
				if (ScoreManager == null)
					Plugin.Log.Error("- ScoreManager");
				if (CanvasUtility == null)
					Plugin.Log.Error("- CanvasUtility");
				if (Settings == null)
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
			counterText.text = $"{counterPrefix}{ScoreManager.PercentageToString(ScoreManager.Percentage)}";
		}
	}
}


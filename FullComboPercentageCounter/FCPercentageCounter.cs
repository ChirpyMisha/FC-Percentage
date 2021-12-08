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
		private TMP_Text counterText;
		private TMP_Text counterNameText;
		private string counterPrefix;
		private string CounterTextFormat => $"{counterPrefix}{ScoreManager.PercentageStr}%";

		private PluginConfig counterConfig;
		
		[Inject] protected ScoreManager ScoreManager;
		[Inject] protected CanvasUtility CanvasUtility;
		[Inject] protected CustomConfigModel Settings;

		public void CounterInit()
		{
			Plugin.Log.Info("Starting FCPercentageCounter Init");

			counterConfig = PluginConfig.Instance;

			InitCounterText();

			ScoreManager.OnScoreUpdate += OnScoreUpdateHandler;
		}

		private void InitCounterText()
		{
			counterPrefix = "";
			if (counterConfig.EnableLabel)
			{
				if (counterConfig.LabelAboveCount)
				{
					counterNameText = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(0.0f, counterConfig.CounterLabelOffsetAboveCount, 0.0f));
					counterNameText.text = counterConfig.CounterLabelTextAboveCount;
					counterNameText.fontSize *= counterConfig.CounterLabelSizeAboveCount;
				}
				else
				{
					counterPrefix = counterConfig.CounterLabelTextPrefix;
				}
			}

			counterText = CanvasUtility.CreateTextFromSettings(Settings);
			counterText.fontSize *= counterConfig.PercentageSize;

			RefreshCounterText();
		}

		public void CounterDestroy()
		{
			ScoreManager.OnScoreUpdate -= OnScoreUpdateHandler;
		}

		private void OnScoreUpdateHandler(object s, EventArgs e)
		{
			RefreshCounterText();
		}

		private void RefreshCounterText()
		{
			counterText.text = CounterTextFormat;
		}
	}
}


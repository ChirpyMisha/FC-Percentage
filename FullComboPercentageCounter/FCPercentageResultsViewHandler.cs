using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using FullComboPercentageCounter.Configuration;
using HMUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace FullComboPercentageCounter
{
	class FCPercentageResultsViewHandler : IInitializable, IDisposable
	{
		private static readonly string ResourceNameFCPercentage = "FullComboPercentageCounter.UI.Views.ResultsViewFCPercentage.bsml";
		private static readonly string ResourceNameFCScore = "FullComboPercentageCounter.UI.Views.ResultsViewFCScore.bsml";

		[UIParams]
		private BSMLParserParams parserParams;
		[UIComponent("fc-percent-text")]
		private TextMeshProUGUI fcPercentText;
		[UIComponent("fc-score-text")]
		private TextMeshProUGUI fcScoreText;

		private readonly ScoreManager scoreManager;
		private readonly ResultsViewController resultsViewController;

		private PluginConfig counterConfig;

		public FCPercentageResultsViewHandler(ScoreManager scoreManager, ResultsViewController resultsViewController)
		{
			this.scoreManager = scoreManager;
			this.resultsViewController = resultsViewController;
		}

		public void Initialize()
		{
			if (resultsViewController != null)
				resultsViewController.didActivateEvent += ResultsViewController_OnActivateEvent;
			counterConfig = PluginConfig.Instance;
		}

		public void Dispose()
		{
			if (resultsViewController != null)
				resultsViewController.didActivateEvent -= ResultsViewController_OnActivateEvent;
		}

		private void ResultsViewController_OnActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			ParseBSML();

			if (counterConfig.ResultsViewMode == ResultsViewModes.On)
				SetResultsViewText();
			else if (counterConfig.ResultsViewMode == ResultsViewModes.OffWhenFullCombo && scoreManager.IsComboBroken)
				SetResultsViewText();
			else
				EmptyResultsViewText();
		}

		private void ParseBSML()
		{
			if (!Plugin.Instance.IsResultsViewBSMLParsed)
			{
				string bsml = Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), ResourceNameFCPercentage);
				BSMLParser.instance.Parse(bsml, resultsViewController.gameObject, this);
				//Plugin.Log.Notice($"BSML = \"{bsml}\"");
				bsml = Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), ResourceNameFCScore);
				BSMLParser.instance.Parse(bsml, resultsViewController.gameObject, this);
				//Plugin.Log.Notice($"BSML = \"{bsml}\"");

				Plugin.Instance.IsResultsViewBSMLParsed = true;
			}
		}

		private void SetResultsViewText()
		{
			string percent = scoreManager.PercentageStr;
			//Plugin.Log.Notice($"ResultsView Activated - currentScore = {scoreManager.ScoreTotal}, currentMaxScore = {scoreManager.MaxScoreTotal}, percentage = {percent}");

			fcPercentText.text = counterConfig.ResultScreenPercentagePrefix + percent + "%";
			string scoreString = scoreManager.ScoreTotalIncMissed.ToString("n", new CultureInfo("nb-NO"));
			fcScoreText.text = counterConfig.ResultScreenScorePrefix + scoreString.Remove(scoreString.Length - 3);
		}

		private void EmptyResultsViewText()
		{
			fcPercentText.text = "";
			fcScoreText.text = "";
		}
	}
}

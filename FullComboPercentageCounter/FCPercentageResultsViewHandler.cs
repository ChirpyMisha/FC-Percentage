using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using FullComboPercentageCounter.Configuration;
using HMUI;
using IPA.Utilities;
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

		public static FieldAccessor<ResultsViewController, LevelCompletionResults>.Accessor LevelCompletionResults = FieldAccessor<ResultsViewController, LevelCompletionResults>.GetAccessor("_levelCompletionResults");

		[UIComponent("fc-percent-text")]
		private TextMeshProUGUI fcPercentText;
		[UIComponent("fc-score-text")]
		private TextMeshProUGUI fcScoreText;

		private readonly ScoreManager scoreManager;
		private ResultsViewController resultsViewController;

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
			ParseAllBSML();
			EmptyResultsViewText();

			LevelCompletionResults levelCompletionResults = LevelCompletionResults(ref resultsViewController);

			if (levelCompletionResults.levelEndStateType == global::LevelCompletionResults.LevelEndStateType.Cleared)
			{
				if (counterConfig.ResultsViewMode == ResultsViewModes.On)
					SetResultsViewText();
				else if (counterConfig.ResultsViewMode == ResultsViewModes.OffWhenFullCombo && !levelCompletionResults.fullCombo)
					SetResultsViewText();
			}
		}

		private void ParseAllBSML()
		{
			if (!Plugin.Instance.IsResultsViewBSMLParsed)
			{
				ParseBSML(ResourceNameFCScore);
				ParseBSML(ResourceNameFCPercentage);

				Plugin.Instance.IsResultsViewBSMLParsed = true;
			}
		}

		private void ParseBSML(string bsmlPath)
		{
			BSMLParser.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), bsmlPath), resultsViewController.gameObject, this);
		}

		private void SetResultsViewText()
		{
			string percentString = scoreManager.PercentageStr;
			fcPercentText.text = counterConfig.ResultScreenPercentagePrefix + percentString + "%";

			// Format the score to norwegian notation (which uses spaces as seperator in large numbers) and then remove the decimal characters ",00" from the end.
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

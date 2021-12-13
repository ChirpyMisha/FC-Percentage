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

		private static string colorPositive = "#00B300";
		private static string colorNegative = "#FF0000";

		public static FieldAccessor<ResultsViewController, LevelCompletionResults>.Accessor LevelCompletionResults = FieldAccessor<ResultsViewController, LevelCompletionResults>.GetAccessor("_levelCompletionResults");

		[UIComponent("fc-score-text")]
		private TextMeshProUGUI fcScoreText;
		[UIComponent("fc-score-diff-text")]
		private TextMeshProUGUI fcScoreDiffText;
		[UIComponent("fc-percent-text")]
		private TextMeshProUGUI fcPercentText;
		[UIComponent("fc-percent-diff-text")]
		private TextMeshProUGUI fcPercentDiffText;

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
			int currentScore = scoreManager.ScoreTotalIncMissed;
			double currentPercent = scoreManager.Percentage;

			fcScoreText.text = counterConfig.ResultScreenScorePrefix + scoreManager.ScoreToString(currentScore);
			fcPercentText.text = counterConfig.ResultScreenPercentagePrefix + scoreManager.PercentageToString(currentPercent);
			//fcScoreDiffText.text = "ScoreDiffText";
			//fcPercentDiffText.text = "PercDiffText";


			if (PluginConfig.Instance.EnableScorePercentageDifference && scoreManager.HighscoreAtLevelStartScore > 0)
			{
				int scoreDiff = currentScore - scoreManager.HighscoreAtLevelStartScore;
				double percentDiff = currentPercent - scoreManager.HighscoreAtLevelStartPercentage;

				string diffStringFormat;
				if (scoreDiff >= 0)
					diffStringFormat = $"<size=90%><color={colorPositive}>+";
				else
					diffStringFormat = $"<size=90%><color={colorNegative}>";

				fcScoreDiffText.text = diffStringFormat + scoreManager.ScoreToString(scoreDiff);
				fcPercentDiffText.text = diffStringFormat + scoreManager.PercentageToString(percentDiff);
			}
		}

		private void EmptyResultsViewText()
		{
			fcScoreText.text = "";
			fcScoreDiffText.text = "";
			fcPercentText.text = "";
			fcPercentDiffText.text = "";
		}
	}
}

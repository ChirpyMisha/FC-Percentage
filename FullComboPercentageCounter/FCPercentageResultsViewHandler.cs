#nullable enable
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using FullComboPercentageCounter.Configuration;
using IPA.Utilities;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using Zenject;

namespace FullComboPercentageCounter
{
	class FCPercentageResultsViewHandler : IInitializable, IDisposable
	{
		// 2 .bsml files are used since the amount of characters in the score would otherwise change the position of the percentage.
		private static readonly string ResourceNameFCPercentage = "FullComboPercentageCounter.UI.Views.ResultsViewFCPercentage.bsml";
		private static readonly string ResourceNameFCScore = "FullComboPercentageCounter.UI.Views.ResultsViewFCScore.bsml";

		// Color values of score and percentage difference.
		private static string colorPositive = "#00B300";
		private static string colorNegative = "#FF0000";

		// Text fields in the bsml
		[UIComponent("fc-score-text")]
		private TextMeshProUGUI? fcScoreText = null!;
		[UIComponent("fc-score-diff-text")]
		private TextMeshProUGUI? fcScoreDiffText = null!;
		[UIComponent("fc-percent-text")]
		private TextMeshProUGUI? fcPercentText = null!;
		[UIComponent("fc-percent-diff-text")]
		private TextMeshProUGUI? fcPercentDiffText = null!;

		private readonly ScoreManager scoreManager;
		private ResultsViewController resultsViewController;
		private static FieldAccessor<ResultsViewController, LevelCompletionResults>.Accessor LevelCompletionResults = FieldAccessor<ResultsViewController, LevelCompletionResults>.GetAccessor("_levelCompletionResults");

		private PluginConfig config;

		private string percentageSeparator = "";
		private string percentageColorTagLeft = "";
		private string percentageColorTagRight = "";
		private string percentageStringFormat = "";

		public FCPercentageResultsViewHandler(ScoreManager scoreManager, ResultsViewController resultsViewController)
		{
			this.scoreManager = scoreManager;
			this.resultsViewController = resultsViewController;

			config = PluginConfig.Instance;
		}

		public void Initialize()
		{
			if (resultsViewController != null)
				resultsViewController.didActivateEvent += ResultsViewController_OnActivateEvent;
		}

		public void Dispose()
		{
			if (resultsViewController != null)
				resultsViewController.didActivateEvent -= ResultsViewController_OnActivateEvent;
		}

		private void RefreshPercentageTextFormatting()
		{
			if (config.SplitPercentage_Counter)
			{
				percentageSeparator = config.SplitPercentageSeparatorText_ScorePercentage;
				if (config.UseSaberColorScheme_Counter)
				{
					percentageSeparator = $"<color=#FFFFFF>{percentageSeparator}";
					percentageColorTagLeft = $"<color={scoreManager.SaberAColor}>";
					percentageColorTagRight = $"<color={scoreManager.SaberBColor}>";
				}
			}

			percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision_ScorePercentage);
		}

		private void ResultsViewController_OnActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			scoreManager.NotifyOfSongEnded();
			ParseAllBSML();

			LevelCompletionResults levelCompletionResults = LevelCompletionResults(ref resultsViewController);

			if (levelCompletionResults.levelEndStateType == global::LevelCompletionResults.LevelEndStateType.Cleared)
			{
				if (config.ResultsViewMode == ResultsViewModes.On)
					SetResultsViewText();
				else if (config.ResultsViewMode == ResultsViewModes.OffWhenFullCombo && !levelCompletionResults.fullCombo)
					SetResultsViewText();
			}
			else
			{
				EmptyResultsViewText();
			}
		}

		private void ParseAllBSML()
		{
			if (fcScoreText == null)
				ParseBSML(ResourceNameFCScore);
			if (fcPercentText == null)
				ParseBSML(ResourceNameFCPercentage);
		}

		private void ParseBSML(string bsmlPath)
		{
			BSMLParser.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), bsmlPath), resultsViewController.gameObject, this);
		}

#pragma warning disable CS8602 // Dereference of a possibly null reference.
		private void SetResultsViewText()
		{
			// Empty the text fields so they can be filled with new information
			EmptyResultsViewText();
			RefreshPercentageTextFormatting();

			int currentScore = scoreManager.ScoreAtCurrentPercentage;
			double currentPercent = scoreManager.PercentageTotal;

			// Set prefix lables if enabled
			if (config.EnableLabel_ScorePercentage)
			{
				fcScoreText.text = config.ResultScreenScorePrefix;
				fcPercentText.text = config.ResultScreenPercentagePrefix;
			}

			// Add the score and percentage to the string.
			fcScoreText.text += scoreManager.ScoreToString(currentScore);
			fcPercentText.text += config.SplitPercentage_ScorePercentage
			?
				config.UseSaberColorScheme_ScorePercentage
				?
					$"{percentageColorTagLeft}{PercentageToString(scoreManager.PercentageA)}" +
						$"{percentageSeparator}{percentageColorTagRight}{PercentageToString(scoreManager.PercentageB)}"
				:
					$"{PercentageToString(scoreManager.PercentageA)}{percentageSeparator}{PercentageToString(scoreManager.PercentageB)}"
			:
				 $"{PercentageToString(currentPercent)}";

			// Add the score and percentage difference if it's enabled.
			if (PluginConfig.Instance.EnableScorePercentageDifference && scoreManager.HighscoreAtLevelStart > 0)
			{
				int scoreDiff = currentScore - scoreManager.HighscoreAtLevelStart;
				double percentDiff = currentPercent - scoreManager.HighscoreAtLevelStartPercentage;

				string diffStringFormat;
				if (scoreDiff >= 0)
					diffStringFormat = $"<size=90%><color={colorPositive}>+";
				else
					diffStringFormat = $"<size=90%><color={colorNegative}>";

				fcScoreDiffText.text = diffStringFormat + scoreManager.ScoreToString(scoreDiff);
				fcPercentDiffText.text = diffStringFormat + PercentageToString(percentDiff);
			}
		}

		private void EmptyResultsViewText()
		{
			fcScoreText.text = "";
			fcScoreDiffText.text = "";
			fcPercentText.text = "";
			fcPercentDiffText.text = "";
		}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

		private string PercentageToString(double percent)
		{
			return scoreManager.PercentageToString(percent, percentageStringFormat, config.KeepTrailingZeros_ScorePercentage);
		}
	}
}

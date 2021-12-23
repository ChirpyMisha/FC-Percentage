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

		// Color tags of score/percentage difference.
		private static string colorPositiveTag = "<color=#00B300>+";
		private static string colorNegativeTag = "<color=#FF0000>";
		// Color tag for default color.
		//private static string colorDefaultTag = "<color=#FFFFFF>";

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
		private LevelCompletionResults levelCompletionResults = null!;

		private SettingsFCScorePercentage config;

		private string percentageColorTagA = "";
		private string percentageColorTagB = "";
		private string percentageStringFormat = "";

		public FCPercentageResultsViewHandler(ScoreManager scoreManager, ResultsViewController resultsViewController)
		{
			this.scoreManager = scoreManager;
			this.resultsViewController = resultsViewController;

			config = PluginConfig.Instance.FcScorePercentageSettings;
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
			if (config.SplitPercentageUseSaberColorScheme)
			{
				percentageColorTagA = $"<color={scoreManager.SaberAColor}>";
				percentageColorTagB = $"<color={scoreManager.SaberBColor}>";
			}
			else
			{
				percentageColorTagA = "";
				percentageColorTagB = "";
			}

			percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision);
		}

		private void ResultsViewController_OnActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			scoreManager.NotifyOfSongEnded();
			ParseAllBSML();

			levelCompletionResults = LevelCompletionResults(ref resultsViewController);

			if (levelCompletionResults.levelEndStateType == global::LevelCompletionResults.LevelEndStateType.Cleared)
				SetResultsViewText();
			else
				EmptyResultsViewText();
		}

		private void ParseAllBSML()
		{
			if (fcScoreText == null)
			{
				ParseBSML(ResourceNameFCScore);

				if (fcScoreDiffText != null)
					fcScoreDiffText.fontSize *= 0.85f;
				else
					Plugin.Log.Error($"Parsing BSML ({ResourceNameFCScore}) has failed. Please notify ChirpyMisha! Game will crash in 3, 2, 1 . . .");
			}
			if (fcPercentText == null)
			{
				ParseBSML(ResourceNameFCPercentage);

				if (fcPercentDiffText != null)
					fcPercentDiffText.fontSize *= 0.85f;
				else
					Plugin.Log.Error($"Parsing BSML ({ResourceNameFCPercentage}) has failed. Please notify ChirpyMisha! Game will crash in 3, 2, 1 . . .");
			}
		}

		private void ParseBSML(string bsmlPath)
		{
			BSMLParser.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), bsmlPath), resultsViewController.gameObject, this);
		}

		private bool IsActiveOnResultsView(ResultsViewModes mode)
		{
			// Checks if the result should be shown
			if (mode == ResultsViewModes.On || (mode == ResultsViewModes.OffWhenFC && !levelCompletionResults.fullCombo))
				return true;
			return false;
		}

#pragma warning disable CS8602 // Dereference of a possibly null reference.
		private void SetResultsViewText()
		{
			// Empty the text fields so they can be filled with new information
			EmptyResultsViewText();
			RefreshPercentageTextFormatting();

			SetPercentageText();
			SetScoreText();
		}

		private void SetPercentageText()
		{
			bool isPercentageAdded = false;
			// Add total percentage if enabled.
			if (IsActiveOnResultsView(config.TotalPercentageMode))
			{
				isPercentageAdded = true;
				fcPercentText.text += GetTotalPercentageText();

				// Add the total percentage difference if enabled.
				if (config.EnableScorePercentageDifference && scoreManager.HighscoreAtLevelStart > 0)
					fcPercentDiffText.text += GetTotalPercentageDiffText();
			}
			// Add split percentage if enabled.
			if (IsActiveOnResultsView(config.SplitPercentageMode))
			{
				isPercentageAdded = true;
				fcPercentText.text += GetSplitPercentageText();

				// Add the split percentage difference if enabled.
				if (config.EnableScorePercentageDifference && scoreManager.HighscoreAtLevelStart > 0)
					fcPercentDiffText.text += GetSplitPercentageDiffText();
			}

			// Set prefix label if enabled.
			if (isPercentageAdded && (config.EnableLabel == ResultsViewLabelOptions.BothOn || config.EnableLabel == ResultsViewLabelOptions.PercentageOn))
			{
				fcPercentText.text = config.Formatting.PercentagePrefixText + fcPercentText.text;
				fcPercentDiffText.text = $"<color=#FFFFFF00>{config.Formatting.PercentagePrefixText}{fcPercentDiffText.text}";
			}

			fcPercentText.text = fcPercentText.text.TrimEnd();
			fcPercentDiffText.text = fcPercentDiffText.text.TrimEnd();
		}

		private void SetScoreText()
		{
			bool isScoreAdded = false;
			// Add total score if it's enabled.
			if (IsActiveOnResultsView(config.TotalScoreMode))
			{
				isScoreAdded = true;
				fcScoreText.text += GetScoreText();

				// Add the score difference if it's enabled.
				if (config.EnableScorePercentageDifference && scoreManager.HighscoreAtLevelStart > 0)
					fcScoreDiffText.text += GetTotalScoreDiffText();
			}

			// Set prefix label if enabled.
			if (isScoreAdded && (config.EnableLabel == ResultsViewLabelOptions.BothOn || config.EnableLabel == ResultsViewLabelOptions.ScoreOn))
			{
				fcScoreText.text = config.Formatting.ScorePrefixText + fcScoreText.text;
			}
		}

		private string GetTotalPercentageText()
		{
			// Percentage Total Prefix Text can be set in the config file. Default value is "".
			return $"{config.Formatting.PercentageTotalPrefixText}{PercentageToString(scoreManager.PercentageTotal)} ";
		}

		private string GetSplitPercentageText()
		{
			// Percentage Split Saber A/B Prefix Text can be set in the config file. Default value is "".
			return $"{percentageColorTagA}{config.Formatting.PercentageSplitSaberAPrefixText}{PercentageToString(scoreManager.PercentageA)} " +
				   $"{percentageColorTagB}{config.Formatting.PercentageSplitSaberBPrefixText}{PercentageToString(scoreManager.PercentageB)} ";
		}

		private string GetScoreText()
		{
			return ScoreToString(scoreManager.ScoreAtCurrentPercentage);
		}

		private string GetTotalPercentageDiffText()
		{
			// Set total percentage diff text.
			double percentTotalDiff = scoreManager.PercentageTotal - scoreManager.HighscoreAtLevelStartPercentage;
			string percentTotalDiffColorTag = GetColorTagFor(percentTotalDiff);

			return $"{percentTotalDiffColorTag}{PercentageToString(percentTotalDiff)}  ";
		}

		private string GetSplitPercentageDiffText()
		{
			// Set split percentage diff text.
			double percentDiffA = scoreManager.PercentageA - scoreManager.HighscoreAtLevelStartPercentage;
			double percentDiffB = scoreManager.PercentageB - scoreManager.HighscoreAtLevelStartPercentage;
			string percentDiffColorTagA = GetColorTagFor(percentDiffA);
			string percentDiffColorTagB = GetColorTagFor(percentDiffB);

			return $"{percentDiffColorTagA}{config.Formatting.PercentageSplitSaberAPrefixText}{PercentageToString(percentDiffA)}  " +
				   $"{percentDiffColorTagB}{config.Formatting.PercentageSplitSaberBPrefixText}{PercentageToString(percentDiffB)}  ";
		}

		private string GetTotalScoreDiffText()
		{
			// Set score diff text.
			int scoreTotalDiff = scoreManager.ScoreAtCurrentPercentage - scoreManager.HighscoreAtLevelStart;
			string scoreTotalDiffColorTag = GetColorTagFor(scoreTotalDiff);

			return $"{scoreTotalDiffColorTag}{ScoreToString(scoreTotalDiff)}";
		}

		private string GetColorTagFor(double val)
		{
			return val >= 0 ? colorPositiveTag : colorNegativeTag;
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
			return scoreManager.PercentageToString(percent, percentageStringFormat, config.KeepTrailingZeros);
		}
		private string ScoreToString(int score)
		{
			return scoreManager.ScoreToString(score);
		}
	}
}

#nullable enable
using System;
using System.Reflection;
using TMPro;
using Zenject;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using IPA.Utilities;
using FCPercentage.FCPCore;
using FCPercentage.FCPCore.Configuration;
using FCPercentage.FCPResults.Configuration;
using UnityEngine;

namespace FCPercentage.FCPResults
{
	class FCPResultsViewController : IInitializable, IDisposable
	{
		// 2 .bsml files are used since the amount of characters in the score would otherwise change the position of the percentage.
		private static readonly string ResourceNameFCPercentage = "FCPercentage.FCPResults.UI.BSML.ResultsPercentageResult.bsml";
		private static readonly string ResourceNameFCScore = "FCPercentage.FCPResults.UI.BSML.ResultsScoreResult.bsml";

		private static readonly string ResourceNameMissionFCPercentage = "FCPercentage.FCPResults.UI.BSML.MissionResultsPercentageResult.bsml";
		private static readonly string ResourceNameMissionFCScore = "FCPercentage.FCPResults.UI.BSML.MissionResultsScoreResult.bsml";

		// Color tags of score/percentage difference.
		private string colorPositiveTag = "";
		private string colorNegativeTag = "";
		// Color tag for default color.
		//private static string colorDefaultTag = "<color=#FFFFFF>";

		// ResultsView - Text fields in the bsml
		[UIComponent("fcScoreText")]
		private TextMeshProUGUI? fcScoreText = null!;
		[UIComponent("fcScoreDiffText")]
		private TextMeshProUGUI? fcScoreDiffText = null!;
		[UIComponent("fcPercentText")]
		private TextMeshProUGUI? fcPercentText = null!;
		[UIComponent("fcPercentDiffText")]
		private TextMeshProUGUI? fcPercentDiffText = null!;

		// MissionResultsView - Text fields in the bsml
		[UIComponent("missionFcScoreText")]
		private TextMeshProUGUI? missionFcScoreText = null!;
		[UIComponent("missionFcScoreDiffText")]
		private TextMeshProUGUI? missionFcScoreDiffText = null!;
		[UIComponent("missionFcPercentText")]
		private TextMeshProUGUI? missionFcPercentText = null!;
		[UIComponent("missionFcPercentDiffText")]
		private TextMeshProUGUI? missionFcPercentDiffText = null!;


		private readonly ScoreManager scoreManager;
		private ResultsViewController resultsViewController;
		private MissionResultsViewController missionResultsViewController;
		private static FieldAccessor<MissionResultsViewController, MissionCompletionResults>.Accessor MissionCompletionResults = FieldAccessor<MissionResultsViewController, MissionCompletionResults>.GetAccessor("_missionCompletionResults");
		private static FieldAccessor<ResultsViewController, LevelCompletionResults>.Accessor LevelCompletionResults = FieldAccessor<ResultsViewController, LevelCompletionResults>.GetAccessor("_levelCompletionResults");
		private static FieldAccessor<ResultsViewController, TextMeshProUGUI>.Accessor RankText = FieldAccessor<ResultsViewController, TextMeshProUGUI>.GetAccessor("_rankText");
		private static FieldAccessor<ResultsViewController, TextMeshProUGUI>.Accessor ScoreText = FieldAccessor<ResultsViewController, TextMeshProUGUI>.GetAccessor("_scoreText");
		private LevelCompletionResults levelCompletionResults = null!;

		private ResultsSettings config;

		private string percentageColorTagA = "";
		private string percentageColorTagB = "";
		private string percentageStringFormat = "";

		private bool IsFullCombo => levelCompletionResults != null && levelCompletionResults.fullCombo;
		//private bool IsFullCombo => levelCompletionResults != null && (levelCompletionResults.fullCombo && !scoreManager.IsBadCutThresholdBroken);

		public FCPResultsViewController(ScoreManager scoreManager, ResultsViewController resultsViewController, MissionResultsViewController missionResultsViewController)
		{
			this.scoreManager = scoreManager;
			this.resultsViewController = resultsViewController;
			this.missionResultsViewController = missionResultsViewController;

			config = PluginConfig.Instance.ResultsSettings;
		}

		public void Initialize()
		{
			if (resultsViewController != null)
			{
				resultsViewController.didActivateEvent += ResultsViewController_OnActivateEvent;
				missionResultsViewController.didActivateEvent += MissionResultsViewController_didActivateEvent;
			}
		}

		public void Dispose()
		{
			if (resultsViewController != null)
			{
				resultsViewController.didActivateEvent -= ResultsViewController_OnActivateEvent;
				missionResultsViewController.didActivateEvent -= MissionResultsViewController_didActivateEvent;
			}
		}

		private void RefreshPercentageTextFormatting()
		{
			percentageColorTagA = config.SplitPercentageUseSaberColorScheme ? $"<color={scoreManager.SaberAColor}>" : "";
			percentageColorTagB = config.SplitPercentageUseSaberColorScheme ? $"<color={scoreManager.SaberBColor}>" : "";

			colorPositiveTag = config.EnableScorePercentageDifference ? $"<color={config.Advanced.DifferencePositiveColor}>+" : "";
			colorNegativeTag = config.EnableScorePercentageDifference ? $"<color={config.Advanced.DifferenceNegativeColor}>" : "";

			percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision);
		}

		private void ResultsViewController_OnActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			levelCompletionResults = LevelCompletionResults(ref resultsViewController);

			scoreManager.NotifyOfSongEnded(levelCompletionResults.modifiedScore);
			ParseAllBSML();

			Plugin.Log.Notice($"levelCompletionResults.levelEndStateType = {levelCompletionResults.levelEndStateType}");

			if (levelCompletionResults.levelEndStateType == global::LevelCompletionResults.LevelEndStateType.Cleared)
				SetResultsViewText();
			else
				EmptyResultsViewText();
		}

		private void MissionResultsViewController_didActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			MissionCompletionResults missionCompletionResults = MissionCompletionResults(ref missionResultsViewController);
			if (missionCompletionResults == null)
				return;
			levelCompletionResults = missionCompletionResults.levelCompletionResults;

			scoreManager.NotifyOfSongEnded(levelCompletionResults.modifiedScore);
			ParseAllBSML();

			Plugin.Log.Notice($"levelCompletionResults.levelEndStateType = {levelCompletionResults.levelEndStateType}");

			if (levelCompletionResults.levelEndStateType == global::LevelCompletionResults.LevelEndStateType.Cleared)
				SetResultsViewText();
			else
				EmptyResultsViewText();
		}

		private void ParseAllBSML()
		{
			if (fcScoreText == null)
			{
				ParseBSML(ResourceNameFCScore, resultsViewController);

				if (fcScoreDiffText != null)
					fcScoreDiffText.fontSize *= 0.85f;
				else
					Plugin.Log.Error($"Parsing BSML ({ResourceNameFCScore}) has failed.");
			}
			if (fcPercentText == null)
			{
				ParseBSML(ResourceNameFCPercentage, resultsViewController);

				if (fcPercentDiffText != null) 
					fcPercentDiffText.fontSize *= 0.85f;
				else 
					Plugin.Log.Error($"Parsing BSML ({ResourceNameFCPercentage}) has failed.");
			}
			if (missionFcScoreDiffText == null)
			{
				ParseBSML(ResourceNameMissionFCScore, missionResultsViewController);

				if (missionFcScoreDiffText != null)
					missionFcScoreDiffText.fontSize *= 0.85f;
				else
					Plugin.Log.Error($"Parsing BSML ({ResourceNameMissionFCScore}) has failed.");
			}
			if (missionFcPercentText == null)
			{
				ParseBSML(ResourceNameMissionFCPercentage, missionResultsViewController);

				if (missionFcPercentDiffText != null)
					missionFcPercentDiffText.fontSize *= 0.85f;
				else
					Plugin.Log.Error($"Parsing BSML ({ResourceNameMissionFCPercentage}) has failed.");
			}
		}
		private void ParseBSML(string bsmlPath, ResultsViewController resultsViewController) => ParseBSML(bsmlPath, resultsViewController.gameObject);
		private void ParseBSML(string bsmlPath, MissionResultsViewController missionResultsViewController) => ParseBSML(bsmlPath, missionResultsViewController.gameObject);
		private void ParseBSML(string bsmlPath, GameObject parentGameObject)
		{
			BSMLParser.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), bsmlPath), parentGameObject, this);
		}

		private bool IsActiveOnResultsView(ResultsViewModes mode)
		{
			// Checks if the result should be shown
			return mode == ResultsViewModes.On ||
				  (mode == ResultsViewModes.OffWhenFC && !IsFullCombo);
		}

#pragma warning disable CS8602 // Dereference of a possibly null reference.
		private void SetResultsViewText()
		{
			// Empty the text fields so they can be filled with new information
			EmptyResultsViewText();
			RefreshPercentageTextFormatting();

			SetPercentageText();
			SetScoreText();
			ModifyScorePercentageDifference();
		}

		private void ModifyScorePercentageDifference()
		{
			if (config.Advanced.ApplyColorsToScorePercentageModDifference)
			{
				TextMeshProUGUI scoreText = ScoreText(ref resultsViewController);
				TextMeshProUGUI rankText = RankText(ref resultsViewController);
				// Due to a bug in the ScorePercentage mod the percentage difference can be 0% and therefore it's displayed green while the score can
				// still be something like -5 which makes it red. This causes a mismatch. By using the scoreText as reference for both values this
				// issue will be fixed.
				if (scoreText.text.Contains(ResultsAdvancedSettings.DefaultDifferencePositiveColor))
				{
					scoreText.text = scoreText.text.Replace(ResultsAdvancedSettings.DefaultDifferencePositiveColor, config.Advanced.DifferencePositiveColor);
					rankText.text = rankText.text.Replace(ResultsAdvancedSettings.DefaultDifferencePositiveColor, config.Advanced.DifferencePositiveColor);
					rankText.text = rankText.text.Replace(ResultsAdvancedSettings.DefaultDifferenceNegativeColor, config.Advanced.DifferencePositiveColor);
				}
				else if (scoreText.text.Contains(ResultsAdvancedSettings.DefaultDifferenceNegativeColor))
				{
					scoreText.text = scoreText.text.Replace(ResultsAdvancedSettings.DefaultDifferenceNegativeColor, config.Advanced.DifferenceNegativeColor);
					rankText.text = rankText.text.Replace(ResultsAdvancedSettings.DefaultDifferenceNegativeColor, config.Advanced.DifferenceNegativeColor);
					rankText.text = rankText.text.Replace(ResultsAdvancedSettings.DefaultDifferencePositiveColor, config.Advanced.DifferencePositiveColor);
				}
			}
		}

		private void SetPercentageText()
		{
			bool isPercentageAdded = false;
			// Add total percentage if enabled.
			if (IsActiveOnResultsView(config.PercentageTotalMode))
			{
				isPercentageAdded = true;
				string totalPercentText = GetTotalPercentageText();
				fcPercentText.text += totalPercentText;
				missionFcPercentText.text += totalPercentText;

				// Add the total percentage difference if enabled.
				if (config.EnableScorePercentageDifference && scoreManager.Highscore > 0)
				{
					string totalPercentDiffText = GetTotalPercentageDiffText();
					fcPercentDiffText.text += totalPercentDiffText;
					missionFcPercentDiffText.text += totalPercentDiffText;
				}
			}
			// Add split percentage if enabled.
			if (IsActiveOnResultsView(config.PercentageSplitMode))
			{
				isPercentageAdded = true;
				string splitPercentText = GetSplitPercentageText();
				fcPercentText.text += splitPercentText;
				missionFcPercentText.text += splitPercentText;

				// Add the split percentage difference if enabled.
				if (config.EnableScorePercentageDifference && scoreManager.Highscore > 0)
				{
					string splitPercentDiffText = GetSplitPercentageDiffText();
					fcPercentDiffText.text += splitPercentDiffText;
					missionFcPercentDiffText.text += splitPercentDiffText;
				}
			}

			// Set prefix label if enabled.
			if (isPercentageAdded && (config.EnableLabel == ResultsViewLabelOptions.BothOn || config.EnableLabel == ResultsViewLabelOptions.PercentageOn))
			{
				fcPercentText.text = config.Advanced.PercentagePrefixText + fcPercentText.text;
				missionFcPercentText.text = config.Advanced.PercentagePrefixText + missionFcPercentText.text;
				//fcPercentDiffText.text = $"<color=#FFFFFF00>{config.Advanced.PercentagePrefixText}{fcPercentDiffText.text}";
			}

			fcPercentText.text = fcPercentText.text.TrimEnd();
			fcPercentDiffText.text = fcPercentDiffText.text.TrimEnd();
			missionFcPercentDiffText.text = missionFcPercentDiffText.text.TrimEnd();
			missionFcPercentText.text = missionFcPercentText.text.TrimEnd();
		}

		private void SetScoreText()
		{
			bool isScoreAdded = false;
			// Add total score if it's enabled.
			if (IsActiveOnResultsView(config.ScoreTotalMode))
			{
				isScoreAdded = true;
				string scoreText = GetScoreText();
				fcScoreText.text += scoreText;
				missionFcScoreText.text += scoreText;

				// Add the score difference if it's enabled.
				if (config.EnableScorePercentageDifference)
				{
					fcScoreDiffText.text += GetScoreDiffText();
					missionFcScoreDiffText.text += GetMissionScoreDiffText();
				}
			}

			// Set prefix label if enabled.
			if (isScoreAdded && (config.EnableLabel == ResultsViewLabelOptions.BothOn || config.EnableLabel == ResultsViewLabelOptions.ScoreOn))
			{
				fcScoreText.text = config.Advanced.ScorePrefixText + fcScoreText.text;
			}
		}

		private string GetTotalPercentageText()
		{
			// Percentage Total Prefix Text can be set in the config file. Default value is "".
			return $"{config.Advanced.PercentageTotalPrefixText}{PercentageToString(scoreManager.PercentageTotal)} ";
		}

		private string GetSplitPercentageText()
		{
			// Percentage Split Saber A/B Prefix Text can be set in the config file. Default value is "".
			return $"{percentageColorTagA}{config.Advanced.PercentageSplitSaberAPrefixText}{PercentageToString(scoreManager.PercentageA)} " +
				   $"{percentageColorTagB}{config.Advanced.PercentageSplitSaberBPrefixText}{PercentageToString(scoreManager.PercentageB)} ";
		}

		private string GetScoreText()
		{
			return ScoreToString(scoreManager.ScoreAtCurrentPercentage);
		}

		private string GetTotalPercentageDiffText()
		{
			// Create total percentage diff text.
			// scoreTotalDiff is used since for instance a score difference of -2 could give a percent difference of 0.00%. Then the score would be red and the percentage would be green.
			int scoreTotalDiff = scoreManager.ScoreAtCurrentPercentage - scoreManager.Highscore;
			string percentTotalDiffColorTag = GetColorTagFor(scoreTotalDiff);
			double percentTotalDiff = Math.Round(scoreManager.PercentageTotal, config.DecimalPrecision) - scoreManager.HighscorePercentage;

			return $"{percentTotalDiffColorTag}{PercentageToString(percentTotalDiff)}  ";
		}

		private string GetSplitPercentageDiffText()
		{
			// Create split percentage diff text.
			double percentDiffA = Math.Round(scoreManager.PercentageA, config.DecimalPrecision) - scoreManager.HighscorePercentage;
			double percentDiffB = Math.Round(scoreManager.PercentageB, config.DecimalPrecision) - scoreManager.HighscorePercentage;
			string percentDiffColorTagA = GetColorTagFor(percentDiffA);
			string percentDiffColorTagB = GetColorTagFor(percentDiffB);

			return $"{percentDiffColorTagA}{config.Advanced.PercentageSplitSaberAPrefixText}{PercentageToString(percentDiffA)}  " +
				   $"{percentDiffColorTagB}{config.Advanced.PercentageSplitSaberBPrefixText}{PercentageToString(percentDiffB)}  ";
		}

		private string GetScoreDiffText()
		{
			// Create score diff text.
			int scoreDiff = scoreManager.ScoreAtCurrentPercentage - scoreManager.Highscore;
			string scoreTotalDiffColorTag = GetColorTagFor(scoreDiff);

			return $"{scoreTotalDiffColorTag}{ScoreToString(scoreDiff)}";
		}

		private string GetMissionScoreDiffText()
		{
			// Create score diff text.
			int missionScoreDiff = scoreManager.ScoreAtCurrentPercentage - scoreManager.HighscoreAtSongStart;
			string missionScoreDiffColorTag = GetColorTagFor(missionScoreDiff);

			return $"{missionScoreDiffColorTag}{ScoreToString(missionScoreDiff)}";
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

			missionFcScoreText.text = "";
			missionFcScoreDiffText.text = "";
			missionFcPercentText.text = "";
			missionFcPercentDiffText.text = "";
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

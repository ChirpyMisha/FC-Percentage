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
using HMUI;

namespace FCPercentage.FCPResults.HUD
{
	class FCPResultsViewController : ResultsController
	{
		// 2 .bsml files are used since the amount of characters in the score would otherwise change the position of the percentage.
		internal override string ResourceNameFCPercentage => "FCPercentage.FCPResults.HUD.BSML.ResultsPercentageResult.bsml";
		internal override string ResourceNameFCScore => "FCPercentage.FCPResults.HUD.BSML.ResultsScoreResult.bsml";

		//// ResultsView - Text fields in the bsml
		//[UIComponent("fcScoreText")]
		//private TextMeshProUGUI? fcScoreText = null!;
		//[UIComponent("fcScoreDiffText")]
		//private TextMeshProUGUI? fcScoreDiffText = null!;
		//[UIComponent("fcPercentText")]
		//private TextMeshProUGUI? fcPercentText = null!;
		//[UIComponent("fcPercentDiffText")]
		//private TextMeshProUGUI? fcPercentDiffText = null!;

		//// MissionResultsView - Text fields in the bsml
		//[UIComponent("missionFcScoreText")]
		//private TextMeshProUGUI? missionFcScoreText = null!;
		//[UIComponent("missionFcScoreDiffText")]
		//private TextMeshProUGUI? missionFcScoreDiffText = null!;
		//[UIComponent("missionFcPercentText")]
		//private TextMeshProUGUI? missionFcPercentText = null!;
		//[UIComponent("missionFcPercentDiffText")]
		//private TextMeshProUGUI? missionFcPercentDiffText = null!;

		public FCPResultsViewController(ScoreManager scoreManager, ResultsViewController resultsViewController) : base(scoreManager)
		{
			this.resultsViewController = resultsViewController;
		}

		internal override LevelCompletionResults GetLevelCompletionResults()
		{
			ResultsViewController resViewController = (ResultsViewController)resultsViewController;
			return Accessors.LevelCompletionResults(ref resViewController);
		}

		internal new void SetResultsViewText()
		{
			base.SetResultsViewText();
			ModifyScorePercentageDifference();
		}

		private void ModifyScorePercentageDifference()
		{
			if (config.Advanced.ApplyColorsToScorePercentageModDifference)
			{
				ResultsViewController resViewController = (ResultsViewController)resultsViewController;
				TextMeshProUGUI scoreText = Accessors.ScoreText(ref resViewController);
				TextMeshProUGUI rankText = Accessors.RankText(ref resViewController);

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

		internal override GameObject GetViewControllerGameObject()
		{
			return ((ResultsViewController)resultsViewController).gameObject;
		}
	}
}

#nullable enable

using TMPro;
using FCPercentage.FCPCore;
using FCPercentage.FCPCore.Configuration;
using FCPercentage.FCPResults.Configuration;
using UnityEngine;

namespace FCPercentage.FCPResults.HUD
{
	class FCPResultsViewController : ResultsController
	{
		// 2 .bsml files are used since the amount of characters in the score would otherwise change the position of the percentage.
		internal override string ResourceNameFCPercentage => "FCPercentage.FCPResults.HUD.BSML.ResultsPercentageResult.bsml";
		internal override string ResourceNameFCScore => "FCPercentage.FCPResults.HUD.BSML.ResultsScoreResult.bsml";

		internal override ResultsSettings config { get; set; }
		internal override ResultsTextFormattingModel textModel { get; set; }

		public FCPResultsViewController(ScoreManager scoreManager, ResultsViewController resultsViewController) : base(scoreManager, resultsViewController) 
		{
			config = PluginConfig.Instance.ResultsSettings;
			textModel = new ResultsTextFormattingModel(scoreManager, config, GetDiffCalculationModel());
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

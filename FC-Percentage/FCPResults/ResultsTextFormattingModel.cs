using FCPercentage.FCPCore;
using FCPercentage.FCPResults.CalculationModels;
using FCPercentage.FCPResults.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCPercentage.FCPResults
{
	class ResultsTextFormattingModel
	{
		private ScoreManager scoreManager;
		private ResultsSettings config;
		private DiffCalculationModel diffCalcModel;

		// Color tags of score/percentage difference.
		private string colorPositiveTag = "";
		private string colorNegativeTag = "";
		// Color tag for default color.
		//private static string colorDefaultTag = "<color=#FFFFFF>";

		private string percentageColorTagA = "";
		private string percentageColorTagB = "";
		private string percentageStringFormat = "";

		private string GetColorTagFor(double val) => val >= 0 ? colorPositiveTag : colorNegativeTag;

		public ResultsTextFormattingModel(ScoreManager scoreManager, ResultsSettings resultsSettings, DiffCalculationModel diffCalcModel)
		{
			this.scoreManager = scoreManager;
			this.config = resultsSettings;
			this.diffCalcModel = diffCalcModel;
		}

		public void RefreshPercentageTextFormatting()
		{
			percentageColorTagA = config.SplitPercentageUseSaberColorScheme ? $"<color={scoreManager.SaberAColor}>" : "";
			percentageColorTagB = config.SplitPercentageUseSaberColorScheme ? $"<color={scoreManager.SaberBColor}>" : "";

			colorPositiveTag = config.EnableScorePercentageDifference ? $"<color={config.Advanced.DifferencePositiveColor}>+" : "";
			colorNegativeTag = config.EnableScorePercentageDifference ? $"<color={config.Advanced.DifferenceNegativeColor}>" : "";

			percentageStringFormat = scoreManager.CreatePercentageStringFormat(config.DecimalPrecision);
		}

		public bool HasValidDifferenceResult() => diffCalcModel.HasValidResult();

		public string GetTotalPercentageText() => $"{config.Advanced.PercentageTotalPrefixText}{PercentageToString(scoreManager.PercentageTotal)} ";
		public string GetSplitPercentageText() => $"{percentageColorTagA}{config.Advanced.PercentageSplitSaberAPrefixText}{PercentageToString(scoreManager.PercentageA)} " +
												  $"{percentageColorTagB}{config.Advanced.PercentageSplitSaberBPrefixText}{PercentageToString(scoreManager.PercentageB)} ";
		public string GetScoreText() => ScoreToString(scoreManager.ScoreAtCurrentPercentage);


		public string GetTotalPercentageDiffText() => $"{TotalPercentDiffColorTag}{PercentageToString(diffCalcModel.TotalPercentageDiff)}  ";
		public string GetSplitPercentageDiffText() => $"{SplitPercentDiffColorTagA}{config.Advanced.PercentageSplitSaberAPrefixText}{PercentageToString(diffCalcModel.PercentDiffA)}  " +
													  $"{SplitPercentDiffColorTagB}{config.Advanced.PercentageSplitSaberBPrefixText}{PercentageToString(diffCalcModel.PercentDiffB)}  ";
		public string GetScoreDiffText() => $"{TotalScoreDiffColorTag}{ScoreToString(diffCalcModel.TotalScoreDiff)}";


		// TotalScoreDiff is used because (for instance) a score difference of -2 could give a percent difference of 0.00%. Then the score would be negative (red) and the percentage would be positive (green).
		private string TotalPercentDiffColorTag => GetColorTagFor(diffCalcModel.TotalScoreDiff);
		public string SplitPercentDiffColorTagA => GetColorTagFor(diffCalcModel.PercentDiffA);
		public string SplitPercentDiffColorTagB => GetColorTagFor(diffCalcModel.PercentDiffB);
		public string TotalScoreDiffColorTag => GetColorTagFor(diffCalcModel.TotalScoreDiff);



		private string PercentageToString(double percent) => scoreManager.PercentageToString(percent, percentageStringFormat, config.KeepTrailingZeros);
		private string ScoreToString(int score) => scoreManager.ScoreToString(score);
	}
}

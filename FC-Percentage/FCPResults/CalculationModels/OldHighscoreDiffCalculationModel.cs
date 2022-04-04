using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCPercentage.FCPResults.CalculationModels
{
	public class OldHighscoreDiffCalculationModel : DiffCalculationModel
	{
		public OldHighscoreDiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings, LevelCompletionResults levelCompletionResults) :
			base(scoreManager, resultsSettings, levelCompletionResults) { }

		public override double TotalPercentageDiff => TotalPercentage - scoreManager.HighscorePercentageAtLevelStart;
		public override double PercentDiffA => PercentA - scoreManager.HighscorePercentageAtLevelStart;
		public override double PercentDiffB => PercentB - scoreManager.HighscorePercentageAtLevelStart;
		public override int TotalScoreDiff => TotalScore - scoreManager.HighscoreAtLevelStart;

		public override bool HasValidResult() => scoreManager.HighscoreAtLevelStart > 0;
	}
}

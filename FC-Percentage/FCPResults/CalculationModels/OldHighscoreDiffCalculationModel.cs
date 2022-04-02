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
		public OldHighscoreDiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings) : base(scoreManager, resultsSettings) { }

		public override double TotalPercentageDiff => TotalPercentage - scoreManager.HighscorePercentageAtLevelStart;
		public override double PercentDiffA => PercentA - scoreManager.HighscorePercentageAtLevelStart;
		public override double PercentDiffB => PercentB - scoreManager.HighscorePercentageAtLevelStart;
		public override int TotalScoreDiff => TotalScore - scoreManager.HighscoreAtLevelStart;
	}
}

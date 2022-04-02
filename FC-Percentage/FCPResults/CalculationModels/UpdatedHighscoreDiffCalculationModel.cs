using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCPercentage.FCPResults.CalculationModels
{
	public class UpdatedHighscoreDiffCalculationModel : DiffCalculationModel
	{
		public UpdatedHighscoreDiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings) : base(scoreManager, resultsSettings) { }

		public override double TotalPercentageDiff => TotalPercentage - scoreManager.HighscorePercentage;
		public override double PercentDiffA => PercentA - scoreManager.HighscorePercentage;
		public override double PercentDiffB => PercentB - scoreManager.HighscorePercentage;
		public override int TotalScoreDiff => TotalScore - scoreManager.Highscore;
	}
}

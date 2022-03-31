using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCPercentage.FCPResults.CalculationModels
{
	public class CurrentResultDiffCalculationModel : DiffCalculationModel
	{
		public CurrentResultDiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings) : base(scoreManager, resultsSettings) { }
		//{
		//	this.scoreManager = scoreManager;
		//	this.config = resultsSettings;
		//}

		public override double TotalPercentageDiff => Math.Round(scoreManager.PercentageTotal, config.DecimalPrecision) - scoreManager.HighscorePercentage;
		public override double PercentDiffA => Math.Round(scoreManager.PercentageA, config.DecimalPrecision) - scoreManager.HighscorePercentage;
		public override double PercentDiffB => Math.Round(scoreManager.PercentageB, config.DecimalPrecision) - scoreManager.HighscorePercentage;
		public override int TotalScoreDiff => scoreManager.ScoreAtCurrentPercentage - scoreManager.Highscore;

		
	}
}

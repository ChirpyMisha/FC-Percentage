using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCPercentage.FCPResults
{
	class ResultsCalculationModel
	{
		private ScoreManager scoreManager;
		private ResultsSettings config;

		public ResultsCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings)
		{
			this.scoreManager = scoreManager;
			this.config = resultsSettings;
		}

		public double TotalPercentageDiff => Math.Round(scoreManager.PercentageTotal, config.DecimalPrecision) - scoreManager.HighscorePercentage;
		public double PercentDiffA => Math.Round(scoreManager.PercentageA, config.DecimalPrecision) - scoreManager.HighscorePercentage;
		public double PercentDiffB => Math.Round(scoreManager.PercentageB, config.DecimalPrecision) - scoreManager.HighscorePercentage;
		public int TotalScoreDiff => scoreManager.ScoreAtCurrentPercentage - scoreManager.Highscore;
		//public int missionScoreDiff => scoreManager.ScoreAtCurrentPercentage - scoreManager.HighscoreAtSongStart;
	}
}

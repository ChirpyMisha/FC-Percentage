using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCPercentage.FCPResults.CalculationModels
{
	public abstract class DiffCalculationModel
	{
		public ScoreManager scoreManager;
		public ResultsSettings config;

		public DiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings)
		{
			this.scoreManager = scoreManager;
			this.config = resultsSettings;
		}

		public abstract double TotalPercentageDiff { get; }
		public abstract double PercentDiffA { get; }
		public abstract double PercentDiffB { get; }
		public abstract int TotalScoreDiff { get; }
	}
}

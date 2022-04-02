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

		internal double TotalPercentage => RoundPercentage(scoreManager.PercentageTotal);
		internal double PercentA => RoundPercentage(scoreManager.PercentageA);
		internal double PercentB => RoundPercentage(scoreManager.PercentageB);
		internal int TotalScore => scoreManager.ScoreAtCurrentPercentage;

		internal double CalculatePercentage(int val, int maxVal) => RoundPercentage(CalculateRatio(val, maxVal) * 100);
		internal double CalculateRatio(int val, int maxVal) => maxVal != 0 ? ((double)val / (double)maxVal) : 0;
		internal double RoundPercentage(double percentage) => Math.Round(percentage, config.DecimalPrecision);
	}
}

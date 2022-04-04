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
		internal ScoreManager scoreManager;
		internal ResultsSettings config;
		internal LevelCompletionResults levelCompletionResults;

		public DiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings, LevelCompletionResults levelCompletionResults)
		{
			this.scoreManager = scoreManager;
			this.config = resultsSettings;
			this.levelCompletionResults = levelCompletionResults;
		}

		public abstract bool HasValidResult();

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

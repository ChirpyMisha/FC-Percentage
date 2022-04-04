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
		public CurrentResultDiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings, LevelCompletionResults levelCompletionResults) : 
			base(scoreManager, resultsSettings, levelCompletionResults) { }

		public override double TotalPercentageDiff => TotalPercentage - ActualPercentage;
		public override double PercentDiffA => PercentA - ActualPercentage;
		public override double PercentDiffB => PercentB - ActualPercentage;
		public override int TotalScoreDiff => TotalScore - ActualScore;

		private double ActualPercentage => CalculatePercentage(ActualScore, scoreManager.MaxScoreTotal);
		private int ActualScore => levelCompletionResults != null ? levelCompletionResults.modifiedScore : 0;

		public override bool HasValidResult() => ActualScore > 0;
	}
}

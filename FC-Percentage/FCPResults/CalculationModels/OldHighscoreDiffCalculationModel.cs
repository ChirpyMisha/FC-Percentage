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
		//{
		//	this.scoreManager = scoreManager;
		//	this.config = resultsSettings;
		//}

		public override double TotalPercentageDiff => throw new NotImplementedException();

		public override double PercentDiffA => throw new NotImplementedException();

		public override double PercentDiffB => throw new NotImplementedException();

		public override int TotalScoreDiff => throw new NotImplementedException();
	}
}

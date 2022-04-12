using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;

namespace FCPercentage.FCPResults.CalculationModels
{
	public class UpdatedHighscoreDiffCalculationModel : DiffCalculationModel
	{
		public UpdatedHighscoreDiffCalculationModel(ScoreManager scoreManager, ResultsSettings resultsSettings, LevelCompletionResults levelCompletionResults) :
			base(scoreManager, resultsSettings, levelCompletionResults) { }

		public override double TotalPercentageDiff => TotalPercentage - scoreManager.HighscorePercentage;
		public override double PercentDiffA => PercentA - scoreManager.HighscorePercentage;
		public override double PercentDiffB => PercentB - scoreManager.HighscorePercentage;
		public override int TotalScoreDiff => TotalScore - scoreManager.Highscore;

		public override bool HasValidResult() => scoreManager.Highscore > 0;
	}
}

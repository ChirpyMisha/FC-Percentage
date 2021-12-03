using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace FullComboPercentageCounter
{
	class FCPercentageResultsViewHandler : IInitializable, IDisposable
	{
		private readonly ScoreManager scoreManager;
		private readonly ResultsViewController resultsViewController;

		public FCPercentageResultsViewHandler(ScoreManager scoreManager, ResultsViewController resultsViewController)
		{
			this.scoreManager = scoreManager;
			this.resultsViewController = resultsViewController;
		}

		public void Initialize()
		{
			if (resultsViewController != null)
				resultsViewController.didActivateEvent += ResultsViewController_OnActivateEvent;
		}

		public void Dispose()
		{
			if (resultsViewController != null)
				resultsViewController.didActivateEvent -= ResultsViewController_OnActivateEvent;
		}

		private void ResultsViewController_OnActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			double cs = scoreManager.ScoreTotal;
			double csm = scoreManager.MaxScoreTotal;
			Plugin.Log.Notice($"ResultsView Activated - currentScore = {cs}, currentMaxScore = {csm}, percentage = {Math.Round((double)(cs / csm * 100), 2)}");
		}
	}
}

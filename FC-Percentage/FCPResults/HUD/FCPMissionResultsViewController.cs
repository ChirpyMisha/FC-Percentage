using FCPercentage.FCPCore;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FCPercentage.FCPResults.HUD
{
	class FCPMissionResultsViewController : ResultsController
	{
		internal override string ResourceNameFCPercentage => "FCPercentage.FCPResults.HUD.BSML.MissionResultsPercentageResult.bsml";
		internal override string ResourceNameFCScore => "FCPercentage.FCPResults.HUD.BSML.MissionResultsScoreResult.bsml";

		public FCPMissionResultsViewController(ScoreManager scoreManager, MissionResultsViewController resultsViewController) : base(scoreManager)
		{
			this.resultsViewController = resultsViewController;
		}

		internal override LevelCompletionResults GetLevelCompletionResults()
		{
			MissionResultsViewController resViewController = (MissionResultsViewController)resultsViewController;
			MissionCompletionResults missionCompletionResults = Accessors.MissionCompletionResults(ref resViewController);
			if (missionCompletionResults == null)
				return null;
			//levelCompletionResults = missionCompletionResults.levelCompletionResults;
			return missionCompletionResults.levelCompletionResults;
		}

		internal override GameObject GetViewControllerGameObject()
		{
			return ((MissionResultsViewController)resultsViewController).gameObject;
		}
	}
}

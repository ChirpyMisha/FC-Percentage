using IPA.Utilities;
using TMPro;

namespace FCPercentage
{
	internal static class Accessors
	{
		public static FieldAccessor<ResultsViewController, LevelCompletionResults>.Accessor LevelCompletionResults = FieldAccessor<ResultsViewController, LevelCompletionResults>.GetAccessor("_levelCompletionResults");
		public static FieldAccessor<ResultsViewController, TextMeshProUGUI>.Accessor RankText = FieldAccessor<ResultsViewController, TextMeshProUGUI>.GetAccessor("_rankText");
		public static FieldAccessor<ResultsViewController, TextMeshProUGUI>.Accessor ScoreText = FieldAccessor<ResultsViewController, TextMeshProUGUI>.GetAccessor("_scoreText");

		public static FieldAccessor<MissionResultsViewController, MissionCompletionResults>.Accessor MissionCompletionResults = FieldAccessor<MissionResultsViewController, MissionCompletionResults>.GetAccessor("_missionCompletionResults");
	}
}

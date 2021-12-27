namespace FCPercentage.Configuration
{
	class ResultsAdvancedSettings
	{
		// Advanced settings available from config file
		public virtual string ScorePrefixText { get; set; } = DefaultScorePrefixText;
		public virtual string PercentagePrefixText { get; set; } = DefaultPercentagePrefixText;
		public virtual string PercentageTotalPrefixText { get; set; } = DefaultPercentageTotalPrefixText;
		public virtual string PercentageSplitSaberAPrefixText { get; set; } = DefaultPercentageSplitSaberAPrefixText;
		public virtual string PercentageSplitSaberBPrefixText { get; set; } = DefaultPercentageSplitSaberBPrefixText;
		public virtual string DifferencePositiveColor { get; set; } = DefaultDifferencePositiveColor;
		public virtual string DifferenceNegativeColor { get; set; } = DefaultDifferenceNegativeColor;

		public static string DefaultScorePrefixText = "FC : ";
		public static string DefaultPercentagePrefixText = "FC : ";
		public static string DefaultPercentageTotalPrefixText = "";
		public static string DefaultPercentageSplitSaberAPrefixText = "";
		public static string DefaultPercentageSplitSaberBPrefixText = "";
		public static string DefaultDifferencePositiveColor = "#00B300";
		public static string DefaultDifferenceNegativeColor = "#FF0000";
	}
}

namespace FullComboPercentageCounter.Configuration
{
	class CounterSettings
	{
		// Counter Settings (Custom Counters+ Counter)
		public virtual CounterPercentageModes PercentageMode { get; set; } = CounterPercentageModes.Total;
		public virtual CounterLabelOptions EnableLabel { get; set; } = CounterLabelOptions.AsPrefix;
		public virtual int DecimalPrecision { get; set; } = 2;
		public virtual float PercentageSize { get; set; } = 0.85f;
		public virtual bool SplitPercentageUseSaberColorScheme { get; set; } = true;
		public virtual bool KeepTrailingZeros { get; set; } = true;

		public virtual CounterAdvancedSettings Advanced { get; set; } = new CounterAdvancedSettings();
	}

	public enum CounterPercentageModes { Total, Split, TotalAndSplit }
	public enum CounterLabelOptions { AboveCounter, AsPrefix, Off }
}

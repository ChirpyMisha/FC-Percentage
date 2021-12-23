using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullComboPercentageCounter.Configuration
{
	class SettingsFCScorePercentage
	{
		//FCScorePercentage Settings (Results View)
		public virtual ResultsViewModes TotalPercentageMode { get; set; } = ResultsViewModes.OffWhenFC;
		public virtual ResultsViewModes SplitPercentageMode { get; set; } = ResultsViewModes.Off;
		public virtual ResultsViewModes TotalScoreMode { get; set; } = ResultsViewModes.OffWhenFC;
		public virtual ResultsViewLabelOptions EnableLabel { get; set; } = ResultsViewLabelOptions.BothOn;
		public virtual int DecimalPrecision { get; set; } = 2;
		public virtual bool EnableScorePercentageDifference { get; set; } = true;
		public virtual bool SplitPercentageUseSaberColorScheme { get; set; } = true;
		public virtual bool KeepTrailingZeros { get; set; } = false;

		public virtual FormatSettingsFCScorePercentage Formatting { get; set; } = new FormatSettingsFCScorePercentage();
	}

	public enum ResultsViewModes { On, OffWhenFC, Off }
	public enum ResultsViewLabelOptions { BothOn, ScoreOn, PercentageOn, BothOff }
}

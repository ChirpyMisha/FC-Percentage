using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullComboPercentageCounter.Configuration
{
	class FormatSettingsFCPercentageCounter
	{
		// Format settings available from config file
		public virtual float LabelAboveCounterTextOffset { get; set; } = 0.32f;
		public virtual float LabelAboveCounterTextSize { get; set; } = 0.95f;
		public virtual float PercentageTotalAndSplitLineHeight { get; set; } = 55.0f;
		public virtual float PercentageTextOffset { get; set; } = 0.0f;
		public virtual string LabelAboveCounterText { get; set; } = "FC Percent";
		public virtual string LabelPrefixText { get; set; } = "FC : ";
		public virtual string PercentageSplitSaberAPrefixText { get; set; } = "  ";
		public virtual string PercentageSplitSaberBPrefixText { get; set; } = "  ";
	}
}

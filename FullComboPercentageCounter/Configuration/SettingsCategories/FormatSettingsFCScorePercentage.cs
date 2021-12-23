using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullComboPercentageCounter.Configuration
{
	class FormatSettingsFCScorePercentage
	{
		// Format settings available from config file
		public virtual string ScorePrefixText { get; set; } = "FC : ";
		public virtual string PercentagePrefixText { get; set; } = "FC : ";
		public virtual string PercentageTotalPrefixText { get; set; } = "";
		public virtual string PercentageSplitSaberAPrefixText { get; set; } = "";
		public virtual string PercentageSplitSaberBPrefixText { get; set; } = "";
	}
}

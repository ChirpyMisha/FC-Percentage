# FullComboPercentageCounter
 
Full Combo Percentage Counter is a custom counter which is used with Counters+.
It shows your percentage without misses, bad cuts, or any other thing that may lower your combo.
This is the first mod I've made. Any feedback is greatly appreciated!

Available settings:
- Decimal Precision: The precision of the counter from 0 decimals up to 4 decimals.
- Percentage Size: The size of the counter value.
- Enable Label: Enables the label.
- Label Above Count: Put the label above the number, similar to a usual Counters+ counter.
- Ignore Multiplier Ramp-up: When this is enabled all cuts will be weighed the same.
- Show On Results Screen: Select if or when the persentage should be shown on the results screen.

Extra settings in config file:
- Label Text Prefix: The text used for the label if "LabelAboveCount" is false.
- Label Text Above Count: The text used for the label if "LabelAboveCount" is true.
- Label Offset Above Count: The offset of the label when shown above the counter.
- Label Size Above Count: The size of the label when shown above the counter.
- Result Screen Score Prefix: The text used as prefix for the score on the results screen.
- Result Screen Percentage Prefix: The text used as prefix for the percentage on the results screen.

Known bugs:
- Stuck on results screen. 
  - How to reproduce: 
    - Play and finish any song. 
    - Click on the gear icon and going into the game's settings menu or the mod settings menu.
    - Exiting by clicking the "OK" button. (Settings do not need to be changed for this bug to appear).
    - Play and finish any song.
    - The results screen should not function correctly and any buttons are not functional.
  - Bug status: Cause found. Solution found. Fix will be applied in the next update.
- "Ignore Multiplier Ramp-up" setting shows incorrect score on results screen.
  - How to reproduce:
    - Enable the "Ignore Multiplier Ramp-up" setting.
    - Play and finish any song.
    - The score should be around 7000 too high (on maps with more than 15 blocks)
  - Bug status: Cause found. Fix will likely be applied in the next update.

Ideas for Future Features:
- Counter: Add setting to show FC Score
- Counter & Results: Add setting to split percentage for left & right saber.
- Results: Add setting to show difference in score and percentage compared to highscore (similar to the ScorePercentage mod) (Included in upcoming version)
- Remove dependency on Counters+ (Included in upcoming version)
  - Add settings to the mods menu (Included in upcoming version)
- Make extra settings accessible through the mods menu.

Want to contribute?
I'm not the best with UI and BSML. If you want to help the mod to look better, then please contact me.

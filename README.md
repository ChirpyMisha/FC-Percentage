# FC-Percentage

A Beat Saber mod to help you practice your accuracy by showing your percentage as if you had a full combo!

## Main Features

- FC Score/Percentage: Adds your FC Score and Percentage to the results screen.
  - Split the FC percentage for your left and right sabers.
  - Your FC Score, FC Percentage, and FC Split Percentage can be individually enabled, disabled, or only be enabled when your FC is broken.
  - Show the difference between your current score and your high score.
  - Set custom Score/Percentage difference colors. These colors can also be applied to the Score/Percentage difference of the ScorePercentage mod.
  - Prefix text can be enabled or disabled and they can be customised to your liking.
- FC Percentage Counter: Adds a custom counter when Counters+ is installed.
  - Can show your FC percentage, can split the FC percentage for your left and right sabers, or can show both your FC percentage and FC split percentage.
  - Label can be customised to be as prefix, above the counter, or disabled.
  - Change the percentage decimal precision to be as accurate as you want.
  - Individual prefix text for each type of percentage can be customised to your liking.

## Usage

To configure the FC Score/Percentage on the results screen, click on `Settings` (gear icon) → `Mod Settings` → `FCPercentage`.<br>
To configure the FC Percentage Counter, in the panel on the left click on `Counters+` → `Counters` → Scroll to the right → `FCPercentage`.

## Settings
### FC Score/Percentage 
#### Main Settings
| Setting name | Explanation / Info | Default value |
| --- | --- | --- |
| Show FC Percentage | Select if or when the FC percentage should be shown on the results screen.<br>- `Show Always`<br>- `Hide When FC`<br>- `Show Never` | `Hide When FC` |
| Show Split FC Percentage | Select if or when the split FC percentage should be shown on the results screen.<br>- `Show Always`<br>- `Hide When FC`<br>- `Show Never` | `Show Never` |
| Show FC Score | Select if or when the FC score should be shown on the results screen.<br>- `Show Always`<br>- `Hide When FC`<br>- `Show Never` | `Hide When FC` |
| Enable Label | Select which labels should be shown.<br>- `Both Labels`<br>- `Only Score Label`<br>- `Only Percentage Label`<br>- `No Labels` | `Both Labels` |
| Decimal Precision | Select how many decimals the percentage should display. | `2` |
| Enable Score Percentage Difference | Select if you want to show the difference between your high score and the FC score and percentage.<br>If you set a new high score then it will use your newly set high score. | `true` |
| Use Saber Color Scheme | Select if you want to use the saber color scheme when "Show Split FC Percentage" is enabled. | `true` |
| Keep Trailing Zeros | Select if you want the zeros at the end of the decimals to be shown.<br>Example without trailing zeros: 83%<br>Example with trailing zeros: 83.00% | `false` |
| Ignore Multiplier Ramp-up | When this is enabled all cuts will be weighed the same.<br>- Also applies to in-game counter. | `false` |

#### Score/Percentage Difference Colors sub-menu
| Setting name | Explanation / Info | Default value |
| --- | --- | --- |
| Score/Percentage Difference Positive | Select the color to be used when the difference is a positive value. | `#00B300` ![#00B300](https://via.placeholder.com/15/00B300/000000?text=+) |
| Score/Percentage Difference Negative | Select the color to be used when the difference is a negative value. | `#FF0000` ![#FF0000](https://via.placeholder.com/15/FF0000/000000?text=+) |
| Apply Colors To ScorePercentage Mod | If the ScorePercentage mod is installed, select if its difference colors should be replaced with the FC-Percentage difference colors. | `false` |

#### Prefix Strings sub-menu
| Setting name | Explanation / Info | Default value |
| --- | --- | --- |
| Score Prefix | When Enable Label is on for the score, this is used as a prefix. | `"FC : "` |
| Percentage Prefix | When Enable Label is on for the percentage, this is used as a prefix. | `"FC : "` |
| Total Percentage Prefix | When showing the total percentage (Activate with the "Show FC Percentage" setting), this string is used as a prefix for the total percentage. | `""` |
| Split Percentage Prefix Left | When showing the split percentage (Activate with the "Show Split FC Percentage" setting), this string is used as a prefix for the left saber's percentage. | `""` |
| Split Percentage Prefix Right | When showing the split percentage (Activate with the "Show Split FC Percentage" setting), this string is used as a prefix for the right saber's percentage. | `""` |
 
### FC Percentage Counter
#### Main Settings 
| Setting name | Explanation / Info | Default value |
| --- | --- | --- |
| Percentage Mode | Select which percentage(s) should be shown.<br>- `FC Percentage`<br>- `Split FC Percentage`<br>- `Both` | `FC Percentage` |
| Enable Label | Select how the label should be shown.<br>- `Label As Prefix`<br>- `Label Above Counter`<br>- `Label Off` | `Label As Prefix` |
| Decimal Precision | Select how many decimals the percentage should display. | `2` |
| Use Saber Color Scheme | Select if you want to use the saber color scheme for the split percentage. | `true` |
| Keep Trailing Zeros | Select if you want the zeros at the end of the decimals to be shown.<br>Example without trailing zeros: 83%<br>Example with trailing zeros: 83.00% | `true` |
| Ignore Multiplier Ramp-up | When this is enabled all cuts will be weighed the same.<br>- Also applies to in-game counter. | `false` |

#### Advanced Settings
| Setting name | Explanation / Info | Default value |
| --- | --- | --- |
| Counter Offset | Select an offset to be applied to the counter to fine-tune its vertical position. | `0.0f` |
| Label Above Counter Offset | Select an offset to be applied to the label above the counter.<br>This setting is only active when the "Enable Label" setting is set to "Label Above Counter". | `0.32f` |
| Label Above Counter Size | Select the size of the label above the counter.<br>This setting is only active when the "Enable Label" setting is set to "Label Above Counter". | `0.85f` |
| Percentage Size | Select the size of the percentage. | `0.65f` |
| Line Height | Select the line height between the 2 lines when the "Percentage Mode" setting is set to "Both" | `-0.55f` |
| Label Above Counter Text | When the "Enable Label" setting is set to "Label Above Counter", this text will be displayed above the counter. | `"FC Percent"` |
| Label Prefix Text | When the "Enable Label" setting is set to "Label As Prefix", this text will be displayed as prefix. | `"FC : "` |
| Split Percentage Prefix Left | When showing the split percentage (Activate with the "Percentage Mode" setting), this string is used as a prefix for the left saber's percentage. | `""` |
| Split Percentage Prefix Right | When showing the split percentage (Activate with the "Percentage Mode" setting), this string is used as a prefix for the right saber's percentage. | `""` |


### Known bugs:
[No known bugs]

### Ideas for Future Features:
- Counter: Add setting to show FC Score
- Results screen: Change "Enable Score Percentage Difference" setting to choose between the previous high score (like the ScorePercentage mod does) or the current high score (so if a new high score is set then it'll compare to the new score).

### Want to contribute?
I'm not the best with UI and BSML. If you want to help make the mod look better, then please contact me or make a pull request.

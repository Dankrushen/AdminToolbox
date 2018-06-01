# AdminToolbox
This is a plugin for SCL:SL servers. It offers multiple admin tools, that do not exist in the base game.

## Plugin Installation:
To install:
1. Grab newest version of AdminToolbox: (https://github.com/Rnen/AdminToolbox/releases/latest)
2. Navigate to your SCP Secret Lab folder.
3. Drag AdminToolbox.dll into the sm_plugins folder

## ServerMod
ServerMod is a server side plugin system with a bunch of additional configuration options, bug fixes, security patches and some optimisations built in.
 * SMOD can be found here: (https://github.com/Grover-c13/Smod2)
 * SMOD Discord: https://discord.gg/8nvmMTr


## Config Additions
Type Info:
- Boolean: True or False value
- Integer: A number without decimals
- Float: A number with decimals (Formatting like "1.0" and "1,0" both work and are the same value)
- List: A list with items separated by ",", for example: `list: 1,2,3,4,5`
- Dictionary: A dictionary with items separated by ":", and each entry separated by ",", for example: `dictionary: 1:2,2:3,3:4`
- Seconds: Time in seconds, usually a value of -1 disables the feature
- Minutes: Time in minutes, usually a value of -1 disables the feature
- R: If the config option has an R before it, it means that you can use a random value in it. A random value is defined by having "{}", items listed like "weight%value" where if you don't put a weight it defaults to a weight of 1, separated by "|", for example: `rlist: {1%1|2%7|6},3,6,{15%3|2|45%2}`

Crossed out config options are removed, unless otherwise specified in the description

### General Settings
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
admintoolbox_tutorial_dmg_allowed | List | -1 | Wich damagetypes the TUTORIAL Role is allowed to take
admintoolbox_endedRound_damageMultiplier | Integer | 1 | Multiples all damage by this number after round ends. For maximum chaos.
admintoolbox_debug_friendly_kill | Boolean | True | Displays friendly kills in server console.
admintoolbox_debug_player_kill | Boolean | False | Displays all not-friendly kills in server console.
admintoolbox_debug_scp_and_self_killed | Boolean | False | Displays suicides and SCP kills in server console.
admintoolbox_debug_friendly_damage | Boolean | False | Displays friendly damage in server console.
admintoolbox_debug_player_damage | Boolean | False | Displays non-friendly damage in server colsole.
admintoolbox_debug_damagetypes | List | 5, 13, 14, 15, 16, 17 | What damagetypes to detect.


### Administration COMMANDS / Gameplay COMMANDS
Command | Value Type | Value Type | Description
--- | :---: | :---: | ---
PLAYERS |   |   | Lists all players + IP's + SteamID's
GOD | Player | Boolean | Turns on/off godmode for that player. Use without bool to toggle.
NODMG | Player | Boolean | Turns on/off damageOutput for that player. Use without bool to toggle.
HP | Player | Integer | Sets player HP to Integer.
HEAL | Player | Integer | Heals player for (Integer) amount. Use without (Integer) to set to Role default max HP.
TPX | Player1 | Player2 | Teleports player1 to player2. (Will not grab all players with same name like TP)
TUT/TUTORIAL | Player |  | Sets player role to TUTORIAL.
CLASS  | Player | CLASSID | Sets player to ROLE without teleporting them to ROLE SPAWN or giving items.
KEEP/KEEPSETTINGS | Player | Boolean | UNFINISHED, DOES NOTHING ATM; Enables keeping player settings on round restart.


Place any suggestions/problems in issues!

Thanks & Enjoy.

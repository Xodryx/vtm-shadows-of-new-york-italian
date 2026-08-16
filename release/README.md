# Ready-to-install build

Everything in this folder goes into your game directory. Nothing else from the
repository is needed to play.

## Install

1. Install **BepInEx 5.4.x (x64)** into the game folder — the directory that contains
   `VtM Shadows of New York.exe`. Download it from
   <https://github.com/BepInEx/BepInEx/releases> (`BepInEx_x64_5.4.*.zip`) and unpack it
   there.
2. Launch the game once and quit. BepInEx creates its folders on that first run.
3. Create the folder `BepInEx/plugins/SoNY-ITA/` and copy into it:
   - `SoNY.Ita.dll`
   - `it.csv`
   - `ui_it.csv`
4. Launch the game, open the options and pick **Italiano**.

## Notes

- The translation is complete: all 5699 dialogue lines, the dictionary, actor names and
  quests. It has not been proofread in-game yet, so expect the occasional rough edge.
- Any line the plugin cannot resolve falls back to English; nothing breaks and no save
  is affected.
- Updating means replacing `it.csv` and `ui_it.csv` — the plugin reloads them on every
  launch, so a new build of the DLL is only needed when the plugin itself changes.

## Uninstall

Set the game back to **English** in the options before you remove anything. The language
choice is stored as a position in the list, and Italian sits at the end of it; once the
plugin is gone the game clamps that position to the last language left, which is Russian.
Nothing is damaged — picking English in the options puts it right whenever you notice.

Then delete `BepInEx/plugins/SoNY-ITA/` to remove the translation, and additionally
`winhttp.dll`, `doorstop_config.ini`, `.doorstop_version` and the `BepInEx` folder to
remove BepInEx itself.

No game file is modified at any point, so nothing needs restoring. Do not touch
`UnityPlayer.dll` or `WinPixEventRuntime.dll` — they are part of the game.

## Verifying it loaded

`BepInEx/LogOutput.log` should contain:

```
[Info   :Shadows of New York - Italian] Italian added to the language selector.
[Info   :Shadows of New York - Italian] it.csv: <n> translations loaded.
[Info   :Shadows of New York - Italian] Localization.language = 'it[Female]'
```

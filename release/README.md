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
- No game file is modified. To uninstall, delete `winhttp.dll`, `doorstop_config.ini`
  and the `BepInEx` folder.
- Updating means replacing `it.csv` and `ui_it.csv` — the plugin reloads them on every
  launch, so a new build of the DLL is only needed when the plugin itself changes.

## Verifying it loaded

`BepInEx/LogOutput.log` should contain:

```
[Info   :Shadows of New York - Italian] Italian added to the language selector.
[Info   :Shadows of New York - Italian] it.csv: <n> translations loaded.
[Info   :Shadows of New York - Italian] Localization.language = 'it[Female]'
```

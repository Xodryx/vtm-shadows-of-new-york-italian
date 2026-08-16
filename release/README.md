# Ready-to-install build

Everything in this folder goes into your game directory. Nothing else from the
repository is needed to play.

## Finding the game folder

Every step below happens in the **game folder**: the one containing
`VtM Shadows of New York.exe`. On a default Steam install that is

```
C:\Program Files (x86)\Steam\steamapps\common\Vampire The Masquerade - Shadows of New York
```

Steam lets you put libraries on other drives, so yours may instead be something like
`D:\SteamLibrary\steamapps\common\Vampire The Masquerade - Shadows of New York`. Rather
than guessing, let Steam open it for you: right-click the game in your library →
**Manage** → **Browse local files**.

## Install

1. Install **BepInEx 5.4.x (x64)** into the game folder. Download it from
   <https://github.com/BepInEx/BepInEx/releases> — the file is named
   `BepInEx_win_x64_5.4.*.zip` — and unpack it there, so that `winhttp.dll` sits next to
   `VtM Shadows of New York.exe`.
2. Launch the game once and quit. BepInEx creates its folders on that first run.
3. Unpack the release archive into the same folder. It carries the full
   `BepInEx/plugins/SoNY-ITA/` path, so the files land in the right place on their own —
   say yes if Windows asks to merge folders.
4. Launch the game, open the options and pick **Italiano**.

Installed correctly, you should end up with:

```
...\Steam\steamapps\common\Vampire The Masquerade - Shadows of New York\
├─ winhttp.dll                  ← BepInEx
├─ BepInEx/
│  └─ plugins/
│     └─ SoNY-ITA/
│        ├─ SoNY.Ita.dll
│        ├─ it.csv
│        └─ ui_it.csv
└─ VtM Shadows of New York.exe
```

## Notes

- **Made with AI — expect mistakes.** Both the Italian text and the plugin were written
  by an AI assistant (Claude) under human direction. Nobody has yet played the game
  through in Italian to proofread it, so treat this as a working first draft. Glossary
  terms are taken from the official Italian of *Coteries of New York* and checked
  automatically, so those should be right; tone, register and the odd overflowing text
  box are where problems will be. Reports of anything that reads wrong are welcome.
- The translation is complete: all 5699 dialogue lines, the dictionary, actor names and
  quests.
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
`winhttp.dll`, `doorstop_config.ini`, `.doorstop_version`, `changelog.txt` and the
`BepInEx` folder to remove BepInEx itself.

No game file is modified at any point, so nothing needs restoring. Do not touch
`UnityPlayer.dll` or `WinPixEventRuntime.dll` — they are part of the game.

## Verifying it loaded

`BepInEx/LogOutput.log` should contain:

```
[Info   :Shadows of New York - Italian] Italian added to the language selector.
[Info   :Shadows of New York - Italian] it.csv: <n> translations loaded.
[Info   :Shadows of New York - Italian] Localization.language = 'it[Female]'
```

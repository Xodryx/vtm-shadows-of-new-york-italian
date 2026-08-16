# Vampire: The Masquerade — Shadows of New York, in Italian

An unofficial Italian translation of *Shadows of New York*, delivered as a BepInEx
plugin. The game ships English, French, Brazilian Portuguese and Russian; this adds
**Italiano** as a fifth entry in the language selector and injects the translated text
at runtime. **No game file is modified.**

**Complete: 5699 of 5699 dialogue lines**, plus the in-game dictionary, actor names and
quest texts. Every conversation, both endings, every optional branch.

> ### Made with AI — expect mistakes
>
> Both the Italian text and the C# plugin were written by an AI assistant (Claude),
> directed and reviewed line by line by a human, but **no human has yet played the game
> through in Italian to proofread the result.** Treat this as a first draft that runs,
> not as a finished localisation.
>
> What that means in practice:
>
> - **Terminology should be reliable.** Glossary terms are not invented — they are
>   extracted from the official Italian translation of *Coteries of New York* and checked
>   automatically on every build. A rendering that departs from the official one is
>   rejected before it reaches the game.
> - **Tone, register and context are where errors will be.** An automated check can tell
>   that `[27;Fratelli]` is correct; it cannot tell that a line meant to be sarcastic
>   reads as sincere, or that a character is addressed as *tu* in one scene and *lei* in
>   the next.
> - **Text may still overflow its box.** Lines are checked against a length budget, but
>   the budget is an estimate, not the game's actual renderer.
>
> Please [open an issue](../../issues) for anything that reads wrong. Mistakes in a
> translation nobody reports simply stay in it.

## Compatibility

**Windows PC only.** That is the only configuration this has been installed and run on.

The game also exists on Nintendo Switch, PS4 and Xbox One. **The mod cannot work there
at all** — it is a BepInEx plugin, and BepInEx needs to inject itself into the process
before the game starts, which a console will not allow. There is no workaround.

Steam and GOG also sell macOS and Linux builds. BepInEx supports both, and nothing in the
plugin is Windows-specific, so it may well work — but **nobody has tried**, and the
install steps below are written for Windows. Same for the GOG build on Windows: it is
presumably the same game, but this was tested on Steam. If you get it running somewhere
else, [say so in an issue](../../issues) and this section can stop hedging.

## Installing

**[⬇ Download the latest release](../../releases/latest)** — one zip, nothing else from
this repository is needed to play.

Everything below happens in the **game folder**: the one containing
`VtM Shadows of New York.exe`. On a default Steam install that is

```
C:\Program Files (x86)\Steam\steamapps\common\Vampire The Masquerade - Shadows of New York
```

If your Steam library lives elsewhere, let Steam tell you: right-click the game in the
library → **Manage** → **Browse local files**.

1. Install [BepInEx 5.4.x (x64)](https://github.com/BepInEx/BepInEx/releases) into the
   game folder and run the game once so it generates its directories.
2. Unzip the release into the game folder, so the files land in
   `BepInEx/plugins/SoNY-ITA/`.
3. Launch the game and pick **Italiano** in the options.

Any line the plugin cannot resolve falls back to English, so a partial or mismatched
`it.csv` degrades gracefully instead of breaking the game.

## Uninstalling

**Switch the game back to English in the options first.** The chosen language is saved as
an index into the language list, and Italian is appended at the end of it. With the
plugin gone the list is one shorter, and the game clamps the saved index to the last
remaining entry — which is Russian, not English. Nothing is corrupted, but a game that
suddenly speaks Russian looks a lot like a broken install. Picking English again in the
options fixes it at any point.

Then:

- **To remove the translation only** — delete `BepInEx/plugins/SoNY-ITA/`. BepInEx stays,
  along with any other plugin.
- **To remove BepInEx as well** — delete `winhttp.dll`, `doorstop_config.ini`,
  `.doorstop_version`, `changelog.txt` and the `BepInEx` folder.

No game file is ever modified, so there is nothing to restore. Leave `UnityPlayer.dll`
and `WinPixEventRuntime.dll` alone — those belong to the game, not to BepInEx.

## How it works

`TranslationManager.Languages` builds a hand-written list of four languages and omits
`italianLanguage` — a field that already exists on the class, next to a `Language.IT`
enum value and a `GetTextIt` method. A Harmony postfix appends the missing entry; the
game then composes the language tag itself as `it` + `[Female]`, which is exactly the
field name the injection writes to.

See [plugin/README.md](plugin/README.md) for the full technical write-up and the
configuration options.

## Repository layout

| | |
|---|---|
| `plugin/` | C# sources for the BepInEx plugin |
| `blocks/*.json` | the translation itself, `"ConvID/EntryID/Field": "Italian text"` |
| `apply.py` | merges the blocks into `it.csv`, checking glossary markers and overflow |
| `reading_order.py` | recovers reading order from the dialogue graph |
| `match_official_ui.py` | builds `ui_it.csv` for the dictionary, actor names and quests |
| `official_glossary.json` | 60 VtM glossary terms, 141 inflected forms, with their official Italian |
| `ui_manual.json` | hand-written translations for the Shadows-specific UI tables |
| `links.csv` | the dialogue graph, 5940 links |
| `release/` | the installable build — DLL plus the translation CSVs |

The full English script dumps are deliberately **not** in this repository — they are the
games' copyrighted text. Regenerate them from your own copy by launching the game once
with the plugin's dump option enabled.

## Terminology

Italian renderings are extracted from the official translation of *Coteries of New York*
— same studio, same setting — rather than invented. `Kindred` is **Fratelli**, `Kine` is
**vacche**, `Final Death` is **Morte Ultima**, `Scourge` is **Frusta**. `apply.py`
rejects any glossary marker that departs from the official rendering.

Shadows introduces nicknames Coteries never used, and those are translated freely: the
checker only constrains a marker when the English term inside it has a recorded official
rendering. Where Coteries recorded a single gender for a term the game later applies to
the other, the glossary carries an explicit inflection (`lick (m)`, `neonate (f)`) rather
than loosening the check.

## Thanks

This translation only exists because of work other people gave away for free.

**[BepInEx](https://github.com/BepInEx/BepInEx)** (LGPL-2.1) does the hard part. Getting
custom code to run inside a shipped Unity game, before the game itself starts, is the
whole problem — and BepInEx solves it so completely that this project never had to think
about it. Everything here is a plugin sitting on top of that work.

**[HarmonyX](https://github.com/BepInEx/HarmonyX)** (MIT), and
**[Harmony](https://github.com/pardeike/Harmony)** underneath it, is what makes the
translation possible without touching a single game file. Adding Italian to the language
selector is one postfix on one property getter; the original method is left exactly as it
was.

**Draw Distance** wrote a game worth the effort, and shipped it with a `Language.IT` enum
value and a `GetTextIt` method already in place — unused, but there. That accident of an
unfinished feature is why Italian slots in as a real language rather than a hack over
another one.

## Licence

The plugin source is free to reuse. The translated text is a derivative work of
*Vampire: The Masquerade — Shadows of New York* © Draw Distance; it is published here
for use with a legally owned copy of the game and for no other purpose.

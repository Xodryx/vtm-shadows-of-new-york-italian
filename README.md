# Vampire: The Masquerade — Shadows of New York, in Italian

An unofficial Italian translation of *Shadows of New York*, delivered as a BepInEx
plugin. The game ships English, French, Brazilian Portuguese and Russian; this adds
**Italiano** as a fifth entry in the language selector and injects the translated text
at runtime. **No game file is modified.**

Work in progress: **2256 of 5699 dialogue lines**, plus the in-game dictionary, actor
names and quest texts.

## Installing

1. Install [BepInEx 5.4.x (x64)](https://github.com/BepInEx/BepInEx/releases) into the
   game folder and run the game once so it generates its directories.
2. Copy everything from [`release/`](release/) into
   `BepInEx/plugins/SoNY-ITA/`.
3. Launch the game and pick **Italiano** in the options.

Untranslated lines fall back to English, so the mod is safe to use at any stage of
completion.

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
| `official_glossary.json` | 60 VtM glossary terms with their official Italian |
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

## Licence

The plugin source is free to reuse. The translated text is a derivative work of
*Vampire: The Masquerade — Shadows of New York* © Draw Distance; it is published here
for use with a legally owned copy of the game and for no other purpose.

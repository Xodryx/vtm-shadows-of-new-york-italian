# Shadows of New York — Italian translation

A BepInEx mod that adds Italian to *Vampire: The Masquerade – Shadows of New York*.
**It modifies no game file**: the language is added and the text injected at runtime.

## What it does

**Adds "Italiano" to the language selector**, as a fifth entry next to English, Français,
Português and Русский.

The game never excluded Italian: `TranslationManager` already has an `italianLanguage`
field, the `Language` enum already contains `IT`, and a `GetTextIt` method already exists.
All that was missing was the entry in a hand-written list:

```csharp
languages = new List<LanguageSO> { englishLanguage, frenchLanguage,
                                   portugueseLanguage, russianLanguage };
```

The mod appends the missing entry with a Harmony postfix on the `Languages` getter. Once
selected, the game composes the language identifier itself as `languageTag + genderTag` =
`it` + `[Female]` = **`it[Female]`**, which is exactly the field name the mod writes the
translations to.

**Injects the Italian text** into the Pixel Crushers `DialogueDatabase` at startup, reading
it from an external CSV. Untranslated lines stay in English.

## Usage

Configuration lives in `BepInEx/config/sony.ita.cfg`, generated on first launch.

### Extracting the text (`[1. Dump]`)

Writes `dump_en.csv` with every localizable string: `ConvID, EntryID, Field, Conversation,
Actor, Text`. The first three columns are the key and must not be edited; `Conversation`
and `Actor` are there to give the translator context.

It also writes `links.csv`, the dialogue graph. Entry IDs are numbered in the order the
authors created the nodes, not the order they are read in — a line added later inside an
already-written scene gets a high number while sitting near the beginning — so reading
order has to be recovered from the links.

`Fields` selects which fields to extract. The default `en-us,Menu Text en-us` takes English.
Pointing the plugin at **Coteries of New York** and setting `en-us[Female],it[Female]`
extracts that game's official Italian alongside the original instead — see Terminology.

The Google2u tables (dictionary, actor names, quests) are dumped separately to
`dump_ui_en.csv`. They are read through the `TranslationManager` getters at runtime,
because the asset type trees are stripped and cannot be parsed offline.

### Translating and injecting (`[2. Inject]`)

`it.csv` has the same shape as the dump, with the last column translated. The plugin
reloads it on every launch, so the text can be translated in batches and tested in game at
any point.

`TargetField` is the field to write to, `it[Female]` by default. The alternative value
`fr[Female]` hijacks French instead; it exists only as a fallback in case the selector
patch stops working on a future build of the game.

`ui_it.csv` (`Type,textId,text`) covers the Google2u tables. Anything missing from it falls
back to English rather than rendering blank.

### Language selector (`[3. Language]`)

`AddToSelector = false` disables the Harmony patch and leaves the menu untouched.

## Terminology

The text contains markers shaped like `[32;Masquerade]`: these are links into the game's
built-in dictionary. **The numeric ID must be preserved exactly**; only the term inside is
translated, and it has to match the official rendering character for character, straight
apostrophe included (`Torre d'Avorio`, not `Torre d’Avorio`).

The Italian renderings are not invented. They are extracted from the official translation
of *Coteries of New York* — same studio, same setting — giving 60 glossary entries and 139
inflected forms. Guessing leads you astray: `Kindred` is **Fratelli**, `Kine` is **vacche**,
`Final Death` is **Morte Ultima**, `Chantry` is **Cappella**, `Scourge` is **Frusta**.

Before writing, `apply.py` checks that:

- every marker ID present in English is present in Italian too
- the term inside each marker is an official rendering, not a synonym
- the line does not overflow the dialogue box

## Verifying

`BepInEx/LogOutput.log` reports, on every launch, the database that was found, how many
translations were loaded and applied, and `Localization.language` on each language change —
with Italian active it must read `it[Female]`.

## Uninstalling

Delete `winhttp.dll`, `doorstop_config.ini` and the `BepInEx/` folder from the game
directory. No original file was ever modified.

# -*- coding: utf-8 -*-
"""Reuse Coteries' official Italian for the Shadows of New York UI tables.

Both games ship in the same executable, so the Google2u tables of Coteries of New
York - which do have an Italian column - sit right next to the empty Shadows ones.
The row names follow the same convention (DIC_THEBEAST_TITLE), so entries can be
matched on the key rather than on the text.

Titles are pure terminology and are lifted verbatim when the English matches.
Descriptions were rewritten for Shadows in Julia's voice, so they differ and have to
be translated - this only reports them.

Usage:
    python match_official_ui.py            -> report what can be reused
    python match_official_ui.py --write    -> write the reusable half to ui_it.csv
"""
import csv, io, json, os, sys

PLUGIN_DIR = (r"C:\Program Files (x86)\Steam\steamapps\common"
              r"\Vampire The Masquerade - Shadows of New York\BepInEx\plugins\SoNY-ITA")
DUMP = os.path.join(PLUGIN_DIR, "dump_ui_en.csv")
TARGET = os.path.join(PLUGIN_DIR, "ui_it.csv")
HERE = os.path.dirname(os.path.abspath(__file__))
TODO = os.path.join(HERE, "ui_todo.csv")

# Shadows table -> the Coteries table holding the same kind of rows.
COUNTERPART = {
    "DictionaryDatabaseShadows": "DictionaryDatabase",
    "ActorNamesShadows": "ActorNames",
    "QuestsShadows": "Quests",
    "Other": "Other",
}


def load():
    with io.open(DUMP, encoding="utf-8-sig", newline="") as handle:
        return list(csv.DictReader(handle))


def main():
    rows = load()
    by_table = {}
    for row in rows:
        by_table.setdefault(row["Type"], {})[row["Row"]] = row

    # Second pass key: the English text itself, so a row whose name does not line up
    # still picks up the official rendering. Role names ("Manager" -> "Direttore") are
    # translated consistently across the Coteries tables, proper names are left alone.
    by_english = {}
    for row in rows:
        if row["Type"].endswith("Shadows") or not row["IT"].strip():
            continue
        by_english.setdefault(row["EN"].strip(), row["IT"].strip())

    reusable, needs_work, missing = [], [], []

    for shadows_table, coteries_table in COUNTERPART.items():
        source = by_table.get(coteries_table, {})
        for row_name, row in by_table.get(shadows_table, {}).items():
            if row["IT"].strip():
                continue           # already translated by the game
            english = row["EN"].strip()
            if not english:
                continue           # empty row, nothing to translate

            official = source.get(row_name)
            if official is not None and official["IT"].strip():
                if official["EN"].strip() == english:
                    reusable.append((shadows_table, row, official["IT"]))
                else:
                    needs_work.append((shadows_table, row, official["IT"]))
            elif english in by_english:
                reusable.append((shadows_table, row, by_english[english]))
            else:
                missing.append((shadows_table, row))

    print("riutilizzabili tali e quali : %d" % len(reusable))
    print("chiave nota, testo diverso  : %d  (riscritte per Shadows)" % len(needs_work))
    print("nessun corrispettivo        : %d" % len(missing))

    # Hand-written translations for everything Coteries could not supply.
    manual = {}
    manual_path = os.path.join(HERE, "ui_manual.json")
    if os.path.exists(manual_path):
        manual = json.load(io.open(manual_path, encoding="utf-8"))

    final = {"%s/%s" % (table, row["textId"]): italian for table, row, italian in reusable}
    still_missing = []
    for table, row, _ in needs_work:
        still_missing.append((table, row))
    still_missing.extend(missing)

    covered_by_hand = 0
    for table, row in still_missing:
        key = "%s/%s" % (table, row["textId"])
        if key in manual:
            final[key] = manual[key]
            covered_by_hand += 1

    print("tradotte a mano             : %d" % covered_by_hand)
    print("ancora scoperte             : %d" % (len(still_missing) - covered_by_hand))

    if "--write" in sys.argv:
        with io.open(TARGET, "w", encoding="utf-8-sig", newline="") as handle:
            writer = csv.writer(handle, lineterminator="\n")
            writer.writerow(["Type", "textId", "Text"])
            for key in sorted(final, key=lambda k: (k.split("/")[0], int(k.split("/")[1]))):
                table, text_id = key.split("/")
                writer.writerow([table, text_id, final[key]])
        print("\nscritto %s con %d voci" % (TARGET, len(final)))

        with io.open(TODO, "w", encoding="utf-8-sig", newline="") as handle:
            writer = csv.writer(handle, lineterminator="\n")
            writer.writerow(["Type", "textId", "Row", "EN", "CoteriesIT"])
            for table, row in still_missing:
                if "%s/%s" % (table, row["textId"]) in manual:
                    continue
                writer.writerow([table, row["textId"], row["Row"], row["EN"], ""])
        print("scritto %s con le voci ancora da tradurre" % TODO)


if __name__ == "__main__":
    sys.stdout.reconfigure(encoding="utf-8")
    main()

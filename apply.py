# -*- coding: utf-8 -*-
"""Merge translation blocks into it.csv, keeping the ones already applied.

Usage:  python apply.py blocks/block_NNN.json
A block file is {"ConvID/EntryID/Field": "Italian text", ...}
"""
import csv, io, json, os, re, sys

PLUGIN_DIR = (r"C:\Program Files (x86)\Steam\steamapps\common"
              r"\Vampire The Masquerade - Shadows of New York\BepInEx\plugins\SoNY-ITA")
SOURCE = os.path.join(PLUGIN_DIR, "dump_en.csv")
TARGET = os.path.join(PLUGIN_DIR, "it.csv")
GLOSSARY = os.path.join(os.path.dirname(os.path.abspath(__file__)),
                        "official_glossary.json")

MARKER = re.compile(r"\[(\d+);([^\]]+)\]")

# Quotes and punctuation sit inside the marker in the original too
# ([27;"Kindred."]), so only the term itself is compared.
TRIMMED = '“”«»"\'.,'


def load_csv(path, key_columns=(0, 1, 2), text_column=5):
    if not os.path.exists(path):
        return {}
    rows = list(csv.reader(io.open(path, encoding="utf-8-sig", newline="")))
    return {"/".join(r[i] for i in key_columns): r[text_column]
            for r in rows[1:] if len(r) > text_column}


def check(source, translations):
    """Automatic checks before writing: markers, length, phantom rows."""
    glossary = json.load(io.open(GLOSSARY, encoding="utf-8"))
    problems = []
    for key, italian in translations.items():
        english = source.get(key)
        if english is None:
            problems.append(f"{key}: key does not exist in the dump")
            continue

        english_markers = MARKER.findall(english)
        italian_markers = MARKER.findall(italian)
        english_ids = sorted(i for i, _ in english_markers)
        italian_ids = sorted(i for i, _ in italian_markers)
        if english_ids != italian_ids:
            problems.append(f"{key}: glossary markers {english_ids} -> {italian_ids}")

        # Only constrain a marker when the English term inside it has a known
        # official rendering. Shadows uses nicknames Coteries never did
        # ([57;Degenerate] for a Toreador), and those are free to translate:
        # checking the ID alone would reject them.
        italian_by_id = dict(italian_markers)
        for marker_id, english_term in english_markers:
            entries = glossary.get(marker_id, {})
            english_term = english_term.strip(TRIMMED)
            official = entries.get(english_term)
            if official is None or marker_id not in italian_by_id:
                continue
            # Coteries only ever used one gender for some terms. An inflection
            # recorded explicitly as "lick (m)" counts as the same rendering;
            # anything else does not.
            accepted = {official}
            accepted.update(v for k, v in entries.items()
                            if k.startswith(english_term + " ("))
            term = italian_by_id[marker_id]
            if term.strip(TRIMMED) not in {a.strip(TRIMMED) for a in accepted}:
                problems.append(
                    f"{key}: [{marker_id};{term}] is not the official rendering "
                    f"of '{english_term}' ({sorted(accepted)})")

        # A percentage says nothing about short lines ("...Sigh." -> "...Sospiro."
        # is +38% but it is 11 characters). What counts is the risk of overflowing
        # the box: the longest line in the game is 228 characters and wraps to three.
        if len(english) >= 40 and len(italian) > len(english) * 1.35:
            problems.append(
                f"{key}: +{round((len(italian)/len(english)-1)*100)}% longer")
        if len(italian) > 300:
            problems.append(f"{key}: {len(italian)} characters, may overflow the box")
    return problems


def main(block_files):
    source = load_csv(SOURCE)
    existing = load_csv(TARGET)

    incoming = {}
    for path in block_files:
        incoming.update(json.load(io.open(path, encoding="utf-8")))

    problems = check(source, incoming)
    for problem in problems:
        print("  ! " + problem)

    existing.update(incoming)

    rows = list(csv.reader(io.open(SOURCE, encoding="utf-8-sig", newline="")))
    out = [rows[0]]
    for row in rows[1:]:
        key = f"{row[0]}/{row[1]}/{row[2]}"
        if key in existing:
            out.append([row[0], row[1], row[2], row[3], row[4], existing[key]])

    with io.open(TARGET, "w", encoding="utf-8-sig", newline="") as handle:
        csv.writer(handle, lineterminator="\n").writerows(out)

    total = len(rows) - 1
    print(f"\n+{len(incoming)} tradotte in questo blocco")
    print(f"totale: {len(out)-1}/{total} righe ({100*(len(out)-1)//total}%)")
    if problems:
        print(f"{len(problems)} avvisi da controllare")


if __name__ == "__main__":
    sys.stdout.reconfigure(encoding="utf-8")
    main(sys.argv[1:])

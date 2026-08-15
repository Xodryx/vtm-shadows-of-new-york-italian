# -*- coding: utf-8 -*-
"""Build the EN/IT parallel corpus from Coteries and extract the official glossary.

Coteries of New York has an official Italian translation, same studio and same
setting. Dumping it with the plugin configured for 'en-us[Female],it[Female]'
yields aligned pairs, and the dictionary markers in those pairs give the official
rendering of every glossary term - extracted rather than guessed.
"""
import csv, io, os, re, sys, collections

COTERIES_DUMP = (r"C:\Program Files (x86)\Steam\steamapps\common"
                 r"\Vampire The Masquerade - Coteries of New York"
                 r"\BepInEx\plugins\SoNY-ITA\dump_en.csv")
OUTPUT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "parallel_corpus.csv")

ENGLISH_FIELD, ITALIAN_FIELD = "en-us[Female]", "it[Female]"
MARKER = re.compile(r"\[(\d+);([^\]]+)\]")


def read_dump():
    rows = list(csv.reader(io.open(COTERIES_DUMP, encoding="utf-8-sig", newline="")))
    return rows[1:]


def build():
    by_entry = collections.defaultdict(dict)
    for conv, entry, field, title, actor, text in read_dump():
        by_entry[(conv, entry)][field] = text
        by_entry[(conv, entry)]["_context"] = (title, actor)

    pairs = []
    for (conv, entry), fields in by_entry.items():
        english = fields.get(ENGLISH_FIELD, "").strip()
        italian = fields.get(ITALIAN_FIELD, "").strip()
        if english and italian:
            title, actor = fields["_context"]
            pairs.append([conv, entry, title, actor, english, italian])

    pairs.sort(key=lambda r: (int(r[0]), int(r[1])))
    with io.open(OUTPUT, "w", encoding="utf-8-sig", newline="") as handle:
        writer = csv.writer(handle, lineterminator="\n")
        writer.writerow(["ConvID", "EntryID", "Conversation", "Actor", "EN", "IT"])
        writer.writerows(pairs)

    print(f"coppie EN/IT allineate : {len(pairs)}")
    print(f"caratteri inglese      : {sum(len(r[4]) for r in pairs):,}")
    print(f"caratteri italiano     : {sum(len(r[5]) for r in pairs):,}")
    print(f"scritto in             : {OUTPUT}")
    return pairs


def glossary(pairs):
    """For each dictionary ID, align the English term with its official Italian."""
    usage = collections.defaultdict(lambda: collections.defaultdict(collections.Counter))
    for _, _, _, _, english, italian in pairs:
        english_markers = {int(i): t for i, t in MARKER.findall(english)}
        italian_markers = {int(i): t for i, t in MARKER.findall(italian)}
        for marker_id, english_term in english_markers.items():
            if marker_id in italian_markers:
                usage[marker_id][english_term][italian_markers[marker_id]] += 1

    print(f"\nID del dizionario con resa ufficiale: {len(usage)}\n")
    for marker_id in sorted(usage):
        for english_term, renderings in sorted(usage[marker_id].items()):
            best, _ = renderings.most_common(1)[0]
            others = "" if len(renderings) == 1 else "   [varianti: " + \
                ", ".join(f"{t} x{c}" for t, c in renderings.most_common()[1:]) + "]"
            print(f"  {marker_id:>3}  {english_term:<28} -> {best}{others}")
    return usage


if __name__ == "__main__":
    sys.stdout.reconfigure(encoding="utf-8")
    glossary(build())

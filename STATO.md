# Traduzione italiana di Shadows of New York — stato

Aggiornato: 16 agosto 2026

## Fatto

- **Il mod funziona.** "Italiano" compare nel selettore lingue del gioco come quinta voce e
  il testo tradotto appare in gioco. Verificato a schermo.
- **Traduzione completa: 5.699 battute su 5.699 (100%).** Tutte le 38 conversazioni, entrambi
  i finali, tutti i rami di dialogo opzionali.
- **Ordine di lettura risolto.** `links.csv` (il grafo dei dialoghi, estratto dal gioco) esiste,
  e `reading_order.py` lo percorre in profondità dal nodo START. Si traduce nell'ordine in cui il
  giocatore legge.
- **Glossario ufficiale acquisito** da Coteries of New York: 60 voci, 139 forme flesse, più un
  corpus parallelo EN/IT di 11.450 coppie allineate.
- **Dizionario, nomi dei personaggi e quest tradotti**: 210 voci in `ui_it.csv`, di cui 76
  riprese tali e quali dall'italiano ufficiale di Coteries e 134 tradotte.

## Da fare subito

Revisione umana completa: la prima stesura è finita, nessuno l'ha ancora riletta giocando.

Procedura, se serve rimettere mano a un blocco:

    python reading_order.py                      # riepilogo e percentuali
    python reading_order.py 114 --batch 90      # le prossime 90 da fare, in ordine di lettura
    # tradurre in blocks/block_114a.json
    python apply.py blocks/*.json      # fonde, verifica e scrive it.csv nel gioco

Su Windows serve `PYTHONIOENCODING=utf-8` davanti a `python`, altrimenti la console va in
errore sugli accenti.

## Trappole già incontrate

- **EntryID ≠ ordine di lettura.** Sono numerati come i nodi sono stati creati in editor:
  `108/410` sta in posizione 19, dentro il monologo iniziale. Risolto da `reading_order.py`; non
  tradurre mai per EntryID crescente.
- **Le opzioni selezionabili stanno nel campo `Menu Text en-us`**, non `en-us`. Sbagliare campo
  fa sparire la traduzione senza errori; `apply.py` lo intercetta.
- **Nomi, dizionario e quest non passano dal DialogueDatabase** ma dalle tabelle Google2u,
  colonna `_IT` vuota in SoNY. Risolto con un ripiego sull'inglese, non ancora tradotti:
  serve `dump_ui_en.csv` e poi `ui_it.csv`. La tabella `MenuUI` fa eccezione, l'italiano ce
  l'ha già — per questo "Dizionario/Nuovo/Non Letto/Tutto" appaiono tradotti.
- **Dentro i marcatori l'apostrofo va dritto**, non curvo: la resa ufficiale è `Torre d'Avorio`,
  e `Torre d’Avorio` non corrisponde. `apply.py` lo intercetta.

## File

| | |
|---|---|
| `dump_en.csv` | testo sorgente, 5.699 righe |
| `blocks/*.json` | traduzioni per blocco, `"ConvID/EntryID/Campo": "italiano"` |
| `apply.py` | fonde i blocchi in `it.csv` e verifica marcatori, glossario, lunghezza |
| `reading_order.py` | ricostruisce l'ordine di lettura dal grafo; estrae il prossimo blocco |
| `links.csv` | grafo dei dialoghi estratto dal gioco, 5.940 collegamenti |
| `official_glossary.json` | 60 voci del dizionario VtM con le rese ufficiali |
| `parallel_corpus.csv` | 11.450 coppie EN/IT dall'italiano ufficiale di Coteries |
| `build_corpus.py` | ricostruisce corpus e glossario da un dump di Coteries |
| `match_official_ui.py` | genera `ui_it.csv`: riusa l'italiano di Coteries, poi innesta `ui_manual.json` |
| `ui_manual.json` | traduzioni a mano di dizionario, nomi e quest specifici di Shadows |
| `plugin/` | sorgenti C# del mod (solo file compilabili: la csproj compila tutta la cartella) |
| `reference/` | `TranslationManager.decompilato.cs`, utile per capire il gioco |
| `bepinex/` | BepInEx.dll e 0Harmony.dll, riferimenti per la compilazione |

Il codice (C# e Python) è in inglese: il mod andrà in un repo pubblico e su Nexus. Restano
in italiano il contenuto tradotto, l'output a schermo degli script e questo documento.

Il plugin compilato e `it.csv` stanno in
`<gioco>\BepInEx\plugins\SoNY-ITA\`. Ricompilare: `dotnet build -c Release` dentro `plugin/`, poi copiare
`plugin/bin/Release/netstandard2.0/SoNY.Ita.dll` nella cartella del plugin. Verificato che
compila da qui. Per decompilare il gioco serve `ilspycmd` **versione 8.2.0.7535**: le versioni
piu' recenti non si installano (`DotnetToolSettings.xml` mancante nel pacchetto).

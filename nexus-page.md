# Testi della pagina Nexus Mods

Contenuto dei cinque campi del modulo di caricamento, in BBCode.
Aggiornare a ogni versione: la riga della versione sta in **Description**.

Campi fuori da questo file:

| Campo | Valore |
|---|---|
| Mod Name | `Traduzione Italiana - Italian Translation` |
| Version | `0.9.0` |
| Category | Translations (o Miscellaneous) |
| Tag IA | **AI-Generated Content** (non "AI Assisted") |

---

## Description

```bbcode
[b]Shadows of New York interamente in italiano.[/b] Tutte le 5699 battute di dialogo, il dizionario in gioco, i nomi dei personaggi e i testi delle quest.

L'italiano viene aggiunto come [b]quinta lingua nel menu delle opzioni[/b], accanto a inglese, francese, portoghese e russo. Non sostituisce nessuna lingua esistente e [b]non modifica nessun file del gioco[/b]: il testo viene iniettato in memoria all'avvio.

La terminologia non è inventata. È estratta dalla traduzione italiana ufficiale di [i]Coteries of New York[/i] — stesso studio, stessa ambientazione — e verificata automaticamente a ogni build: [i]Kindred[/i] è [b]Fratelli[/b], [i]Final Death[/i] è [b]Morte Ultima[/b], [i]Kine[/i] è [b]vacche[/b].

[b]— Da leggere prima di installare —[/b]

Il testo italiano e il plugin sono stati scritti da un'intelligenza artificiale (Claude) sotto la direzione di una persona, che ha rivisto il lavoro riga per riga. [b]Nessuno ha però ancora giocato il gioco per intero in italiano per rileggerlo.[/b] Consideratela una prima stesura funzionante, non una localizzazione rifinita.

In concreto:

[list]
[*][b]La terminologia dovrebbe essere affidabile[/b] — viene dall'italiano ufficiale ed è verificata a ogni build.
[*][b]Gli errori staranno nel tono e nel registro[/b] — un controllo automatico sa dire che un termine è giusto, non sa dire che una battuta sarcastica suona sincera.
[*][b]Qualche riga può uscire dal riquadro[/b] — la lunghezza è controllata, ma con una stima, non con il vero renderer del gioco.
[/list]

Le segnalazioni sono benvenute e sono l'unico modo in cui questi errori vengono corretti. Se trovate qualcosa che suona male, riportate [b]la frase italiana così come appare a schermo[/b]: bastano poche parole per risalire alla riga esatta tra 5699.
```

## Installation instructions

```bbcode
[b]Solo Windows.[/b] Su Switch, PS4 e Xbox il mod non può funzionare (vedi Requirements).

[b]1.[/b] Installate [url=https://github.com/BepInEx/BepInEx/releases]BepInEx 5.4.x (x64)[/url] nella cartella del gioco — quella che contiene [i]VtM Shadows of New York.exe[/i]. Il file da scaricare si chiama [i]BepInEx_win_x64_5.4.*.zip[/i]. Scompattatelo lì, in modo che [i]winhttp.dll[/i] finisca accanto all'eseguibile.

Su un'installazione Steam predefinita la cartella è:
[code]C:\Program Files (x86)\Steam\steamapps\common\Vampire The Masquerade - Shadows of New York[/code]

Se la vostra libreria Steam è su un altro disco, fate aprire la cartella a Steam: tasto destro sul gioco, [i]Gestisci[/i], [i]Sfoglia i file locali[/i].

[b]2.[/b] Avviate il gioco una volta e chiudetelo. BepInEx crea le sue cartelle a questo primo avvio.

[b]3.[/b] Scompattate l'archivio del mod nella stessa cartella. Contiene già il percorso completo [i]BepInEx/plugins/SoNY-ITA/[/i], quindi i file si sistemano da soli — confermate se Windows chiede di unire le cartelle.

[b]4.[/b] Avviate il gioco, aprite le opzioni e scegliete [b]Italiano[/b].

[b]Verificare che sia partito:[/b] il file [i]BepInEx/LogOutput.log[/i] deve contenere una riga simile a [i]it.csv: 5699 translations loaded.[/i]

[b]— Disinstallazione —[/b]

[b]Rimettete prima l'inglese nelle opzioni del gioco.[/b] La lingua è salvata come posizione nell'elenco e l'italiano sta in fondo; tolto il mod, il gioco riporta quella posizione all'ultima lingua rimasta, che è il russo. Non si rompe niente, ma un gioco che di colpo parla russo sembra un'installazione guasta. Riscegliere l'inglese lo risolve in qualsiasi momento.

Poi cancellate la cartella [i]BepInEx/plugins/SoNY-ITA/[/i]. Per togliere anche BepInEx: [i]winhttp.dll[/i], [i]doorstop_config.ini[/i], [i].doorstop_version[/i], [i]changelog.txt[/i] e la cartella [i]BepInEx[/i].

Nessun file del gioco viene toccato in nessun momento, quindi non c'è niente da ripristinare. Non cancellate [i]UnityPlayer.dll[/i] né [i]WinPixEventRuntime.dll[/i]: sono del gioco.
```

## Main features

```bbcode
[list]
[*][b]Traduzione completa[/b] — 5699 battute su 5699. Tutte le conversazioni, entrambi i finali, ogni ramo di dialogo opzionale.
[*][b]Anche l'interfaccia[/b] — dizionario in gioco, nomi dei personaggi, quest e diario: 210 voci.
[*][b]Italiano come lingua vera[/b] — compare come quinta voce nel selettore, non ne sostituisce un'altra. Il gioco aveva già un valore [i]Language.IT[/i] inutilizzato nel codice: il mod si limita a completarlo.
[*][b]Nessun file del gioco modificato[/b] — il testo è iniettato in memoria all'avvio. Disinstallare significa cancellare una cartella.
[*][b]Terminologia ufficiale[/b] — le rese vengono dalla traduzione italiana ufficiale di Coteries of New York, non inventate, e un controllo automatico rifiuta ogni termine che se ne discosti.
[*][b]Ripiego sicuro[/b] — qualunque riga il plugin non riesca a risolvere resta in inglese, senza errori e senza toccare i salvataggi.
[*][b]Codice aperto[/b] — sorgenti del plugin, traduzioni e strumenti sono su GitHub, verificabili da chiunque.
[/list]
```

## Requirements

```bbcode
[list]
[*][b][url=https://github.com/BepInEx/BepInEx/releases]BepInEx 5.4.x (x64)[/url][/b] — obbligatorio, da installare separatamente. Attenzione alla versione [b]x64[/b], non x86.
[*][b]Windows[/b] — l'unica configurazione su cui il mod è stato provato.
[/list]

[b]Piattaforme non supportate[/b]

[list]
[*][b]Nintendo Switch, PS4, Xbox One: impossibile.[/b] Il mod è un plugin BepInEx, e BepInEx deve caricarsi dentro il gioco prima che parta: una console non lo consente. Non esiste soluzione alternativa.
[*][b]macOS e Linux: non provato.[/b] Il gioco esiste per entrambi e BepInEx li supporta, quindi potrebbe funzionare, ma nessuno l'ha ancora verificato.
[*][b]GOG: non provato[/b], ma presumibilmente identico. Le prove sono state fatte sulla versione Steam.
[/list]

Nessun altro mod è richiesto, e non ci sono incompatibilità note.
```

## Shout outs

```bbcode
Questa traduzione esiste solo grazie a lavoro che altri hanno regalato.

[b][url=https://github.com/BepInEx/BepInEx]BepInEx[/url][/b] fa la parte difficile. Far girare codice dentro un gioco Unity già pubblicato, prima che il gioco stesso parta, è l'intero problema — e BepInEx lo risolve così bene che qui non ci si è mai dovuti pensare.

[b][url=https://github.com/BepInEx/HarmonyX]HarmonyX[/url][/b], e [url=https://github.com/pardeike/Harmony]Harmony[/url] sotto di esso, è ciò che permette di tradurre senza toccare un solo file del gioco.

[b]Draw Distance[/b] ha scritto un gioco che meritava la fatica, e lo ha pubblicato con un valore [i]Language.IT[/i] e un metodo [i]GetTextIt[/i] già presenti nel codice, inutilizzati. È quella funzionalità lasciata a metà a permettere all'italiano di inserirsi come lingua vera invece che come sostituzione di un'altra.
```

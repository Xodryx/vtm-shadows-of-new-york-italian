# Testi della pagina Nexus Mods

Un file per campo del modulo di caricamento. Sono file di testo puro: si apre,
si seleziona tutto, si incolla. Il contenuto è in BBCode, che è il markup di
Nexus — per questo non sono file `.md`, altrimenti l'anteprima Markdown
dell'editor proverebbe a interpretarli e si vedrebbero deformati.

| File | Campo sul modulo |
|---|---|
| `01-description.txt` | Description |
| `02-installation.txt` | Installation instructions |
| `03-features.txt` | Main features |
| `04-requirements.txt` | Requirements |
| `05-shout-outs.txt` | Shout outs |

## Campi da compilare a mano

| Campo | Valore |
|---|---|
| Mod Name | `Traduzione Italiana - Italian Translation` |
| Summary | vedi sotto |
| Version | `0.9.0` |
| Category | Translations, oppure Miscellaneous se non esiste |
| Tag IA | **AI-Generated Content** — non "AI Assisted" |

Summary, 250 caratteri:

    Traduzione italiana completa di Shadows of New York: tutte le 5699 battute, il dizionario, i nomi dei personaggi e le quest. Plugin BepInEx: nessun file del gioco viene modificato. Terminologia ripresa dall'italiano ufficiale di Coteries of New York.

## A ogni nuova versione

Il numero di battute compare in `01-description.txt`, `03-features.txt` e nel
Summary qui sopra. Il numero di versione va aggiornato nel campo Version e nel
tag git.

Il gioco non ha ancora una pagina su Nexus: va richiesta con il pulsante
**ADD NEW GAME** durante il caricamento, indicando il titolo per esteso
`Vampire: The Masquerade - Shadows of New York`. La voce viene approvata solo
se il mod allegato è completo, quindi il caricamento va portato fino in fondo.

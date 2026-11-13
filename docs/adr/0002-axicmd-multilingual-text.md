# 0002. AxiCMD takes every translation from axlanguage

AxiCMD must work in any language an application configures, without a developer adding files per language, so every translated text comes from the product's `axlanguage` table for the User Language, and the original text is shown when a row is missing. Structure, field and report-column captions use the product's own keys (menu row `compname = axpages.name`; `sname = 't'||transid` / `'i'||iname` with the field or column name; `x__headtext` / `x__head` for titles). AxiCMD's own text (Palette Text, option names, prompt hints) uses rows with `sname = 'axicmd'`: Palette Text keyed by a stable dotted key (e.g. `hint.nomatch`), option names and prompt hints keyed by their original text. The English Palette Text stays built into `axicmdmain.js` as the fallback. Text direction follows the shell (`btextDir-rtl` on the body), never a list of language names. Command Words are always typed in English.

## Considered Options

- **The product's numbered `lcm[]` arrays (`Js/lang/content-<lang>.js`)**: rejected; shared product files, index collisions, and only `en`/`ar` exist.
- **Plugin-owned JS files per language**: rejected; a new language would need a developer.
- **Adding AxiCMD rows to `vw_axlanguage_export`**: rejected; the view is product-owned and upgrades would overwrite it.

## Consequences

- The Options → Languages export does not list `axicmd` keys. An English seed script (Postgres and Oracle) is the key template; a Translations tab on the AxiCMD Config page is the planned way to edit them.
- Translations are cached like other data sources (browser 30 minutes, ADS cache 6 hours); after editing rows, clear the ADS cache.

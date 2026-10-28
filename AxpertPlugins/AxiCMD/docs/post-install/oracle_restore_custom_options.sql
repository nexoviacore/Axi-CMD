-- AxiCMD post-install step: restore Custom Options to the palette (Oracle)
--
-- Recovery only. axi_command_tables.sql no longer drops AXI_COMMAND_PROMPTS, so a normal install keeps Custom Options.
-- Run this by hand only if AXI_COMMAND_PROMPTS was dropped or recreated some other way. It needs both AXI_COMMAND_PROMPTS and AXI_COMMAND_CONFIG
-- (with the RECORD_SOURCE column) to exist. Safe to run more than once: names already in a list are skipped.
--
-- Custom Options (IS_SYSTEM = 'F') live in AXI_COMMAND_CONFIG, but the palette reads option names and record sources from
-- AXI_COMMAND_PROMPTS, which used to be recreated on every install. This statement puts every Custom Option name back at the end of the option list of its command (WORDPOS 2) and its RECORD_SOURCE (Axi_Dummy =
-- None when null) at the same position of the paired source list (WORDPOS 3). Names already in the list are skipped, so it is safe to run again.
-- Configure = CMDTOKEN 4, SDK = CMDTOKEN 7. One statement, so both lists change together.
-- The columns hold 4000 characters. LISTAGG ON OVERFLOW TRUNCATE (Oracle 12.2+) marks an aggregate that does not fit instead of raising
-- ORA-01489, and a command whose lists would pass 4000 characters after the append is skipped as a whole (both lists stay as they are,
-- paired); add the missing options from the AxiCMD Config page after removing others.
MERGE INTO AXI_COMMAND_PROMPTS p
USING (
    SELECT a.CMDTOKEN, a.ADD_NAMES, a.ADD_SOURCES
    FROM (
        SELECT k.CMDTOKEN,
               MAX(NVL(LENGTH(n.PROMPTVALUES), 0)) AS CUR_NAMES_LEN,
               MAX(NVL(LENGTH(s.PROMPTSOURCE), 0)) AS CUR_SOURCES_LEN,
               LISTAGG(',' || c.PROMPT_OPTIONS ON OVERFLOW TRUNCATE '~~OVERFLOW~~' WITHOUT COUNT) WITHIN GROUP (ORDER BY c.CONFIG_ID) AS ADD_NAMES,
               LISTAGG(',' || NVL(c.RECORD_SOURCE, 'Axi_Dummy') ON OVERFLOW TRUNCATE '~~OVERFLOW~~' WITHOUT COUNT) WITHIN GROUP (ORDER BY c.CONFIG_ID) AS ADD_SOURCES
        FROM (SELECT 'configure' AS COMMAND, 4 AS CMDTOKEN FROM dual UNION ALL SELECT 'sdk', 7 FROM dual) k
        JOIN AXI_COMMAND_CONFIG c ON LOWER(c.COMMAND) = k.COMMAND AND NVL(c.IS_SYSTEM, 'F') = 'F'
        JOIN AXI_COMMAND_PROMPTS n ON n.CMDTOKEN = k.CMDTOKEN AND n.WORDPOS = 2
        JOIN AXI_COMMAND_PROMPTS s ON s.CMDTOKEN = k.CMDTOKEN AND s.WORDPOS = 3
        WHERE INSTR(',' || LOWER(REPLACE(REPLACE(n.PROMPTVALUES, ', ', ','), ' ,', ',')) || ',', ',' || LOWER(c.PROMPT_OPTIONS) || ',') = 0
        GROUP BY k.CMDTOKEN
    ) a
    WHERE INSTR(a.ADD_NAMES, '~~OVERFLOW~~') = 0 AND INSTR(a.ADD_SOURCES, '~~OVERFLOW~~') = 0
      AND a.CUR_NAMES_LEN + LENGTH(a.ADD_NAMES) <= 4000 AND a.CUR_SOURCES_LEN + LENGTH(a.ADD_SOURCES) <= 4000
) m
ON (p.CMDTOKEN = m.CMDTOKEN AND p.WORDPOS IN (2, 3))
WHEN MATCHED THEN UPDATE SET
    p.PROMPTVALUES = CASE WHEN p.WORDPOS = 2 THEN p.PROMPTVALUES || m.ADD_NAMES ELSE p.PROMPTVALUES END,
    p.PROMPTSOURCE = CASE WHEN p.WORDPOS = 3 THEN p.PROMPTSOURCE || m.ADD_SOURCES ELSE p.PROMPTSOURCE END;

COMMIT;

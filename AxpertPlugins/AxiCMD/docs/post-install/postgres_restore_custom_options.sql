-- AxiCMD post-install step: restore Custom Options to the palette (PostgreSQL)
--
-- Recovery only. axi_command_tables.sql no longer drops AXI_COMMAND_PROMPTS, so a normal install keeps Custom Options.
-- Run this by hand only if AXI_COMMAND_PROMPTS was dropped or recreated some other way. It needs both AXI_COMMAND_PROMPTS and AXI_COMMAND_CONFIG
-- (with the RECORD_SOURCE column) to exist. Safe to run more than once: names already in a list are skipped.
--
-- Custom Options (is_system = 'F') live in axi_command_config, but the palette reads option names and record sources from
-- axi_command_prompts, which used to be recreated on every install. This statement puts every Custom Option name back at the end of the option list of its command (wordpos 2) and its record_source (Axi_Dummy = None
-- when null) at the same position of the paired source list (wordpos 3). Names already in the list are skipped, so it is safe to run again.
-- Configure = cmdtoken 4, SDK = cmdtoken 7. One statement, so both lists change together.
-- The columns hold 4000 characters: a command whose lists would pass that after the append is skipped as a whole (both lists stay as they
-- are, paired) instead of failing, the same as the Oracle script; add the missing options from the AxiCMD Config page after removing others.
UPDATE axi_command_prompts p
SET promptvalues = CASE WHEN p.wordpos = 2 THEN COALESCE(p.promptvalues, '') || m.add_names ELSE p.promptvalues END,
    promptsource = CASE WHEN p.wordpos = 3 THEN COALESCE(p.promptsource, '') || m.add_sources ELSE p.promptsource END
FROM (
    SELECT k.cmdtoken,
           string_agg(',' || c.prompt_options, '' ORDER BY c.config_id) AS add_names,
           string_agg(',' || COALESCE(c.record_source, 'Axi_Dummy'), '' ORDER BY c.config_id) AS add_sources
    FROM (VALUES ('configure', 4), ('sdk', 7)) AS k(command, cmdtoken)
    JOIN axi_command_config c ON LOWER(c.command) = k.command AND COALESCE(c.is_system, 'F') = 'F'
    JOIN axi_command_prompts n ON n.cmdtoken = k.cmdtoken AND n.wordpos = 2
    JOIN axi_command_prompts s ON s.cmdtoken = k.cmdtoken AND s.wordpos = 3
    WHERE strpos(',' || LOWER(REPLACE(REPLACE(COALESCE(n.promptvalues, ''), ', ', ','), ' ,', ',')) || ',', ',' || LOWER(c.prompt_options) || ',') = 0
    GROUP BY k.cmdtoken
    HAVING MAX(LENGTH(COALESCE(n.promptvalues, ''))) + LENGTH(string_agg(',' || c.prompt_options, '' ORDER BY c.config_id)) <= 4000
       AND MAX(LENGTH(COALESCE(s.promptsource, ''))) + LENGTH(string_agg(',' || COALESCE(c.record_source, 'Axi_Dummy'), '' ORDER BY c.config_id)) <= 4000
) m
WHERE p.cmdtoken = m.cmdtoken AND p.wordpos IN (2, 3);

# AxiCMD | Release Notes

## AxiCMD Config page (Custom Options)

Users with Configure Access can now manage the `Configure` and `SDK` command options from the **AxiCMD Config** page (gear button > **Configure**):

*   Switch any option, Built-in or Custom, on or off.
*   Add a **Custom Option** of type url, tstruct, iview or ivtoivload, with an optional record source for the third word.
*   Edit or rename a Custom Option. Built-in Options can only be switched on or off.

Other users see a change after they press **Refresh** in AxiCMD.

---

## ⚠️ Install notes

### Expected errors when re-installing

`axi_command_tables.sql` no longer drops `axi_command_prompts`, because that table now also holds the Custom Option names. On a database that already has AxiCMD, AxInstaller reports these errors for `axi_command_tables.sql`. They are expected and can be ignored:

*   `CREATE TABLE axi_command_prompts`: the table already exists.
*   The `INSERT INTO axi_command_prompts` statements: duplicate key, the rows already exist.

### Recovery only: restoring Custom Options to the palette

No manual step is needed after an install. If `axi_command_prompts` is ever dropped or recreated by hand, Custom Options stay on the AxiCMD Config page but disappear from the palette. To put them back, run the script for your database once, in a SQL client connected to the application schema:

*   PostgreSQL: `AxpertPlugins/AxiCMD/docs/post-install/postgres_restore_custom_options.sql`
*   Oracle: `AxpertPlugins/AxiCMD/docs/post-install/oracle_restore_custom_options.sql` (needs Oracle 12.2 or later; it ends with `COMMIT`)

Running it more than once is safe. Then press **Refresh** in AxiCMD.

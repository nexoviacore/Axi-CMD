# Domain Context & Glossary

## Axi CMD Command Palette

### Commands & Terms

- **SDK Builder Commands**: Administration/developer commands under the `sdk` verb prefix used to launch developer studio builders or form configurations (`sdk tstruct`, `sdk iview`, `sdk page`, `sdk axpert data sources`).
- **"Create New" Option**: Pinned top-level actionable suggestion item presented when invoking SDK builder commands, allowing immediate creation of new structures without selecting existing items.
- **TStruct Builder**: Developer Studio builder interface for transaction structures (`tstreact`).
- **IView Builder**: Developer Studio builder interface for interactive report views (`ivreact`).
- **Page Designer Form**: Configuration interface for system pages (`tstruct.aspx?transid=sect`).
- **ADS Builder Form**: Axpert Data Source SQL configuration interface (`tstruct.aspx?transid=b_sql`).
- **Dynamic Command Configuration**: Database-driven handler and navigation metadata table (`axi_command_config`) that maps command verbs (e.g. `configure`, `sdk`, `upload`, `download`) and prompt options to structure IDs, target URLs, and parameter fields, eliminating hardcoded client-side routing.
- **`AXI-Sec`**: The fixed/sticky DOM container hosting the Axi AI Command Palette input (`#Axi-Searchinp`), action triggers (`#runBtn`, `#axiAddFavoriteBtn`, `#btnRefresh`), and the combined suggestions/favorites mega-dropdown (`#axiMegaDropdown`).
- **`hiddenLoader`**: Background hidden iframe utility used by the Axi engine to asynchronously execute command dispatches and popup requests without interrupting the primary workspace.
- **`mainHomeConfigTemplate`**: The default Axpert application shell template (`aspx/mainHomeConfigTemplate.html`), responsible for rendering the primary layout, navigation frames, application parameters, and the embedded `AXI-Sec` command palette.
- **Large Dataset Lazy Autocomplete**: The mechanism in AxiCMD to handle Tstruct record suggestions for tables exceeding 50,000+ records by loading an initial page of 100 records and dynamically querying server-side matching records when typing 3 or more characters with debounce.
- **`axi_getstructsdata`**: The Axpert Data Source (ADS) executing `fn_axi_getstructs_obj` to retrieve primary key field records, field names, and captions for target Tstructs.
- **Dynamic On-Demand Token Resolution**: The fallback mechanism ensuring typed or pasted record identifiers not present in the locally cached initial page are queried and resolved directly from the server before command execution.
- **`fn_axi_getstructs_obj` Parameter Extension**: The 13-parameter database function accepting optional `psearchterm`, `ppageno`, and `ppagesize` defaults, enabling SQL-level pagination (`LIMIT / OFFSET`) and multi-column keyword filtering (`LIKE %term%`) across primary key, caption, and record ID.
- **Dynamic Search Debounce**: A 300ms–350ms delay timer buffering server-side ADS requests during continuous keystrokes to prevent excessive backend queries.
- **Search In-Memory Cache**: The in-session JavaScript cache (`axDatasourceObj`) storing dynamic keyword results by term key (`_q_<term>`), invalidated upon explicit refresh (`#btnRefresh`) or record save/update actions.

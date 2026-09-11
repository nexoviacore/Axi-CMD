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

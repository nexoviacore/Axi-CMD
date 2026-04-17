# Axi Command Palette | Installation & Configuration Guide

Follow these steps to deploy the Axi plugin and bridge the backend API with the Axpert web interface.

---

## 🛠 Prerequisites
* **Server Runtime:** [.NET 8.0 Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/8.0) (Required for IIS hosting).
* **Access Level:** Administrator privileges for IIS Manager and File System modifications.
* **Tooling:** **AxInstaller** (Latest Version).

---

## 📂 Step 1: Core Plugin Deployment
1. Launch **AxInstaller**.
2. Select and install the **Axi** package. 
3. **Verification:** Ensure the source files are populated in your `/AxpertPlugins/Axi/` directory.

---

## 📄 Step 2: UI Template Integration
Register the Axi frontend template within the Axpert ecosystem.

1. **Source:** `../AxpertPlugins/Axi/HTMLPages/axi_mainpagetemplate_V2.html`
2. **Destination:** Copy to `../CustomPages/`
3. **Note:** Do not rename the file; Axpert metadata relies on the exact filename.

---

## ⚙️ Step 3: Axpert Environment Mapping
Configure the schema to utilize the new UI template.

1. Log in to **AxpertWeb** -> Navigate to **Dev Options**.
2. Locate **Application Template** and select `axi_mainpagetemplate_v2.html` from the Property value dropdown.
3. **Manual Override:** If the file does not appear:
   * Navigate to **Configuration Property List**.
   * Edit the **Application Template** property.
   * Manually append `axi_mainpagetemplate_V2.html` to the **Values** collection.

---

## 🌐 Step 4: IIS Backend Configuration (AxiApi)
Host the API service as a high-performance .NET 8 application.

1. **Application Pool:** * Name: `AxiApi`
   * .NET CLR Version: **No Managed Code**
2. **Site Creation:** Create a new site/application pointing to:
   `../AxpertPlugins/Axi/PluginScripts/AxiApi/Release/net8.0/publish`
3. **Dependency Injection:** * Copy `appsettings.ini` from `../AxpertWebScript/`.
   * Paste it into the folder: `../AxiApi/Release/net8.0/`.
4. **Permissions:** Ensure the App Pool Identity has **Full Control** over the Publish folder.

---

## 🔗 Step 5: Endpoint Connectivity
Update the configuration to bridge the UI and the API.

1. Open `axiConfig.json` (located in the Axi plugin directory).
2. Configure the endpoints to match your IIS binding:

```json
{
    "API_METADATA": "https://<Your_IIS_URL>/api/v1/Axi/axi_get",
    "AXI_FAVORITES_URL": "https://<Your_IIS_URL>/api/v1/Axi/user-favourites"
}
```
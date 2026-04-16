# Axi Command Palette | Installation & Configuration Guide

Follow these steps to deploy the Axi plugin and configure the backend API.

---

## 🛠 Prerequisites
* **Runtime:** .NET 8.0 Hosting Bundle installed on the server.
* **Permissions:** Administrative access to IIS and the Axpert Web directory.
* **Tooling:** Latest version of **AxInstaller**.

---

## 📂 Step 1: Plugin Installation
Execute the **AxInstaller** and select the latest **Axi Plugin** package. This will deploy the necessary source files and scripts to your `/AxpertPlugins/` directory.

---

## 📄 Step 2: UI Template Deployment
Move the Axi frontend template to the custom pages directory to ensure it is recognized by the Axpert engine.

1. Locate the source file:  
   `../AxpertPlugins/Axi/HTMLPages/axi_mainpagetemplate_V2.html`
2. Copy and paste it into:  
   `../CustomPages/`

---

## ⚙️ Step 3: Axpert Web Configuration
Map the application template within your schema settings.

1. Open **AxpertWeb** in your browser and log in to the required schema.
2. Navigate to **Dev Options** > **Application Template**.
3. Select `axi_mainpagetemplate_v2.html` from the property value dropdown list.
4. **Troubleshooting:** If the file is missing from the dropdown:
   * Go to **Configuration Property List**.
   * Open the **Application Template** property.
   * Add `axi_mainpagetemplate_V2.html` to the **Values** section manually.

---

## 🌐 Step 4: IIS API Setup (AxiApi)
Host the .NET 8 backend service in IIS.

1. **Application Pool:** Create a new pool named `AxiApi`. Set the .NET CLR version to **"No Managed Code"**.
2. **Site/Application:** Create a new site (or sub-application) and point the physical path to:  
   `../AxpertPlugins/Axi/PluginScripts/AxiApi/Release/net8.0/publish`
3. Ensure the identity assigned to the App Pool has **Read/Write permissions** to this folder.

---

## 🔗 Step 5: Endpoint Configuration
Link the frontend to the backend by updating the configuration JSON.

1. Open `axiConfig.json`.
2. Update the `API_METADATA` and `AXI_FAVORITES_URL` with your specific **AxiApi** URL:

```json
{
    "API_METADATA": "https://<YourAxiApiUrl>/api/v1/Axi/axi_get",
    "AXI_FAVORITES_URL": "https://<YourAxiApiUrl>/api/v1/Axi/user-favourites"
}
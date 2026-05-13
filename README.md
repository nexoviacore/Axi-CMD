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

1. **Source:** `../AxpertPlugins/Axi/HTMLPages/AxiCMDMainPage.html`
2. **Destination:** Copy to `../CustomPages/`
3. **Note:** Do not rename the file; Axpert metadata relies on the exact filename.

---

## ⚙️ Step 3: Axpert Environment Mapping
Configure the schema to utilize the new UI template.

1. Log in to **AxpertWeb** -> Navigate to **Dev Options**.
2. Locate **Application Template** and select `AxiCMDMainPage.html` from the Property value dropdown.
3. **Manual Override:** If the file does not appear:
   * Navigate to **Configuration Property List**.
   * Edit the **Application Template** property.
   * Manually append `AxiCMDMainPage.html` to the **Values** collection.

---

## 🌐 Step 4: IIS Backend Configuration (AxiApi)
Host the API service as a high-performance .NET 8 application.

1. **Application Pool:** * Name: `AxiApi`
   * .NET CLR Version: **No Managed Code**
2. Navigate to AxiApi in PluginScripts folder and copy and paste AxiApi into Arm Microservices folder

---



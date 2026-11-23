# Axi Commandline Release Notes – 23 Nov 2026

## Bug Fixes

### 1. Data source opens as a blank screen in Pop-Up mode

**Status:** Fixed (needs the product patch release)

When a data source was opened in normal mode and then opened in Pop-Up mode, a blank screen was displayed in Pop-Up mode.

### 2. Data sources are not opening

**Status:** Fixed (needs the product patch release)

Data sources showed a blank screen.

#### Instructions to QA

- Fixes 1 and 2 need the file `AxpertHtmlAuthModule.cs`, which will be released by Malakonda Sir in Product Patch Release.

### 3. Commands that start with "Error" did not work in AxiCMD

**Status:** Fixed

### 4. Configure "Publish Config Studio" listed records differently in Pop-Up mode

**Status:** Fixed

In normal mode, Configure "Publish Config Studio" does not list the records until the Server parameter is selected. In Pop-Up mode, the same command displayed the records without the Server parameter. Pop-Up mode now also asks for the Server parameter first.

## Changes

### 5. Remove the With option from the Edit command

**Status:** Done

The `with` option is no longer available for the Edit command.

### 6. Use the configured PAT for authentication during installation

**Status:** Done

The Install command now handles these keys when it pushes to the queue:

- `ARMSessionId`
- `gitaccountname`
- `gitrepository`
- `gittoken`
- `type`

Sample install payload in the queue message:

```json
{
  "action": "install",
  "requestId": "06e2742d-2b78-40d9-bf90-72bcac7c4288",
  "ARMSessionId": "ARM-xxxxxxx",
  "type": "productversion",
  "gitaccountname": "Agileaxpert",
  "gitrepository": "AxiProductVersions",
  "gittoken": "axcsdfsfsgsgr",
  "managerType": "productversion",
  "packageName": "Axi Masters 01",
  "packageVersion": "Latest",
  "commitSha": "e64bea48664bd0ed4654c8bf686befcd0bcbdb64",
  "appName": "ORGNEWXXM",
  "requestedBy": "admin",
  "requestedOn": "2026-11-16T13:05:16.715Z"
}
```

### 7. Change the terminology from Product to ProductVersion in the Install command

**Status:** Done

The Install command now uses ProductVersion instead of Product:

```
Install ProductVersion <Package> <Version>
```

### 8. Remove the separate Git Account, Repository and PAT configuration

**Status:** Done

Git Account, Repository and PAT can no longer be configured separately through the Axi Command Line. These commands have been removed:

- `Configure Product`
- `Configure Plugin`
- `Configure "Change Request"`

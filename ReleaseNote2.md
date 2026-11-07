Release Notes — 06/11/2026
QA Instructions
After installing AxiCMD v1.0.0, a new Dev option named Axi Command Line Enable will be available.
Use this Dev option to enable or disable the Axi Command Line functionality during testing.
Enhancements
Renamed Axi_Beta to AxiCMD.
Renamed AxiApi_Beta to AxiCMDApi.
Moved the Axi CMD HTML implementation to Product Mainpage template MainHomeConfigTemplate.html. And removed the existing Axi command line template (AxiCMDMainPage.html).
Command Line Enablement: A command-line flag, axicmdEnabled, has been added to the MainNew page.
Based on the value of axicmdEnabled, the Command Line
should be show or hide  in the Product Main Page template.
Developer Option:
Property: Axi Command Line Enable
Property Code: General
Property value: true / false ( by default true)
Form/Report :
User Role: All / Single role
Note to QA: Please make sure AxiCMDMainPage.html file should not exist in AxpertPlugins/AxiCMD folder in git and in server (AxpertWeb/AxpertPlugins/AxiCMD/HTMLPages/ and AxpertWeb/CustomPages/)
Bug Fixes
TKT-1301 — ERPDEMO Command Line Issues
Customer: Agile Labs Pvt Ltd
Partner: AXPERTAGILE

TKT-1212 — Command Line Getting Stuck in erplive
Resolved an issue where the command line became unresponsive and did not allow further user input.
Customer: Axi ERP

TKT-1214 — Axi Command Line Input Field Becomes Unresponsive After Pressing Backspace
Resolved an issue where the command line input field lost focus and became unresponsive after pressing the Backspace key.
Customer: Agile Labs Pvt Ltd
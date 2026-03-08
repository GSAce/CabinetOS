# 🎮 CabinetOS — A Modular Arcade Operating Layer

CabinetOS is a modular, high‑performance operating layer designed for custom arcade cabinets and hybrid gaming setups. It combines a polished WPF shell, a powerful backend for ROM management, and a fully customizable frontend experience — all engineered for reliability, scalability, and long‑term maintainability.

At its core, CabinetOS is built to feel like a real arcade operating system: fast, intentional, visually cohesive, and capable of handling both modern PC titles and classic emulation with equal elegance.

---

# 📑 Table of Contents

- [Part 1 — Project Overview](#-part-1--project-overview)
- [Part 2 — Folder & Solution Structure](#-part-2--folder--solution-structure)
- [Part 3 — Core Features](#-part-3--core-features)
- [Part 4 — Build & Run Instructions](#-part-4--build--run-instructions)
- [Part 5 — Configuration & Customization](#-part-5--configuration--customization)
- [Part 6 — Roadmap & Contribution Guidelines](#-part-6--roadmap--contribution-guidelines)
- [Part 7 — Licensing & Attribution](#-part-7--licensing--attribution)

---

# 📘 Part 1 — Project Overview

## Core Philosophy
- Modularity first  
- Deterministic behavior  
- Scalable architecture  
- Theme‑driven UI  
- Legal and ethical asset usage  
- Real‑world practicality  

## High‑Level Architecture
- CabinetOS.Shell — WPF UI  
- CabinetOS.Core — backend logic  
- CabinetOS.SetBuilder — ROM set builder  
- CabinetOS.Playlists — playlist + metadata generators  

## Project Goals
- Console‑like experience  
- Robust backend  
- Developer‑friendly architecture  
- Future‑proof foundation  

---

# 📁 Part 2 — Folder & Solution Structure

CabinetOS/
  CabinetOS.Shell/
    Controls/
    Screens/
    Themes/
    Assets/
    Services/
    ViewModels/
    App.xaml

  CabinetOS.Core/
    Platforms/
    RomIndex/
    Devices/
    Logging/
    Configuration/
    Utilities/

  CabinetOS.SetBuilder/
    DatParsing/
    Builders/
    Models/
    Services/

  CabinetOS.Playlists/
    ES/
    RetroArch/
    Models/

  CabinetOS.Tests/
  docs/
    pipeline.md
    architecture.md
    assets/

## Project Responsibilities
- Shell: UI, navigation, themes  
- Core: platforms, ROM index, devices, config  
- SetBuilder: DAT ingestion, ZIP/CHD processing, logs  
- Playlists: ES and RetroArch generators  

---

# ⚙️ Part 3 — Core Features

## Shell Features
- SystemBar  
- Sidebar navigation  
- Theme engine  
- Avatar system  
- MVVM screen framework  

## Core Features
- JSON platform definitions  
- ROM indexing + metadata  
- Device detection (PC, wired Pi, Wi‑Fi Pi)  
- Config system  
- Logging framework  

## SetBuilder Features
- DAT ingestion  
- ZIP + CHD processing  
- Salvage mode  
- Parallel builds  
- Progress + summaries  

## Playlist Features
- ES gamelist.xml  
- RetroArch .lpl  
- Future playlist folder integration  

---

# 🛠️ Part 4 — Build & Run Instructions

## System Requirements
Minimum:
- Windows 10/11  
- .NET 8 SDK  
- Visual Studio 2022  

Recommended:
- Windows 11  
- GPU for WPF  
- SSD for SetBuilder  

## Cloning
git clone https://github.com/YourUser/CabinetOS.git  
cd CabinetOS

## Dependencies
dotnet restore

## Building
Visual Studio: open solution → Build  
CLI: dotnet build CabinetOS.sln -c Release

## Running the Shell
dotnet run --project CabinetOS.Shell

## Running SetBuilder
dotnet run --project CabinetOS.SetBuilder

## Config Files
CabinetOS.Core/Configuration/

Includes:
- platforms.json  
- settings.json  
- devices.json  
- paths.json  

## Themes
CabinetOS.Shell/Themes/

## Cleaning
dotnet clean  
or delete /bin and /obj

## Troubleshooting
- Install .NET 8  
- Restart XAML designer  
- Ensure icons are Resources  
- Validate CHDs and paths  

---

# 🧩 Part 5 — Configuration & Customization

## Global Settings (settings.json)
Controls:
- Logging  
- Default platform  
- Playlist behavior  

Example:
{
  "DefaultPlatform": "mame",
  "EnableLogging": true
}

## Platform Definitions (platforms.json)
Defines:
- Name  
- Shortname  
- Extensions  
- DAT version  
- Playlist rules  

Example:
{
  "Name": "MAME",
  "ShortName": "mame",
  "RomExtensions": [ ".zip" ]
}

## Device Detection (devices.json)
Example:
{
  "PreferredMode": "Auto",
  "WifiSSID": "CabinetOS"
}

## Paths (paths.json)
Example:
{
  "RomSource": "D:/ROMs/MAME",
  "Output": "E:/CabinetOS/Builds"
}

## Themes
Themes/
  Default/
  NeonBlue/

Customizable:
- Colors  
- Icons  
- Typography  
- Layout  

## Icons
CabinetOS.Shell/Assets/Icons/

Rules:
- Must be licensed  
- Attribution required  
- SVG preferred  

## SetBuilder Customization
- Parallelism  
- Salvage mode  
- CHD hashing  
- Output structure  

---

# 🗺️ Part 6 — Roadmap & Contribution Guidelines

## Shell Roadmap
- Platform browser  
- Game list  
- Settings  
- Theme packs  
- Animated transitions  
- Dynamic marquees  

## Core Roadmap
- Expanded metadata  
- Faster indexing  
- Cloud sync  
- Multi-device profiles  

## SetBuilder Roadmap
- Build resume  
- Incremental builds  
- Auto DAT updates  
- Build caching  

## Playlist Roadmap
- Playlist folders  
- More metadata  
- LaunchBox support  

## Contribution Guidelines
- Follow folder + namespace conventions  
- One class per file  
- No circular dependencies  
- Deterministic logic  
- Clear commit messages  
- Update attribution.md for assets  

---

# 📜 Part 7 — Licensing & Attribution

## Licensing Philosophy
- Only legally licensed assets  
- Attribution required where applicable  
- No ROMs, BIOS, or proprietary files  

## Asset Types
Icons:
- Must allow redistribution + modification  
- SVG preferred  

Fonts:
- OFL preferred  
- Must allow embedding  

Artwork:
- Must allow modification + redistribution  

## Attribution File
docs/attribution.md

Example entry:
Icon: Wi-Fi Signal  
Author: Jane Doe  
License: CC-BY 4.0  
Source: https://example.com  
Modifications: Adjusted stroke width  

## Prohibited Content
- ROMs  
- BIOS files  
- Commercial fonts without rights  
- Assets with unclear licenses  

## Preferred Licenses
- CC0  
- CC-BY  
- MIT  
- OFL  

---

# ✔️ README Complete

CabinetOS now has a complete, polished, GitHub‑ready README designed for long‑term maintainability, contributor clarity, and professional presentation.
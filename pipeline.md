📘 ROM Intake & System Architecture — Master Pipeline Checklist
A complete, itemized reference of every subsystem in your ROM ecosystem.
Use this page to track progress, plan development, and ensure every component integrates cleanly.

PHASE 1 — Incoming Folder Intake
• 	[ ] Incoming folder watcher / manual trigger
• 	[ ] Preserve ES‑style folder structure
• 	[ ] Initial file scan + indexing

PHASE 2 — System Detection (Folder‑First)
• 	[ ] Folder‑name system detection
• 	[ ] Arcade folder detection (, , etc.)
• 	[ ] File signature detection (fallback)
• 	[ ] Extension‑based detection (fallback)
• 	[ ] Unknown system handler

PHASE 3 — Arcade Path (DAT‑Driven)
• 	[ ] DAT loader (MAME + FBNeo)
• 	[ ] Arcade ROM → DAT matching
• 	[ ] Canonical naming
• 	[ ] ZIP structure normalization
• 	[ ] CHD folder normalization
• 	[ ] BIOS/device normalization
• 	[ ] Parent/clone resolution
• 	[ ] Salvage detection
• 	[ ] Arcade duplicate detection
• 	[ ] Arcade missing ROM detection
• 	[ ] Exclude arcade from console normalization

PHASE 4 — Console/Handheld Normalization (Rule‑Driven)
• 	[ ] Name normalization
• 	[ ] Region normalization
• 	[ ] Scene tag stripping
• 	[ ] Multi‑disc normalization
• 	[ ] Multi‑track CD grouping
• 	[ ] Format normalization (ZIP where allowed)
• 	[ ] Console duplicate detection
• 	[ ] Corrupt file detection

PHASE 5 — BIOS Detection (Non‑Arcade Systems)
• 	[ ] BIOS rules file ()
• 	[ ] BIOS scanner
• 	[ ] BIOS alias matching
• 	[ ] BIOS validation (size/hash/region)
• 	[ ] BIOS normalization
• 	[ ] Move BIOS to dedicated folders
• 	[ ] BIOS logging
• 	[ ] BIOS summary reporting

PHASE 6 — Sorting & Output
• 	[ ] Move normalized ROMs to MasterRoms
• 	[ ] Preserve ES‑style folder structure
• 	[ ] Move duplicates to Recovery
• 	[ ] Move corrupt files to Recovery
• 	[ ] Recovery logs
• 	[ ] Full undo capability

PHASE 7 — Metadata Generation
• 	[ ] ES‑style  generator (per system)
• 	[ ] RetroArch playlist generator (.lpl)
• 	[ ] Playlist output folder ()
• 	[ ] Optional: auto‑copy playlists to user’s RetroArch folder

PHASE 8 — System Summary JSON
• 	[ ] System summary generator
• 	[ ] ROM counts
• 	[ ] Duplicate counts
• 	[ ] Corrupt file counts
• 	[ ] Unknown file counts
• 	[ ] Multi‑disc set counts
• 	[ ] BIOS status summary
• 	[ ] Timestamped summary file

PHASE 9 — Scraper System
• 	[ ] Scraper settings (region, language, overwrite rules, etc.)
• 	[ ] Scraper engine
• 	[ ] Metadata retrieval
• 	[ ] Image retrieval (boxart, logos, screenshots, marquees)
• 	[ ] Video retrieval (future)
• 	[ ] Media output folder ()
• 	[ ] Metadata output ()
• 	[ ] Update gamelist.xml with scraped metadata
• 	[ ] Scraper cache
• 	[ ] Scraper logging

PHASE 10 — Future: Pi Image Builder
• 	[ ] Base OS image selection
• 	[ ] Image mounting
• 	[ ] Inject ROMs
• 	[ ] Inject BIOS
• 	[ ] Inject CHDs
• 	[ ] Inject playlists
• 	[ ] Inject gamelists
• 	[ ] Inject themes
• 	[ ] Inject configs
• 	[ ] Finalize + unmount image
• 	[ ] Output flash‑ready 

PHASE 11 — Future: ROM Manager + Sync System
• 	[ ] Compare PC MasterRoms with Pi ROMs
• 	[ ] Detect new ROMs
• 	[ ] Detect missing ROMs
• 	[ ] Detect BIOS differences
• 	[ ] Detect playlist differences
• 	[ ] Detect gamelist differences
• 	[ ] Sync new ROMs to Pi
• 	[ ] Sync changes from Pi to PC
• 	[ ] Curate Pi library
• 	[ ] Pi → PC metadata sync
• 	[ ] Pi → PC save file sync

PHASE 12 — Future: Custom ES‑Style Frontend (PC)
• 	[ ] Frontend core engine
• 	[ ] Theme engine
• 	[ ] SystemBar integration
• 	[ ] Avatar system
• 	[ ] Platform definitions (JSON‑driven)
• 	[ ] Game list viewer
• 	[ ] Metadata viewer
• 	[ ] Media viewer (images, videos)
• 	[ ] Game launcher
• 	[ ] Settings UI
• 	[ ] Integration with ROM Manager
• 	[ ] Integration with scraper metadata
• 	[ ] Integration with playlists + gamelists
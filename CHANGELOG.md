## [0.4.0] - 2026-03-07
### Changed
- Migrated all settings categories to the unified SettingsService architecture
- Updated SettingsViewModel to use dependency injection and async save operations
- Rebuilt GeneralSettingsView with proper bindings and consistent layout
- Standardized naming conventions across all settings views and categories

### Added
- New ScraperNetworkSettings model for scraper-specific HTTP configuration
- Full scraper configuration integration into unified settings
- Updated PlatformInfo handling in NonDatScanner

### Fixed
- Resolved CS0117 error caused by outdated PlatformInfo.Id reference
- Removed deprecated per-category JSON loading and encryption logic
- Corrected multiple binding issues across settings views
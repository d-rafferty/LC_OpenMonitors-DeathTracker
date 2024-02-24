# 1.0.5

- `Player's Life Support` monitor:
  - Disabled word-wrap.
  - Last three characters in their names are replaced with `...` if exceeding a length greater than 15.
  - Patches added for `Player's Life Support` when any player receives damage.
  - Changed `(INJURED)` to `(HURT)`.
- `Life Support` monitor now shows `ALIVE: <currently living players> / <total players>`.

# 1.0.4

- Added `Player's Life Support` monitor!
    - Disabled by default.
    - Now you can, in real time, see your friends go from happy and jovial, to dead and cold.
    - The text is overlayed on the bigger monitor for the outside ship camera.
        - Does not disable the camera.
- Removed configuration setting for hiding the `Profit Quota` and `Deadline` monitors.
  - Will be re-added later 
- More patches for `LifeSupportMonitor` to fix the counter not updating.
- Added `CHANGELOG.md` to zip file when creating a release
- Minor refactoring for classes and files.

# 1.0.3

- More debug logging statements, more patches for `LifeSupportMonitor`.

# 1.0.2

- Updated `manifest.json`, forgot BepInEx version.
- Moved releases to `CHANGELOG.md`

# 1.0.1

- Fixing readme images... whoops

# 1.0.0

- Initial release!

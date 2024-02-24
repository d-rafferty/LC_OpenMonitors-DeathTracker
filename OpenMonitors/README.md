# OpenMonitors

- Inspiration and 95% credit goes
  to [Jamil's CorporateRestructure](https://thunderstore.io/c/lethal-company/p/Jamil/Corporate_Restructure/)
- Completely client-side, meaning you can have it installed and play with your friends, even if they don't have this
  mod installed!

While the `CorporateRestructure` mod gave most of what I wanted:

1. I wanted to add extra configurability, such as where you could place the monitors, and hiding them.
2. I also wanted to add a `Life Support` monitor, which shows the number of players still alive.
3. Fix a first-time loading bug that happens with the `Credits` monitor.

- On hosting a game for the first time that session, the `Credits` monitor throws an NPE since the `TextMeshProGUI`
  object hasn't completely loaded into the game yet. Exiting the lobby, and then re-hosting fixes it, but still
  annoying.
    - I _could_ have made a `Prefix` patch for that one specific method, but to me that's a band-aid fix, and patching
      patches is a _really_ bad practice IMO.

## Issues / Bug Reports

Please open a [Github issue here](https://github.com/julian-perge/LC_OpenMonitors/issues) if you encounter any problems,
or quirks!

With that, please describe the steps to reproduce the issue if you can, and if possible, provide a short video!

## New Monitors [Client]

_Each monitor may be hidden from view via configuration setting._

- Loot
- Time of Day
- Credits
- Total Days (Clients display `?` until the first day is completed)
- Life Support (Number of players still alive)
- Players Life Support
    - Overlays on top of the outside camera ship monitor.
    - Shows, by name, the life support of each player.
        - Names exceeding length greater than fifteen will have the last three characters replaced with `...`
    - Should a player go to forever sleep, their name will have `(DEAD)` in red next to their name.
    - Should a player receive harm to their health, but not sleep forever (<= 50), their name will have `(HURT)` in
      yellow next to their name.

![img.png](https://imgur.com/v5hdqpF.png)

![img.png](https://imgur.com/d2Dts7I.png)

![img.png](https://imgur.com/tuK5cED.png)

## Navigation Monitor [Client]

_Weather can be hidden from `Terminal` and `Navigation` monitor via configuration setting._

Weather condition is colored based on the condition:

- None / Unknown = <span style="color:#69FF69;">Green</span>
- Dust Clouds = <span style="color:#69FF69;">Green</span>
- Rainy = <span style="color:#FFF01C;">Yellow</span>
- Foggy = <span style="color:#FFF01C;">Yellow</span>
- Stormy = <span style="color:#FF9B00;">Orange</span>
- Flooded = <span style="color:#FF9B00;">Orange</span>
- Eclipsed = <span style="color:#FF0000;">Red</span>

<img src="https://imgur.com/vsYq94q.png" width="42%" height="42%" />
<img src="https://imgur.com/ohZKRTg.png" width="42%" height="42%" />
<img src="https://imgur.com/Qtzn8O6.png" width="42%" height="42%" />
<img src="https://imgur.com/t0DhrAo.png" width="42%" height="42%" />

## Monitor Layout

`1`: PROFIT QUOTA

`2`: DEADLINE

`3`: CAMERA INSIDE SHIP

- This slot is not a possible choice for the new monitors, because it's the camera inside the ship, which I'd rather
  not touch for now.
- If you set a monitor to use slot 3, that monitor will revert back to it's default slot position.

`4`: LIFE SUPPORT

`5`: LOOT

`6`: TIME

`7`: DAY

`8`: CREDITS

| 5 | 6 | 7     | 8 |
|---|---|-------|---|
| 1 | 2 | ~~3~~ | 4 |

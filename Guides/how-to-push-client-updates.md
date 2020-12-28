#### How to push client updates

```xml
<!-- 
    EXPANSION IDS:
    1 = Classic
    2 = Burning Crusade
    3 = Wrath of the Lich King
    4 = Cataclysm
    5 = Mists of Pandaria
    6 = Warlords of Draenor
    7 = Legion
    8 = Battle for Azeroth
    9 = Shadowlands
-->
```

For example we want to push updates to wrath of the lich king so we go to ``yourdomain.com/launcher/game/3`` and **add** or **replace** the files:
  - I.E: If you want to add a new patch for example ``patch-x.MPQ`` then we upload it to ``yourdomain.com/launcher/game/3/data/patch-x.MPQ``
  - I.E: If you want to replace the ``Wow.exe`` then we upload it to ``yourdomain.com/launcher/game/3/Wow.exe``
  
Conclusions:
  Folder number corresponds to the expansion id.
  Inside the expansion id folder paths are exactly as in the World of Warcraft folder.

Edit ``yourdomain.com/launcher/Config.xml`` and look for your expansion id by the ``<Expansion>`` node:
  - Change attribute ``update_version="?"`` by increasing or decreasing the version number
  - Save

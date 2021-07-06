## SETUP GUIDE

### Table of Contents

- [Requirements](#Requirements) 
- [Setup steps before compiling the launcher](#Setup-steps-before-compiling-the-launcher) 
- [Compiling the launcher](#Compiling-the-launcher) 
- [Distributing the launcher](#Distributing-the-launcher) 
- [Uploading full client, minimal client or only custom patches](#uploading-full-client-minimal-client-or-only-custom-patches) 
- [Pushing a new client update](#Pushing-a-new-client-update)  
- [Giving access to admin panel](#Giving-access-to-admin-panel)  

### Requirements
- Visual Studio 2019 (with C#: ".NET Desktop Development")
- Webhost or Apache server with PHP version 7.+
- PHP extensions enabled: mysqli, Soap, cUrl, JSON
- SOAP outgoing port open (TrinityCore, AzerothCore SOAP port: 7878)

### Pulling the source
Use the following git command to pull the source and its submodule(s):
```git
git clone --recurse-submodules https://github.com/Oracle-Launcher/Oracle-Launcher.git Oracle-Launcher
```

### Setup steps before compiling the launcher
 1. Create a folder for your launcher on your webhost or local server named **"launcher"** or anything you prefer.
 2. Upload the files from "Oracle-API" folder regarding your Emulator and CMS to your webhost or local server in the new created launcher folder.
 3. Go to your webhost **launcher** folder and edit Config.xml to your preferences.
 4. Create a database called **launcher** or whatever you want.
 5. Run the db installer from **launcher/application/sql/Launcher_db.sql** to your launcher database.
 6. Run the auth.account table update from **launcher/application/sql/Auth.account.sql** to your auth database.
 7. Go to your webhost **launcher/application/config.php** and edit:
    - Launcher domain path
    - Mysql database connection details
    - Soap address, port and URI (TC = TrinityCore or AC = AzerothCore)
    - Define your GM Ranks (must be the same rank ids as in your server)
    - Edit the ranks that are allowed to access gm or admin panel and its functions.
    - Set your discord webhook url (this will be used to report launcher erros in your custom discord channel).

### Compiling the launcher
 1. Open source/Oracle/Oracle.sln with visual studio 2019.
 2. The solution has 4 projects:
    - Oracle Launcher
    - Oracle Login
    - Oracle Updater
    - WebHandler
 3. Edit **Oracle Launcher->Properties->Settings**
    - Change the url to your launcher **Config.xml**
    - Change Server Name
 4. Edit **Oracle Login->Properties->Settings**
    - Change Server Name
    - Change the register url
    - Change the account recovery url
 5. Edit **Oracle Updater->Config.cs**
    - Change **AppUrl** with the correct url for example "http://yourdomain.com/launcher/application/application.php"
 6. Edit **WebHandler->Config.cs**
    - Change **AppUrl** with the correct url for example "http://yourdomain.com/launcher/application/application.php"
    - Change **GameFolderUrl** with the correct url for example "http://yourdomain.com/launcher/game"
 7. You can now compile the launcher by pressing **CTRL+SHIFT+B** or right-click on **solution->Build Solution**

### Distributing the launcher
 - The compiled launcher files can be found in source/**Oracle/bin** folder
 - To start the launcher you must open **Oracle Login.exe** so this is your main launcher file.
 - Distribute the launcher as:
   - ZIP archive
   - SFX Archive with desktop shortcut [video tutorial here](https://www.youtube.com/watch?v=koFeRlOuZgw)
   - Or 3rd party installer, for example [Advanced Installer](https://www.advancedinstaller.com/download.html) or [Actual Installer](https://www.actualinstaller.com/)

### Uploading full client, minimal client or only custom patches
 1. First of all, get used to the following expansion ids:
    - 1 = Classic
    - 2 = Burning Crusade
    - 3 = Wrath of the Lich King
    - 4 = Cataclysm
    - 5 = Mists of Pandaria
    - 6 = Warlords of Draenor
    - 7 = Legion
    - 8 = Battle for Azeroth
    - 9 = Shadowlands
 2. Create a folder named "**game**" on your webhost/**launcher/game**
 3. Inside the game folder create a subfolder named "**1**" for Classic or "**2**" Burning Crusader or "**3**" for Wotlk, etc. Depending on your expansion id.
 4. Upload your game client files:
    - For full client upload the content of your **WoW Folder** to your webhost/**launcher/game/X** where **X** is the expansion id.
    - For minimal client upload the minimal client files to your webhost/**launcher/game/X** where **X** is the expansion id.
    - For custom patches upload them to your  webhost/**launcher/game/X/data** where **X** is the expansion id.
 5. Conclusion: the webhost/**launcher/game/X** folder files paths are the same as in your **World of Warcraft folder**.

### Pushing a new client update
 1. Upload or replace your client files on webhost/**launcher/game/X** where **X** is the expansion id.
 2. Open and edit your webhost/**launcher/Config.xml** for example
    - Find node "<Expansion id="5" update_version="1">" and increase **update_version**.
    - Done, now when your players will start the launcher will be forced to update their client.

### Giving access to admin panel
We are using a MD5 password in database for security reasons.
 1. Go to your **auth** database -> **account** table and search for your account.
 2. Generate a **MD5 Hash password**, for example on this website [MD5 Hash Generator](https://www.md5hashgenerator.com/)
 3. Store your **non** MD5 password somewhere safe.
 4. After you found your admin account go to **sec_pa** column and insert the **MD5 Hash password**.
 5. The required password to login the admin panel is the **non** MD5 password.

#### Requirements
  - Visual Studio Community 2019 (with **.NET desktop development** C#)

#### How to setup the launcher

1. Pull the source with git ``git pull https://github.com/CyberMist2/Oracle-Launcher.git`` or just download.

2. Upload the content of ``Oracle-Launcher/Upload`` to your webhost on example: ``yourdomain.com/launcher``

3. Open and edit ``yourdomain.com/launcher/application/config.php`` and save.

4. Open and edit ``yourdomain.com/launcher/Config.xml`` and save.

5. Open ``Oracle-Launcher/Oracle/Oracle.sln``

6. Edit project ``WebHandler->Config.cs`` and save.
```
AppUrl = "http://yourdomain.com/launcher/application/application.php"
  
GameFolderUrl = "http://yourdomain.com/launcher/game"
```

7. Edit project ``Oracle Login->Properties(double-click) Go to Settings->ServerName`` and save.

8. Edit project ``Oracle Login->Properties(double-click) Go to Settings->RegisterAccountUrl`` and save.

9. Edit project ``Oracle Login->Properties(double-click) Go to Settings->ResetPasswordUrl`` and save.

10. Edit project ``Oracle Launcher->Properties(double-click) Go to Settings->ServerName`` and save.

11. Edit project ``Oracle Launcher->Properties(double-click) Go to Settings->XMLDocumentUrl`` (make sure Config.xml starts with uppercase) and save:
```
http://yourdomain.com/launcher/Config.xml
```

10. Right click on the solution ``Oracle->Clean Solution`` and ``Oracle->Build Solution``

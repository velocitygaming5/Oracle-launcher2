# How to rename the launcher completely
______
## Renaming Oracle Launcher project

1. From the Solution Explorer on the right side right click on "Oracle Launcher" -> Rename.

2. Right click on the solution -> Clean Solution then -> Build solution.

3. Right click on the renamed **Project -> Properties -> Application** and change:
   - In the **Assembly name** field change to ``Server Name Launcher``
   - In the **Default namespace** field change to ``Server_Name_Launcher``
   - Click on the **Assembly Information..** button and update to your preferences
   - Hit ``CTRL+S`` to save.

4. In the project explorer find and open file **OracleLauncher.xaml.cs**
   - Rename the namespace ``Oracle_Launcher`` to ``Server_Name_Launcher`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**
   - Rename the partial class ``OracleLauncher`` to ``ServerNameLauncher`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**

5. Right click on the solution -> Clean Solution then -> Build solution.

6. Right click on **OracleLauncher.xaml** -> Rename to **ServerNameLauncher.xaml**

7. Right click on the solution -> Clean Solution then -> Build solution.

8. In the Project Explorer find and open file **App.xaml**
   - Rename ``StartupUri="OracleLauncher.xaml"`` to ``"StartUpUri="ServerNameLauncher.xaml"``

9. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``oracleLauncher``
   - In the **Replace** text box write ``serverNameLauncher`
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

10. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``/Oracle Launcher;component``
   - In the **Replace** text box write ``/Server Name Launcher;component`
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

11. Right click on the solution -> Clean Solution then -> Build solution.

______
## Renaming Oracle Login project

1. From the Solution Explorer on the right side right click on **Oracle Login** -> Rename.

2. Right click on the solution -> Clean Solution then -> Build solution.

3. Right click on the renamed **Project -> Properties -> Application** and change:
   - In the **Assembly name** field change to ``Server Name Login``
   - In the **Default namespace** field change to ``Server_Name_Login``
   - Click on the **Assembly Information..** button and update to your preferences
   - Hit ``CTRL+S`` to save.

4. In the project explorer find and open file **OracleLogin.xaml.cs**
   - Rename the namespace ``Oracle_Login`` to ``Server_Name_Login`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**
   - Rename the partial class ``OracleLogin`` to ``ServerNameLogin`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**

5. Right click on the solution -> Clean Solution then -> Build solution.

6. Right click on **OracleLogin.xaml** -> rename to **ServerNameLogin.xaml**

7. Right click on the solution -> Clean Solution then -> Build solution.

8. In the Project Explorer find and open file **App.xaml**
   - Rename ``StartupUri="OracleLogin.xaml"`` to ``"StartUpUri="ServerNameLogin.xaml"``

9. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``OracleLogin``
   - In the **Replace** text box write ``ServerNameLogin``
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

10. Right click on the solution -> Clean Solution then -> Build solution.

11. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``/Oracle Login;component``
   - In the **Replace** text box write ``/Server Name Login;componen``
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all


12. Right click on the solution -> Clean Solution then -> Build solution.

13. In the Project Explorer find and open file **ServerNameLogin.xaml.cs**
    - Replace line:
       - Process.Start($"Oracle Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");
    - To:
       - Process.Start($"Server Name Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");

14. Right click on the solution -> Clean Solution then -> Build solution.

______
## Renaming the Oracle Updater project

1. From the Solution Explorer on the right side right click on **Oracle Updater** -> Rename.

2. Right click on the solution -> Clean Solution then -> Build solution.

3. Right click on the renamed **Project -> Properties -> Application** and change:
   - In the **Assembly name** field change to ``Server Name Updater``
   - In the **Default namespace** field change to ``Server_Name_Updater``
   - Click on the **Assembly Information..** button and update to your preferences
   - Hit ``CTRL+S`` to save.

4. In the project explorer find and open file **MainWindow.xaml.cs**
   - Rename the namespace ``Oracle_Updater`` to ``Server_Name_Updater`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**
   - Rename the partial class ``MainWindow`` to ``ServerNameUpdater`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**

5. Right click on the solution -> Clean Solution then -> Build solution.

6. Right click on **OracleLogin.xaml** -> rename to **ServerNameUpdater.xaml**

7. Right click on the solution -> Clean Solution then -> Build solution.

In the project explorer find and open file App.xaml and rename StartupUri="MainWindow.xaml" to StartUpUri="ServerNameUpdater.xaml"

Press "CTRL+SHIFT+F" -> click on Replace in files tab -> Find = "/Oracle Updater;component" -> Replace = "/Server Name Updater;component" then click Replace All button.
	After replace is successfull press "CTRL+SHIFT+S" to save all.

Right click on the solution -> clean solution then -> build solution.

In the project explorer find and open file "ServerNameUpdater.xaml.cs" and find the line:
	Process.Start($"Oracle Login.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");
replace:
	Process.Start($"Server Name Login.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");

Right click on the solution -> clean solution then -> build solution.


=================================================================================================
##### LAST STEP updating the api

Go to Upload/application/modules/avatars.php open and edit:

Replace this line:
	if(substr($db_avatar_url, 0, 29) === "/Oracle Launcher;component/")
	
To (don't forget to recount all characters in the quotes including the spaces)

	if(substr($db_avatar_url, 0, 32) === "/Server Name Launcher;component/")

Save, done.

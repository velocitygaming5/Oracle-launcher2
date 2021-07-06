Open Visual Studio


=================================================================================================
##### Renaming the Oracle Launcher project

From the solution explorer on the right side right click on "Oracle Launcher" -> Rename.

Right click on the solution -> clean solution then -> build solution.

Right click on the renamed project -> properties.
Go to Properties -> Application and change:
	"Assembly name" to "Server Name Launcher"
	"Default namespace" to "Server_Name_Launcher"
In the same tab find and click on the "Assembly Information.." button and change the assembly information to your preferences

In the project explorer find and open file "OracleLauncher.xaml.cs"
	Rename the namespace "Oracle_Launcher" to "Server_Name_Launcher" then right click on it -> "Quick Actions and Refactoring" -> rename all..
	Rename the partial class "OracleLauncher" to "ServerNameLauncher" then right click on it -> "Quick Actions and Refactoring" -> rename all...

Right click on the solution -> clean solution then -> build solution.

Right click on OracleLauncher.xaml -> rename to "ServerNameLauncher.xaml".

Right click on the solution -> clean solution then -> build solution.

In the project explorer find and open file App.xaml and rename StartupUri="OracleLauncher.xaml" to StartUpUri="ServerNameLauncher.xaml"

Press "CTRL+SHIFT+F" -> click on Replace in files tab -> Find = "oracleLauncher" -> Replace = "serverNameLauncher" then click Replace All button.
	After replace is successfull press "CTRL+SHIFT+S" to save all.

Press "CTRL+SHIFT+F" -> click on Replace in files tab -> Find = "/Oracle Launcher;component" -> Replace = "/Server Name Launcher;component" then click Replace All button.
	This process may take a while, so be patient...
	After replace is successfull press "CTRL+SHIFT+S" to save all.

Right click on the solution -> clean solution then -> build solution.


=================================================================================================
##### Renaming the Oracle Login project

From the solution explorer on the right side right click on "Oracle Login" -> Rename.

Right click on the solution -> clean solution then -> build solution.

Right click on the renamed project -> properties.
Go to Properties -> Application and change:
	"Assembly name" to "Server Name Login"
	"Default namespace" to "Server_Name_Login"
In the same tab find and click on the "Assembly Information.." button and change the assembly information to your preferences

In the project explorer find and open file "OracleLogin.xaml.cs"
	Rename the namespace "Oracle_Login" to "Server_Name_Login" then right click on it -> "Quick Actions and Refactoring" -> rename all..
	Rename the partial class "OracleLogin" to "ServerNameLogin" then right click on it -> "Quick Actions and Refactoring" -> rename all...

Right click on the solution -> clean solution then -> build solution.

Right click on OracleLogin.xaml -> rename to "ServerNameLogin.xaml".

Right click on the solution -> clean solution then -> build solution.

In the project explorer find and open file App.xaml and rename StartupUri="OracleLogin.xaml" to StartUpUri="ServerNameLogin.xaml"

Press "CTRL+F" -> click on Replace in files tab -> Find = "OracleLogin" -> Replace = "ServerNameLogin" then click Replace All button.
	After replace is successfull press "CTRL+SHIFT+S" to save all.

Right click on the solution -> clean solution then -> build solution.

Press "CTRL+SHIFT+F" -> click on Replace in files tab -> Find = "/Oracle Login;component" -> Replace = "/Server Name Login;component" then click Replace All button.
	After replace is successfull press "CTRL+SHIFT+S" to save all.

Right click on the solution -> clean solution then -> build solution.

In the project explorer find and open file "ServerNameLogin.xaml.cs" and find the line:
	Process.Start($"Oracle Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");
replace:
	Process.Start($"Server Name Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");

Right click on the solution -> clean solution then -> build solution.


=================================================================================================
##### Renaming the Oracle Updater project

From the solution explorer on the right side right click on "Oracle Login" -> Rename.

Right click on the solution -> clean solution then -> build solution.

Right click on the renamed project -> properties.
Go to Properties -> Application and change:
	"Assembly name" to "Server Name Login"
	"Default namespace" to "Server_Name_Login"
In the same tab find and click on the "Assembly Information.." button and change the assembly information to your preferences

In the project explorer find and open file "MainWindow.xaml.cs"
	Rename the namespace "Oracle_Updater" to "Server_Name_Updater" then right click on it -> "Quick Actions and Refactoring" -> rename all..
	Rename the partial class "MainWindow" to "ServerNameUpdater" then right click on it -> "Quick Actions and Refactoring" -> rename all...

Right click on the solution -> clean solution then -> build solution.

Right click on MainWindow.xaml -> rename to "ServerNameUpdater.xaml".

Right click on the solution -> clean solution then -> build solution.

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

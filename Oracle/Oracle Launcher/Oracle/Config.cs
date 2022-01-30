namespace WebHandler
{
    public class Config
    {
        /* -----------------------------------------------------------------------------------------------------------------

            DO NOT TOUCH ANYTHING BELOW !!

            UNLESS YOU KNOW WHAT YOU ARE DOING !!

            I WILL NOT BE RESPONSIBLE IF YOU MESS UP WITH THE PATHS BELOW !!

        ----------------------------------------------------------------------------------------------------------------- */

        public static readonly string 
            AppUrl = $"{Oracle_Launcher.Properties.Settings.Default.APIUrl}" + "/application/application.php";

        public static readonly string 
            GameFolderUrl = $"{Oracle_Launcher.Properties.Settings.Default.APIUrl}/game";
    }
}

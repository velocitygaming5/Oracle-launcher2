namespace Oracle_Updater
{
    public class Config // ORACLE UPDATER CONFIG
    {
        /* -----------------------------------------------------------------------------------------------------------------

            DO NOT TOUCH ANYTHING BELOW !!

            UNLESS YOU KNOW WHAT YOU ARE DOING !!

            I WILL NOT BE RESPONSIBLE IF YOU MESS UP WITH THE PATHS BELOW !!

        ----------------------------------------------------------------------------------------------------------------- */

        public static readonly string
            AppUrl = $"{Properties.Settings.Default.APIUrl}" + "/application/application.php";
    }
}

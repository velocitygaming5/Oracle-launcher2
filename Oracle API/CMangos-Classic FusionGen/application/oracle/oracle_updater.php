<?php
header('Content-Type: application/json; charset=UTF-8');

class OracleUpdater
{
    public static function GetOracleUpdaterFilesList($api_path)
    {
        global $config;
        $jsonArray = array();

        $di = new RecursiveDirectoryIterator("updates");
        $ri = new RecursiveIteratorIterator($di);

        foreach ($ri as $filename => $file)
        {
            if ($file->isFile() && strcmp(basename($filename), ".htaccess") !== 0)
            {
                $fileInfoArray = array();

                // replace "../" with "/"
                $finalFilePath = str_replace("..", "", $filename);

                // replace "\" with "/"
                $finalFilePath = str_replace("\\", "/", $finalFilePath);

                // replace spaces " " with "%20"
                // $finalFilePath = str_replace(" ", "%20", $finalFilePath);

                // remote url
                $remoteUrl = $api_path."/".$finalFilePath;
                $infoRow['remote_url'] = $remoteUrl;

                // local path
                $localPath = str_replace("updates/", "", $finalFilePath);
                $localPath = str_replace("%20", " ", $localPath);
                $infoRow['local_path'] = $localPath;

                // file name
                $infoRow['file_name'] = basename($filename);

                array_push($jsonArray, $infoRow);
            }
        }
        echo json_encode($jsonArray, JSON_PRETTY_PRINT);
    }
}

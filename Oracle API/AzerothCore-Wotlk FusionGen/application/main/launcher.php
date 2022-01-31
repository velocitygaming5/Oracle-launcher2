<?php
header('Content-Type: application/json; charset=UTF-8');

class Launcher
{
    public static function NewDBConnection()
    {
        global $config;
        $connection = mysqli_connect(
            $config['mysqli']['launcher']['hostname'],
            $config['mysqli']['launcher']['user'],
            $config['mysqli']['launcher']['pass'],
            $config['mysqli']['launcher']['database'],
            $config['mysqli']['launcher']['port']
        );
        return $connection;
    }
}

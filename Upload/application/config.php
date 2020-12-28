<?php
    // SESSION POST REQUEST COOLDOWN IN SECONDS
    $config['session_cooldown']     = 5;
    
    // USE TRINITYCORE'S LATEST AUTH ENCRYPTION METHOD OR USE "false" FOR PREVIOUS SHA1
    $config['srp6']                 = false;

    // AUTH DB
    $config['mysql_auth_hostname']  = 'localhost';
    $config['mysql_auth_port']      = 3306;
    $config['mysql_auth_user']      = 'trinity';
    $config['mysql_auth_pass']      = 'trinity';
    $config['mysql_auth_dbname']    = 'auth';

    // WEB DB
    $config['mysql_web_hostname']   = 'localhost';
    $config['mysql_web_port']       = 3306;
    $config['mysql_web_user']       = 'trinity';
    $config['mysql_web_pass']       = 'trinity';
    $config['mysql_web_dbname']     = 'wowcms';

    // REALMS DB
    $config['realms'] = array(
        array(
            'name'                  => 'Realm Name ONE', 
            'mysql_char_hostname'   => 'localhost',
            'mysql_char_port'       => 3306,
            'mysql_char_user'       => 'trinity',
            'mysql_char_pass'       => 'trinity',
            'mysql_char_dbname'     => 'characters'
        ),
        array(
            'name'                  => 'Realm Name TWO', 
            'mysql_char_hostname'   => 'localhost',
            'mysql_char_port'       => 3306,
            'mysql_char_user'       => 'trinity',
            'mysql_char_pass'       => 'trinity',
            'mysql_char_dbname'     => 'characters_two'
        ),
        array(
            'name'                  => 'Realm Name THREE', 
            'mysql_char_hostname'   => 'localhost',
            'mysql_char_port'       => 3306,
            'mysql_char_user'       => 'trinity',
            'mysql_char_pass'       => 'trinity',
            'mysql_char_dbname'     => 'characters_three'
        ),
    );
?>
<?php
    if(!isset($_SESSION)){session_start();}

    header('Content-Type: application/json');

    require_once 'config.php';
    
	require_once 'srp6_lib.php';
    
    error_reporting(0);
    
    if(!empty($_POST['type']) && !Tool::HasSessionCooldown())
    {
        switch ($_POST['type']) 
        {
            case 'login_response':
                Auth::GetLoginResponse($_POST['user'], $_POST['pass']);
                break;
            case 'account_state':
                Auth::IsAccountBanned($_POST['user'], $_POST['pass']);
                break;
            case 'characters_list':
                Char::GetCharactersList($_POST['user'], $_POST['pass']);
                break;
            case 'account_balance':
                Web::GetAccountBalance($_POST['user'], $_POST['pass']);
                break;
            case 'files_list':
                Game::GetFilesList($_POST['expansion']);
                break;
        }
    }

    class Auth
    {
        public static function GetLoginResponse($user, $pass)
        {	
            if (self::IsValidLogin($user, $pass))
                echo 1;
            else
                echo 0;
        }
        
        public static function IsAccountBanned($user, $pass)
        {
            if (self::IsValidLogin($user, $pass))
            {
                global $config;
                
                $mysqli = Tool::NewAuthConnection();
                
                $accountId = Auth::GetAccountId($user);
            
                if ($query = $mysqli->prepare('SELECT unbandate FROM `account_banned` WHERE `active` = 1 AND id = ?'))
                {
                    
                    $query->bind_param('i', $accountId);
                    $query->execute();
                    $query->bind_result($unbanDate);
                    
                    $jsonArray = array();
                    
                    $row_array['state']         = 'OK';
                    $row_array['unbanDate']     = '0';
                    
                    array_push($jsonArray, $row_array);
                    
                    while ($query->fetch()) 
                    {
                        $row_array['state']         = 'Banned';
                        $row_array['unbanDate']     = $unbanDate;
                        
                        unset($jsonArray);
                        
                        $jsonArray = array();
                        
                        array_push($jsonArray, $row_array);
                    }
                    
                    echo json_encode($jsonArray, JSON_PRETTY_PRINT);
                    
                    $query->close();
                }
                
                mysqli_close($mysqli);
            }
        }
    
        public static function GetAccountId($accountName)
        {
            global $config;
            
            $mysqli = Tool::NewAuthConnection();
        
            if ($mysqli == true)
            {
                $query = $mysqli->prepare('SELECT id FROM account WHERE username = ?');
                
                assert($query);
                
                $query->bind_param('s', $accountName);
                $query->execute();
                $query->bind_result($accountId);
                
                while ($query->fetch())
                {
                    mysqli_close($mysqli);
                    return $accountId;
                }
            }
            
            mysqli_close($mysqli);
            
            return 0;
        }
        
        public static function IsValidLogin($user, $pass)
        {
            global $config;
            
            $mysqli = Tool::NewAuthConnection();
        
            if ($mysqli == true)
            {
                if ($config['srp6'])
                {
                    $query = $mysqli->prepare('SELECT username, salt, verifier FROM account WHERE username = ?');
                    
                    assert($query);
                    
                    $query->bind_param('s',$user);
                    $query->execute();
                    $query->bind_result($dbUsername, $salt, $verifier);
                    
                    while ($query->fetch())
                    {
                        if (VerifySRP6Login($user, $pass, $salt, $verifier))
                            return true;
                        else 
                            return false;
                    }
                }
                else
                {
                    $query = $mysqli->prepare('SELECT username FROM account WHERE username = ? AND sha_pass_hash = ?');
                    
                    assert($query);
                    
                    $passhash = sha1(strtoupper($user.':'.$pass));
                    
                    $query->bind_param('ss', $user, $passhash);
                    $query->execute();
                    $query->bind_result($dbUsername);
                    
                    while ($query->fetch())
                    {
                        if ($dbUsername)
                            return true;
                    }
                }
            }
                
            mysqli_close($mysqli);
            
            return false;
        }
    }
    
    class Char
    {
        public static function GetCharactersList($user, $pass)
        {
            if (Auth::IsValidLogin($user, $pass))
            {
                global $config;
                
                $accountId = Auth::GetAccountId($user);
                    
                $final_array = array();
                
                foreach ($config["realms"] as $realm)
                {
                    if (!empty($realm["name"]) && !empty($realm["mysql_char_hostname"]) && !empty($realm["mysql_char_port"]) 
                        && !empty($realm["mysql_char_user"]) && !empty($realm["mysql_char_pass"]) && !empty($realm["mysql_char_dbname"])) 
                    {
                        $mysqli = mysqli_connect($realm['mysql_char_hostname'], 
                                                 $realm['mysql_char_user'], 
                                                 $realm['mysql_char_pass'], 
                                                 $realm['mysql_char_dbname'], 
                                                 $realm['mysql_char_port']);
                        
                        if ($query = $mysqli->prepare('SELECT `name`, `gender`, `level`, `race`, `class` FROM `characters` WHERE `account`= ?'))
                        {
                            $query->bind_param('i', $accountId);
                            $query->execute();
                            $query->bind_result($charName, $charGender, $charLevel, $charRace, $charClass);
                            
                            $realmArray = array();
                            while ($query->fetch()) 
                            {
                                $row_array['realm']     = $realm['name'];
                                $row_array['name']      = $charName;
                                $row_array['gender']    = $charGender;
                                $row_array['level']     = $charLevel;
                                $row_array['race']      = $charRace;
                                $row_array['class']     = $charClass;
                                
                                array_push($realmArray, $row_array);
                                array_push($final_array, $realmArray);
                            }
                            
                            $query->close();
                        }
                    }
                }
                
                if (empty($final_array) == false)
                    echo json_encode($final_array, JSON_PRETTY_PRINT);
            }
        } 
    }
    
    class Web
    {
        public static function GetAccountBalance($user, $pass)
        {
            if (Auth::IsValidLogin($user, $pass))
            {
                global $config;
                
                $mysqli = Tool::NewAuthConnection();
                
                $accountId = Auth::GetAccountId($user);
            
                if ($query = $mysqli->prepare('SELECT dp, vp FROM `account_balance` WHERE `id` = ?'))
                {
                    $query->bind_param('i', $accountId);
                    $query->execute();
                    $query->bind_result($dp, $vp);
                    
                    $jsonArray = array();
                    
                    $row_array['dp'] = 0;
                    $row_array['vp'] = 0;
                    
                    array_push($jsonArray, $row_array);
                    
                    while ($query->fetch()) 
                    {
                        $row_array['dp'] = $dp;
                        $row_array['vp'] = $vp;
                        
                        unset($jsonArray);
                        
                        $jsonArray = array();
                        
                        array_push($jsonArray, $row_array);
                    }
                    
                    echo json_encode($jsonArray, JSON_PRETTY_PRINT);
                    
                    $query->close();
                }
                
                mysqli_close($mysqli);
            }
        }
    }
    
    class Tool
    {
        public static function HasSessionCooldown()
        {
            global $config;
            
            if(isset($_SESSION['last_submit']) && ((time() - $_SESSION['last_submit']) <= $config['session_cooldown'])) 
            {
                $timecounter =  time() - $config['session_cooldown'] - $_SESSION['last_submit'];
                return true;
            } 
            else 
            {
                $_SESSION['last_submit'] = time();
                return false;
            }
        }
        
        public static function NewAuthConnection()
        {
            global $config;
            
            $conn = mysqli_connect($config['mysql_auth_hostname'], $config['mysql_auth_user'], $config['mysql_auth_pass'], $config['mysql_auth_dbname'], $config['mysql_auth_port']);
            
            return $conn;
        }
    }
    
    class Game
    {
        public static $finalArray = array();
        
        public static function GetFilesList($expansionID, $dir = "")
        {   
            ob_start();
            
            if (!$dir)
                $dir = "../game/".$expansionID;
            
            $dh = new DirectoryIterator($dir);
            
            $pathArray = array();
            
            foreach ($dh as $item)
            {
                if (!$item->isDot())
                {
                    if ($item->isDir())
                        self::GetFilesList($expansionID, "$dir/$item");
                    else 
                    {
                        $fileName = $item->getFilename();
                        $extension = $item->getExtension();
        
                        if ($extension != "php" && $extension != "htaccess")
                        {
                            $path = $dir ."/".$fileName;

                            $fileSize = self::GetFileSize($path);
                            $modifiedTime = self::GetFileModifiedTime($path);
                            
                            $path = str_replace('../game/'.$expansionID.'/', '', $path);
                            
                            $row_array['filePath'] = $path;
                            $row_array['fileSize'] = $fileSize;
                            $row_array['modifiedTime'] = $modifiedTime;

                            array_push($pathArray, $row_array);
                        }
                    }
                }
            }
                
            array_push(self::$finalArray, $pathArray);
            
            ob_end_clean();
            
            echo json_encode(self::$finalArray, JSON_PRETTY_PRINT);
        }
        
        public static function GetMd5Hash($filePath)
        {
            return md5_file($filePath);
        }  
        
        public static function GetSha1Hash($filePath)
        {
            return sha1_file($filePath);
        }
        
        public static function GetFileSize($file) 
        {
            $size = filesize($file);
            if ($size <= 0)
            {
                if (!(strtoupper(substr(PHP_OS, 0, 3)) == 'WIN')) {
                    $size = trim(`stat -c%s $file`);
                }
                else{
                    $fsobj = new COM("Scripting.FileSystemObject");
                    $f = $fsobj->GetFile($file);
                    $size = $f->Size;
                }
            }
            return $size;
        }
        
        public static function GetFileModifiedTime($filename)
        {
            return filemtime($filename);
        }
    }
    
    // if (isset($_GET["test"]))
    // {
        // echo 'calling test function';
        // Char::GetCharactersList($_GET["u"], $_GET["p"]);
        // Auth::GetLoginResponse($_GET["u"], $_GET["p"]);
        // Auth::IsAccountBanned($_GET["u"], $_GET["p"]);
        // Web::GetAccountBalance($_GET["u"], $_GET["p"]);
    // }
    
?>
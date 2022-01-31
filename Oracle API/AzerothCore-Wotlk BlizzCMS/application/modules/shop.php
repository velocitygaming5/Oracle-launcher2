<?php
header('Content-Type: application/json; charset=UTF-8');

class Shop
{
    public static function GetShopList($user, $pass)
    {
        if (Auth::IsValidLogin($user, $pass))
        {
            global $config;

            $mysqli = Launcher::NewDBConnection();

            mysqli_set_charset($mysqli, "utf8");
    
            if ($mysqli == true)
            {
                if ($query = $mysqli->prepare('SELECT id, title, description, img_url, price_dp, price_vp, category, soap_command, realm_id FROM shop_list'))
                {
                    $query->execute();
                    $query->bind_result($id, $title, $description, $img_url, $price_dp, $price_vp, $category, $soap_command, $realm_id);
                    $query->store_result();

                    $jsonArray = array();

                    while ($query->fetch()) 
                    {
                        $rowArray['id'] = $id;
                        $rowArray['title'] = $title;
                        $rowArray['description'] = $description;
                        $rowArray['img_url'] = $img_url;
                        $rowArray['price_dp'] = $price_dp;
                        $rowArray['price_vp'] = $price_vp;
                        $rowArray['category'] = $category;
                        // $rowArray['soap_command'] = $soap_command;
                        $rowArray['realmid'] = $realm_id;

                        array_push($jsonArray, $rowArray);
                    }
                    echo json_encode($jsonArray, JSON_PRETTY_PRINT);
                    $query->close();
                }
                // else
                // {
                    // echo("GetShopList error: " . $mysqli->error);
                // }
                mysqli_close($mysqli);
            }
        }
    }
    
    public static function GetShopItemPrice($id, $currency)
    {
        global $config;
        
        $col_name = ($currency == 0) ? "price_dp" : "price_vp"; // 0 = dp, other = vp

        $mysqli = Launcher::NewDBConnection();

        mysqli_set_charset($mysqli, "utf8");

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT '.$col_name.' FROM shop_list WHERE id = ?'))
            {
                $query->bind_param('i', $id);
                $query->execute();
                $query->bind_result($price_dp);
                $query->store_result();

                while ($query->fetch()) 
                {
                    return $price_dp;
                }
                $query->close();
            }
            // else
            // {
                // echo("GetShopItemPrice error: " . $mysqli->error);
            // }
            mysqli_close($mysqli);
        }
        
        return 0;
    }

    public static function GetUserBalance($user_id, $currency)
    {
        global $config;
        
        $col_name = ($currency == 0) ? "dp" : "vp"; // 0 = dp, other = vp

        $mysqli = Web::NewDBConnection();

        mysqli_set_charset($mysqli, "utf8");

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT '.$col_name.' FROM users WHERE id = ?'))
            {
                $query->bind_param('i', $user_id);
                $query->execute();
                $query->bind_result($balance_vp);
                $query->store_result();

                $jsonArray = array();

                while ($query->fetch()) 
                {
                    return $balance_vp;
                }
                $query->close();
            }
            // else
            // {
                // echo("GetUserBalance error: " . $mysqli->error);
            // }
            mysqli_close($mysqli);
        }
        
        return 0;
    }
    
    public static function SpendUserBalance($user, $pass, $currency, $amount)
    {
        if (Auth::IsValidLogin($user, $pass))
        {
            global $config;
            
            $accountId = Auth::GetAccountId($user);
            
            $col_name = ($currency == 0) ? "dp" : "vp"; // 0 = dp, other = vp

            $mysqli = Web::NewDBConnection();

            mysqli_set_charset($mysqli, "utf8");

            if ($mysqli == true)
            {
                if ($query = $mysqli->prepare('UPDATE users SET '.$col_name.' = '.$col_name.' - ? WHERE id = ?'))
                {
                    $query->bind_param('ii', $amount, $accountId);
                    $query->execute();

                    if ($mysqli->affected_rows != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                    $query->close();
                    mysqli_close($mysqli);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    public static function GetShopItemQuery($id)
    {
        global $config;

        $mysqli = Launcher::NewDBConnection();

        mysqli_set_charset($mysqli, "utf8");

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT soap_command FROM shop_list WHERE id = ?'))
            {
                $query->bind_param('i', $id);
                $query->execute();
                $query->bind_result($soap_command);
                $query->store_result();

                while ($query->fetch()) 
                {
                    return $soap_command;
                }
                $query->close();
            }
            // else
            // {
                // echo("GetShopItemQuery error: " . $mysqli->error);
            // }
            mysqli_close($mysqli);
        }
        
        return 0;
    }

    public static function GetShopItemRealmId($id)
    {
        global $config;

        $mysqli = Launcher::NewDBConnection();

        mysqli_set_charset($mysqli, "utf8");

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT realm_id FROM shop_list WHERE id = ?'))
            {
                $query->bind_param('i', $id);
                $query->execute();
                $query->bind_result($realm_id);
                $query->store_result();

                while ($query->fetch()) 
                {
                    return $realm_id;
                }
                
                $query->close();
            }
            // else
            // {
                // echo("GetShopItemRealmId error: " . $mysqli->error);
            // }
            mysqli_close($mysqli);
        }
        
        return 0;
    }
    
    private static function AccHasCharNamed($accountId, $characterName)
    {
        global $config;
        
        $validName = false;
        
        foreach ($config['mysqli']['realms'] as $realm)
        {
            $mysqli = mysqli_connect($realm['hostname'], $realm['user'], $realm['pass'], $realm['database'], $realm['port']);

            if ($mysqli == true)
            {
                mysqli_set_charset($mysqli, "utf8");
    
                if ($query = $mysqli->prepare('SELECT `name` FROM `characters` WHERE `account`= ? AND `name`= ?'))
                {
                    $query->bind_param('is', $accountId, $characterName);
                    $query->execute();
                    $query->bind_result($name);

                    while ($query->fetch()) 
                    {
                        if($name === $characterName)
                        {
                            $validName = true;
                        }
                    }
    
                    $query->close();
                    mysqli_close($mysqli);
                }
            }
        }
                    
        return $validName;
    }
    
    public static function PurchaseId($user, $pass, $id, $currency, $playerName = "Unknown", $accountName = "Unknown")
    {
        ob_start();
        $jsonObj = new \stdClass();
        $jsonObj->responseMsg = "Unable to purchase, try again later..";
        $jsonObj->response = false;
        
        if (Auth::IsValidLogin($user, $pass))
        {
            $user = strtolower($user);
            $accountName = strtolower($accountName);
            
            $accountID = Auth::GetAccountId($user);
            $balanceName = ($currency == 0) ? "DP" : "VP"; // 0 = dp, other = vp
            
            $shopSoapCommand = self::GetShopItemQuery($id);
            
            // Check if the accountid is matching the account name
            $checkedAccName = strtolower(Auth::GetAccountName($accountID));
            
            // Check if character with the provided name actually exists on this account:
            $isValidChar = self::AccHasCharNamed($accountID, $playerName);
            
            if($accountName === $checkedAccName && $isValidChar == true)
            {
                $shopSoapCommand = str_replace("{PLAYER}", $playerName, $shopSoapCommand);
                $shopSoapCommand = str_replace("{ACCOUNT}", $accountName, $shopSoapCommand);
        
                if (self::GetUserBalance($accountID, $currency) < self::GetShopItemPrice($id, $currency))
                {
                    $jsonObj->responseMsg = "Sorry, not enough ".$balanceName."!";
                    $jsonObj->response = false;
                }
                else
                {
                    // stops exploit that allows buying shop items with currency cost 0 if other currency type is not 0
                    if ( ($currency == 0 /*DP*/ && self::GetShopItemPrice($id, 0 /*DP*/) == 0 && self::GetShopItemPrice($id, 1 /*VP*/) != 0)
                        || ($currency == 1 /*VP*/ && self::GetShopItemPrice($id, 1 /*VP*/) == 0 && self::GetShopItemPrice($id, 0 /*DP*/) != 0) )
                    {
                        $jsonObj->responseMsg = "Oh fuck, I see what you doing :)";
                        $jsonObj->response = false;
                    }
                    else
                    {
                        
                        if (self::SpendUserBalance($user, $pass, $currency, self::GetShopItemPrice($id, $currency)))
                        {
                            global $config;

                            $json = new \stdClass();
                            // if there are multiple soap commands in the query, run all one 1 by 1
                            foreach(explode(PHP_EOL, $shopSoapCommand) as $cmd)
                            {
                                $json = SoapHandler::SendRequest($config['soap'][self::GetShopItemRealmId($id)]['user'], 
                                        $config['soap'][self::GetShopItemRealmId($id)]['pass'], 
                                            $cmd, self::GetShopItemRealmId($id), $user);
                            }

                            ob_end_clean();
            
                            $transaction = json_decode($json);
                        
                            if ($transaction->success)
                            {
                                $jsonObj->responseMsg = "Successfully spent ".self::GetShopItemPrice($id, $currency)." ".$balanceName."!";
                                $jsonObj->response = true;
                            }
                            else
                            {
                                $jsonObj->responseMsg = "Server error: ".$transaction->responseMsg;
                                $jsonObj->response = false;
                            }
                        }
                        else
                        {
                            $jsonObj->responseMsg = "Sorry, not enough ".$balanceName."!";
                            $jsonObj->response = false;
                        }
                    }
                }
            }
            else
            {
                $jsonObj->responseMsg = "Invalid account data!? -- ".$checkedAccName;
                $jsonObj->response = false;
            }
        }

        echo json_encode($jsonObj, JSON_PRETTY_PRINT);
    }
}

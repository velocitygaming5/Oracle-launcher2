<?php
header('Content-Type: application/json; charset=UTF-8');

class Vote
{
    private static function GetSiteCooldownByID($siteID)
    {
        $mysqli = Web::NewDBConnection();

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT time FROM votes WHERE id = ?'))
            {
                $query->bind_param('i', $siteID);
                $query->execute();
                $query->bind_result($CDSeconds);

                while ($query->fetch()) 
                {
                    return $CDSeconds*3600;
                }
            }
            $query->close();
        }
        // else
        // {
            // echo("Error description: " . $mysqli->error);
        // }
        mysqli_close($mysqli);
        return 0;
    }

    private static function GetSitePointsAmountByID($siteID)
    {
        $mysqli = Web::NewDBConnection();

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT points FROM votes WHERE id = ?'))
            {
                $query->bind_param('i', $siteID);
                $query->execute();
                $query->bind_result($points);

                while ($query->fetch()) 
                {
                    return $points;
                }
            }
            $query->close();
        }
        // else
        // {
            // echo("Error description: " . $mysqli->error);
        // }
        mysqli_close($mysqli);
        return 0;
    }

    public static function GetAccountSiteIDCooldown($accountID, $siteID)
    {
        $mysqli = Web::NewDBConnection();

        $pSiteCD = self::GetSiteCooldownByID($siteID);

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('SELECT max(lasttime), UNIX_TIMESTAMP() AS unixNow FROM votes_logs WHERE idaccount = ? AND idvote = ?'))
            {
                $query->bind_param('ii', $accountID, $siteID);
                $query->execute();
                $query->bind_result($dateVoted, $unixNow);

                while ($query->fetch()) 
                {
                    if ($dateVoted != 0)
                    {
                        if (($unixNow - $dateVoted) >= $pSiteCD)
                        {
                            return 0;
                        }
                        else
                        {
                            return $pSiteCD - ($unixNow - $dateVoted);
                        }
                    }
                }

                $query->close();
            }
        }
        // else
        // {
            // echo("Error description: " . $mysqli->error);
        // }
        mysqli_close($mysqli);
        return 0;
    }

    public static function GetVotesList($user, $pass)
    {
        if (Auth::IsValidLogin($user, $pass))
        {
            global $config;

            $mysqli = Web::NewDBConnection();

            $accountID = Auth::GetAccountId($user);

            mysqli_set_charset($mysqli, "utf8");

            if ($mysqli == true)
            {
                if ($query = $mysqli->prepare('SELECT id, name, image, url, points FROM votes'))
                {
                    $query->execute();
                    $query->bind_result($siteID, $siteName, $imageUrl, $voteUrl, $points);
                    $query->store_result();

                    $jsonArray = array();

                    while ($query->fetch()) 
                    {
                        $cooldownSecLeft = self::GetAccountSiteIDCooldown($accountID, $siteID);

                        $rowArray['siteID'] = $siteID;
                        $rowArray['siteName'] = $siteName;
                        $rowArray['cooldownSecLeft'] = $cooldownSecLeft;
                        $rowArray['imageUrl'] = $imageUrl;
                        $rowArray['voteUrl'] = $voteUrl;
                        $rowArray['points'] = $points;

                        array_push($jsonArray, $rowArray);
                    }
                    echo json_encode($jsonArray, JSON_PRETTY_PRINT);
                    $query->close();
                }
                // else
                // {
                    // echo("Error description: " . $mysqli->error);
                // }
                mysqli_close($mysqli);
            }
        }
    }
    
    public static function RegisterVoteCooldown($accountID, $siteID, $points)
    {
        $mysqli = Web::NewDBConnection();

        mysqli_set_charset($mysqli, "utf8");

        $pIP = $_SERVER['REMOTE_ADDR'];

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('INSERT INTO votes_logs (idvote, idaccount, points, lasttime, expired_at) 
                VALUES (?, ?, ?, UNIX_TIMESTAMP(), UNIX_TIMESTAMP() + ?)'))
            {
                $expired_at = self::GetSiteCooldownByID($siteID);

                $query->bind_param('iiii', $siteID, $accountID, $points, $expired_at);
                $query->execute();

                if ($mysqli->affected_rows != 0)
                {
                    return true;
                }

                $query->close();
            }
        }

        mysqli_close($mysqli);

        return false;
    }
    
    public static function RewardAccountVotePoints($accountID, $points)
    {
        $mysqli = Web::NewDBConnection();

        mysqli_set_charset($mysqli, "utf8");

        if ($mysqli == true)
        {
            if ($query = $mysqli->prepare('UPDATE `users` SET vp = vp + ? WHERE id = ?'))
            {
                $query->bind_param('ii', $points, $accountID);
                $query->execute();
    
                if ($mysqli->affected_rows != 0)
                {
                    return true;
                }
                $query->close();
            }   
        }

        mysqli_close($mysqli);

        return false;
    }
    
    public static function SelfVoteClick($user, $pass, $siteID)
    {
        $jsonObj = new \stdClass();
        $jsonObj->responseMsg = "Invalid authentification";
        $jsonObj->voteRegistered = false;

        if (Auth::IsValidLogin($user, $pass))
        {
            global $config;

            $accountID = Auth::GetAccountId($user);
            $points = self::GetSitePointsAmountByID($siteID);
            $cooldown = self::GetAccountSiteIDCooldown($accountID, $siteID);

            if ($cooldown <= 0)
            {
                if (self::RegisterVoteCooldown(Auth::GetAccountId($user), $siteID, $points))
                {
                    if (self::RewardAccountVotePoints($accountID, $points))
                    {
                        $jsonObj->responseMsg = "Vote registered, you earned ".$points.".";
                        $jsonObj->voteRegistered = true;
                    }
                    else
                    {
                        $jsonObj->responseMsg = "Vote not registered, no rows were affected for account.";
                    }
                }
                else
                {
                    $jsonObj->responseMsg = "Vote not registered, no rows were affected for cooldowns.";
                }
            }
            else
            {
                $jsonObj->responseMsg = "Vote not registered, cooldown left: ".$cooldown." seconds!";
            }
        }
        echo json_encode($jsonObj, JSON_PRETTY_PRINT);
    }
}

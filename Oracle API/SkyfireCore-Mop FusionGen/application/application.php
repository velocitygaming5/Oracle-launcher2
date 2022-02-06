<?php
header('Content-Type: application/json');
// error_reporting(0);

// config
include 'config.php';

// helpers
include(dirname(__FILE__)."/helpers/tools.php");

// libs
include(dirname(__FILE__)."/libs/srp6_lib.php");

// main
include(dirname(__FILE__)."/main/auth.php");
include(dirname(__FILE__)."/main/characters.php");
include(dirname(__FILE__)."/main/launcher.php");
include(dirname(__FILE__)."/main/web.php");

// modules
include(dirname(__FILE__)."/modules/avatars.php");
include(dirname(__FILE__)."/modules/ci_sessions.php");
include(dirname(__FILE__)."/modules/characters_market.php");
include(dirname(__FILE__)."/modules/discord.php");
include(dirname(__FILE__)."/modules/gamemaster.php");
include(dirname(__FILE__)."/modules/news.php");
include(dirname(__FILE__)."/modules/notifications.php");
include(dirname(__FILE__)."/modules/vote.php");
include(dirname(__FILE__)."/modules/shop.php");
include(dirname(__FILE__)."/modules/sins_history.php");
include(dirname(__FILE__)."/modules/soapclient.php");

// oracle
include(dirname(__FILE__)."/oracle/oracle_updater.php");

if(!isset($_SESSION))
{
    session_start();
}

            

if(isset($_POST['type']))
{
    switch ($_POST['type']) 
    {
        // Temporary encryption method
        if(isset($_POST['user']) && isset($_POST['pass']))
        {
            $_POST['user'] = base64_decode($_POST['user'], false);
            $_POST['pass'] = base64_decode($_POST['pass'], false);
        }

        // AVATARS
        case 'db_avatars_list':
        {
            Avatars::GetDBAvatars($_POST['user'], $_POST['pass']);
            break;
        }
        case 'get_self_avatar':
        {
            Avatars::GetSelfAvatar($_POST['user'], $_POST['pass']);
            break;
        }
        case 'set_self_avatar':
        {
            Avatars::SetSelfAvatar($_POST['user'], $_POST['pass'], $_POST['db_avatar_url']);
            break;
        }
        // AUTH
        case 'login_response':
        {
            Auth::GetLoginResponse($_POST['user'], $_POST['pass']);
            break;
        }
        case 'secpa_response':
        {
            Auth::GetSecPaResponse($_POST['user'], $_POST['pass'], $_POST['md5secpa']);
            break;
        }
        case 'account_rank_name':
        {
            Auth::GetAccountRankName($_POST['user'], $_POST['pass']);
            break;
        }
        case 'account_standing':
        {
            Auth::GetAccountStanding($_POST['user'], $_POST['pass']);
            break;
        }
        case 'account_balance':
        {
            Web::GetAccountBalance($_POST['user'], $_POST['pass']);
            break;
        }
        // CI_SESSIONS
        case 'ping_me_alive':
        {
            CiSessions::PingMeAlive($_POST['user']);
            break;
        }
        case 'ci_active_sessions_list':
        {
            CiSessions::GetActiveSessionsList($_POST['user'], $_POST['pass'], $_POST['md5secpa']);
            break;
        }
        // CHARACTERS
        case 'characters_list':
        {
            Characters::GetCharactersList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'realm_characters_list':
        {
            Characters::GetRealmCharactersList($_POST['user'], $_POST['pass'], $_POST['realmid']);
            break;
        }
        case 'top_pvp_list':
        {
            Characters::GetTopPvPList($_POST['limit']);
            break;
        }
        case 'online_list':
        {
            Characters::GetOnlinePlayersList();
            break;
        }
        // DISCORD
        case 'discord_issue_report':
        {
            Discord::NewIssueReport($_POST['by'], $_POST['version'], $_POST['at'], $_POST['issue']);
            break;
        }
        // GAME MASTER || ADMIN
        case 'gmpanel_access':
        {
            GameMaster::CanAccessGMPanel($_POST['user'], $_POST['pass']);
            break;
        }
        case 'adminpanel_access':
        {
            GameMaster::CanAccessAdminPanel($_POST['user'], $_POST['pass']);
            break;
        }
        case 'tickets_list':
        {
            GameMaster::GetTicketsList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'all_bans_list':
        {
            GameMaster::GetAllBansList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'mute_logs':
        {
            GameMaster::GetMuteLogs($_POST['user'], $_POST['pass']);
            break;
        }
        case 'shop_logs':
        {
            GameMaster::GetShopLogs($_POST['user'], $_POST['pass']);
            break;
        }
        case 'command_logs':
        {
            GameMaster::GetCommandLogs($_POST['user'], $_POST['pass'], $_POST['md5secpa']);
            break;
        }
        case 'soap_logs':
        {
            GameMaster::GetSoapLogs($_POST['user'], $_POST['pass'], $_POST['md5secpa']);
            break;
        }
        case 'pinfo':
        {
            GameMaster::GetPlayerInfo($_POST['user'], $_POST['pass'], $_POST['playername'], $_POST['realmid']);
            break;
        }
        case 'unban_account':
        {
            GameMaster::UnbanAccount($_POST['user'], $_POST['pass'], $_POST['targetacc']);
            break;
        }
        case 'realms_list':
        {
            GameMaster::GetRealms($_POST['user'], $_POST['pass']);
            break;
        }
        case 'gmranks_list':
        {
            GameMaster::GMRanks($_POST['user'], $_POST['pass']);
            break;
        }
        // HELPERS
        case 'files_list':
        {
            Tools::GetFilesList($_POST['expansion'], "");
            break;
        }
        case 'launcher_version':
        {
            Tools::GetLauncherVersion();
            break;
        }
        case 'patches_whitelist':
        {
            Tools::GetPatchesWhitelist();
            break;
        }
        // NEWS
        case 'news_list':
        {
            News::GetLatestNews($_POST['expansionid'], $_POST['limit']);
            break;
        }
        case 'news_article_create':
        {
            News::CreateArticle($_POST['user'], $_POST['pass'], $_POST['md5secpa'],
                $_POST['expansionid'], $_POST['newtitle'], $_POST['newarticleurl'], $_POST['newimageurl']);
            break;
        }
        case 'news_article_edit':
        {
            News::EditArticle($_POST['user'], $_POST['pass'], $_POST['md5secpa'],
                $_POST['articleid'], $_POST['expansionid'], $_POST['newtitle'], $_POST['newarticleurl'], $_POST['newimageurl']);
            break;
        }
        case 'news_article_delete':
        {
            News::DeleteArticle($_POST['user'], $_POST['pass'], $_POST['md5secpa'], $_POST['articleid'], $_POST['expansionid']);
            break;
        }
        case 'news_list':
        {
            News::GetLatestNews($_POST['expansionid'], $_POST['limit']);
            break;
        }
        // NOTIFICATIONS
        case 'notifications_list':
        {
            Notifications::GetNotificationsList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'notifications_list_as_admin':
        {
            Notifications::GetNotificationsListAdmin($_POST['user'], $_POST['pass'], $_POST['md5secpa']);
            break;
        }
        case 'notification_mark_as_read':
        {
            Notifications::MarkNotificationAsRead($_POST['user'], $_POST['pass'], $_POST['notificationid']);
            break;
        }
        case 'notification_create':
        {
            Notifications::CreateNotification($_POST['user'], $_POST['pass'], $_POST['md5secpa'],
                $_POST['newsubject'], $_POST['newmessage'], $_POST['newimageurl'], $_POST['newredirecturl'], $_POST['newaccountid']);
            break;
        }
        case 'notification_delete':
        {
            Notifications::DeleteNotification($_POST['user'], $_POST['pass'], $_POST['md5secpa'], $_POST['notificationid']);
            break;
        }
        // ORACLE
        case 'oracle_updater_list':
        {
            if( isset($_SERVER['HTTPS'] ) )
            {
                OracleUpdater::GetOracleUpdaterFilesList((((!empty($_SERVER['HTTPS']) && $_SERVER['HTTPS'] != 'off') || $_SERVER['SERVER_PORT'] == 443) ? "https://".$_SERVER['SERVER_NAME'] : "https://".$_SERVER['SERVER_NAME']).dirname($_SERVER['PHP_SELF']));
            }
            else
            {
               OracleUpdater::GetOracleUpdaterFilesList((((!empty($_SERVER['HTTP']) && $_SERVER['HTTP'] != 'off') || $_SERVER['SERVER_PORT'] == 443) ? "http://".$_SERVER['SERVER_NAME'] : "http://".$_SERVER['SERVER_NAME']).dirname($_SERVER['PHP_SELF']));
            }
            break;
        }
        // VOTE
        case 'vote_sites_list':
        {
            Vote::GetVotesList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'self_vote_click':
        {
            Vote::SelfVoteClick($_POST['user'], $_POST['pass'], $_POST['siteid']);
            break;
        }
        // SHOP
        case 'shop_list':
        {
            Shop::GetShoplist($_POST['user'], $_POST['pass']);
            break;
        }
        case 'shop_purchase':
        {
            Shop::PurchaseId($_POST['user'], $_POST['pass'], $_POST['id'],
                $_POST['currencyType'], $_POST['playerName'], $_POST['accountName']);
            break;
        }
        // CHARACTERS MARKET
        case 'characters_market_list':
        {
            CharactersMarket::GetMarketList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'characters_market_own_list':
        {
            CharactersMarket::GetOwnListing($_POST['user'], $_POST['pass']);
            break;
        }
        case 'characters_market_purchase_id':
        {
            CharactersMarket::PurchaseId($_POST['market_id'], $_POST['user'], $_POST['pass']);
            break;
        }
        case 'characters_market_own_characters_list':
        {
            CharactersMarket::GetOwnCharactersList($_POST['user'], $_POST['pass']);
            break;
        }
        case 'characters_market_sell_character':
        {
            CharactersMarket::ListCharacterForSale($_POST['user'], $_POST['pass'], $_POST['guid'], $_POST['price_dp'], $_POST['realm_id']);
            break;
        }
        case 'characters_market_cancel_sale':
        {
            CharactersMarket::CancelCharacterSale($_POST['user'], $_POST['pass'], $_POST['guid'], $_POST['realm_id']);
            break;
        }
        // SINS HISTORY
        case 'sins_history_list':
        {
            SinsHistory::GetSinsHistory($_POST['user'], $_POST['pass']);
            break;
        }
        // SOAP
        case 'raw_command':
        {
            if (Auth::IsValidSecPa($_POST['user'], $_POST['pass'], $_POST['md5secpa']))
            {
                SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                $_POST['command'],
                $_POST['realmid'],
                $_POST['user']);
            }
            break;
        }
        case 'unban_character':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'unban character '.$_POST['playername'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'kick_player':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'kick '.$_POST['playername'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'unstuck_player':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'unstuck '.$_POST['playername'].' inn',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ticket_close':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'ticket close '.$_POST['ticketid'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ticket_delete':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'ticket delete '.$_POST['ticketid'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ticket_assign':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'ticket assign '.$_POST['ticketid'].' '.$_POST['gmname'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ticket_unassign':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'ticket unassign '.$_POST['ticketid'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ticket_reply':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'send mail '.$_POST['charname'].' "Custommer Support" "'.$_POST['reply'].'"',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ban_account':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'ban account '.$_POST['accountname'].' '.$_POST['bantime'].' '.$_POST['banreason'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'ban_character':
        {
            SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                'ban character '.$_POST['charname'].' '.$_POST['bantime'].' '.$_POST['banreason'].'',
                $_POST['realmid'],
                $_POST['user']);
            break;
        }
        case 'set_gmlevel':
        {
            if (Auth::IsValidSecPa($_POST['user'], $_POST['pass'], $_POST['md5secpa']))
            {
                SoapHandler::SendRequest($_POST['user'], $_POST['pass'],
                    'account set gmlevel '.$_POST['accountname'].' '.$_POST['gmlevel'].' '.$_POST['gmrealmid'].'',
                    $_POST['realmid'],
                    $_POST['user']);
            }
            break;
        }
        case 'set_password':
        {
            if (Auth::IsValidSecPa($_POST['user'], $_POST['pass'], $_POST['md5secpa']))
            {
                SoapHandler::SendRequest($_POST['user'], $_POST['pass'], 
                    'account set password '.$_POST['accountname'].' '.$_POST['newpassword'].' '.$_POST['newpassword'].'',
                    $_POST['realmid'],
                $_POST['user']);
            }
            break;
        }
        default:
        {
            break;
        }
    }
}

if (isset($_GET["test"]))
{
    // empty
    Tools::GetLauncherVersion();
}
-- ----------------------------------------------
-- ORACLE LAUNCHER DB SETUP
-- ----------------------------------------------

DROP TABLE IF EXISTS `avatars_list`;
CREATE TABLE IF NOT EXISTS `avatars_list` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(250) NOT NULL DEFAULT 'Unknown' COLLATE 'utf8_general_ci',
	`url` VARCHAR(250) NOT NULL DEFAULT 'http://localhost/launcher/application/avatars/default.jpg' COLLATE 'utf8_general_ci',
	PRIMARY KEY (`id`) USING BTREE,
	UNIQUE INDEX `url` (`url`) USING BTREE
) COLLATE='utf8_general_ci' ENGINE=InnoDB AUTO_INCREMENT=0;
INSERT INTO `avatars_list` (`id`, `name`, `url`) VALUES (1, 'Default', 'http://localhost/launcher/application/avatars/default.jpg');

DROP TABLE IF EXISTS `characters_market`;
CREATE TABLE IF NOT EXISTS `characters_market` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`seller_account` INT(11) NOT NULL DEFAULT '0',
	`guid` INT(11) NOT NULL,
	`price_dp` INT(11) NOT NULL DEFAULT '0',
	`realm_id` INT(11) NOT NULL DEFAULT '1',
	`date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`realm_id`, `guid`) USING BTREE,
	UNIQUE INDEX `id` (`id`) USING BTREE
)
COLLATE='utf8_general_ci' ENGINE=InnoDB AUTO_INCREMENT=0;

DROP TABLE IF EXISTS `characters_market_logs`;
CREATE TABLE IF NOT EXISTS `characters_market_logs` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`buyer_id` INT(11) NOT NULL,
	`seller_id` INT(11) NOT NULL,
	`market_id` INT(11) NOT NULL,
	`character_guid` INT(11) NOT NULL,
	`price_dp` INT(11) NOT NULL,
	`date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`) USING BTREE
)
COLLATE='latin1_swedish_ci' ENGINE=InnoDB AUTO_INCREMENT=0;

DROP TABLE IF EXISTS `ci_sessions`;
CREATE TABLE IF NOT EXISTS `ci_sessions` (
	`account_name` VARCHAR(255) NOT NULL COLLATE 'latin1_swedish_ci',
	`last_session_id` VARCHAR(255) NOT NULL COLLATE 'latin1_swedish_ci',
	`last_ip` VARCHAR(250) NOT NULL COLLATE 'latin1_swedish_ci',
	`last_seen` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`account_name`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

DROP TABLE IF EXISTS `ci_sessions_geo`;
CREATE TABLE IF NOT EXISTS `ci_sessions_geo`  (
  `user` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `city` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `country` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `isp` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `lat` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `lon` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ip` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `regionName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `last_seen` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`user`) USING BTREE,
  UNIQUE INDEX `user`(`user`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

DROP TABLE IF EXISTS `news`;
CREATE TABLE IF NOT EXISTS `news` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`title` VARCHAR(250) NOT NULL DEFAULT 'New Article' COLLATE 'utf8_general_ci',
	`date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`articleUrl` VARCHAR(250) NOT NULL COLLATE 'utf8_general_ci',
	`imageUrl` VARCHAR(250) NOT NULL DEFAULT '' COLLATE 'utf8_general_ci',
	`expansionID` INT(11) NOT NULL,
	PRIMARY KEY (`id`, `expansionID`) USING BTREE
) COLLATE='utf8_general_ci' ENGINE=InnoDB AUTO_INCREMENT=0;

DROP TABLE IF EXISTS `notifications`;
CREATE TABLE IF NOT EXISTS `notifications` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`subject` VARCHAR(250) NOT NULL DEFAULT '' COLLATE 'latin1_swedish_ci',
	`message` VARCHAR(250) NOT NULL DEFAULT '' COLLATE 'latin1_swedish_ci',
	`imageUrl` VARCHAR(250) NULL DEFAULT NULL COLLATE 'latin1_swedish_ci',
	`redirectUrl` VARCHAR(1000) NOT NULL COLLATE 'latin1_swedish_ci',
	`accountID` INT(11) NOT NULL DEFAULT '0' COMMENT '0 = everyone',
	PRIMARY KEY (`id`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB AUTO_INCREMENT=0;

DROP TABLE IF EXISTS `notifications_read`;
CREATE TABLE IF NOT EXISTS `notifications_read` (
	`accountID` INT(11) NOT NULL,
	`notificationID` INT(11) NOT NULL,
	`dateRead` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`accountID`, `notificationID`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

DROP TABLE IF EXISTS `patches_whitelist`;
CREATE TABLE `patches_whitelist` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`path` VARCHAR(255) NOT NULL COLLATE 'latin1_swedish_ci',
	`enable` TINYINT(4) NOT NULL DEFAULT '1',
	PRIMARY KEY (`id`) USING BTREE,
	UNIQUE INDEX `path` (`path`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB AUTO_INCREMENT=0;

INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\common.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\common-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\expansion.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\lichking.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\patch.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\patch-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\patch-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\backup-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\base-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\expansion-locale-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\expansion-speech-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\lichking-locale-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\lichking-speech-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\locale-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\patch-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\patch-enus-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\patch-enus-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\enus\\speech-enus.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\backup-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\base-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\expansion-locale-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\expansion-speech-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\lichking-locale-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\lichking-speech-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\locale-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\patch-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\patch-engb-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\patch-engb-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\engb\\speech-engb.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\backup-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\base-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\expansion-locale-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\expansion-speech-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\lichking-locale-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\lichking-speech-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\locale-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\patch-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\patch-dede-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\patch-dede-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\dede\\speech-dede.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\backup-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\base-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\expansion-locale-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\expansion-speech-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\lichking-locale-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\lichking-speech-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\locale-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\patch-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\patch-eses-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\patch-eses-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\eses\\speech-eses.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\backup-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\base-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\expansion-locale-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\expansion-speech-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\lichking-locale-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\lichking-speech-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\locale-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\patch-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\patch-esmx-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\patch-esmx-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\esmx\\speech-esmx.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\backup-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\base-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\expansion-locale-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\expansion-speech-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\lichking-locale-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\lichking-speech-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\locale-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\patch-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\patch-frfr-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\patch-frfr-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\frfr\\speech-frfr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\backup-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\base-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\expansion-locale-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\expansion-speech-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\lichking-locale-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\lichking-speech-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\locale-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\patch-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\patch-krkr-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\patch-krkr-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\krkr\\speech-krkr.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\backup-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\base-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\expansion-locale-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\expansion-speech-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\lichking-locale-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\lichking-speech-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\locale-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\patch-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\patch-ptBR-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\patch-ptBR-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ptBR\\speech-ptBR.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\backup-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\base-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\expansion-locale-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\expansion-speech-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\lichking-locale-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\lichking-speech-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\locale-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\patch-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\patch-ruru-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\patch-ruru-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\ruru\\speech-ruru.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\backup-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\base-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\expansion-locale-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\expansion-speech-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\lichking-locale-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\lichking-speech-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\locale-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\patch-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\patch-zhcn-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\patch-zhcn-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhcn\\speech-zhcn.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\backup-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\base-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\expansion-locale-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\expansion-speech-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\lichking-locale-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\lichking-speech-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\locale-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\patch-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\patch-zhtw-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\patch-zhtw-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\zhtw\\speech-zhtw.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\backup-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\base-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\expansion-locale-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\expansion-speech-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\lichking-locale-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\lichking-speech-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\locale-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\patch-itit.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\patch-itit-2.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\patch-itit-3.mpq', 1);
INSERT INTO `patches_whitelist` (`path`, `enable`) VALUES ('data\\itit\\speech-itit.mpq', 1);


DROP TABLE IF EXISTS `soap_logs`;
CREATE TABLE IF NOT EXISTS `soap_logs` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`account_id` INT(11) NOT NULL DEFAULT '0',
	`account_name` VARCHAR(250) NOT NULL DEFAULT 'Unknown' COLLATE 'latin1_swedish_ci',
	`date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`realm_id` INT(11) NOT NULL DEFAULT '0',
	`command` VARCHAR(1000) NULL DEFAULT NULL COLLATE 'latin1_swedish_ci',
	PRIMARY KEY (`id`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

DROP TABLE IF EXISTS `user_avatars`;
CREATE TABLE IF NOT EXISTS `user_avatars` (
	`account_id` INT(11) NOT NULL,
	`avatar_url` VARCHAR(250) NOT NULL COLLATE 'latin1_swedish_ci',
	UNIQUE INDEX `account_id` (`account_id`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

DROP TABLE IF EXISTS `shop_list`;
CREATE TABLE IF NOT EXISTS `shop_list` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`title` VARCHAR(250) NOT NULL COLLATE 'latin1_swedish_ci',
	`description` VARCHAR(255) NOT NULL COLLATE 'latin1_swedish_ci',
	`img_url` VARCHAR(250) NOT NULL COLLATE 'latin1_swedish_ci',
	`price_dp` INT(11) NOT NULL DEFAULT '0',
	`price_vp` INT(11) NOT NULL DEFAULT '0',
	`category` INT(11) NOT NULL DEFAULT '1' COMMENT '1 = service,\r\n2 = bundle,\r\n3 = item,\r\n4 = mount,\r\n5 = pet',
	`soap_command` VARCHAR(10000) NOT NULL COMMENT '{PLAYER} = player name\r\n{ACCOUNT} = account name' COLLATE 'latin1_swedish_ci',
	`realm_id` TINYINT(4) NOT NULL DEFAULT '1',
	PRIMARY KEY (`id`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB AUTO_INCREMENT=0;

INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (1, 'Warglaive of Azzinoth', 'Main hand Warglaive of Azzinoth', 'https://i.pinimg.com/originals/f5/f9/6a/f5f96ae070586b5e4c96e79339841523.png', 100, 1500, 3, 'send items {PLAYER} "Shop Receipt" "Thanks for your purchase(s)!" 32837:1', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (2, 'Warglaive of Azzinoth', 'Off hand Warglaive of Azzinoth', 'https://cdna.artstation.com/p/assets/images/images/024/452/678/original/stian-s-sundby-200223-wg-gif-03.gif?1582478756', 90, 1500, 3, 'send items {PLAYER} "Shop Receipt" "Thanks for your purchase(s)!" 32838:1', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (3, 'Illidan\'s Warglaives', 'Full set Warglaives of Azzinoth', 'https://buy-boost.com/static/data/product/376/Warglaives%20of%20Azzinoth%20Boost%201.jpg', 180, 2800, 2, 'send items {PLAYER} "Shop Receipt" "Thanks for your purchase(s)!" 32838:1 32837:1', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (4, 'Faction Change', 'Change your character faction to Alliance or Horde', 'https://static.wikia.nocookie.net/wowpedia/images/a/a3/Faction_Change_service.jpg/revision/latest?cb=20180722014335', 100, 0, 1, 'character changefaction {PLAYER}', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (5, 'Race Change', 'Change your character race to any race of your current faction', 'https://theworldofmmo.com/wp-content/uploads/2020/10/Save-30-on-Select-Game-Services-Race-Change-Faction.jpg', 100, 0, 1, 'character changerace {PLAYER}', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (6, 'Appearance Change', 'Change your character appearance and name', 'https://d2skuhm0vrry40.cloudfront.net/2020/articles/2020-07-09-14-32/-1594301567485.jpg/EG11/resize/1200x-1/-1594301567485.jpg', 100, 0, 1, 'character customize {PLAYER}', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (7, 'Swift Flying Broom', 'Enjoy flying with the Magic Broom!', 'https://wow.zamimg.com/uploads/screenshots/normal/147609-swift-flying-broom.jpg', 80, 0, 4, 'send items {PLAYER} "Shop Receipt" "Thanks for your purchase(s)!" 33182:1', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (8, 'Phoenix Hatchling', 'Take care of a new companion, a pheonix!', 'https://static.wikia.nocookie.net/wowpedia/images/c/c2/Phoenix_Hatchling.jpg/revision/latest?cb=20080213095952', 25, 0, 5, 'send items {PLAYER} "Shop Receipt" "Thanks for your purchase(s)!" 35504:1', 1);
INSERT INTO `shop_list` (`id`, `title`, `description`, `img_url`, `price_dp`, `price_vp`, `category`, `soap_command`, `realm_id`) VALUES (9, 'Phoenix Hatchling on Realm 3', 'Take care of a new companion, a pheonix!', 'https://static.wikia.nocookie.net/wowpedia/images/c/c2/Phoenix_Hatchling.jpg/revision/latest?cb=20080213095952', 25, 0, 5, 'send items {PLAYER} "Shop Receipt" "Thanks for your purchase(s)!" 35504:1', 3);

CREATE TABLE `promo_codes` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`code` VARCHAR(100) NOT NULL COLLATE 'latin1_swedish_ci',
	`dp_points` INT(11) NOT NULL,
	`valid_until` DATETIME NOT NULL,
	`max_redeems` INT(11) NOT NULL DEFAULT '-1',
	PRIMARY KEY (`id`) USING BTREE
)
COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

CREATE TABLE `promo_redeems` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`account_id` INT(11) NOT NULL,
	`code` VARCHAR(100) NOT NULL COLLATE 'latin1_swedish_ci',
	`date_redeemed` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`) USING BTREE,
	UNIQUE INDEX `code` (`code`) USING BTREE
)
COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

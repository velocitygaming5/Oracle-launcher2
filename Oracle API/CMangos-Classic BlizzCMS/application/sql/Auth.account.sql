ALTER TABLE `account`
	ADD COLUMN `sec_pa` VARCHAR(100) NULL DEFAULT NULL AFTER `token`;
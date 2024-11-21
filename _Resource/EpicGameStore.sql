-- MySQL Script generated by MySQL Workbench
-- Thu Nov 21 16:43:10 2024
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema EpicGameWebApp
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `EpicGameWebApp` ;

-- -----------------------------------------------------
-- Schema EpicGameWebApp
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `EpicGameWebApp` DEFAULT CHARACTER SET utf8 ;
USE `EpicGameWebApp` ;

-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Publisher`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Publisher` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Publisher` (
  `PublisherID` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NULL DEFAULT NULL,
  `Address` VARCHAR(255) NULL DEFAULT NULL,
  `Email` VARCHAR(255) NULL DEFAULT NULL,
  `Phone` VARCHAR(255) NULL DEFAULT NULL,
  `Website` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`PublisherID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Genre`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Genre` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Genre` (
  `GenreID` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`GenreID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Role` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Role` (
  `RoleId` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NULL,
  `Permission` JSON NULL,
  PRIMARY KEY (`RoleId`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Account`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Account` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Account` (
  `AccountID` INT NOT NULL AUTO_INCREMENT,
  `RoleId` INT NULL,
  `IsActive` ENUM('N', 'Y') NOT NULL DEFAULT 'Y',
  `Username` VARCHAR(45) NULL DEFAULT NULL,
  `Password` VARCHAR(45) NULL DEFAULT NULL,
  `Email` NVARCHAR(255) NULL DEFAULT NULL,
  `CreatedOn` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`AccountID`),
  CONSTRAINT `FK_Account_Role`
    FOREIGN KEY (`RoleId`)
    REFERENCES `EpicGameWebApp`.`Role` (`RoleId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `FK_Account_Role_INDEX` ON `EpicGameWebApp`.`Account` (`RoleId` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`PaymentMethod`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`PaymentMethod` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`PaymentMethod` (
  `PaymentMethodID` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`PaymentMethodID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Cart`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Cart` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Cart` (
  `CartID` INT NOT NULL AUTO_INCREMENT,
  `AccountID` INT NOT NULL,
  `PaymentMethodID` INT NOT NULL,
  `TotalAmount` DECIMAL(8,2) NULL DEFAULT NULL,
  `CreatedOn` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`CartID`),
  CONSTRAINT `FK_Cart_Account`
    FOREIGN KEY (`AccountID`)
    REFERENCES `EpicGameWebApp`.`Account` (`AccountID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_Cart_PaymentMethod`
    FOREIGN KEY (`PaymentMethodID`)
    REFERENCES `EpicGameWebApp`.`PaymentMethod` (`PaymentMethodID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `AccountID_INDEX` ON `EpicGameWebApp`.`Cart` (`AccountID` ASC) VISIBLE;

CREATE INDEX `PaymentMethodID_INDEX` ON `EpicGameWebApp`.`Cart` (`PaymentMethodID` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Game`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Game` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Game` (
  `GameID` INT NOT NULL AUTO_INCREMENT,
  `PublisherID` INT NULL DEFAULT NULL,
  `GenreID` INT NULL DEFAULT NULL,
  `ImageID` INT NULL DEFAULT NULL,
  `Title` NVARCHAR(255) NULL DEFAULT NULL,
  `Price` DECIMAL(8,2) NULL DEFAULT NULL,
  `Author` NVARCHAR(255) NULL DEFAULT NULL,
  `Release` DATETIME NULL DEFAULT NULL,
  `Rating` DECIMAL(2,1) NULL DEFAULT NULL,
  `Description` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`GameID`),
  CONSTRAINT `FK_Game_Publisher`
    FOREIGN KEY (`PublisherID`)
    REFERENCES `EpicGameWebApp`.`Publisher` (`PublisherID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `FK_Game_Genre`
    FOREIGN KEY (`GenreID`)
    REFERENCES `EpicGameWebApp`.`Genre` (`GenreID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;

CREATE INDEX `PublisherID_INDEX` ON `EpicGameWebApp`.`Game` (`PublisherID` ASC) VISIBLE;

CREATE INDEX `GenreID_INDEX` ON `EpicGameWebApp`.`Game` (`GenreID` ASC) INVISIBLE;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`CartDetail`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`CartDetail` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`CartDetail` (
  `CartDetailID` INT NOT NULL AUTO_INCREMENT,
  `CartID` INT NOT NULL,
  `GameID` INT NOT NULL,
  `Price` DECIMAL(8,2) NULL DEFAULT NULL,
  `Discount` DECIMAL(5,2) NULL DEFAULT NULL,
  PRIMARY KEY (`CartDetailID`),
  CONSTRAINT `FK_CartDetail_Cart`
    FOREIGN KEY (`CartID`)
    REFERENCES `EpicGameWebApp`.`Cart` (`CartID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_CartDetail_Game`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Game` (`GameID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `CartID_INDEX` ON `EpicGameWebApp`.`CartDetail` (`CartID` ASC) VISIBLE;

CREATE INDEX `GameID_INDEX` ON `EpicGameWebApp`.`CartDetail` (`GameID` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Discount`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`Discount` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Discount` (
  `DiscountID` INT NOT NULL AUTO_INCREMENT,
  `GameID` INT NULL DEFAULT NULL,
  `Percent` DECIMAL(5,2) NULL DEFAULT NULL,
  `Code` VARCHAR(45) NULL DEFAULT NULL,
  `StartOn` DATETIME NULL DEFAULT NULL,
  `EndOn` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`DiscountID`),
  CONSTRAINT `FK_Discount_Game`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Game` (`GameID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `GameID_INDEX` ON `EpicGameWebApp`.`Discount` (`GameID` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`AccountGame`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`AccountGame` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`AccountGame` (
  `AccountGameId` INT NOT NULL AUTO_INCREMENT,
  `AccountID` INT NOT NULL,
  `GameID` INT NOT NULL,
  `DateAdded` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`AccountGameId`),
  CONSTRAINT `FK_AccountGame_Account`
    FOREIGN KEY (`AccountID`)
    REFERENCES `EpicGameWebApp`.`Account` (`AccountID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_AccountGame_Game`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Game` (`GameID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `FK_AccountGame_Account_INDEX` ON `EpicGameWebApp`.`AccountGame` (`AccountID` ASC) VISIBLE;

CREATE INDEX `FK_AccountGame_Game_INDEX` ON `EpicGameWebApp`.`AccountGame` (`GameID` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`ImageGame`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EpicGameWebApp`.`ImageGame` ;

CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`ImageGame` (
  `ImageID` INT NOT NULL AUTO_INCREMENT,
  `GameID` INT NOT NULL,
  `file_name` VARCHAR(255) NOT NULL,
  `file_path` VARCHAR(255) NULL,
  `create_at` DATETIME NULL,
  PRIMARY KEY (`ImageID`),
  CONSTRAINT `FK_Image_Game`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Game` (`GameID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `GameID_INDEX` ON `EpicGameWebApp`.`ImageGame` (`GameID` ASC) INVISIBLE;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- MySQL Script generated by MySQL Workbench
-- Fri Oct 11 13:15:05 2024
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
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Publisher` (
  `PublisherID` INT NOT NULL,
  `Name` VARCHAR(255) NULL,
  `Address` VARCHAR(255) NULL,
  `Email` VARCHAR(255) NULL,
  `Phone` VARCHAR(255) NULL,
  `Website` VARCHAR(255) NULL,
  PRIMARY KEY (`PublisherID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Genre`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Genre` (
  `GenreID` INT NOT NULL,
  `Name` VARCHAR(255) NULL,
  PRIMARY KEY (`GenreID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Cart`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Cart` (
  `CartID` INT NOT NULL,
  `GameID` INT NULL,
  `AccountID` INT NULL,
  `PaymentMethodID` INT NULL,
  `TotalAmount` DECIMAL(8,2) NULL,
  `CreatedOn` DATETIME NULL,
  PRIMARY KEY (`CartID`),
  INDEX `GameID_INDEX` (`GameID` ASC) INVISIBLE,
  INDEX `AccountID_INDEX` (`AccountID` ASC) INVISIBLE,
  INDEX `PaymentMethodID_INDEX` (`PaymentMethodID` ASC) INVISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`CartDetail`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`CartDetail` (
  `CartDetailID` INT NOT NULL,
  `CartID` INT NULL,
  `GameID` INT NULL,
  `Quantity` INT NULL,
  `Price` DECIMAL(8,2) NULL,
  `Discount` DECIMAL(5,2) NULL,
  PRIMARY KEY (`CartDetailID`),
  INDEX `CartID_INDEX` (`CartID` ASC) VISIBLE,
  INDEX `GameID_INDEX` (`GameID` ASC) VISIBLE,
  CONSTRAINT `FK_CartDetail_Cart`
    FOREIGN KEY (`CartID`)
    REFERENCES `EpicGameWebApp`.`Cart` (`CartID`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Game`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Game` (
  `GameID` INT NOT NULL,
  `PublisherID` INT NULL,
  `GenreID` INT NULL,
  `Title` NVARCHAR(255) NULL,
  `Price` DECIMAL(8,2) NULL,
  `Author` NVARCHAR(255) NULL,
  `Release` DATETIME NULL,
  `Rating` DECIMAL(2,1) NULL,
  `Description` LONGTEXT NULL,
  PRIMARY KEY (`GameID`),
  INDEX `PublisherID_INDEX` (`PublisherID` ASC) VISIBLE,
  INDEX `GenreID_INDEX` (`GenreID` ASC) VISIBLE,
  CONSTRAINT `FK_Game_Publisher`
    FOREIGN KEY (`PublisherID`)
    REFERENCES `EpicGameWebApp`.`Publisher` (`PublisherID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `FK_Game_Genre`
    FOREIGN KEY (`GenreID`)
    REFERENCES `EpicGameWebApp`.`Genre` (`GenreID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `FK_Game_CartDetail`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`CartDetail` (`GameID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `FK_Game_Cart`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Cart` (`GameID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Account`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Account` (
  `AccountID` INT NOT NULL,
  `GameID` INT NULL,
  `Username` VARCHAR(45) NULL,
  `Password` VARCHAR(45) NULL,
  `Email` NVARCHAR(255) NULL,
  `IsAdmin` ENUM('N', 'Y') NULL,
  `CreatedOn` DATETIME NULL,
  PRIMARY KEY (`AccountID`),
  INDEX `GameID_INDEX` (`GameID` ASC) VISIBLE,
  CONSTRAINT `FK_Account_Cart`
    FOREIGN KEY (`AccountID`)
    REFERENCES `EpicGameWebApp`.`Cart` (`AccountID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_Account_Game`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Game` (`GameID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`PaymentMethod`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`PaymentMethod` (
  `PaymentMethodID` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`PaymentMethodID`),
  CONSTRAINT `FK_PaymentMethod_Cart`
    FOREIGN KEY (`PaymentMethodID`)
    REFERENCES `EpicGameWebApp`.`Cart` (`PaymentMethodID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`Discount`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`Discount` (
  `DiscountID` INT NOT NULL,
  `GameID` INT NULL,
  `Percent` DECIMAL(5,2) NULL,
  `Code` VARCHAR(45) NULL,
  `StartOn` DATETIME NULL,
  `EndOn` DATETIME NULL,
  PRIMARY KEY (`DiscountID`),
  INDEX `GameID_INDEX` (`GameID` ASC) VISIBLE,
  CONSTRAINT `FK_Discount_Game`
    FOREIGN KEY (`GameID`)
    REFERENCES `EpicGameWebApp`.`Game` (`GameID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EpicGameWebApp`.`AccountGame`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `EpicGameWebApp`.`AccountGame` (
  `AccountID` INT NOT NULL,
  `GameID` INT NOT NULL,
  `DateAdded` DATETIME NULL,
  INDEX `FK_AccountGame_Account_idx` (`AccountID` ASC) VISIBLE,
  INDEX `FK_AccountGame_Game_idx` (`GameID` ASC) VISIBLE,
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


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

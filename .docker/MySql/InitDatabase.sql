USE `DistributedSkeleton`;

 CREATE TABLE `DistributedSkeleton`.`WeatherForecast` (
 	`Id` INT auto_increment NOT NULL,
 	`Date` DATETIME NULL,
 	`TemperatureC` INT NULL,
 	`TemperatureF` INT NULL,
 	`Summary` TEXT NULL,
 	CONSTRAINT WeatherForecast_PK PRIMARY KEY (Id)
 );
 
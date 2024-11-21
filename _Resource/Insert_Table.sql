-- Insert sample data into Roles table
INSERT INTO `EpicGameWebApp`.`Role` (`RoleId`, `Name`, `Permission`) VALUES
(1, 'Admin', '["read", "write", "delete", "manage_users", "manage_roles"]'),
(2, 'Moderator', '["read", "write", "delete"]'),
(3, 'User', '["read"]'),
(4, 'Editor', '["read", "write"]'),
(5, 'Guest', '[]');

-- Insert sample data into Publisher table
INSERT INTO `EpicGameWebApp`.`Publisher` (`PublisherID`, `Name`, `Address`, `Email`, `Phone`, `Website`) VALUES
(1, 'Electronic Arts', '209 Redwood Shores Parkway, Redwood City, CA', 'contact@ea.com', '650-628-1500', 'https://www.ea.com'),
(2, 'Ubisoft', '28 Rue Armand Carrel, 93100 Montreuil, France', 'contact@ubisoft.com', '+33 1 48 18 50 00', 'https://www.ubisoft.com'),
(3, 'Activision Blizzard', '3100 Ocean Park Blvd, Santa Monica, CA', 'contact@activisionblizzard.com', '310-255-2000', 'https://www.activisionblizzard.com'),
(4, 'Nintendo', '11-1 Kamitoba Hokotate-cho, Minami-ku, Kyoto, Japan', 'contact@nintendo.com', '+81 75-662-9600', 'https://www.nintendo.com'),
(5, 'Sony Interactive Entertainment', '2207 Bridgepointe Pkwy, San Mateo, CA', 'contact@playstation.com', '650-655-8000', 'https://www.playstation.com');

-- Insert sample data into Genre table
INSERT INTO `EpicGameWebApp`.`Genre` (`GenreID`, `Name`) VALUES
(1, 'Action'),
(2, 'Adventure'),
(3, 'Role-Playing'),
(4, 'Simulation'),
(5, 'Strategy'),
(6, 'Sports'),
(7, 'Shooter'),
(8, 'Platformer');

-- Insert sample data into Game table
INSERT INTO `EpicGameWebApp`.`Game` (`GameID`, `PublisherID`, `GenreID`, `ImageID`, `Title`, `Price`, `Author`, `Release`, `Rating`, `Description`) VALUES
(1, 1, 7, 1, 'Apex Legends', 0.00, 'Respawn Entertainment', '2019-02-04', 8.5, 'A free-to-play battle royale hero shooter game.'),
(2, 2, 1, 2, 'Assassin\'s Creed Valhalla', 59.99, 'Ubisoft Montreal', '2020-11-10', 8.2, 'An action role-playing game set in the Viking Age.'),
(3, 3, 3, 3, 'World of Warcraft', 14.99, 'Blizzard Entertainment', '2004-11-23', 9.0, 'A massively multiplayer online role-playing game set in the Warcraft universe.'),
(4, 4, 2, 4, 'The Legend of Zelda: Breath of the Wild', 59.99, 'Nintendo EPD', '2017-03-03', 9.5, 'An action-adventure game set in an open world Hyrule.'),
(5, 5, 1, 5, 'God of War Ragnarök', 69.99, 'Santa Monica Studio', '2022-11-09', 9.2, 'An action-adventure game based on Norse mythology.'),
(6, 1, 6, 6, 'FIFA 23', 59.99, 'EA Vancouver', '2022-09-30', 7.8, 'A football simulation game featuring real-world teams and players.'),
(7, 2, 5, 7, 'Anno 1800', 59.99, 'Blue Byte', '2019-04-16', 8.4, 'A city-building and strategy game set in the 19th century.'),
(8, 3, 7, 8, 'Call of Duty: Warzone', 0.00, 'Infinity Ward', '2020-03-10', 8.0, 'A free-to-play battle royale game in the Call of Duty universe.');

-- Insert sample data into Image table
INSERT INTO `EpicGameWebApp`.`ImageGame` (`ImageID`, `GameID`, `file_name`, `file_path`, `create_at`) VALUES
(1, 1, 'apex_legends_cover.jpg', '/images/games/apex_legends/', '2019-02-04 00:00:00'),
(2, 2, 'ac_valhalla_cover.jpg', '/images/games/ac_valhalla/', '2020-11-10 00:00:00'),
(3, 3, 'wow_cover.jpg', '/images/games/world_of_warcraft/', '2004-11-23 00:00:00'),
(4, 4, 'zelda_botw_cover.jpg', '/images/games/zelda/', '2017-03-03 00:00:00'),
(5, 5, 'god_of_war_cover.jpg', '/images/games/god_of_war/', '2022-11-09 00:00:00'),
(6, 6, 'fifa23_cover.jpg', '/images/games/fifa23/', '2022-09-30 00:00:00'),
(7, 7, 'anno1800_cover.jpg', '/images/games/anno1800/', '2019-04-16 00:00:00'),
(8, 8, 'cod_warzone_cover.jpg', '/images/games/cod_warzone/', '2020-03-10 00:00:00');

-- Insert sample data into Account table
INSERT INTO `EpicGameWebApp`.`Account` (`AccountID`, `RoleId`, `Username`, `Password`, `Email`, `CreatedOn`) VALUES
(1, 1, 'john_doe', 'password123', 'john@example.com', '2024-01-01 10:00:00'),
(2, 2, 'jane_admin', 'securepass', 'jane@example.com', '2024-02-15 12:30:00'),
(3, 3, 'mike_mod', 'modpass123', 'mike@example.com', '2024-03-01 09:15:00');

-- Insert sample data into PaymentMethod table
INSERT INTO `EpicGameWebApp`.`PaymentMethod` (`PaymentMethodID`, `Name`) VALUES
(1, 'Credit Card'),
(2, 'PayPal'),
(3, 'Bank Transfer'),
(4, 'Gift Card'),
(5, 'Cryptocurrency');

-- Insert sample data into Cart table
INSERT INTO `EpicGameWebApp`.`Cart` (`CartID`, `AccountID`, `PaymentMethodID`, `TotalAmount`, `CreatedOn`) VALUES
(1, 1, 1, 119.98, '2024-10-01 14:00:00'),
(2, 2, 2, 59.99, '2024-10-05 16:45:00');

-- Insert sample data into CartDetail table
INSERT INTO `EpicGameWebApp`.`CartDetail` (`CartDetailID`, `CartID`, `GameID`, `Price`, `Discount`) VALUES
(1, 1, 2, 59.99, 0.00),
(2, 1, 4, 59.99, 0.00),
(3, 2, 5, 69.99, 10.00);

-- Insert sample data into Discount table
INSERT INTO `EpicGameWebApp`.`Discount` (`DiscountID`, `GameID`, `Percent`, `Code`, `StartOn`, `EndOn`) VALUES
(1, 5, 10.00, 'GODSALE', '2024-10-01 00:00:00', '2024-10-31 23:59:59');

-- Insert sample data into AccountGame table
INSERT INTO `EpicGameWebApp`.`AccountGame` (`AccountID`, `GameID`, `DateAdded`) VALUES
(1, 1, '2024-10-01 14:05:00'),
(1, 3, '2024-10-01 14:10:00'),
(2, 5, '2024-10-05 16:50:00');


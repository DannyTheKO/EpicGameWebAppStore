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
(5, 'Sony Interactive Entertainment', '2207 Bridgepointe Pkwy, San Mateo, CA', 'contact@playstation.com', '650-655-8000', 'https://www.playstation.com'),
(6, 'Square Enix', '6-27-30 Shinjuku, Tokyo, Japan', 'contact@square-enix.com', '+81 3-5292-8000', 'https://www.square-enix.com'),
(7, 'CD Projekt', 'ul. Jagiellońska 74, Warsaw, Poland', 'contact@cdprojekt.com', '+48 22 519 69 00', 'https://www.cdprojekt.com'),
(8, 'Valve Corporation', 'PO Box 1688, Bellevue, WA', 'contact@valvesoftware.com', '425-889-9642', 'https://www.valvesoftware.com');


-- Insert sample data into Genre table
INSERT INTO `EpicGameWebApp`.`Genre` (`GenreID`, `Name`) VALUES
(1, 'Action'),
(2, 'Adventure'),
(3, 'Role-Playing'),
(4, 'Simulation'),
(5, 'Strategy'),
(6, 'Sports'),
(7, 'Shooter'),
(8, 'Platformer'),
(9, 'Horror'),
(10, 'Fighting'),
(11, 'Educational'),
(12, 'Music');

-- Insert sample data into Game table
INSERT INTO `EpicGameWebApp`.`Game` (`GameID`, `PublisherID`, `GenreID`, `ImageID`, `Title`, `Price`, `Author`, `Release`, `Rating`, `Description`) VALUES
(1, 1, 7, 1, 'Apex Legends', 0.00, 'Respawn Entertainment', '2019-02-04', 8.5, 'A free-to-play battle royale hero shooter game.'),
(2, 2, 1, 2, 'Assassin\'s Creed Valhalla', 59.99, 'Ubisoft Montreal', '2020-11-10', 8.2, 'An action role-playing game set in the Viking Age.'),
(3, 3, 3, 3, 'World of Warcraft', 14.99, 'Blizzard Entertainment', '2004-11-23', 9.0, 'A massively multiplayer online role-playing game set in the Warcraft universe.'),
(4, 4, 2, 4, 'The Legend of Zelda: Breath of the Wild', 59.99, 'Nintendo EPD', '2017-03-03', 9.5, 'An action-adventure game set in an open world Hyrule.'),
(5, 5, 1, 5, 'God of War Ragnarök', 69.99, 'Santa Monica Studio', '2022-11-09', 9.2, 'An action-adventure game based on Norse mythology.'),
(6, 1, 6, 6, 'FIFA 23', 59.99, 'EA Vancouver', '2022-09-30', 7.8, 'A football simulation game featuring real-world teams and players.'),
(7, 2, 5, 7, 'Anno 1800', 59.99, 'Blue Byte', '2019-04-16', 8.4, 'A city-building and strategy game set in the 19th century.'),
(8, 3, 7, 8, 'Call of Duty: Warzone', 0.00, 'Infinity Ward', '2020-03-10', 8.0, 'A free-to-play battle royale game in the Call of Duty universe.'),
(9, 1, 1, 9, 'Mass Effect Legendary Edition', 59.99, 'BioWare', '2021-05-14', 9.0, 'A remastered collection of the critically acclaimed Mass Effect trilogy.'),
(10, 2, 2, 10, 'Far Cry 6', 59.99, 'Ubisoft Toronto', '2021-10-07', 8.0, 'An action-adventure first-person shooter set in a tropical paradise.'),
(11, 3, 7, 11, 'Overwatch 2', 0.00, 'Blizzard Entertainment', '2022-10-04', 8.3, 'A team-based action game set in an optimistic future.'),
(12, 4, 8, 12, 'Super Mario Odyssey', 59.99, 'Nintendo EPD', '2017-10-27', 9.7, 'A 3D platform game featuring Mario globe-trotting adventure.'),
(13, 6, 3, 13, 'Final Fantasy XVI', 69.99, 'Square Enix', '2023-06-22', 8.8, 'An action role-playing game set in the fantasy world of Valisthea.'),
(14, 7, 3, 14, 'Cyberpunk 2077', 59.99, 'CD Projekt Red', '2020-12-10', 7.5, 'An open-world action RPG set in Night City.'),
(15, 8, 7, 15, 'Half-Life: Alyx', 59.99, 'Valve', '2020-03-23', 9.3, 'A virtual reality first-person shooter game.'),
(16, 6, 2, 16, 'Kingdom Hearts III', 49.99, 'Square Enix', '2019-01-29', 8.7, 'An action RPG combining Disney and Final Fantasy elements.'),
(17, 1, 4, 17, 'The Sims 4', 39.99, 'Maxis', '2014-09-02', 8.0, 'A life simulation game.');


-- Insert sample data into Image table
INSERT INTO `EpicGameWebApp`.`ImageGame` (`ImageID`, `GameID`, `file_name`, `file_path`, `create_at`) VALUES
(1,1,'apex_legends_cover.jpg','/images/games/apex_legends/cover/','2019-02-04 00:00:00'),
(2,2,'ac_valhalla_cover.jpg','/images/games/ac_valhalla/cover/','2020-11-10 00:00:00'),
(3,3,'wow_cover.jpg','/images/games/world_of_warcraft/cover/','2004-11-23 00:00:00'),
(4,4,'zelda_botw_cover.jpg','/images/games/zelda/cover/','2017-03-03 00:00:00'),
(5,5,'god_of_war_cover.jpg','/images/games/god_of_war/cover/','2022-11-09 00:00:00'),
(6,6,'fifa23_cover.jpg','/images/games/fifa23/cover/','2022-09-30 00:00:00'),
(7,7,'anno1800_cover.jpg','/images/games/anno1800/cover/','2019-04-16 00:00:00'),
(9,9,'mass_cover.jpg','/images/games/mass_effect/cover/','2020-03-10 00:00:00'),
(10,10,'farcry6_cover.jpg','/images/games/fc_6/cover/','2020-03-10 00:00:00'),
(11,11,'overwatch_cover.jpg','/images/games/overwatch/cover/','2020-03-10 00:00:00'),
(12,12,'supermario_cover.jpg','/images/games/supermario_odyssey/cover/','2020-03-10 00:00:00'),
(13,13,'ff16_cover.jpg','/images/games/ff16/cover/','2023-06-22 00:00:00'),
(14,14,'cyberpunk_cover.jpg','/images/games/cyberpunk/cover/','2020-12-10 00:00:00'),
(15,15,'half_life_alyx_cover.jpg','/images/games/half_life/cover/','2020-03-23 00:00:00'),
(16,16,'kingdom_hearts_cover.jpg','/images/games/kingdom_hearts/cover/','2019-01-29 00:00:00'),
(17,17,'sims4_cover.jpg','/images/games/sims4/cover/','2014-09-02 00:00:00'),
(18,1,'apex_legends_gameplay1.jpg','/images/games/apex_legends/gameplay/','2019-02-04 00:00:00'),
(19,1,'apex_legends_gameplay2.jpg','/images/games/apex_legends/gameplay/','2019-02-04 00:00:00'),
(20,1,'apex_legends_gameplay3.jpg','/images/games/apex_legends/gameplay/','2019-02-04 00:00:00'),
(21,2,'ac_valhalla_gameplay1.jpg','/images/games/ac_valhalla/gameplay/','2020-11-10 00:00:00'),
(22,2,'ac_valhalla_gameplay2.jpg','/images/games/ac_valhalla/gameplay/','2020-11-10 00:00:00'),
(23,2,'ac_valhalla_gameplay3.jpg','/images/games/ac_valhalla/gameplay/','2020-11-10 00:00:00'),
(24,3,'wow_gameplay1.jpg','/images/games/world_of_warcraft/gameplay/','2004-11-23 00:00:00'),
(25,3,'wow_gameplay2.jpg','/images/games/world_of_warcraft/gameplay/','2004-11-23 00:00:00'),
(26,3,'wow_gameplay3.jpg','/images/games/world_of_warcraft/gameplay/','2004-11-23 00:00:00'),
(27,4,'zelda_botw_gameplay1.jpg','/images/games/zelda/gameplay/','2017-03-03 00:00:00'),
(28,4,'zelda_botw_gameplay2.jpg','/images/games/zelda/gameplay/','2017-03-03 00:00:00'),
(29,4,'zelda_botw_gameplay3.jpg','/images/games/zelda/gameplay/','2017-03-03 00:00:00');





-- Insert sample data into Account table
INSERT INTO `EpicGameWebApp`.`Account` (`AccountID`, `RoleId`, `Username`, `Password`, `Email`, `CreatedOn`) VALUES
(1, 1, 'john_doe', 'password123', 'john@example.com', '2024-01-01 10:00:00'),
(2, 2, 'jane_admin', 'securepass', 'jane@example.com', '2024-02-15 12:30:00'),
(3, 3, 'mike_mod', 'modpass123', 'mike@example.com', '2024-03-01 09:15:00'),
(4, 4, 'sarah_editor', 'edit123pass', 'sarah@example.com', '2024-03-15 11:20:00'),
(5, 5, 'guest_user', 'guestpass', 'guest@example.com', '2024-03-20 09:45:00'),
(6, 3, 'tom_user', 'tompass123', 'tom@example.com', '2024-03-25 14:30:00'),
(7, 3, 'alex_gamer', 'alexpass456', 'alex@example.com', '2024-04-01 15:20:00'),
(8, 3, 'emma_player', 'emmapass789', 'emma@example.com', '2024-04-05 16:45:00'),
(9, 3, 'chris_game', 'chrispass321', 'chris@example.com', '2024-04-10 14:30:00');


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
(2, 2, 2, 59.99, '2024-10-05 16:45:00'),
(3, 3, 3, 179.97, '2024-10-10 15:30:00'),
(4, 4, 1, 59.99, '2024-10-12 11:20:00'),
(5, 7, 2, 129.98, '2024-04-01 16:00:00'),
(6, 8, 1, 59.99, '2024-04-05 17:00:00'),
(7, 9, 3, 109.98, '2024-04-10 15:00:00');


-- Insert sample data into CartDetail table
INSERT INTO `EpicGameWebApp`.`CartDetail` (`CartDetailID`, `CartID`, `GameID`, `Price`, `Discount`) VALUES
(1, 1, 2, 59.99, 0.00),
(2, 1, 4, 59.99, 0.00),
(3, 2, 5, 69.99, 10.00),
(4, 3, 9, 59.99, 0.00),
(5, 3, 10, 59.99, 0.00),
(6, 3, 11, 59.99, 0.00),
(7, 4, 12, 59.99, 0.00),
(8, 5, 13, 69.99, 0.00),
(9, 5, 14, 59.99, 0.00),
(10, 6, 15, 59.99, 0.00),
(11, 7, 16, 49.99, 0.00),
(12, 7, 17, 59.99, 0.00);


-- Insert sample data into Discount table
INSERT INTO `EpicGameWebApp`.`Discount` (`DiscountID`, `GameID`, `Percent`, `Code`, `StartOn`, `EndOn`) VALUES
(1, 5, 10.00, 'GODSALE', '2024-10-01 00:00:00', '2024-10-31 23:59:59'),
(2, 9, 25.00, 'MASSEFFECT25', '2024-11-01 00:00:00', '2024-11-30 23:59:59'),
(3, 10, 15.00, 'FARCRY15', '2024-11-15 00:00:00', '2024-12-15 23:59:59'),
(4, 13, 20.00, 'FF16LAUNCH', '2024-04-01 00:00:00', '2024-04-30 23:59:59'),
(5, 14, 30.00, 'CYBER30', '2024-04-15 00:00:00', '2024-05-15 23:59:59'),
(6, 15, 15.00, 'ALYX15', '2024-05-01 00:00:00', '2024-05-31 23:59:59');


-- Insert sample data into AccountGame table
INSERT INTO `EpicGameWebApp`.`AccountGame` (`AccountID`, `GameID`, `DateAdded`) VALUES
(1, 1, '2024-10-01 14:05:00'),
(1, 3, '2024-10-01 14:10:00'),
(2, 5, '2024-10-05 16:50:00'),
(3, 9, '2024-10-10 15:35:00'),
(4, 10, '2024-10-12 11:25:00'),
(5, 11, '2024-10-15 16:40:00'),
(7, 13, '2024-04-01 16:05:00'),
(7, 14, '2024-04-01 16:05:00'),
(8, 15, '2024-04-05 17:05:00'),
(9, 16, '2024-04-10 15:05:00'),
(9, 17, '2024-04-10 15:05:00');


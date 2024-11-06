-- Insert sample data into Publisher table
INSERT INTO `EpicGameWebApp`.`Publisher` (`PublisherID`, `Name`, `Address`, `Email`, `Phone`, `Website`) VALUES
(1, 'Electronic Arts', '209 Redwood Shores Parkway, Redwood City, CA', 'contact@ea.com', '650-628-1500', 'https://www.ea.com'),
(2, 'Ubisoft', '28 Rue Armand Carrel, 93100 Montreuil, France', 'contact@ubisoft.com', '+33 1 48 18 50 00', 'https://www.ubisoft.com'),
(3, 'Activision', '3100 Ocean Park Blvd, Santa Monica, CA', 'contact@activision.com', '310-255-2000', 'https://www.activision.com'),
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
(7, 'Puzzle'),
(8, 'Racing');

-- Insert sample data into Game table
INSERT INTO `EpicGameWebApp`.`Game` (`GameID`, `PublisherID`, `GenreID`, `Title`, `Price`, `Author`, `Release`, `Rating`, `Description`) VALUES
(1, 1, 1, 'Battlefield 2042', 59.99, 'DICE', '2021-11-19', 7.5, 'A first-person shooter game set in a near-future world.'),
(2, 2, 2, 'Assassin\'s Creed Valhalla', 49.99, 'Ubisoft Montreal', '2020-11-10', 8.2, 'An open-world action-adventure game set in the Viking era.'),
(3, 3, 3, 'World of Warcraft', 39.99, 'Blizzard Entertainment', '2004-11-23', 9.0, 'A massively multiplayer online role-playing game.'),
(4, 4, 4, 'Animal Crossing: New Horizons', 59.99, 'Nintendo', '2020-03-20', 8.9, 'A social simulation game where players develop a deserted island.'),
(5, 5, 5, 'Gran Turismo 7', 69.99, 'Polyphony Digital', '2022-03-04', 8.5, 'A racing simulation game with realistic graphics and physics.'),
(6, 1, 6, 'FIFA 23', 59.99, 'EA Sports', '2022-09-30', 7.8, 'A football simulation game featuring real-world teams and players.'),
(7, 2, 7, 'Tetris Effect', 29.99, 'Monstars Inc.', '2018-11-09', 9.1, 'A puzzle game with mesmerizing visuals and music.'),
(8, 3, 8, 'Need for Speed Heat', 49.99, 'Ghost Games', '2019-11-08', 7.0, 'A racing game set in an open-world environment.');

-- Insert sample data into Account table
INSERT INTO `EpicGameWebApp`.`Account` (`AccountID`, `Username`, `Password`, `Email`, `IsAdmin`, `CreatedOn`) VALUES
(1, 'john_doe', 'password123', 'john@example.com', 'N', '2024-01-01 10:00:00'),
(2, 'jane_smith', 'securepass', 'jane@example.com', 'Y', '2024-02-15 12:30:00');

-- Insert sample data into PaymentMethod table
INSERT INTO `EpicGameWebApp`.`PaymentMethod` (`PaymentMethodID`, `Name`) VALUES
(1, 'Credit Card'),
(2, 'PayPal'),
(3, 'Bank Transfer'),
(4, 'Gift Card'),
(5, 'Cryptocurrency');

-- Insert sample data into Cart table
INSERT INTO `EpicGameWebApp`.`Cart` (`CartID`, `AccountID`, `PaymentMethodID`, `TotalAmount`, `CreatedOn`) VALUES
(1, 1, 1, 109.98, '2024-10-01 14:00:00'),
(2, 2, 2, 49.99, '2024-10-05 16:45:00');

-- Insert sample data into CartDetail table
INSERT INTO `EpicGameWebApp`.`CartDetail` (`CartDetailID`, `CartID`, `GameID`, `Quantity`, `Price`, `Discount`) VALUES
(1, 1, 1, 1, 59.99, 0.00),
(2, 1, 2, 1, 49.99, 0.00),
(3, 2, 3, 1, 39.99, 0.00);

-- Insert sample data into Discount table
INSERT INTO `EpicGameWebApp`.`Discount` (`DiscountID`, `GameID`, `Percent`, `Code`, `StartOn`, `EndOn`) VALUES
(1, 1, 10.00, 'BF2042SALE', '2024-10-01 00:00:00', '2024-10-31 23:59:59');

-- Insert sample data into AccountGame table
INSERT INTO `EpicGameWebApp`.`AccountGame` (`AccountID`, `GameID`, `DateAdded`) VALUES
(1, 1, '2024-10-01 14:05:00'),
(2, 3, '2024-10-05 16:50:00');

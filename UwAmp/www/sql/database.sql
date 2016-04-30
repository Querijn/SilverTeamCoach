SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
CREATE DATABASE IF NOT EXISTS `stc` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `stc`;

CREATE TABLE `players` (
  `id` int(11) NOT NULL,
  `user` int(11) NOT NULL,
  `alternative_name` text NOT NULL,
  `title` enum('Player','Challenger','Developer','Bot') NOT NULL,
  `main_team` int(11) NOT NULL,
  `cash` double NOT NULL,
  `starting_cash` double NOT NULL,
  `owned_champions` text NOT NULL,
  `admin` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `teams` (
  `id` int(11) NOT NULL,
  `name` text NOT NULL,
  `player` int(11) NOT NULL,
  `enabled` int(11) NOT NULL,
  `mid` int(11) NOT NULL,
  `top` int(11) NOT NULL,
  `support` int(11) NOT NULL,
  `marksman` int(11) NOT NULL,
  `skin_mid` int(11) NOT NULL,
  `skin_top` int(11) NOT NULL,
  `skin_support` int(11) NOT NULL,
  `skin_marksman` int(11) NOT NULL,
  `skin_jungle` int(11) NOT NULL,
  `jungle` int(11) NOT NULL,
  `wins` int(11) NOT NULL,
  `losses` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `creep_score` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


ALTER TABLE `players`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `teams`
  ADD PRIMARY KEY (`id`);


ALTER TABLE `players`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
ALTER TABLE `teams`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;
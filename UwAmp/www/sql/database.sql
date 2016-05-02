SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
CREATE DATABASE IF NOT EXISTS `stc` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `stc`;

CREATE TABLE `champions` (
  `id` int(11) NOT NULL,
  `champion_id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `skin_id` int(11) NOT NULL DEFAULT '0',
  `steal_mastery_from` int(11) DEFAULT '0',
  `wins` int(11) NOT NULL DEFAULT '0',
  `losses` int(11) NOT NULL DEFAULT '0',
  `kills` int(11) NOT NULL DEFAULT '0',
  `deaths` int(11) NOT NULL DEFAULT '0',
  `creep_score` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `matches` (
  `id` int(11) NOT NULL,
  `team1` int(11) NOT NULL,
  `team2` int(11) NOT NULL,
  `team1_stats` int(11) NOT NULL,
  `team2_stats` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `match_champion_stats` (
  `id` int(11) NOT NULL,
  `champion_id` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `creep_score` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `match_stats` (
  `id` int(11) NOT NULL,
  `team_id` int(11) NOT NULL,
  `top` int(11) NOT NULL,
  `mid` int(11) NOT NULL,
  `support` int(11) NOT NULL,
  `marksman` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `messages` (
  `id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `title` text NOT NULL,
  `message` text NOT NULL,
  `time` int(11) NOT NULL,
  `unread` int(11) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `players` (
  `id` int(11) NOT NULL,
  `user` int(11) NOT NULL,
  `alternative_name` text NOT NULL,
  `title` enum('Player','Challenger','Developer','Bot') NOT NULL,
  `main_team` int(11) NOT NULL,
  `cash` double NOT NULL,
  `starting_cash` double NOT NULL,
  `owned_champions` text NOT NULL,
  `bots_beaten` int(11) NOT NULL DEFAULT '0',
  `admin` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `teams` (
  `id` int(11) NOT NULL,
  `name` text NOT NULL,
  `player` int(11) NOT NULL,
  `enabled` int(11) NOT NULL,
  `league` enum('BRONZE','SILVER','GOLD','PLATINUM','DIAMOND','MASTER','CHALLENGER') NOT NULL DEFAULT 'SILVER',
  `division` int(11) NOT NULL DEFAULT '3',
  `mid` int(11) NOT NULL,
  `top` int(11) NOT NULL,
  `support` int(11) NOT NULL,
  `marksman` int(11) NOT NULL,
  `jungle` int(11) NOT NULL,
  `wins` int(11) NOT NULL,
  `losses` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `creep_score` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


ALTER TABLE `champions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `matches`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `match_champion_stats`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `match_stats`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `messages`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `players`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `teams`
  ADD PRIMARY KEY (`id`);


ALTER TABLE `champions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `matches`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `match_champion_stats`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `match_stats`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `messages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `players`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `teams`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
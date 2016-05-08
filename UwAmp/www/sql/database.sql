SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
CREATE DATABASE IF NOT EXISTS `stc` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `stc`;

CREATE TABLE `PREFIXGOESHEREchampions` (
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

CREATE TABLE `PREFIXGOESHEREmatches` (
  `id` int(11) NOT NULL,
  `team1` int(11) NOT NULL,
  `team2` int(11) NOT NULL,
  `team1_stats` int(11) NOT NULL,
  `team2_stats` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `PREFIXGOESHEREmatch_champion_stats` (
  `id` int(11) NOT NULL,
  `champion_id` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `creep_score` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `PREFIXGOESHEREmatch_stats` (
  `id` int(11) NOT NULL,
  `team_id` int(11) NOT NULL,
  `top` int(11) NOT NULL,
  `mid` int(11) NOT NULL,
  `support` int(11) NOT NULL,
  `marksman` int(11) NOT NULL,
  `jungle` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `PREFIXGOESHEREmessages` (
  `id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `title` text NOT NULL,
  `message` text NOT NULL,
  `time` int(11) NOT NULL,
  `unread` int(11) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `PREFIXGOESHEREplayers` (
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

CREATE TABLE `PREFIXGOESHEREteams` (
  `id` int(11) NOT NULL,
  `name` text NOT NULL,
  `player` int(11) NOT NULL,
  `enabled` int(11) NOT NULL,
  `elo` int(11) NOT NULL DEFAULT '1290',
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


ALTER TABLE `PREFIXGOESHEREchampions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `PREFIXGOESHEREmatches`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `PREFIXGOESHEREmatch_champion_stats`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `PREFIXGOESHEREmatch_stats`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `PREFIXGOESHEREmessages`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `PREFIXGOESHEREplayers`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `PREFIXGOESHEREteams`
  ADD PRIMARY KEY (`id`);


ALTER TABLE `PREFIXGOESHEREchampions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `PREFIXGOESHEREmatches`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `PREFIXGOESHEREmatch_champion_stats`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `PREFIXGOESHEREmatch_stats`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `PREFIXGOESHEREmessages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `PREFIXGOESHEREplayers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
ALTER TABLE `PREFIXGOESHEREteams`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
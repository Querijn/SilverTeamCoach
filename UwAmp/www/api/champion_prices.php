<?php
if(!defined("INCLUDED")) 
	die();

function GetChampionPrices()
{	
	$t_6300 = array
	(
		'Aatrox',
		'Aurelion Sol',
		'Azir',
		'Bard',
		'Braum',
		'Darius',
		'Diana',
		'Draven',
		'Ekko',
		'Elise',
		'Gnar',
		'Illaoi',
		'Jayce',
		'Jhin',
		'Jinx',
		'Kalista',
		'Kha\'Zix',
		'Kindred',
		'Lissandra',
		'Lucian',
		'Nami',
		'Quinn',
		'Rek\'Sai',
		'Rengar',
		'Syndra',
		'Tahm Kench',
		'Thresh',
		'Vel\'Koz',
		'Vi',
		'Yasuo',
		'Zac',
		'Zed',
		'Zyra',
	);
	
	$t_4800 = array
	(	
		'Ahri',
		'Brand',
		'Caitlyn',
		'Cassiopeia',
		'Ezreal',
		'Fiora',
		'Fizz',
		'Galio',
		'Graves',
		'Hecarim',
		'Irelia',
		'Jarvan IV',
		'Kennen',
		'Kog\'Maw',
		'Lee Sin',
		'Leona',
		'Lulu',
		'Malzahar',
		'Maokai',
		'Nautilus',
		'Nocturne',
		'Orianna',
		'Renekton',
		'Riven',
		'Rumble',
		'Sejuani',
		'Skarner',
		'Swain',
		'Talon',
		'Trundle',
		'Varus',
		'Vayne',
		'Viktor',
		'Vladimir',
		'Volibear',
		'Wukong',
		'Xerath',
		'Yorick',
		'Ziggs',
	);
	
	$t_3150 = array
	(	
		'Akali',
		'Anivia',
		'Blitzcrank',
		'Corki',
		'Gangplank',
		'Gragas',
		'Heimerdinger',
		'Karma',
		'Karthus',
		'Kassadin',
		'Katarina',
		'LeBlanc',
		'Lux',
		'Miss Fortune',
		'Mordekaiser',
		'Nidalee',
		'Olaf',
		'Pantheon',
		'Shaco',
		'Shen',
		'Shyvana',
		'Sona',
		'Twitch',
		'Urgot',
	);
	
	$t_1350 = array
	(	
		'Alistar',
		'Cho\'Gath',
		'Dr. Mundo',
		'Evelynn',
		'Fiddlesticks',
		'Janna',
		'Jax',
		'Malphite',
		'Morgana',
		'Nasus',
		'Rammus',
		'Singed',
		'Sion',
		'Taric',
		'Teemo',
		'Tristana',
		'Tryndamere',
		'Twisted Fate',
		'Udyr',
		'Veigar',
		'Xin Zhao',
		'Zilean',
	);
		
	$t_450 = array
	(
		'Amumu',
		'Annie',
		'Ashe',
		'Garen',
		'Kayle',
		'Master Yi',
		'Nunu',
		'Poppy',
		'Ryze',
		'Sivir',
		'Soraka',
		'Warwick',
	);
	
	$t_Return = array();
	foreach($t_6300 as $t_Price)
		$t_Return[$t_Price] = 6300;
	foreach($t_4800 as $t_Price)
		$t_Return[$t_Price] = 4800;
	foreach($t_3150 as $t_Price)
		$t_Return[$t_Price] = 3150;	
	foreach($t_1350 as $t_Price)
		$t_Return[$t_Price] = 1350;
	foreach($t_450 as $t_Price)
		$t_Return[$t_Price] = 450;
		
	return $t_Return;
}
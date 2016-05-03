<?php
if(!defined("INCLUDED")) 
	die();

CreateEvent("connect", "{Champion} makes an attempt to connect to the game.", $g_CouldAFK);
CreateEvent("no_connect",  array
				(
					"{Champion} doesn't seem to have joined the game.",
					"{Champion} can't find his keyboard.",
					"{Champion} is watching a really interesting video on Facebook.",
					"{Champion} is complaining on reddit about the recent nerfs to himself.",
				), $g_CouldTiltEveryone);

CreateEvent("afk", array
				(
					"{Champion} went AFK.", 
					"{Champion} has to help his mom in the kitchen.",
					"{Champion} has to go eat dinner.",
					"{Champion} stands still, silently, looking into the void.",
					"{Champion}'s house is on fire.",
					"{Champion} got high and forgets he was playing.",
					"{Champion} is complaining on reddit about the recent nerfs to himself.",
				), $g_CouldTiltEveryone);
//CreateEvent("pull", "", );
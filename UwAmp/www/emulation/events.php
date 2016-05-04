<?php
if(!defined("INCLUDED")) 
	die();

CreateEvent("connect", "{Champion} attempts to connect to the game.", $g_CouldAFK);
CreateEvent("no_connect",  array
				(
					"{Champion} doesn't seem to have joined the game.",
					"{Champion} can't find the keyboard.",
					"{Champion} is watching a really interesting video on Facebook.",
					"{Champion} is complaining on reddit about the recent nerfs to himself.",
				), $g_CouldTiltEveryone);

CreateEvent("afk", array
				(
					"{Champion} went AFK.", 
					"{Champion} has to help mom in the kitchen.",
					"{Champion} has to go eat dinner.",
					"{Champion} stands still, silently, looking into the void.",
					"{Champion}'s house is on fire.",
					"{Champion} forgot he should be playing.",
					"{Champion} is complaining on reddit about the recent nerfs to himself.",
				), $g_CouldTiltEveryone);
				
CreateEvent("tilt", array
				(
					"{Champion} is tilting!", 
					"{Champion} missed a siege minion and broke the E key. {Champion} tilts.",
					"{Champion} accidentally flashes! {Champion} tilts.",
					"{Champion} tilts and spams the chat with hatred.",
				));
				
CreateEvent("init_game_pos", "", $g_MoveToPullOrProtect);
CreateEvent("move_to");
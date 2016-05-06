<?php
if(!defined("INCLUDED")) 
	die();

CreateEvent("connect", $g_CouldAFK);
CreateEvent("no_connect", $g_CouldTiltEveryone);

CreateEvent("afk", $g_CouldTiltEveryone);
 // array
				// (
					// "{Champion} doesn't seem to have joined the game.",
					// "{Champion} can't find the keyboard.",
					// "{Champion} is watching a really interesting video on Facebook.",
					// "{Champion} is complaining on reddit about the recent nerfs to himself.",
				// )

// array
				// (
					// "{Champion} went AFK.", 
					// "{Champion} has to help mom in the kitchen.",
					// "{Champion} has to go eat dinner.",
					// "{Champion} stands still, silently, looking into the void.",
					// "{Champion}'s house is on fire.",
					// "{Champion} forgot he should be playing.",
					// "{Champion} is complaining on reddit about the recent nerfs to himself.",
				// )
			
// array
				// (
					// "{Champion} is tilting!", 
					// "{Champion} missed a siege minion and broke the E key. {Champion} tilts.",
					// "{Champion} accidentally flashes! {Champion} tilts.",
					// "{Champion} tilts and spams the chat with hatred.",
				// )			
CreateEvent("tilt");
CreateEvent("invade");
CreateEvent("empty_jungle");
CreateEvent("executed");
CreateEvent("tower_destroyed");
CreateEvent("useless_objective");
CreateEvent("inhibitor_destroyed");
CreateEvent("game_over", $g_EndGame);
CreateEvent("kill", $g_Kill);
CreateEvent("death", $g_Death);
CreateEvent("surrender", $g_EndGame);
CreateEvent("play");
CreateEvent("end_of_timeline");
CreateEvent("laning_phase");
CreateEvent("post_laning_phase");

CreateEvent("bot_tower_attack");
CreateEvent("mid_tower_attack");
CreateEvent("top_tower_attack");
CreateEvent("base_tower_attack");

CreateEvent("bot_tower");
CreateEvent("mid_tower");
CreateEvent("top_tower");
CreateEvent("base_tower");

CreateEvent("teamfight", $g_TeamFight);
CreateEvent("dragon", $g_Dragon);
CreateEvent("baron", $g_Baron);
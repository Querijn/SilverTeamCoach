<?php 
global $g_Error;
require_once(API_FOLDER . "verification.php");
if(!defined("INCLUDED")) 
	die();

if(!isset( $_SESSION["summoner"]))
	die();

if(isset($_POST["check"]))
{
	require_once(RIOT_FOLDER . "php-riot-api.php");
	require_once(RIOT_FOLDER . "FileSystemCache.php");
	
	// Init API
	try
	{
		$t_API = new riotapi($settings["riot_key"], $_SESSION["region"]);
			
		$t_ID = $_SESSION["summoner"]["id"];
		$t_Pages = $t_API->getSummoner($t_ID, "runes")[$t_ID]['pages'];
		foreach($t_Pages as $t_Page)	
		{
			if(verify_verification($t_ID, $t_Page["name"]))
			{
				$_SESSION["user"] = $_SESSION["summoner"]["id"];
				require_once(BASE_FOLDER . "done.php");
				die();
			}
		}
	}
	catch(Exception $e){}
	
	$g_Error = "Seems like we couldn't find it! Try again. It could take up to five minutes for the server to notice the change, so be patient!";
}
?>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="utf-8">
	<title>Silver Team Coach</title>
	<meta name="description" content="Manage a team created from your Champion Mastery.">
	<meta name="author" content="Querijn Heijmans">
	<meta name="viewport" content="width=device-width, initial-scale=1">

	<link href="//fonts.googleapis.com/css?family=Raleway:400,300,600" rel="stylesheet" type="text/css">

	<link rel="stylesheet" href="css/normalize.css">
	<link rel="stylesheet" href="css/skeleton.css">
	<link rel="stylesheet" href="css/custom.css">
	<link rel="icon" type="image/png" href="images/favicon.png">

</head>
<body>
	<div class="container">
		<?php include("header.php"); ?>
		
		<div class="row">
			<div class="two columns">
				<img class="summoner_icon" src="//ddragon.leagueoflegends.com/cdn/6.8.1/img/profileicon/<?php echo $_SESSION["summoner"]["profileIconId"]; ?>.png" /> 
			</div>
			<div class="four columns">
				<h5>Is this you?</h5>
				<p>
					<?php echo $_SESSION["summoner"]["name"]; ?>,
					level <?php echo $_SESSION["summoner"]["summonerLevel"]; ?>
				</p>
			</div>
			
			<div class="six columns">
				<h5>What's next?</h5>
				<p>
					<ol>
						<li>
							Log into your account.
						</li>
						<li>
							Please rename one of your rune-pages to "<font class="verify_text"><?php echo make_verification($_SESSION["summoner"]["id"]); ?></font>".
						</li>
						<li>
							After that, click on the button below.
						</li>
					</ol>
					<form method="post" style="margin-bottom: 5px;">
						<input type="submit" name="check" value="I have changed my runepage!">
					</form>
					<form method="post" style="margin-top: -5px;">
						<input type="submit" name="reset" value="Go back">
					</form>
				</p>
			</div>
		</div>
	</div>
</body>
</html>

<?php 
global $g_Error;
if(!defined("INCLUDED")) 
	die();
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
	<link rel="icon" type="image/png" href="images/stc.ico">

</head>
<body>
	<div style="text-align: center; background: #fff; position: absolute; left:  0px; right:  0px; top:  0px; bottom: 0px;" onclick="this.style.display = 'none';" >
		<div class="container">
			<?php //if(isset($_SESSION['first_time']) == false)
			{
				$_SESSION['first_time'] = false;?>
				<div class="row">
					<div class="twelve columns">
						<h3>Thank you for playing!</h3>
						
						<p>Holy moly, we have spent a lot of time on this.</p>
						
						<p>
							This might be the biggest project we have made (full-stop here for Liane) within 16 days. We have coded soooo many lines of code in both C# and PHP to make this work.
							We seriously hope you enjoy this game, and feel free to report any issues. You can mail me personally at querijn at irule dot at, or message on Skype. You can find more ways to contact me on 
							<a href="http://irule.at/">my portfolio</a>. If you're into coding, you can check out the source at <a href="https://github.com/Querijn/SilverTeamCoach">the Github repository</a>.
						</p>
						
						<p>
							Enjoy the game! You can click anywhere to continue.
						</p>
						
						<p>
							With love,<br><br>Warm Up The Lube and Iets ronds ofzo.
						</p>
						
						<img src="images/thankyouforplaying.png"/>
					</div>
				</div>
			<?php }?>
		</div>
	</div>
	
	<p style="text-align: center; ">
		<canvas class="emscripten" id="canvas" oncontextmenu="event.preventDefault()" width="1280px" height="720px" ></canvas>
		<script type='text/javascript'>			
			var Module = 
			{
				TOTAL_MEMORY: 536870912,
				errorhandler: null,			// arguments: err, url, line. This function must return 'true' if the error is handled, otherwise 'false'
				compatibilitycheck: null,// arguments: err, url, line. This function must return 'true' if the error is handled, otherwise 'false'
				dataUrl: "<?php echo $settings["build_path"]; ?>/www.data",
				codeUrl: "<?php echo $settings["build_path"]; ?>/www.js",
				memUrl: "<?php echo $settings["build_path"]; ?>/www.mem",
			};
		</script>
		<script src="<?php echo $settings["build_path"]; ?>/UnityLoader.js"></script><BR>
		Be aware that it could take a while for the game to show up.<br>
		<br>
		If your browser alerts you of an incorrect header, or unknown compression method, reload the page.<BR><BR>
		<button onclick="window.location.href = '?reset';">Log out</button>
	</p>
</body>
</html>
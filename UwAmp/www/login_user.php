<?php 
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
	<link rel="icon" type="image/png" href="images/favicon.png">

</head>
<body>
	<div class="container">
		<div class="row">
			<div class="twelve columns" style="margin-top: 5%; margin-bottom: 2%;">
				<h3>Silver Team Coach</h3>
			</div>
		</div>
		
		<div class="row">
		
			<div class="six columns">
				<h3>What is this?</h3>
				<p>
					How well can you play your champions?
					
					Play your champions and make your own Silver Team, taking on other teams until you're experienced enough to take on LCS teams.
				</p>
			</div>
			
			<div class="six columns">
				<h4>Log in</h4>
				<p>
					<form class="login" method="post">
					
						<label for="summoner_name">
							Summoner Name: 
						</label>
						<input id="summoner_name" name="summoner_name" type="text">
						
						<label for="summoner_name">
							Region:
						</label>
						<select name="region">
							<option value="euw">
								Europe West
							</option>
						</select>
					</form>
				</p>
			</div>
			
		</div>
	</div>
</body>
</html>

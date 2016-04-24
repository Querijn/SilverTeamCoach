<?php 
global $g_Error;
if(!defined("INCLUDED")) 
	die();
?>
<div class="row">
	<div class="twelve columns" style="margin-top: 5%; <?php if(empty($g_Error)) echo "margin-bottom: 2%;" ?>">
		<h3>Silver Team Coach</h3>
	</div>
</div>
		
<?php
if(!empty($g_Error))
{
?>
	<div class="row">
		<div class="twelve columns error" style="margin-bottom: 2%;">
			<?php echo $g_Error; ?>
		</div>
	</div>
	<?php
}
?>
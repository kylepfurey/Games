/// @description Insert description here
// You can write your code in this editor
if(image_index == 0){
	audio_play_sound(snd_Bomb, 1, false);
	instance_create_layer(obj_ball.x, obj_ball.y, "Instances", obj_boom)
	
	} instance_destroy();
	
	
if(image_index == 1){
	audio_play_sound(snd_Hammer, 1, false);
	global.bounce = false;
	
	} instance_destroy();


if(image_index == 2){
	audio_play_sound(snd_Coin, 1, false);
	global.player_score += 30; 
	
	} instance_destroy()
	
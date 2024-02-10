/// @description Insert description here
// You can write your code in this editor
audio_play_sound(snd_Reset, 1, false);
	global.player_lives -= 1;
		instance_destroy();
		
		if(global.player_lives <= 0){
		audio_play_sound(snd_Lose, 1, false);
		obj_control.gameover = true;
		if(global.player_score > global.high_score){
			global.high_score = global.player_score;
			global.levelclear = 0;
			randomise();
		}
	} else {
		instance_create_layer(xstart, ystart, "Instances", obj_ball);
		with(obj_bat){
			image_xscale = 1;
			
		}
	}
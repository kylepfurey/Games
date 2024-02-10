/// @description Insert description here
// You can write your code in this editor
if(bbox_left < 0 or bbox_right > room_width){
	audio_play_sound(snd_Bounce, 1, false);
	x = clamp (x, sprite_xoffset, room_width - sprite_xoffset)
	hspeed = hspeed * -1;	
}

if(bbox_top < 0){
	audio_play_sound(snd_Bounce, 1, false);
	y = clamp (y, sprite_yoffset, room_height - sprite_yoffset)
	vspeed *= -1;
}
	
if(bbox_bottom > room_height){
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
}
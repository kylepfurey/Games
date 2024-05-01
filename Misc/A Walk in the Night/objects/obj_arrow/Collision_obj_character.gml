/// @description Insert description here
// You can write your code in this editor
 if (room = rm_7){
	with (obj_character){
		x = 55;
		y = 390;
		image_xscale = 1.5;
		image_yscale = 1.5;
		sprite_index = spr_character_rightidle;
	}
 }

 if (room = rm_8){
	with (obj_character){
		x = 256;
		y = 416;
		image_xscale = 1;
		image_yscale = 1;
		sprite_index = spr_character_upidle;
	}
	audio_stop_all()
	audio_play_sound(snd_static, 1, true)
}

room_goto_next()
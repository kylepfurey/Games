//which room
 if (room = rm_6){
	with (obj_character){
		x = 55;
		y = 390;
		image_xscale = 1.5;
		image_yscale = 1.5;
		sprite_index = spr_character_rightidle;
	}
	audio_stop_all();
	audio_play_sound(snd_ambience, 1, true)
	
	instance_destroy(obj_enemysign5)
	instance_destroy(obj_enemysign6)
	instance_destroy(obj_enemystop7)
}

if (room = rm_5){
	with (obj_character){
		x = 55;
		y = 256;
		sprite_index = spr_character_leftidle;
	}

}

if (room = rm_4){
	with (obj_character){
		x = 55;
		y = 256;
		sprite_index = spr_character_leftidle;
	}
	instance_destroy(obj_enemystop1)
}
 
if (room = rm_3){
	with (obj_character){
		x = 256;
		y = 83;
		sprite_index = spr_character_downidle;
	}
	instance_destroy(obj_enemysign2)
	instance_destroy(obj_enemysign3)
}

if (room = rm_2){
	with (obj_character){
		x = 256;
		y = 83;
		sprite_index = spr_character_downidle;
	}
	instance_destroy(obj_enemysign0)
}


room_goto_next();
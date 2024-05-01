 //collision

if (room = rm_2 and global.text = 0 and global.talk = true) {
	if (collision_rectangle(120, 0, 385, 60, obj_character, true, false)){
			
		audio_stop_sound(snd_talk);
		audio_play_sound(snd_talk, 1, false);
		global.talk = false;
		alarm[0] = 10;
		global.text = 9;
		global.move = 0;
		global.othermove = 0;

	}
	
}

if (room = rm_4 and global.text = 0 and global.talk = true) {
	if ((collision_rectangle(154, 450, 385, 500, obj_character, true, false)) xor (collision_rectangle(0, 163, 60, 355, obj_character, true, false))){
			
		audio_stop_sound(snd_talk);
		audio_play_sound(snd_talk, 1, false);
		global.talk = false;
		alarm[0] = 10;
		global.text = 14;
		global.move = 0;
		global.othermove = 0;

	}
	
}

if (room = rm_7 and global.text = 0 and global.talk = true ) {
	if (collision_rectangle(5, 265, 505, 346, obj_character, true, false)){
		
		with (obj_character){
		sprite_index = spr_character_upidle
		}
		audio_stop_sound(snd_talk);
		audio_play_sound(snd_talk, 1, false);
		global.talk = false;
		alarm[0] = 10;
		global.text = 16;
		global.move = 0;
		global.othermove = 0;

	}
	
}
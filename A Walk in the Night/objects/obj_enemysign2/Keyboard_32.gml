//collision

if (global.text = 0 and global.talk = true) {
	if (collision_circle (x, y, 80, obj_character, true, false)){
			
		audio_stop_sound(snd_talk);
		audio_play_sound(snd_talk, 1, false);
		global.talk = false;
		alarm[1] = 10;
		global.text = 11;
		global.move = 0;
		global.othermove = 0;

	}
	
}
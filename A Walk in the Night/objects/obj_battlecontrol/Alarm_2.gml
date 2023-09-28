audio_stop_all();

audio_play_sound(snd_enemyatk, 1, false);
audio_play_sound(snd_lose, 1, false);

alarm[3] = 80;
	
instance_create_layer(0, 0, "Transitions", obj_transition);

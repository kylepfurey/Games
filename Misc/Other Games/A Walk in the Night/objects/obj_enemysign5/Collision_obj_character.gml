/// @description Insert description here
// You can write your code in this editor
if (touch = 0){
	
	alarm[0] = 80;
	
	with (obj_enemystop7) {
		sprite_index = spr_enemystopdisguise_hard;	
	}
	
	audio_stop_sound(snd_music);
	audio_play_sound(snd_battle, 1, true)
	
	instance_create_layer(0, 0, "Transitions", obj_transition);
	
	touch = 1;
	
	global.move = 0;
	global.othermove = 0;
	global.enemynum = 5;
	 
	
}
 /// @description Insert description here
// You can write your code in this editor
if (global.othermove = 1 and (collision_circle (x, y, 160, obj_character, true, false))) {
	
	if (run = 0){
		audio_play_sound (snd_run, 1 , false)
	}
	
	run = 1;
	
	direction = point_direction (x, y, obj_character.x, obj_character.y);
	speed = 7;
	sprite_index = spr_enemystop_hard;
	
}

if (global.othermove = 0) {
	speed = 0;
}

if (room = rm_6 and not(collision_circle (x, y, 160, obj_character, true, false))) {
	sprite_index = spr_enemystopdisguise_hard
}
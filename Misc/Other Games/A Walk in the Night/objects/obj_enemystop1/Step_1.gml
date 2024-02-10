 /// @description Insert description here
// You can write your code in this editor
if (global.othermove = 1 and (collision_circle (x, y, 160, obj_character, true, false))) {
	
	if (run = 0){
		audio_play_sound (snd_run, 1 , false)
	}
	
	run = 1;
	
	direction = point_direction (x, y, obj_character.x, obj_character.y);
	speed = 7;
	sprite_index = spr_enemystop;
	
}

if (global.othermove = 0) {
	speed = 0;
}
/// @description Insert description here
// You can write your code in this editor
with (obj_character) {
	sprite_index = spr_NOTHING
}

if (start = 0){
	
	audio_play_sound(snd_start, 1, false);
	
	alarm[1] = 180;
	
	instance_create_layer(0, 0, "Transitions", obj_opening);

	start = 1;

}
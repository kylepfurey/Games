/// @description Insert description here
// You can write your code in this editor
if (start = 0){
	
	audio_stop_sound(snd_titlemusic);
	
	audio_play_sound(snd_start, 1, false);
	
	alarm[0] = 180;
	
	instance_create_layer(0, 0, "Transitions", obj_opening);

	start = 1;

	sprite_index = spr_NOTHING;

}
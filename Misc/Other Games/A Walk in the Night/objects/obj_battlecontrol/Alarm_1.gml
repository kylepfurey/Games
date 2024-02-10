room_goto(global.previous_room);

if (room = rm_2 xor rm_3 xor rm_4 xor rm_5 xor rm_6){
	audio_play_sound(snd_music, 1, true)}

if (room = rm_7 xor rm_8 xor rm_9){
	audio_play_sound(snd_ambience, 1, true)}

with (obj_character){
	sprite_index = spr_character_downidle;
}
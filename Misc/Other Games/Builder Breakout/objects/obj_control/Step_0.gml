/// @description Insert description here
// You can write your code in this editor
if(instance_number(obj_brick) <= 0){
		audio_play_sound(snd_Win, 1, false);
		room_restart();
		global.levelclear += 1;
}

if(gameover = true){
	if(keyboard_check_pressed(vk_anykey)){
		room_restart();
		global.player_score = 0;
		global.player_lives = 3;
		gameover = false;
	}
}
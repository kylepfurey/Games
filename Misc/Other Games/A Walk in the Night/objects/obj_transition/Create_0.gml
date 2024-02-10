/// @description Insert description here
// You can write your code in this editor
depth = 0;
alarm[0] = 168;
global.move = 0;
global.othermove = 0;

if not(room = rm_battle) {
	global.previous_room = room;
}

with (obj_border){
sprite_index = spr_borderbattle;	
}

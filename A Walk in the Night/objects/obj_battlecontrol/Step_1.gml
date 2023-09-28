/// @description Insert description here
// You can write your code in this editor
if (trigger = false and global.hp <= 0){
	trigger = true;
	alarm[2] = 1;
}

if (trigger = false and global.enemyhp <= 0){
	trigger = true;
	alarm[0] = 81;

}

if (room = rm_battle){
	global.move = 0;
	global.othermove = 0;
	with (obj_character) {
		sprite_index = spr_NOTHING;
	}
}
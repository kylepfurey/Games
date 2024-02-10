 room_speed = 60;
trigger = false;

draw_set_halign(fa_left);
draw_set_alpha(1);
draw_set_font(fnt_text);

global.turn = 0;
global.cooldown = 0;

//enemy stats

//television (unused)
if (global.enemynum = 9){
	global.enemyhp = 12;
	global.enemyatk = 9;
	global.enemydef = 0;
	global.enemyxp = 5;
}
//jeep boss
if (global.enemynum = 8){
	global.enemyhp = 10;
	global.enemyatk = 4;
	global.enemydef = 4;
	global.enemyxp = 10;
}
//super stop
if (global.enemynum = 7){
	global.enemyhp = 15;
	global.enemyatk = 4;
	global.enemydef = 3;
	global.enemyxp = 10;
}
//super sign
if (global.enemynum = 6){
	global.enemyhp = 9;
	global.enemyatk = 4;
	global.enemydef = 1;
	global.enemyxp = 5;
}
//super sign
if (global.enemynum = 5){
	global.enemyhp = 9;
	global.enemyatk = 4;
	global.enemydef = 1;
	global.enemyxp = 5;
}
//super sign
if (global.enemynum = 4){
	global.enemyhp = 9;
	global.enemyatk = 4;
	global.enemydef = 1;
	global.enemyxp = 5;
}
//sign
if (global.enemynum = 3){
	global.enemyhp = 7;
	global.enemyatk = 2;
	global.enemydef = 0;
	global.enemyxp = 5;
}
//sign
if (global.enemynum = 2){
	global.enemyhp = 7;
	global.enemyatk = 2;
	global.enemydef = 0;
	global.enemyxp = 5;
}
//stop
if (global.enemynum = 1){
	global.enemyhp = 13;
	global.enemyatk = 2;
	global.enemydef = 2;
	global.enemyxp = 5;
}
//sign
if (global.enemynum = 0){
	global.enemyhp = 7;
	global.enemyatk = 2;
	global.enemydef = 0;
	global.enemyxp = 5;
}


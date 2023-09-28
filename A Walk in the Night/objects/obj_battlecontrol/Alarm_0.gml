audio_stop_sound(snd_battle);
audio_stop_sound(snd_boss);
audio_play_sound(snd_start, 1, false);

alarm[1] = 180;
	
instance_create_layer(0, 0, "Transitions", obj_opening);

global.xp = global.xp + global.enemyxp;


//which enemy defeated
if (global.enemynum = 9){
	global.enemy9 = false;
	instance_destroy(obj_enemytv9);
}

if (global.enemynum = 8){
	global.enemy8 = false;
	instance_destroy(obj_enemyboss8);
}

if (global.enemynum = 7){
	global.enemy7 = false;
	instance_destroy(obj_enemystop7);
}

if (global.enemynum = 6){
	global.enemy6 = false;
	instance_destroy(obj_enemysign6);
}

if (global.enemynum = 5){
	global.enemy5 = false;
	instance_destroy(obj_enemysign5);
}

if (global.enemynum = 4){
	global.enemy4 = false;
	instance_destroy(obj_enemysign4);
}

if (global.enemynum = 3){
	global.enemy3 = false;
	instance_destroy(obj_enemysign3);
}

if (global.enemynum = 2){
	global.enemy2 = false;
	instance_destroy(obj_enemysign2);
}

if (global.enemynum = 1){
	global.enemy1 = false;
	instance_destroy(obj_enemystop1);
}

if (global.enemynum = 0){
	global.enemy0 = false; 
	instance_destroy(obj_enemysign0);
}


//exp
if (global.xp >= 10 and global.lv = 0){
	audio_stop_sound(snd_start)
	audio_play_sound(snd_levelup, 1, false)
	global.lv = 1;
	global.hp = 15;
	global.atk = 3;
	global.def = 1;
}

if (global.xp >= 20 and global.lv = 1){
	audio_stop_sound(snd_start)
	audio_play_sound(snd_levelup, 1, false)
	global.lv = 2;
	global.hp = 15;
	global.atk = 3;
	global.def = 2;
}

if (global.xp >= 30 and global.lv = 2){
	audio_stop_sound(snd_start)
	audio_play_sound(snd_levelup, 1, false)
	global.lv = 3;
	global.hp = 20;
	global.atk = 4;
	global.def = 2;
}

if (global.xp >= 40 and global.lv = 3){
	audio_stop_sound(snd_start)
	audio_play_sound(snd_levelup, 1, false)
	global.lv = 4;
	global.hp = 20;
	global.atk = 4;
	global.def = 3;
}

if (global.xp >= 50 and global.lv = 4){
	audio_stop_sound(snd_start)
	audio_play_sound(snd_levelup, 1, false)
	global.lv = 5;
	global.hp = 25;
	global.atk = 5;
	global.def = 3;
}

if (global.xp >= 60 and global.lv = 5){
	audio_stop_sound(snd_start)
	audio_play_sound(snd_levelup, 1, false)
	global.lv = 6;
	global.hp = 25;
	global.atk = 5;
	global.def = 4;
}
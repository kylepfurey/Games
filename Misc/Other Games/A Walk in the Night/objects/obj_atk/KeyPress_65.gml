if (global.turn = 0 and global.enemyhp > 0 and global.hp > 0) {
	
	audio_play_sound(snd_atk, 1, false);
	
	with (obj_animation){
		sprite_index = spr_slash
		x = 256
		y = 160
		alarm[1] = 45
	}
	
	global.enemyhp = global.enemyhp - (global.atk - global.enemydef);
	
	alarm[0] = 80;
		
	sprite_index = spr_NOTHING;
	
	with (obj_def){
	sprite_index = spr_NOTHING;
	}
	with (obj_spc){
	sprite_index = spr_NOTHING;
	}
	
	global.turn = 1;
	
	
//which enemy is damaged (if damaged)
if (global.enemynum = 9 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemytv9){
		sprite_index = spr_enemytvhurt;
		}
	}

if (global.enemynum = 8 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemyboss8){
		sprite_index = spr_bosshurt;
		}
	}

if (global.enemynum = 7 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemystop7){
		sprite_index = spr_enemystophurt_hard;
		}
	}

if (global.enemynum = 6 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemysign6){
		sprite_index = spr_enemysignhurt_hard;
		}
	}

if (global.enemynum = 5 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemysign5){
		sprite_index = spr_enemysignhurt_hard;
		}
	}
	
if (global.enemynum = 4 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemysign4){
		sprite_index = spr_enemysignhurt_hard;
		}
	}

if (global.enemynum = 3 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemysign3){
		sprite_index = spr_enemysignhurt;
		}
	}

if (global.enemynum = 2 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemysign2){
		sprite_index = spr_enemysignhurt;
		}
	}

if (global.enemynum = 1 and global.turn  = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemystop1){
		sprite_index = spr_enemystophurt;
		}
	}
	
if (global.enemynum = 0 and global.turn = 1 and (global.enemyhp > global.enemyhp - (global.atk - global.enemydef))){
	with (obj_enemysign0){
		sprite_index = spr_enemysignhurt;
		}
	}

}
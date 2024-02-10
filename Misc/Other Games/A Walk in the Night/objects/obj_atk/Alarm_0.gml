 if (global.enemyhp > 0 and global.hp > 0) {

	audio_play_sound(snd_enemyatk, 1, false);
	
	if (global.hp > global.hp - (global.enemyatk - global.def)){
		with (obj_border){
			sprite_index = spr_borderbattle
			alarm[0] = 60
		}
	}
	
	global.turn = 0;
	global.cooldown = global.cooldown - 1;

	sprite_index = spr_atk;

	global.hp = global.hp - (global.enemyatk - global.def);
	
	with (obj_def){
		sprite_index = spr_def;
	}
	with (obj_spc){
		if (global.cooldown <= 0){
			sprite_index = spr_spc;
		}
	}
}

//which enemy is damaged
if (global.enemynum = 9){
	with (obj_enemytv9){
		sprite_index = spr_television2;
	}
}

if (global.enemynum = 8){
	with (obj_enemyboss8){
		sprite_index = spr_boss;
	}
}

if (global.enemynum = 7){
	with (obj_enemystop7){
		sprite_index = spr_enemystop_hard;
	}
}

if (global.enemynum = 6){
	with (obj_enemysign6){
		sprite_index = spr_enemysign_hard;
	}
}

if (global.enemynum = 5){
	with (obj_enemysign5){
		sprite_index = spr_enemysign_hard;
	}
}

if (global.enemynum = 4){
	with (obj_enemysign4){
		sprite_index = spr_enemysign_hard;
	}
}

if (global.enemynum = 3){
	with (obj_enemysign3){
		sprite_index = spr_enemysign;
	}
}

if (global.enemynum = 2){
	with (obj_enemysign2){
		sprite_index = spr_enemysign;
	}
}

if (global.enemynum = 1){
	with (obj_enemystop1){
		sprite_index = spr_enemystop;
	}
}

if (global.enemynum = 0){
	with (obj_enemysign0){
		sprite_index = spr_enemysign;
	}
}
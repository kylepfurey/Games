draw_set_halign(fa_left);
draw_set_alpha(1);
draw_set_font(fnt_text);

draw_set_color(c_white);
draw_text(33, 32, "Health:" + string(global.hp))
draw_set_color(c_red);
draw_text(32, 32, "Health:" + string(global.hp))

draw_set_color(c_white);
draw_text(33, 60, "Attack:" + string(global.atk))
draw_set_color(c_blue);
draw_text(32, 60, "Attack:" + string(global.atk))

draw_set_color(c_white);
draw_text(33, 88, "Defense:" + string(global.def))
draw_set_color(c_green);
draw_text(32, 88, "Defense:" + string(global.def))

draw_set_halign(fa_right);
draw_set_color(c_white);
draw_text(470, 32, "EXP:" + string(global.xp))
draw_set_color(c_yellow);
draw_text(469, 32, "EXP:" + string(global.xp))

draw_set_color(c_white);
draw_text(470, 60, "Level:" + string(global.lv))
draw_set_color(c_aqua);
draw_text(469, 60, "Level:" + string(global.lv))

 
//enemy name
draw_set_halign(fa_center);

if (global.enemynum = 9){
	draw_set_color(c_purple);
	draw_text(255, 32, "Tele-vicious")
	draw_set_color(c_fuchsia);
	draw_text(254, 32, "Tele-vicious")
}

if (global.enemynum = 8){
	draw_set_color(c_purple);
	draw_text(255, 32, "JEEP CREEP")
	draw_set_color(c_red);
	draw_text(254, 32, "JEEP CREEP")
}

if (global.enemynum = 7){
	draw_set_color(c_purple);
	draw_text(265, 32, "Super STOP Man")
	draw_set_color(c_fuchsia);
	draw_text(264, 32, "Super STOP Man")
}

if (global.enemynum = 6){
	draw_set_color(c_purple);
	draw_text(250, 30, "Steely\nSign Guy")
	draw_set_color(c_fuchsia);
	draw_text(249, 30, "Steely\nSign Guy")
}

if (global.enemynum = 5){
	draw_set_color(c_purple)
	draw_text(250, 30, "Steely\nSign Guy")
	draw_set_color(c_fuchsia);
	draw_text(249, 30, "Steely\nSign Guy")
}

if (global.enemynum = 4){
	draw_set_color(c_purple)
	draw_text(250, 30, "Steely\nSign Guy")
	draw_set_color(c_fuchsia);
	draw_text(249, 30, "Steely\nSign Guy")
}

if (global.enemynum = 3){
	draw_set_color(c_purple);
	draw_text(255, 32, "Sign Guy")
	draw_set_color(c_fuchsia);
	draw_text(254, 32, "Sign Guy")
}

if (global.enemynum = 2){
	draw_set_color(c_purple);
	draw_text(255, 32, "Sign Guy")
	draw_set_color(c_fuchsia);
	draw_text(254, 32, "Sign Guy")
}

if (global.enemynum = 1){
	draw_set_color(c_purple);
	draw_text(255, 32, "STOP Man")
	draw_set_color(c_fuchsia);
	draw_text(254, 32, "STOP Man")
}

if (global.enemynum = 0){
	draw_set_color(c_purple);
	draw_text(255, 32, "Sign Guy")
	draw_set_color(c_fuchsia);
	draw_text(254, 32, "Sign Guy")
}
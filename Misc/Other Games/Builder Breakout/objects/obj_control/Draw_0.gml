/// @description Insert description here
// You can write your code in this editor
draw_set_halign(fa_left);
draw_text(8, 8, "Score: " + string(global.player_score));

draw_set_halign(fa_right);
draw_text(room_width-8, 8, "High Score: " + string(global.high_score));

draw_set_halign(fa_left);
draw_text(8, 470, "Lives: " + string(global.player_lives));

draw_set_halign(fa_left);
draw_text(room_width - 220, 470, "Levels Cleared: " + string(global.levelclear));

draw_sprite_ext(
		spr_bat, 
		0,
		105, 
		468,
		1,
		1,
		0,
		c_yellow,
		0.75
	);
if (instance_exists(current_mine_tile) and target_and_current_mismatch)
{
	draw_sprite_ext(current_mine_tile.sprite_index, 0, current_mine_tile.x,current_mine_tile.y, 1, 1, 0, c_black, 0.5)
}
draw_sprite_ext(spr_wall_original, 0, placing_x,placing_y, 1, 1, 0, c_white, 0.25)
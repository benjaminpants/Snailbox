if (keyboard_check_pressed(vk_f1))
{
	x = mouse_x
	y = mouse_y
	hspeed = 0
	vspeed = 0
}













//EVERYTHING BELOW RELATES TO TILES, THUS THE MASK IS CHANGED.






var prev_m = mask_index

mask_index = spr_wall_original
last_mine_tile = current_mine_tile


var nx = mouse_x
var ny = mouse_y
placing_x = floor(nx / 60) * 60
placing_y = floor(ny / 60) * 60

var distance_max = 60 * 6

current_mine_tile = collision_line_first(x,y,x + lengthdir_x(distance_max,point_direction(x,y,mouse_x,mouse_y)),y + lengthdir_y(distance_max,point_direction(x,y,mouse_x,mouse_y)),obj_wall,false,true)



if (instance_exists(current_mine_tile)) //if the tile does exist, check to actually see if the player has it selected
{
	if (!place_meeting(placing_x,placing_y,current_mine_tile.object_index))
	{
		current_mine_tile = noone
	}
	else
	{
		target_and_current_mismatch = ((placing_x != current_mine_tile.x) or placing_y != current_mine_tile.y)
	}
}

if (current_mine_tile != last_mine_tile)
{
	mining_time = 0
}
else
{
	if (mining_time == 60)
	{
		instance_destroy(current_mine_tile)
		audio_play_sound(choose(sou_BlockBreak_1,sou_BlockBreak_2,sou_BlockBreak_3,sou_BlockBreak_4),0,false)
	}
}

if (mouse_check_button(mb_left))
{
	if (current_mine_tile != noone)
	{
		mining_time++
		if (mining_time mod 10 == 0)
		{
			audio_play_sound(choose(sou_iron_hit_a,sou_iron_hit_b,sou_iron_hit_c),0,false)
		}
	}
}
else
{
	mining_time = 0
}

can_place = ((!place_free(placing_x + 60,placing_y)) or (!place_free(placing_x - 60,placing_y)) or (!place_free(placing_x,placing_y + 60)) or (!place_free(placing_x,placing_y - 60))) and (place_free(placing_x,placing_y))

mask_index = prev_m

if (mouse_check_button_pressed(mb_right) and can_place)
{
	instance_create_layer(placing_x, placing_y, "Walls", obj_wall)
}




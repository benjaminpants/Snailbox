var vals = generate_world_values(69) //im not writing a world generator in GML if I can avoid
for(i=0; i < array_length_1d(vals); i++)
{
	var name = vals[i][0]
	var val_x = real(vals[i][1])
	var val_y = real(vals[i][2])
	if (name == "Wall")
	{
		instance_create_layer(val_x * 60, val_y * 60, "Walls", obj_wall)
	}
	else if (name == "WallB")
	{
		instance_create_layer(val_x * 60, val_y * 60, "Walls", obj_wallB)
	}
	else if (name == "PlayerSpawn")
	{
		var inst = instance_create_layer((val_x * 60) + 30, (val_y * 60) + 30, "Player", obj_playerspawn)
		obj_player.xstart = inst.x
		obj_player.x = inst.x
		obj_player.ystart = inst.y
		obj_player.y = inst.y
	}
}


var mx = (window_mouse_get_x()/window_get_width()) * display_get_gui_width()
var my = (window_mouse_get_y()/window_get_height()) * display_get_gui_height()
draw_sprite_ext(spr_arrow_button_tip, 0, mx + 10, my + 10, 0.5,0.5, 140, c_white, 1)
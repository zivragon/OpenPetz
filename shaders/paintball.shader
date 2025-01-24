shader_type canvas_item;
uniform vec2 center;
uniform float diameter;
uniform float outline_width;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float color_index;
uniform float transparent_color_index = 1.0;
uniform float outline_color;
uniform sampler2D palette: filter_nearest;

@LoadColorShaderComponent

void fragment() {
	
	vec4 color = get_color(color_index / 256.0);
	
	COLOR = color;
}
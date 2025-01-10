shader_type canvas_item;

uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;

uniform sampler2D palette: filter_nearest;

uniform float color_index;
uniform float transparent_color_index = 75.0;

@LoadColorShaderComponent

void fragment() {
	vec2 coord = FRAGCOORD.xy;
	vec2 texUV = fract(coord / vec2(textureSize(tex, 0)));
	texUV.y = 1.0 - texUV.y;
	
	float tex_index = texture(tex, texUV).r;
	
	vec4 color = get_color(tex_index, false);
	
	COLOR = color;
}
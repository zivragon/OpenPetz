shader_type canvas_item;

uniform vec2 size = vec2(1., 1.);

uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;

void vertex() {
	VERTEX = VERTEX * size;
}

void fragment() {
	vec2 coord = FRAGCOORD.xy / vec2(textureSize(tex, 0));
	
	vec4 color = texture(tex, coord);
	
	COLOR = color;
}
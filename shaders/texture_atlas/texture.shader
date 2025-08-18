shader_type canvas_item;

uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;

uniform vec2 position;
uniform vec2 size;

void vertex() {
	VERTEX = VERTEX * size + position;
}

void fragment() {
	vec2 coord = FRAGCOORD.xy;
	vec2 texUV = fract(coord / vec2(textureSize(tex, 0)));
	texUV.y = 1.0 - texUV.y;
	
	float tex_index = texture(tex, texUV).r;
	
	vec4 color = vec4(tex_index, 0.0, 0.0, 1.0);
	
	COLOR = color;
}
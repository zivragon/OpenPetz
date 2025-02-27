shader_type canvas_item;

uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;

//uniform sampler2D palette: filter_nearest;

void fragment() {
	vec2 coord = FRAGCOORD.xy;
	vec2 texUV_unnormalized = coord / vec2(textureSize(tex, 0));
	vec2 texUV = fract(texUV_unnormalized);
	
	texUV.y = 1.0 - texUV.y;
	
	float trans_color = (floor(texUV_unnormalized.x) + 1.0) * 10.0;
	float pal_index = texture(tex, texUV).r;
	
	pal_index *= 255.0;
	
	if (pal_index != 253.0)
	{
		pal_index = mod(pal_index, 10.0);
		pal_index += trans_color;
	}
	pal_index /= 255.0;
	
	vec4 color = vec4(pal_index, 0.0, 0.0, 1.0);
	
	COLOR = color;
}
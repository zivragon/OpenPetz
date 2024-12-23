shader_type canvas_item;
uniform vec2 center;
uniform float radius;
uniform float outline_width;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float color_index;
uniform float transparent_color_index;
uniform vec3 outline_color = vec3(1.0);
uniform sampler2D palette: filter_nearest;

@LoadTextureShaderComponent

float random (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453) - 0.5;
}

void fragment() {
	vec2 frag = FRAGCOORD.xy;
	vec2 texFrag = FRAGCOORD.xy;
	
	frag.x += random(vec2(frag.y - center.y + fuzz)) * fuzz;
	
	float len = length(frag - center);
	float inside = step(len, radius);
	float outline = 0.0;
	if (outline_width > 2.0) 
		outline = (1.0 - step(len, radius - (outline_width - 1.0)));
	else if (outline_width == -2.0) {
		len = length((frag - vec2(1.0, 0.0)) - center);
		outline = (1.0 - step(len, radius));
	} else if (outline_width == 0.0) {
		len = length((frag + vec2(1.0, 0.0)) - center);
		outline = (1.0 - step(len, radius));
	} 
	else if (outline_width == 1.0) {
		len = length((frag + vec2(1.0, 0.0)) - center);
		outline = (1.0 - step(len, radius));
		len = length((frag - vec2(1.0, 0.0)) - center);
		outline = outline + (1.0 - step(len, radius));
	}
	vec2 uv = fract(texFrag / vec2(textureSize(tex, 0)));
	
	uv.y = 1.0 - uv.y;
	
	float tex_index = texture(tex, uv).r;
	vec4 texcolor = get_shifted_color(tex_index);
	COLOR = (outline * vec4(outline_color, 1.0) + (1.0 - outline) * texcolor) * inside;
}
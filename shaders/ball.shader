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

@LoadCircleShaderComponent

float random (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453) - 0.5;
}

void vertex() {
	VERTEX *= diameter / 2.0;
}

void fragment() {
	vec2 coord = FRAGCOORD.xy - center;
	vec2 texUV = fract(coord / vec2(textureSize(tex, 0)));
	texUV.y = 1.0 - texUV.y;
	
	float radius = diameter / 2.0;
	
	coord.x += random(vec2(coord.y + fuzz)) * fuzz;
	
	float tex_index = texture(tex, texUV).r;
	
	vec4 outline = vec4(circle(coord, radius));
	
	outline *= get_color(outline_color / 256.0, false);
	
	vec4 ball = vec4(0.0);
	
	if (outline_width == 1.0)
	{
		coord.x = abs(coord.x) + 1.0;
		ball = vec4(circle(coord, radius));
	} else {
		ball = vec4(circle(coord, radius - outline_width));
	}
	
	float is_ball = ball.a;
	
	ball *= vec4(texture(palette, vec2(tex_index, 0.0)).bgr, 1.0);
	
	vec4 color = mix(outline, ball, is_ball);
	
	COLOR = color;
}
shader_type canvas_item;

uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;

uniform vec2 start_ball;
uniform vec2 end_ball;

uniform float start_diameter;
uniform float end_diameter;

uniform float outline1_enabled = 1.0;
uniform float outline2_enabled = 1.0;

uniform float color_index;
uniform float transparent_color_index = 1.0;

uniform float r_outline_color;
uniform float l_outline_color;

uniform float fuzz = 0.0;

uniform sampler2D palette: filter_nearest;

@LoadColorShaderComponent

vec2 rotate2d(float angle, vec2 coord){
    vec2 rotatable = vec2(1.0);
	rotatable.y = coord.y * cos(angle) + coord.x * sin(angle);
	rotatable.x = coord.x * cos(angle) - coord.y * sin(angle);
	
	return rotatable;
}

float random(float x) {
    return fract(sin(dot(vec2(x,x),
                         vec2(12.9898,78.233)))*
        43758.5453123) - 0.5;
}

void fragment() {

	vec2 coord = FRAGCOORD.xy - end_ball;
	vec2 texUV = fract(coord / vec2(textureSize(tex, 0)));
	texUV.y = 1.0 - texUV.y;
	
	coord = rotate2d(atan(end_ball.y - start_ball.y, start_ball.x - end_ball.x), coord);
	
	float alpha = 1.0;
	
	float distance = length(start_ball - end_ball);
	
	float slope = (end_diameter/2.0 - start_diameter/2.0) / distance;
	//if someone has a better name for this, please let me know :)
	float girth = coord.x * slope;
	
	alpha = step(-end_diameter/2.0 + girth, coord.y) * step(coord.y, end_diameter/2.0 - girth);
	
	float tex_index = texture(tex, texUV).r;
	
	vec4 line_color = get_color(tex_index, true);
	
	line_color.a *= alpha;

	COLOR = line_color;
}
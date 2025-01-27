shader_type canvas_item;
uniform vec2 center;
uniform float diameter;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float transparent_color_index = 1.0;
uniform sampler2D palette: filter_nearest;

uniform float outline_width = 1.0;
uniform float outline_color = 80.0;

uniform float color_index = 0.0;
uniform float eye_diameter = 32.0;

uniform vec3 rotation = vec3(0.0);

//varying vec3 eye_vector;

@LoadColorShaderComponent

@LoadCircleShaderComponent

vec3 rotate3d(vec3 angle, vec3 coord){
    vec3 v1 = vec3(0.0);
    vec3 v2 = vec3(0.0);
    vec3 v3 = vec3(0.0);
    
    //Y
	v1.z = coord.z * cos(angle.y) - coord.x * sin(angle.y);
	v1.x = coord.x * cos(angle.y) + coord.z * sin(angle.y);
	
	//Z
	v2.y = coord.y * cos(angle.z) - v1.x * sin(angle.z);
	v2.x = v1.x * cos(angle.z) + coord.y * sin(angle.z);
	
	//X
	v3.z = v1.z * cos(angle.x) - v2.y * sin(angle.x);
	v3.y = v2.y * cos(angle.x) + v1.z * sin(angle.x);
	
	vec3 rotatable = vec3(v2.x, v3.y, v3.z);
	
	return rotatable;
}

float random (vec2 st) {    // @pasted from ball.shader
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453) - 0.5;
}

void vertex() {
	//eye_vector = vec3(0.0, 0.0, 1.0);
}

void fragment() {

    vec2 coord = FRAGCOORD.xy - center;

    coord.x += random(vec2(coord.y + fuzz)) * fuzz;

    float radius = diameter / 2.0;
	float eye_radius = eye_diameter / 2.0;
    
    vec4 clip = vec4(circle(coord, radius));
    
    vec4 outline = vec4(circle(coord, eye_radius));
	
	outline *= get_color(outline_color / 255.0, false);
	
	vec4 ball = vec4(circle(coord, radius - outline_width));
	
	float is_ball = ball.a;
	
	ball *= get_color(color_index / 255.0, true);
	
	vec4 color = mix(outline, ball, is_ball);

	COLOR = ball * clip;
}
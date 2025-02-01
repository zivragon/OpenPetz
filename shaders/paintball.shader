shader_type canvas_item;
uniform vec2 center;
uniform float diameter;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float transparent_color_index = 1.0;
uniform sampler2D palette: filter_nearest;

uniform vec3 rotation = vec3(0.0);


varying float v_radius;
varying float v_total_radius; 
varying float v_fuzz;
varying float color_index; // @todo(naming consistency) rename to v_color_index
varying float v_is_visible;

varying vec3 v_position;

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

    v_radius = CUSTOM0.r * diameter / 2.0;
    v_total_radius = v_radius / 2.0 + diameter/ 2.0;
   
    // we store coords in color. Coords are -1,1, but COLOR allows 0,1. 
    // So we had to convert -1,1 to 0,1, let's undo that and get the original coord:
    vec3 xyz = COLOR.xyz * 2.0f - 1.0f;
    
    xyz = rotate3d(rotation, xyz);
        
    v_position = floor(xyz * v_total_radius);
    color_index = COLOR.a * 255.0;
    
    v_is_visible = v_position.z > 0.0 ? 0.0 : 1.0;
    
    v_fuzz = CUSTOM0.g;
    
    VERTEX *= (v_radius + vec2(fuzz, 0.0));
    VERTEX += v_position.xy;
}

void fragment() {

    vec2 coord = FRAGCOORD.xy - center;
    vec2 p_coord = FRAGCOORD.xy - center - v_position.xy;

    coord.x += random(vec2(coord.y + fuzz)) * fuzz;
	p_coord.x += random(vec2(p_coord.y + 0.125 + v_fuzz)) * v_fuzz;

    float radius = diameter / 2.0;
    
    vec4 clip = vec4(circle(coord, radius, 0.0));
    
    vec4 ball = vec4(circle(p_coord, v_radius, 0.0));
    
    vec4 color = get_color(color_index / 256.0, false);

	COLOR = color * ball * clip * vec4(v_is_visible);
}
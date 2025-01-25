shader_type canvas_item;
uniform vec2 center;
uniform float diameter;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float transparent_color_index = 1.0;
uniform sampler2D palette: filter_nearest;


varying float v_radius;
varying float v_total_radius; // @todo unused variable
varying float v_fuzz;
varying float color_index; // @todo(naming consistency) rename to v_color_index
varying float v_is_visible;

varying vec3 v_position;

@LoadColorShaderComponent

@LoadCircleShaderComponent

float random (vec2 st) {    // @pasted from ball.shader
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453) - 0.5;
}

void vertex() {

    v_radius = CUSTOM0.r * diameter / 2.0;
    // v_total_radius = v_radius + diameter/ 2.0;
   
    // we store coords in color. Coords are -1,1, but COLOR allows 0,1. 
    // So we had to convert -1,1 to 0,1, let's undo that and get the original coord:
    vec3 xyz = COLOR.xyz * 2.0f - 1.0f;
        
    v_position = xyz * diameter / 2.0;
    color_index = COLOR.a * 255.0;
    
    v_is_visible = v_position.z > 0.0 ? 0.0 : 1.0;
    
    v_fuzz = CUSTOM0.g;
    
    VERTEX *= v_radius;
    VERTEX += v_position.xy;
}

void fragment() {

    vec2 coord = FRAGCOORD.xy - center;
    vec2 p_coord = FRAGCOORD.xy - center - v_position.xy;

    coord.x += random(vec2(coord.y + fuzz)) * fuzz;
    p_coord.x += random(vec2(p_coord.y + v_fuzz)) * v_fuzz;
    
    float radius = diameter / 2.0;
    
    vec4 clip = vec4(circle(coord, radius));
    
    vec4 ball = vec4(circle(p_coord, v_radius));
    
    vec4 color = get_color(color_index / 256.0, false);

    COLOR = color * ball * clip * vec4(v_is_visible);
}

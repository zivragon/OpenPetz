shader_type canvas_item;
uniform vec2 center;
uniform float diameter;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float transparent_color_index = 1.0;
uniform sampler2D palette: filter_nearest;

@LoadColorShaderComponent

@LoadCircleShaderComponent

varying float v_radius;
varying float v_total_radius;
varying float v_fuzz;
varying float v_color_index;
varying float v_is_visible;

varying vec3 v_position;

void vertex() {

    v_radius = CUSTOM0.r * diameter / 2.0;
    v_total_radius = v_radius + diameter/ 2.0;
    
    v_position = COLOR.xyz * v_radius;
    v_color_index = COLOR.a;
    
    v_is_visible = v_position.z < 0.0 ? 0.0 : 1.0;
    
    v_fuzz = CUDTOM0.g;
    
    VERTEX *= v_radius;
    VERTEX += v_position.xy;
}

void fragment() {

    vec2 coord = FRAGCOORD.xy - center;
    vec2 p_coord = FRAGCOORD.xy - center - v_position.xy;
    
    float radius = diameter / 2.0;
    
    vec4 clip = vec4(circle(coord, radius));
    
    vec4 ball = vec4(circle(p_coord, v_radius));
    
    vec4 color = get_color(v_color_index / 256.0, false);

    COLOR = color * ball * clip * vec4(v_is_visible);
}


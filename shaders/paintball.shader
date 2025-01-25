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

varying float v_radius;
varying float v_total_radius;
varying vec3 v_position;

void vertex() {

    v_radius = CUSTOM0.r * diameter / 2.0;
    
    v_position = COLOR.xyz * v_radius;

    VERTEX *= v_radius;
    VERTEX += v_position.xy;
}

void fragment() {

    vec2 coord = FRAGCOORD.xy - center;
    vec2 p_coord = FRAGCOORD.xy - center - v_position.xy;
    
    float radius = diameter / 2.0;
    
    vec4 clip = vec4(circle(coord, radius));
    
    vec4 ball = vec4(circle(p_coord, v_radius));
    
    vec4 color = get_color(color_index / 256.0, false);

    COLOR = color * ball * clip;
}
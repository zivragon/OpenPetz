shader_type canvas_item;
uniform vec2 center;
uniform float diameter;
uniform sampler2D tex : hint_default_white, filter_nearest, repeat_enable;
uniform float fuzz = 0.0;
uniform float transparent_color_index = 1.0;
uniform sampler2D palette: filter_nearest;

uniform vec3 rotation = vec3(0.0);

uniform float p_color[8];
uniform float p_size[8];
uniform float p_fuzz[8];
uniform vec3 p_coordination[8];

uniform vec2 p_atlas_position[8];
uniform vec2 p_atlas_size[8];

varying float v_radius;
varying float v_total_radius; 
varying float v_fuzz;
varying float color_index; // @todo(naming consistency) rename to v_color_index
varying float v_is_visible;
varying float v_index;

varying vec3 v_position;

@LoadColorShaderComponent

@LoadSubTextureShaderComponent

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

	int index = int(floor(CUSTOM0.r));
	v_index = floor(CUSTOM0.r);
	float size = p_size[index];

    v_radius = size * diameter / 2.0;
    v_total_radius = v_radius / 2.0 + diameter/ 2.0;
   
    vec3 xyz = p_coordination[index];
    
    xyz = rotate3d(rotation, xyz);
        
    v_position = floor(xyz * v_total_radius);
    color_index = p_color[index];
    
    v_is_visible = v_position.z > 0.0 ? 0.0 : 1.0;
    
    v_fuzz = p_fuzz[index];
    
    VERTEX *= (v_radius + vec2(v_fuzz, 0.0));
    VERTEX += v_position.xy;
}

void fragment() {

	int index = int(v_index);
	
	vec2 coord = FRAGCOORD.xy - center;
    vec2 p_coord = FRAGCOORD.xy - center - v_position.xy;
	
	coord.x += random(vec2(coord.y + fuzz)) * fuzz;
	p_coord.x += random(vec2(p_coord.y + 0.125 + v_fuzz)) * v_fuzz;
	
	vec2 atlas_texture_unnormalized = vec2(textureSize(tex, 0));
	vec2 atlas_subtexture_unnormalized = vec2(p_atlas_size[index].x * atlas_texture_unnormalized.x, p_atlas_size[index].y * atlas_texture_unnormalized.y);

	vec2 texUV = fract(p_coord / atlas_subtexture_unnormalized);
	texUV.y = 1.0 - texUV.y;

	vec2 atlas_texUV = get_subtexture_uv(p_atlas_position[index], p_atlas_size[index], texUV);
	
	float tex_index = texture(tex, atlas_texUV).r;

    float radius = diameter / 2.0;
    
    vec4 clip = vec4(circle(coord, radius));
    
    vec4 ball = vec4(circle(p_coord, v_radius));
    
    vec4 color = vec4(texture(palette, vec2(tex_index, 0.0)).bgr, 1.0);

	COLOR = color * ball * clip * vec4(v_is_visible);
}
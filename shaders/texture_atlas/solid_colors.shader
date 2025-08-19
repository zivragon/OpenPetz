shader_type canvas_item;

varying vec2 v_coord;

void vertex() {
	VERTEX = VERTEX * 16.;
	v_coord = VERTEX;
}

void fragment() {
	vec2 coord = v_coord;
	
	float index = coord.y * 16. + coord.x - 8.;
	index /= 256.;
	
	vec4 color = vec4(index, 0.0, 0.0, 1.0);
	
	COLOR = color;
}
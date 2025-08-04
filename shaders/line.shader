shader_type canvas_item;

uniform vec2 ball_coords[2];
uniform float ball_diameters[2];
uniform float angle_to;

vec2 rotate2d(float angle){
    vec2 rotatable = vec2(1.0);
	rotatable.y = cos(angle) + sin(angle);
	rotatable.x = cos(angle) - sin(angle);
	
	return rotatable;
}

void vertex () {
	int index = int(floor(VERTEX.x));
	float indice[2];
	indice[0] = -1.;
	indice[1] = 1.;
	
	VERTEX.x = ball_coords[index].x + rotate2d(angle_to).x * ball_diameters[index] / 2.;
	VERTEX.y = ball_coords[index].y + VERTEX.y * rotate2d(angle_to).y * ball_diameters[index] / 2.;
}

void fragment() {

	COLOR = vec4(0.0, 0.0, 1.0, 1.0);
}
float circle(vec2 coord, float radius){
	float err = fract(radius);
	bool bigger_than_zero = radius > 0.;
	
	bool condition = (pow(coord.x - err, 2.0) + pow(coord.y - err, 2.0)) < pow(radius, 2.0);
	condition = condition && bigger_than_zero;
	
	return !condition ? 0.0 : 1.0;
}
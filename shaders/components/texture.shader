//CANNOT BE USED AS A SHADER ON ITS OWN

vec4 get_shifted_color(float palette_index) {

	float palette_index256 = palette_index * 255.0;

	if (palette_index256 >= 10.0 && palette_index256 <= 149.0){
		float modded_color = color_index - mod(color_index, 10);
		float amount_to_shift_index = mod(palette_index256, 10);
		float new_palette_index = modded_color + amount_to_shift_index;
		vec4 new_color = texture(palette, vec2(new_palette_index / 255.0, 0.0));
		
		return new_color;
	}
	
	if (palette_index*255.0 == 253.0)
		return vec4(0.0);
	
	vec4 new_color = vec4(texture(palette, vec2(palette_index, 0.0)).rgb, 1.0);
	return new_color;
}

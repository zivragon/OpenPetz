//CANNOT BE USED AS A SHADER ON ITS OWN

vec4 get_color(float palette_index, bool color_map) {

	float palette_index256 = palette_index * 255.0;

	float normalized_transparent_color = transparent_color_index - mod(transparent_color_index, 10);
	float normalized_palette_index = palette_index256 - mod(palette_index256, 10);
	
	if (normalized_palette_index == normalized_transparent_color && color_map && transparent_color_index > 1.0){
		vec4 color = vec4(texture(palette, vec2(color_index / 256.0, 0.0)).rgb, 1.0);
		return color;
	}
	
	if (palette_index256 >= 10.0 && palette_index256 <= 149.0 && color_map && transparent_color_index == 1.0){
		float modded_color = color_index - mod(color_index, 10);
		float amount_to_shift_index = mod(palette_index256, 10);
		float new_palette_index = modded_color + amount_to_shift_index;
		vec4 new_color = texture(palette, vec2(new_palette_index / 256.0, 0.0));
		
		return vec4(new_color.bgr, 1.0);
	}
	
	if (palette_index256 == 253.0)
		return vec4(0.0);
	
	vec4 new_color = vec4(texture(palette, vec2(palette_index, 0.0)).bgr, 1.0);
	return new_color;
}

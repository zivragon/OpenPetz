//CANNOT BE USED AS A SHADER ON ITS OWN

vec2 get_subtexture_uv(vec2 _dest, vec2 _size, vec2 _uv) {

	vec2 uv = vec2(_uv.x * _size.x, _uv.y * _size.y);
    
	vec2 result = _dest + uv;
	
	return result;
}

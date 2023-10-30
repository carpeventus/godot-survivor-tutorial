shader_type canvas_item;

uniform float weight : hint_range(0.0, 1.0, 0.1);

void fragment() {
	vec4 texture_color = texture(TEXTURE, UV);
	vec4 final_color = mix(texture_color, vec4(1.0,1.0,1.0, texture_color.a), weight);
	COLOR = final_color;
	// Called for every pixel the material is visible on.
}



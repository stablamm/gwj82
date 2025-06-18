shader_type canvas_item;

uniform vec4 sand_color = vec4(0.9, 0.85, 0.7, 1.0);  // base sand color
uniform vec3 groove_color = vec3(0.3, 0.25, 0.2);     // dark groove color
uniform float groove_depth = 0.8;                    // 0 = no groove, 1 = full darkness
uniform float grain_strength = 0.15;

void fragment() {
    vec4 tex = texture(TEXTURE, UV);

    // Skip fully transparent pixels
    if (tex.a < 0.01) {
        discard;
    }

    // Grain noise
    float grain = fract(sin(dot(SCREEN_UV * 800.0, vec2(12.9898, 78.233))) * 43758.5453);
    grain = mix(1.0, grain, grain_strength);

    // Darken where text appears (simulate groove)
    vec3 groove = mix(sand_color.rgb, groove_color, groove_depth * tex.a);

    // Add grain
    groove *= grain;

    COLOR = vec4(groove, tex.a);
}

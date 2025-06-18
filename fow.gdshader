shader_type canvas_item;

uniform sampler2D screen_tex : hint_screen_texture;
uniform vec2 player_screen_pos;      // Player position in screen coordinates
uniform float reveal_radius = 100.0; // How far the player can see
uniform float softness = 40.0;       // Edge fade softness

void fragment() {
    vec2 screen_uv = FRAGCOORD.xy;
    float dist = distance(screen_uv, player_screen_pos);
    float alpha = 1.0 - smoothstep(reveal_radius - softness, reveal_radius, dist);
    vec4 screen_color = texture(screen_tex, SCREEN_UV);
    COLOR = mix(vec4(0.0, 0.0, 0.0, 1.0), screen_color, alpha);
}

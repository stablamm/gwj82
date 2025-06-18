shader_type canvas_item;

uniform bool glint_enabled = false;
uniform float glint_speed   : hint_range(0.0, 10.0) = 2.0;
uniform float glint_width   : hint_range(0.0, 1.0)  = 0.2;
uniform float glint_intensity: hint_range(0.0, 2.0) = 0.8;
uniform vec4 glint_color               = vec4(1.0,1.0,1.0,1.0);
uniform vec2 glint_dir                 = vec2(1.0,0.5);

void fragment() {
    vec4 base = texture(TEXTURE, UV);

    float proj  = dot(UV, normalize(glint_dir));
    float pulse = fract(proj * (1.0/glint_width) + TIME * glint_speed);
    float band  = smoothstep(0.0, 0.1, pulse)
                * (1.0 - smoothstep(0.9, 1.0, pulse));

    vec3 glow = vec3(0.0);
    if (glint_enabled) {
        glow = glint_color.rgb * band * glint_intensity;
    }

    COLOR = vec4(base.rgb + glow, base.a);
}
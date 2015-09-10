#ifdef GL_ES
precision mediump float;
#define LOWP lowp
#else
#define LOWP
#endif

uniform sampler2D u_tex;
uniform vec3 u_map_position;
uniform vec3 u_tile_origin;
uniform vec2 u_resolution;
uniform float u_time;
uniform float u_meters_per_pixel;

varying vec2 v_uv;
varying float v_alpha;
varying vec4 v_color;

#ifdef TANGRAM_POINT
const float borderWidth = 0.3;
const float circleRadiusIn = 0.5;
const float circleRadiusOut = circleRadiusIn + borderWidth;

float circle(vec2 r, vec2 center, float radius) {
    return 1.0 - smoothstep(radius - 0.2, radius + 0.2, length(r - center));
}
#else
uniform sampler2D u_tex;
#endif

void main(void) {
    if (v_alpha < TANGRAM_EPSILON) {
        discard;
    } else {
        #ifdef TANGRAM_POINT
            vec2 uv = v_uv * 2.0 - 1.0;
            float c1 = circle(uv, vec2(0.0), circleRadiusIn);
            float c2 = circle(uv, vec2(0.0), circleRadiusOut);
            gl_FragColor = vec4(vec3(c1) * v_color.rgb, c2 * v_alpha * v_color.a);
        #else
            vec4 color = texture2D(u_tex, v_uv);
            gl_FragColor = vec4(color.rgb * v_color.rgb, v_alpha * color.a * v_color.a);
        #endif
    }
}

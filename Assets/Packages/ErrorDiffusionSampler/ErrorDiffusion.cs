using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ErrorDiffusionSamplerSys {

    // https://en.wikipedia.org/wiki/Error_diffusion
    public static class ErrorDiffusion {

        public delegate float GetColor(Vector2 uv);

        public const float THRESHOLD = 0.5f;
        public static readonly (int x, int y, int v)[] JJN = new[] {
            (1, 0, 7), (2, 0, 5),
            (-2, 1, 3), (-1, 1, 5), (0, 1, 7), (1, 1, 5), (2, 1, 3),
            (-2, 2, 1), (-1, 2, 3), (0, 2, 5), (1, 2, 3), (2, 2, 1)
        };
        public static readonly (int x, int y, float v)[] NJJN;

        static ErrorDiffusion() {
            var inv_sum = 1f / JJN.Sum(v => v.v);
            NJJN = JJN.Select(v => (v.x, v.y, v.v * inv_sum)).ToArray();
        }


        public static Vector2 UV(this Vector2 dx, int x, int y)
            => new Vector2(dx.x * (x + 0.5f), dx.y * (y + 0.5f));

        public static IEnumerable<(int x, int y)> Generate(
            this Texture2D src, 
            float density,
            bool linear = false) {

            var size = new Vector2Int(src.width, src.height);
            GetColor color = (Vector2 uv) => {
                var c = src.GetPixelBilinear(uv.x, uv.y);
                return linear ? c.r : c.linear.r;
            };
            return color.GenerateWithSize(size, density);
        }
        public static IEnumerable<(int x, int y)> GenerateWithSize(
            this GetColor color,
            Vector2Int size,
            float density) {

            var errors = new float[size.x * size.y];
            var dx = new Vector2(1f / size.x, 1f / size.y);
            for (var y = 0; y < size.y; y++)
                for (var x = 0; x < size.x; x++)
                    errors[x + y * size.x] = density * color(dx.UV(x, y));

            for (var y = 0; y < size.y; y++) {
                for (var x = 0; x < size.x; x++) {
                    var e = errors[x + y * size.x];

                    if (e >= THRESHOLD) {
                        e -= 1f;
                        yield return (x, y);
                    }

                    for (var i = 0; i < NJJN.Length; i++) {
                        var d = NJJN[i];
                        var x1 = x + d.x;
                        var y1 = y + d.y;
                        if (x1 < 0 || size.x <= x1 || y1 < 0 || size.y <= y1) continue;
                        errors[x1 + y1 * size.x] += e * d.v;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorDiffusionSamplerSys {

    public enum ColorIndex {
        R = 0,
        G = 1,
        B = 2,
        A = 3,
    }
    [System.Flags]
    public enum ColorFlag {
        None = 0,
        R = 1 << ColorIndex.R,
        G = 1 << ColorIndex.G,
        B = 1 << ColorIndex.B,
        A = 1 << ColorIndex.A,
    }

    public static class ColorExtension {

        public static Color ToColor(this ColorFlag cf)
            => new Color(
                (cf & ColorFlag.R) != 0 ? 1f : 0f,
                (cf & ColorFlag.G) != 0 ? 1f : 0f,
                (cf & ColorFlag.B) != 0 ? 1f : 0f,
                (cf & ColorFlag.A) != 0 ? 1f : 0f);
        public static ColorFlag ToColorFlag(this Color c) {
            var ci = default(ColorFlag);
            for (var i = 0; i < 4; i++)
                if (c[i] > 0f) ci |= (ColorFlag)(1 << i);
            return ci;
        }
    }
}
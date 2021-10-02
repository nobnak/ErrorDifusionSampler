using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorDiffusionSamplerSys {

    public static class NumericsExtension {

        public static float Rand(this float range) => range * (2f * Random.value - 1f);
        public static float Noise(this float range, float x, float y) 
            => range * (2f * Mathf.PerlinNoise(x, y) - 1f);

    }
}
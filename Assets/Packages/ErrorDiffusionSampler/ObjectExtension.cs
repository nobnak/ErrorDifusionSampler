using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorDiffusionSamplerSys {

    public static class ObjectExtension {

        public static void Destroy(this Object obj) {
            if (obj == null) return;

            if (Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
        }
    }
}

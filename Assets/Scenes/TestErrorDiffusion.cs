using ErrorDiffusionSamplerSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class TestErrorDiffusion : MonoBehaviour {

    [SerializeField]
    protected Events events = new Events();
    [SerializeField]
    protected Link link = new Link();
    [SerializeField]
    protected Tuner tuner = new Tuner();

    protected bool valid;
    protected Texture2D dst;

    #region member

    #region unity
    private void OnValidate() {
        valid = false;
    }
    private void OnDisable() {
        dst.Destroy();
    }
    private void Update() {
        Validate();
    }
    #endregion

    protected virtual void Validate() {
        if (valid) return;
        valid = true;

        events.InputOnUpdate?.Invoke(link.source);
        if (link.source == null || !link.source.isReadable) return;

        dst.Destroy();

        var gen = link.source.Generate(tuner.samples, tuner.density, tuner.sampleMode, tuner.colorIndex);
        dst = CreateDestinationTexture(gen.size, Color.black);
        foreach (var s in gen.samples)
            dst.SetPixel(s.x, s.y, Color.white);
        dst.Apply();

        events.OutputOnUpdate.Invoke(dst);
    }
    public static Texture2D CreateDestinationTexture(Vector2Int size, Color initialColor) {
        var dst = new Texture2D(size.x, size.y);
        dst.filterMode = FilterMode.Point;
        dst.wrapMode = TextureWrapMode.Clamp;
        for (var y = 0; y < size.y; y++)
            for (var x = 0; x < size.x; x++)
                dst.SetPixel(x, y, initialColor);
        dst.Apply();
        return dst;
    }
    #endregion

    [System.Serializable]
    public class Events {
        public UnityEvent<Texture> InputOnUpdate = new UnityEvent<Texture>();
        public UnityEvent<Texture> OutputOnUpdate = new UnityEvent<Texture>();
    }
    [System.Serializable]
    public class Link {
        public Texture2D source;
    }
    [System.Serializable]
    public class Tuner {
        [Range(100, 1000000)]
        public int samples = 100;
        [Range(0.01f, 1f)]
        public float density = 0.1f;
        public ColorIndex colorIndex = ColorIndex.R;
        public ErrorDiffusion.SampleMode sampleMode = default;
    }
}
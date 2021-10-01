using ErrorDiffusionSamplerSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        events.InputOnUpdate.Invoke(link.source);
        if (link.source == null || !link.source.isReadable) return;

        dst.Destroy();
        dst = new Texture2D(link.source.width, link.source.height);
        dst.filterMode = FilterMode.Point;
        dst.wrapMode = TextureWrapMode.Clamp;

        var samples = link.source.Generate(tuner.density);
        var colors = dst.GetPixels();
        System.Array.Clear(colors, 0, colors.Length);
        dst.SetPixels(colors);
        foreach (var s in samples)
            dst.SetPixel(s.x, s.y, Color.white);
        dst.Apply();

        events.OutputOnUpdate.Invoke(dst);
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
        [Range(0.01f, 1f)]
        public float density = 0.1f;
    }
}
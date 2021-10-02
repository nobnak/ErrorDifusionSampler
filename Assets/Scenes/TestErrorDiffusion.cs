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

    protected Validator valid = new Validator();
    protected List<Transform> polls = new List<Transform>();

    #region member

    #region unity
    private void OnEnable() {
        valid.Validation += () => {
            if (link.source == null || !link.source.isReadable) return;
            events?.InputOnUpdate?.Invoke(link.source);

            var gen = link.source.Generate(tuner.samples, tuner.density, tuner.sampleMode, tuner.colorIndex);

            ClearPolls();
            foreach (var s in gen.samples) {
                var uv = gen.UV(s);
                var pos = link.destination.TransformPoint(new Vector3(uv.x - 0.5f, uv.y - 0.5f));
                var poll = Instantiate(link.pollfab);
                poll.gameObject.hideFlags = HideFlags.HideAndDontSave;
                poll.SetParent(link.pollparent != null ? link.pollparent : transform);
                poll.localRotation = Quaternion.identity;
                poll.position = pos;
                var c = link.source.GetPixelBilinear(uv.x, uv.y);
                poll.localScale *= c[(int)tuner.colorIndex];
                polls.Add(poll);
            }
        };
    }
    private void OnValidate() {
        valid.Invalidate();
    }
    private void OnDisable() {
        valid.Reset();
        ClearPolls();
    }
    private void Update() {
        valid.Validate();
    }
    #endregion

    private void ClearPolls() {
        foreach (var p in polls) p.Destroy();
        polls.Clear();
    }
    #endregion

    [System.Serializable]
    public class Events {
        public UnityEvent<Texture> InputOnUpdate = new UnityEvent<Texture>();
    }
    [System.Serializable]
    public class Link {
        public Texture2D source;
        public Transform destination;
        public Transform pollparent;
        public Transform pollfab;
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
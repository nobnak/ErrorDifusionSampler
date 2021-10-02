using ErrorDiffusionSamplerSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

    public const float DEG_ROUND = 360f;

    [SerializeField]
    protected Tuner tuner = new Tuner();

    protected Quaternion initq;
    protected Vector2 initrand;

    #region member

    #region unity
    private void OnEnable() {
        initq = transform.localRotation;
        initrand = new Vector2(1000f.Rand(), 1000f.Rand());
    }
    private void Update() {
        var dt = Time.deltaTime * tuner.speed;
        var t = Time.realtimeSinceStartup * tuner.freq;

        transform.localRotation *= Quaternion.Euler(
            dt * DEG_ROUND.Noise(initrand.x, t),
            dt * DEG_ROUND.Noise(initrand.y, t),
            0f);
    }
    #endregion

    #endregion

    #region definitions
    [System.Serializable]
    public class Tuner {
        public float speed = 0.1f;
        public float freq = 0.01f;
    }
    #endregion
}
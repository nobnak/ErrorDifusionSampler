using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureApplier : MonoBehaviour {

    public const string P_MainTex = "_MainTex";

    [SerializeField]
    protected Tuner tuner = new Tuner();

    protected Renderer rend;

    #region interface
    public void Listen(Texture tex) {
        var blk = new MaterialPropertyBlock();
        rend?.GetPropertyBlock(blk);
        blk.SetTexture(P_MainTex, tex);
        rend?.SetPropertyBlock(blk);

        var s = transform.localScale;
        switch (tuner.stretch) {
            default:
                s.x = s.y * tex.width / tex.height;
                break;
            case StretchMode.Vertical:
                s.y = s.x * tex.height / tex.width;
                break;
        }
        transform.localScale = s;
    }
    #endregion

    #region unity
    private void OnEnable() {
        rend = GetComponent<Renderer>();
    }
    #endregion

    #region definitions
    public enum StretchMode { Horizontal = 0, Vertical }
    [System.Serializable]
    public class Tuner {
        public StretchMode stretch;
    }
    #endregion
}
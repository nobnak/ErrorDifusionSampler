using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureApplier : MonoBehaviour {

    public const string P_MainTex = "_MainTex";

    protected Renderer rend;

    #region interface
    public void Listen(Texture tex) {
        var blk = new MaterialPropertyBlock();
        rend?.GetPropertyBlock(blk);
        blk.SetTexture(P_MainTex, tex);
        rend?.SetPropertyBlock(blk);

        var s = transform.localScale;
        s.x = s.y * tex.width / tex.height;
        transform.localScale = s;
    }
    #endregion

    #region unity
    private void OnEnable() {
        rend = GetComponent<Renderer>();
    }
    #endregion
}
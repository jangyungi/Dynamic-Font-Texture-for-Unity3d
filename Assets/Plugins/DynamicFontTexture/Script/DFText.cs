using UnityEngine;
using System.Collections;

public class DFText : MonoBehaviour {

    #region Public Fields
    [SerializeField]
    private int factor;
    public string fontName;
    public int fontSize;
    #endregion

    #region Private Fields
    //For checking whether this scripts is inited
    private bool isInited;
    //Texture to write on
    private Texture textureToUse = null;
    private int width = 256;
    private int height = 256;
    private string text = "";
    #endregion

    private void setupTexture()
    {
        if (textureToUse != null) return;
        //Need to make Alpha8 Texture
        width = factor * Mathf.FloorToInt(this.transform.localScale.x);
        height = factor * Mathf.FloorToInt(this.transform.localScale.z);
        textureToUse = new Texture2D(width,height,TextureFormat.Alpha8,false);

        //Use the texture for this GameObject's renderer.
        this.transform.renderer.material.mainTexture = textureToUse;
        /*
        this.transform.renderer.material.color=new Color(this.transform.renderer.material.color.r,
            this.transform.renderer.material.color.g,
            this.transform.renderer.material.color.b,
            1f);
        */
    }

    public void setTextLeft(string contents)
    {
        setText(contents, DFTAlignment.Left);
    }

    public void setTextRight(string contents)
    {
        setText(contents, DFTAlignment.Right);
    }

    public void setTextCenter(string contents)
    {
        setText(contents, DFTAlignment.Center);
    }

    public void setText(string contents, DFTAlignment alignment)
    {
        text = contents;
        //Ensure inited
        setupTexture();
        //Write text on the texture
        DynamicFontTextureBinding.WriteOnTexture(textureToUse, contents, fontName, alignment, fontSize, width, height);
    }

    public string getText() {
        return text;
    }
}

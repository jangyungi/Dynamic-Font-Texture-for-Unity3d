using UnityEngine;
using System.Collections;

public class DFTexture : MonoBehaviour {
	
	#region Public Fields
	[SerializeField]
	private int width;
	[SerializeField]
	private int height;
	public string fontName;
	public int fontSize;
	[SerializeField]
	private string currentlyShowing;
	public string CurrentlyShowing
	{
		get{
			return currentlyShowing;
		}
	}
	#endregion
	
	#region Private Fields
	//For checking whether this scripts is inited
	private bool isInited;	
	//Texture to write on
	private Texture textureToUse;
	#endregion
	
	void Start () {
		Init();
		SetString(currentlyShowing);
	}
	
	private void Init()
	{
		if(isInited)return;
		//Need to make Alpha8 Texture
		textureToUse=new Texture2D(width,height,TextureFormat.Alpha8,false);
		this.transform.renderer.material.SetTexture("_MainTex",textureToUse);
		isInited=true;
	}
	
	public void SetString(string contents)
	{
		SetString(contents,DFTAlignment.Left);
	}
	
	public void SetString(string contents,DFTAlignment alignment)
	{
		//Ensure inited
		Init();
		//Write text on the texture
		DynamicFontTextureBinding.WriteOnTexture(textureToUse,contents,fontName,alignment,fontSize,width,height);
		//Use the texture for this GameObject's renderer.
		this.transform.renderer.material.SetTexture("_MainTex",textureToUse);
		this.transform.renderer.material.color=new Color(this.transform.renderer.material.color.r,
			this.transform.renderer.material.color.g,
			this.transform.renderer.material.color.b,
			1f);
		currentlyShowing=contents;
	}
}

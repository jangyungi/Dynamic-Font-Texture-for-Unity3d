using UnityEngine;
using System.Collections;

public class DFTTest : MonoBehaviour {
	public bool resizeTransformToPoT;
	[SerializeField]
	private int width;
	[SerializeField]
	private int height;
	[SerializeField]
	private string currentlyShowing;
	public string CurrentlyShowing
	{
		get{
			return currentlyShowing;
		}
	}
	public string fontName;
	public int fontSize;
	//For checking whether this scripts is inited
	private bool isInited;	
	//Texture to write on
	private Texture textureToUse;

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
		this.transform.renderer.material.color=new Color(1f,1f,1f,1f);
		currentlyShowing=contents;
		if(resizeTransformToPoT)Resize();
	}
	
	#region For Resizing Texture
	
	void Resize()
	{
		float ratio=(float)NextPoT(width)/(float)NextPoT(height);
		if(ratio>1f)
		{
			//If width>height
			
			//Translate upward
			this.transform.localPosition+=10f*new Vector3(0f,(this.transform.localScale.z-this.transform.localScale.z/ratio)/2f,0f);
			
			//Adjust height
			this.transform.localScale=new Vector3(this.transform.localScale.x,this.transform.localScale.y,this.transform.localScale.z/ratio);			
			
		}
		else if(ratio<1f)
		{
			//If height>width
			
			//Translate to left
			this.transform.localPosition-=10f*new Vector3((this.transform.localScale.x-this.transform.localScale.x*ratio)/2f,0f,0f);
			
			//Adjust width
			this.transform.localScale=new Vector3(this.transform.localScale.x*ratio,this.transform.localScale.y,this.transform.localScale.z);
			
		}
	}
	
	int NextPoT(int size)
	{
	    size = size - 1;
	    size = size | (size >> 1);
	    size = size | (size >> 2);
	    size = size | (size >> 4);
	    size = size | (size >> 8);
	    size = size | (size >>16);
	    return size + 1;
	}
	
	
	#endregion
}

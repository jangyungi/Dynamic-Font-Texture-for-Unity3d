using UnityEngine;
using System.Collections;

public class DFTTest : MonoBehaviour {
	public bool resizeTransformToPoT;
	public int width;
	public int height;
	public string stringToWrite;
	public string fontName;
	public int fontSize;
	
	void Start () {
		//Need to make Alpha8 Texture
		Texture tmpTexture=new Texture2D(width,height,TextureFormat.Alpha8,false);
		//Write text on the texture
		DynamicFontTextureBinding.WriteOnTexture(tmpTexture,stringToWrite,fontName,fontSize,width,height);
		//Use the texture for this GameObject's renderer.
		this.transform.renderer.material.SetTexture("_MainTex",tmpTexture);
		if(resizeTransformToPoT)Resize();
	}
	
	void Update()
	{
		//this.transform.Rotate(new Vector3(20f*Time.deltaTime,0f,20f*Time.deltaTime));
	}
	
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
	
}

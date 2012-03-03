using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class DynamicFontTextureBinding : MonoBehaviour {
	
	[DllImport("__Internal")]
	
    private static extern string _writeOnGLTexture(int textureID, string text, string fontName, int fontSize, int width, int height);
 
    public static void WriteOnTexture(Texture textureToWrite, string text, string fontName, int fontSize, int width, int height)
    {
		if(width==0||height==0||fontSize==0)
		{
			Debug.Log("Please check arguments.");
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			_writeOnGLTexture(textureToWrite.GetNativeTextureID(),text,fontName,fontSize,width,height);
		}
    }
    

}
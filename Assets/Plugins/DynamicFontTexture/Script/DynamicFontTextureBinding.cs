using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public enum DFTAlignment{Left,Right,Center}

public class DynamicFontTextureBinding : MonoBehaviour {
	
#if UNITY_ANDROID
	private static AndroidJavaClass jc;
	private static void LoadJC()
	{
		if( jc!=null)return;
		if( Application.platform != RuntimePlatform.Android ) return;
		jc=new AndroidJavaClass("com.crystani.dft.DFTFont");
		Debug.Log(jc);
	}
#endif
#if UNITY_IPHONE
	[DllImport("__Internal")]
    private static extern string _writeOnGLTexture(int textureID, string text, string fontName, int fontSize, int width, int height);
#endif
    public static void WriteOnTexture(Texture textureToWrite, string text, string fontName, DFTAlignment alignment, int fontSize, int width, int height)
    {
		if(width==0||height==0||fontSize==0)
		{
			Debug.Log("Please check arguments.");
		}
		else
		{
#if UNITY_IPHONE
			_writeOnGLTexture(textureToWrite.GetNativeTextureID(),text,fontName,fontSize,width,height);
#endif
#if UNITY_ANDROID
			LoadJC();
			//jc.CallStatic("textToBitmapByList",textureToWrite.GetNativeTextureID(),text,width,height,(int)alignment,fontName,fontSize);
			jc.CallStatic("textToBitmapByList",textureToWrite.GetNativeTextureID(),text);
#endif
		}
    }
}
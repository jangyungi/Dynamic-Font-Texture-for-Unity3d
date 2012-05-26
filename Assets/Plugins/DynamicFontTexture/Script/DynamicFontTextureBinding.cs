using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public enum DFTAlignment{Left,Right,Center}

public class DynamicFontTextureBinding : MonoBehaviour {

#if UNITY_EDITOR
    [DllImport("DynamicFontTexture")]
    private static extern void _writeOnGLTexture(int textureID, string text, string fontName, int alignment, int fontSize, int width, int height);
    public static void WriteOnTexture(Texture textureToWrite, string text, string fontName, DFTAlignment alignment, int fontSize, int width, int height)
    {
        if(width==0||height==0||fontSize==0)
        {
            Debug.Log("DynamicFontTextureBinding.WriteOnTexture: Please check arguments.");
        }
        else
        {
            Debug.Log("DynamicFontTextureBinding.WriteOnTexture: width = " + width + ", height = " + height + ", fontName = " + fontName + ", fontSize = " + fontSize + ", text = " + text);
            _writeOnGLTexture(textureToWrite.GetNativeTextureID(),text,fontName,(int)alignment,fontSize,width,height);
            Debug.Log("DynamicFontTextureBinding.WriteOnTexture Done");
         }
    }

    [DllImport("DynamicFontTexture")]
    private static extern int _widthWithFont(string text, string fontName, int fontSize);
    public static int WidthWithFont(string text, string fontName, int fontSize)
    {
        Debug.Log("DynamicFontTextureBinding.WidthWithFont: fontName = " + fontName + ", fontSize = " + fontSize + ", text = " + text);
        int result = _widthWithFont(text,fontName,fontSize);
        Debug.Log("DynamicFontTextureBinding.WidthWithFont -> " + result);
        return result;
    }

#elif UNITY_ANDROID

    private static AndroidJavaClass jc;
    private static void LoadJC()
    {
        if( jc!=null)return;
        if( Application.platform != RuntimePlatform.Android ) return;
        jc=new AndroidJavaClass("com.crystani.dft.DFTFont");
    }
    public static void WriteOnTexture(Texture textureToWrite, string text, string fontName, DFTAlignment alignment, int fontSize, int width, int height)
    {
        if(width==0||height==0||fontSize==0)
        {
            Debug.Log("Please check arguments.");
        }
        else
        {
            LoadJC();
            string combinedValues=width.ToString()+"_"+height.ToString()+"_"+((int)alignment).ToString()+"_"+fontSize;
            if( jc!=null)jc.CallStatic("textToBitmap",textureToWrite.GetNativeTextureID(),text,fontName,combinedValues);
        }
    }

    public static int WidthWithFont(string text, string fontName, int fontSize)
    {
        LoadJC();
        if( jc!=null)
        {
            return jc.CallStatic<int>("widthWithFont",text,fontName,fontSize);
        }
        else return 0;

#elif UNITY_IPHONE

    [DllImport("__Internal")]
    private static extern void _writeOnGLTexture(int textureID, string text, string fontName, int alignment, int fontSize, int width, int height);
    public static void WriteOnTexture(Texture textureToWrite, string text, string fontName, DFTAlignment alignment, int fontSize, int width, int height)
    {
        if(width==0||height==0||fontSize==0)
        {
            Debug.Log("Please check arguments.");
        }
        else
        {
            _writeOnGLTexture(textureToWrite.GetNativeTextureID(),text,fontName,(int)alignment,fontSize,width,height);
        }
    }

    [DllImport("__Internal")]
    private static extern int _widthWithFont(string text, string fontName, int fontSize);
    public static int WidthWithFont(string text, string fontName, int fontSize)
    {
        return _widthWithFont(text,fontName,fontSize);
    }

#endif

}

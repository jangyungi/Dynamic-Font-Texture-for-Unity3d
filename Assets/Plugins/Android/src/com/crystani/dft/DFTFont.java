package com.crystani.dft;

import java.util.ArrayList;
import java.util.HashMap;

import javax.microedition.khronos.egl.EGL10;
import javax.microedition.khronos.egl.EGLContext;
import javax.microedition.khronos.opengles.GL10;

import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.graphics.Matrix;
import android.opengl.GLUtils;
import android.os.Debug;
import android.util.Log;

public class DFTFont {
	private static HashMap<String, Typeface> typefaces = new HashMap<String, Typeface>();
	
	public static int widthWithFont(String text, String fontName, int fontSize)
	{
		Paint textPaint = new Paint();
		Typeface typeface=typeFaceCache(fontName);
	    textPaint.setTypeface(typeface);
	    textPaint.setTextSize(fontSize);
	    textPaint.setAntiAlias(true);
	    return (int)textPaint.measureText(text);
	}
	
	public static void textToBitmap(int textureID,String text,String fontName,String combinedValues)
	{
		String[] array=combinedValues.split("_");
		int width=Integer.parseInt(array[0]);
		int height=Integer.parseInt(array[1]);
		int alignment=Integer.parseInt(array[2]);
		int fontSize=Integer.parseInt(array[3]);
		textToBitmap(textureID,text,width,height,alignment,fontName,fontSize);
	}
	
	private static void textToBitmap(int textureID,String text, int width, int height, int alignment, String fontName, float fontSize) {
    	
		//Get Typeface
		Typeface typeface=typeFaceCache(fontName);
        
        Paint textPaint = new Paint();
        textPaint.setTypeface(typeface);
        textPaint.setTextSize(fontSize);
        textPaint.setAntiAlias(true);
        
        float ascent = -textPaint.ascent(); 
        float descent = textPaint.descent();
        
        int textHeight = (int)(ascent + descent);
        int spacing = (int) Math.ceil((ascent + descent) * 0.1f);
        
        //Get power of 2 size for bitmap
        int widthPow2 = toPow2(width);
        int heightPow2 = toPow2(height);
        
        
        //Create an empty bitmap
        Bitmap bitmap = Bitmap.createBitmap(widthPow2, heightPow2, Bitmap.Config.ALPHA_8);
        bitmap.eraseColor(Color.TRANSPARENT);
        
        Canvas canvas = new Canvas(bitmap);
        
        //Flip Y axis
        Matrix yFlip = new Matrix();
        yFlip.postScale(1, -1); 
        canvas.setMatrix(yFlip);
        
        //Write on canvas
        ArrayList<String> wrapped = WrapText(textPaint, text, width);
        float blockHeight = (ascent + descent) * wrapped.size();
        for(int i = 0; i < wrapped.size(); ++i)
        {
        	String str = wrapped.get(i);
            
        	float offset = 0;
        	float vOffset = 0;
            
	        switch (alignment) {
	        		//Left
	            case 0:
	                offset = 0;
	                break;
	                //Right
	            case 1:
	            	offset = (width - textPaint.measureText(str));
	                break;
	                //Center
	            case 2:
	            	offset = (width - textPaint.measureText(str)) * 0.5f;
	            	vOffset = (height - blockHeight) * 0.5f;
	                break;
	        }
	        canvas.drawText(str,offset,-1*(vOffset + ascent + ((textHeight + spacing) * (wrapped.size()-i))),textPaint);
        }
        
        
        //Write on the texture with textureID with bitmap
        EGL10 egl = (EGL10)EGLContext.getEGL(); 
        GL10 gl = (GL10)egl.eglGetCurrentContext().getGL();
        gl.glBindTexture(GL10.GL_TEXTURE_2D, textureID);
        GLUtils.texImage2D(GL10.GL_TEXTURE_2D, 0, bitmap, 0);
        
        //Release bitmap
        bitmap.recycle();
        bitmap=null;
    }
	
	private static Typeface typeFaceCache(String fontname)
	{
		Typeface typeface;
		if(!typefaces.containsKey(fontname)) {
	        typeface = Typeface.create(fontname, Typeface.NORMAL);
	        typefaces.put(fontname, typeface);
    	} else {
    		typeface = typefaces.get(fontname);
    	}
		return typeface;
	}
	
	//Get power of two size
	private static int toPow2(int v) {
        if ((v != 1) && (v & (v - 1)) != 0) {
            int i = 1;
            while (i < v)
                i *= 2;
            v = i;
        }
        return v;
    }
	
	//Get Wraped text
	protected static ArrayList<String> WrapText(Paint textPaint, String text, float width)
    {
        float spaceLeft = width;
        
        String [] words = text.split(" ");
        ArrayList<String> lines = new ArrayList<String>();
        float spaceWidth = textPaint.measureText(" ");
        StringBuilder tempLine = new StringBuilder("");
        
        for(String word : words)
        {
            float wordWidth = textPaint.measureText(word);
            
            if (wordWidth > spaceLeft) {
            	if(tempLine.length() > 0) {
                	tempLine.deleteCharAt(tempLine.length() - 1);
                }
            	
                lines.add(tempLine.toString());
                
                tempLine = new StringBuilder("");
                tempLine.append(word);
                
                spaceLeft = width - (wordWidth + spaceWidth);
            }
            else
            {
                tempLine.append(word);
                spaceLeft -= (wordWidth + spaceWidth);
            }
            
            tempLine.append(" ");
        }
        
        if(tempLine.length() > 0) {
        	tempLine.deleteCharAt(tempLine.length() - 1);
        }
        
        lines.add(tempLine.toString());
        
        
        
        return lines;
    }
}

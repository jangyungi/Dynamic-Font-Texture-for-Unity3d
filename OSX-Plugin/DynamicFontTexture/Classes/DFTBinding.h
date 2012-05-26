//
//  DFTBinding.h
//  DynamicFontTexture
//
//  Created by YJ Park on 12-5-26.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

#ifndef DynamicFontTexture_DFTBinding_h
#define DynamicFontTexture_DFTBinding_h

extern "C" {
    void _writeOnGLTexture(int textureID, const char* text, const char* fontName, int alignment, int fontSize, int width, int height);
    int _widthWithFont(const char* text, const char* fontName, int fontSize);
}
    
#endif

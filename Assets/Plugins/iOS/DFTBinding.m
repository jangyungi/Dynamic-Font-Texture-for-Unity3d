#import "DFTManager.h"

#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Convert 
void _writeOnGLTexture(int textureID, const char* text, const char* fontName, int fontSize, int width, int height)
{
    NSString *textString = GetStringParam(text);
    NSString *fontNameString = GetStringParam(fontName);

    CGSize dimensions = CGSizeMake( width, height );
    
	[[DFTManager sharedManager] writeOnTexture:textString textureID:textureID textureSize:dimensions alignment:UITextAlignmentLeft lineBreakMode:UILineBreakModeWordWrap fontName:fontNameString fontSize:fontSize];
}



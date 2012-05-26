#import "DFTManager.h"

#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Convert
void _writeOnGLTexture(int textureID, const char* text, const char* fontName, int alignment, int fontSize, int width, int height)
{
    NSString *textString = GetStringParam(text);
    NSString *fontNameString = GetStringParam(fontName);
    CGSize dimensions = CGSizeMake( width, height );

    if(alignment==2)
    {
        [[DFTManager sharedManager] writeOnTexture:textString textureID:textureID textureSize:dimensions alignment:NSCenterTextAlignment lineBreakMode:NSLineBreakByCharWrapping fontName:fontNameString fontSize:fontSize];
    }
    else if(alignment==1)
    {
        [[DFTManager sharedManager] writeOnTexture:textString textureID:textureID textureSize:dimensions alignment:NSRightTextAlignment lineBreakMode:NSLineBreakByCharWrapping fontName:fontNameString fontSize:fontSize];
    }
    else
    {
        [[DFTManager sharedManager] writeOnTexture:textString textureID:textureID textureSize:dimensions alignment:NSLeftTextAlignment lineBreakMode:NSLineBreakByCharWrapping fontName:fontNameString fontSize:fontSize];
    }
}


int _widthWithFont(const char* text, const char* fontName, int fontSize)
{
    return [[DFTManager sharedManager] widthWithFont:GetStringParam(text) fontName:GetStringParam(fontName) fontSize:fontSize];
}

#import "DFTManager.h"
#import <CoreGraphics/CoreGraphics.h>
#import <Foundation/Foundation.h>

void UnitySendMessage( const char * className, const char * methodName, const char * param );

@implementation DFTManager

#pragma mark NSObject

+ (DFTManager*)sharedManager
{
	static DFTManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[DFTManager alloc] init];
	
	return sharedSingleton;
}

- (void) writeOnTexture:(NSString*)text textureID:(int)textureID textureSize:(CGSize)dimensions alignment:(UITextAlignment)alignment lineBreakMode:(UILineBreakMode)lineBreakMode fontName:(NSString*)fontName fontSize:(int)fontSize
{
    UIFont* font=[UIFont fontWithName:fontName size:fontSize];
    NSUInteger PoTWidth=[self nextPoT:dimensions.width];
    NSUInteger PoTHeight=[self nextPoT:dimensions.height];
    unsigned char* data=(unsigned char *)calloc(PoTWidth,PoTHeight);
    
	CGColorSpace* colorSpace=CGColorSpaceCreateDeviceGray();
    CGContext* context = CGBitmapContextCreate(data, PoTWidth, PoTHeight, 8, PoTWidth, colorSpace, kCGImageAlphaNone);
	CGColorSpaceRelease(colorSpace);
    
    if(!context)free(data);
    else
    {
        CGContextSetGrayFillColor(context, 1.0f, 1.0f);
        CGContextScaleCTM(context, 1.0f, 1.0f);
        
        UIGraphicsPushContext(context);
        //Write Text on specified position
        [text drawInRect:CGRectMake(0, 0, dimensions.width, dimensions.height) withFont:font lineBreakMode:lineBreakMode alignment:alignment];
        UIGraphicsPopContext();
        
        //Update GL Texture
        glBindTexture(GL_TEXTURE_2D, textureID);
        glTexImage2D(GL_TEXTURE_2D, 0, GL_ALPHA, (GLsizei) PoTWidth, (GLsizei) PoTHeight, 0, GL_ALPHA, GL_UNSIGNED_BYTE, data);
        
        CGContextRelease(context);
        free(data);
    }
}

- (int) nextPoT:(int)size
{
    size = size - 1;
    size = size | (size >> 1);
    size = size | (size >> 2);
    size = size | (size >> 4);
    size = size | (size >> 8);
    size = size | (size >>16);
    return size + 1;
}
@end

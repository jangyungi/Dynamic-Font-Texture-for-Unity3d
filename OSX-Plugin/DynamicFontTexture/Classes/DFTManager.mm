#import "DFTManager.h"
#import <ApplicationServices/ApplicationServices.h>
#import <Foundation/Foundation.h>
#import <AppKit/AppKit.h>
#import <OpenGL/gl.h>
#import <OpenGL/glext.h>
#import <OpenGL/OpenGL.h>

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

- (void) writeOnTexture:(NSString*)text textureID:(int)textureID textureSize:(CGSize)dimensions alignment:(NSTextAlignment)alignment lineBreakMode:(NSLineBreakMode)lineBreakMode fontName:(NSString*)fontName fontSize:(int)fontSize
{
    NSUInteger PoTWidth=[self nextPoT:dimensions.width];
    NSUInteger PoTHeight=[self nextPoT:dimensions.height];
    unsigned char* data=(unsigned char *)calloc(PoTWidth,PoTHeight);

    CGColorSpace* colorSpace=CGColorSpaceCreateDeviceGray();
    CGContext* context = CGBitmapContextCreate(data, PoTWidth, PoTHeight, 8, PoTWidth, colorSpace, kCGImageAlphaNone);
    CGColorSpaceRelease(colorSpace);

    if(!context) {
        free(data);
        NSLog(@"writeOnTexture: Can't create context!");
    } else {
        CGContextSetGrayFillColor(context, 1.0f, 1.0f);
        CGContextScaleCTM(context, 1.0f, 1.0f);
        
        CGContextSetFillColorWithColor(context, CGColorCreateGenericRGB(1, 0, 0, 0.5));

        NSGraphicsContext *maskGraphicsContext = [NSGraphicsContext
                                                  graphicsContextWithGraphicsPort:context flipped:YES];
        [NSGraphicsContext saveGraphicsState];
        [NSGraphicsContext setCurrentContext:maskGraphicsContext];
        //Write Text on specified position
        NSFont* font=[NSFont fontWithName:fontName size:fontSize];

        NSLog(@"AAAA %@, %f, %f, %@", font, dimensions.width, dimensions.height, text);
        
        NSMutableParagraphStyle* style = [[[NSMutableParagraphStyle alloc] init] autorelease];
        [style setLineBreakMode:lineBreakMode];
        [style setAlignment:alignment];
        NSDictionary* attributes = [NSDictionary dictionaryWithObjectsAndKeys:
            font, NSFontAttributeName,
            style, NSParagraphStyleAttributeName,
            [NSColor whiteColor], NSForegroundColorAttributeName,
            [NSColor blackColor], NSBackgroundColorAttributeName,
                                    nil];
        [text drawInRect:NSRectFromCGRect(CGRectMake(0, 0, dimensions.width, dimensions.height)) withAttributes:attributes];
        [NSGraphicsContext restoreGraphicsState];

        //Update GL Texture
        glBindTexture(GL_TEXTURE_2D, textureID);
        glTexImage2D(GL_TEXTURE_2D, 0, GL_ALPHA, (GLsizei) PoTWidth, (GLsizei) PoTHeight, 0, GL_ALPHA, GL_UNSIGNED_BYTE, data);

        CGContextRelease(context);
        free(data);
    }
}

-(int) widthWithFont:(NSString*)text fontName:(NSString*)fontName fontSize:(int)fontSize
{
    NSFont* font=[NSFont fontWithName:fontName size:fontSize];
    return ((NSSize)[text sizeWithAttributes:[NSDictionary dictionaryWithObjectsAndKeys:font, NSFontAttributeName, nil]]).width;
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

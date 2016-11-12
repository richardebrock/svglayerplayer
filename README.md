#SVGLayerPlayer

SVGLayerPlayer is a C# application that draws the layers of an SVG file in a sequence to create an animation. The application watches for file changes and reloads the SVG file in real-time, allowing you to edit your animation in Inkscape and see the changes applied in real-time. Layers can be exported as a sprite sheet, as individual sprites in PNG format or as an animated GIF.

This project uses the arc segment code from the C# SVG rendering library on codeplex: https://svg.codeplex.com/

##Known Issues

* GIF animation does not loop due to an issue in System.Windows.Media.Imaging.GifBitmapEncoder.

##Compatibility

The application has been tested with SVG files created in Inkscape 0.91.

##Dependencies

At this time the application does not have any external dependencies.
# DisplayUtil

DisplayUtil is a C# webserver for building status screens for an ESP32 with a 
epaper display.

## Features

 - Rendering an XML Template with [Scriban](https://github.com/scriban/scriban)
 - Apply an XML document to a layouting system
 - Apply the layout system on an [SkiaSharp](https://github.com/mono/SkiaSharp) canvas
 - Compress the image for the two color eInk display

## Main idea

Rendering an image where I have massive power and submit it to the display.
Since I'm using ESPHome, I send a URL over MQTT and the ESP fetches the image
over HTTP.
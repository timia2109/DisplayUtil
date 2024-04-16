# DisplayUtil

DisplayUtil is a C# web server for creating status screens for an ESP32 with an 
epaper display.

## Features (of server)

 - Render an XML template with [Scriban](https://github.com/scriban/scriban)
 - Applying an XML document to a layout system
 - Applying the layout system to a [SkiaSharp](https://github.com/mono/SkiaSharp) canvas
 - Compressing the image for the two-color eInk display

## Main idea

Render an image where I have massive power and submit it to the display.
Since I'm using ESPHome, I send a URL over MQTT and the ESP fetches the image over HTTP.

## Project structure

 - components/mqtt_screen: ESPHome implementation for image handling
 - DisplayUtil/EspUtil: Converting images to two-color images and ESP compression
 - DisplayUtil/HomeAssistant: utilities for working with HomeAssistant entities
 - DisplayUtil/Layouting: Layouting system
 - DisplayUtil/MqttExport: Export URL to MQTT
 - DisplayUtil/Providers: various providers (for fonts & svg icons)
 - DisplayUtil/Screens: Utilities for screen management (various outputs)
 - DisplayUtil/Template: Utilities for handling Scriban templates and their rendering
 - DisplayUtil/Utils: various utils
 - DisplayUtil/XmlModel: Deserialization of XML content
  
## Setup

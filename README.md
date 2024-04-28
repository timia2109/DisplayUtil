# DisplayUtil

DisplayUtil is a C# web server for creating status screens for an ESP32 with an
epaper display.

## Features (of server)

-   Render an XML template with [Scriban](https://github.com/scriban/scriban)
-   Applying an XML document to a layout system
-   Applying the layout system to a [SkiaSharp](https://github.com/mono/SkiaSharp) canvas
-   Compressing the image for the two-color eInk display

## Main idea

Render an image where I have massive power and submit it to the display.
Since I'm using ESPHome, I send a URL over MQTT and the ESP fetches the image over HTTP.
You can find the [ESPHome implementation here](https://github.com/timia2109/esphome_http_screen)

## Project structure

-   DisplayUtil/EspUtil: Converting images to two-color images and ESP compression
-   DisplayUtil/HomeAssistant: utilities for working with HomeAssistant entities
-   DisplayUtil/Layouting: Layouting system
-   DisplayUtil/MqttExport: Export URL to MQTT
-   DisplayUtil/Providers: various providers (for fonts & svg icons)
-   DisplayUtil/Screens: Utilities for screen management (various outputs)
-   DisplayUtil/Template: Utilities for handling Scriban templates and their rendering
-   DisplayUtil/Utils: various utils
-   DisplayUtil/XmlModel: Deserialization of XML content

## Configuration

This application requires many modules, with different configuration

### HomeAssistant

When enabling it mirrors the Hass States and exposes the following Scriban functions

| Scriban Function                                                  | Description                     |
| ----------------------------------------------------------------- | ------------------------------- |
| `hass.get_state(entity_id: string): string`                       | Gets a state from HomeAssistant |
| `hass.get_attribute(entity_id: string, attribute:string): string` | Gets an attribute of a entity   |
| `hass.get_float_state(entity_id: string): float`                  | Gets a state as float           |
| `hass.get_datetime_state(entity_id: string): DateTime`            | Gets a state as DateTime        |

To enable the Hass support the following configuration needs to be provided:

| Field                                          | Description                   |
| ---------------------------------------------- | ----------------------------- |
| **`HomeAssistant__Host`**                      | Host of the Hass instance     |
| **`HomeAssistant__Token`**                     | Token for Hass WebSocket      |
| `HomeAssistant__Port = 8123`                   | Port of WebSocket API         |
| `HomeAssistant__Ssl = false`                   | Enables SSL for the WebSocket |
| `HomeAssistant__WebsocketPath = api/websocket` | Path to the WebSocket Api     |

### HomeAssistant Calendar Integration

On top of Hass Integration, an additional handling for Calendars can be enabled. Therefor additional configuration is required.
Calendar Appointments are fetched once an hour and are available in Scriban with the `appointments` variable

| Field                                                  | Description                                            |
| ------------------------------------------------------ | ------------------------------------------------------ |
| `HomeAssistant__CalendarEntities__<number start at 0>` | Calendar entities which are used to fetch appointments |

import esphome.codegen as cg
import esphome.config_validation as cv
from esphome.components import mqtt, display, http_request
from esphome.const import CONF_ID

DEPENDENCIES = ['mqtt', 'display', 'http_request']

AUTO_LOAD = ["mqtt_screen"]

mqtt_ns = cg.esphome_ns.namespace("mqtt_screen")

MqttScreen = mqtt_ns.class_("MqttScreen", cg.Component)

CONF_CLIENT = "client"
CONF_SCREEN = "display"
CONF_TOPIC = "topic"
CONF_HTTP_CLIENT = "http_client"

CONFIG_SCHEMA = cv.COMPONENT_SCHEMA.extend({
    cv.GenerateID(): cv.declare_id(MqttScreen),
    cv.Required(CONF_CLIENT): cv.use_id(mqtt.MQTTClientComponent),
    cv.Required(CONF_SCREEN): cv.use_id(display.DisplayBuffer),
    cv.Required(CONF_HTTP_CLIENT): cv.use_id(http_request.HttpRequestComponent),
    cv.Required(CONF_TOPIC): cv.string
})

async def to_code(config):
    var = cg.new_Pvariable(config[CONF_ID])
    screen = await cg.get_variable(config[CONF_SCREEN])
    mqttClient = await cg.get_variable(config[CONF_CLIENT])
    httpClient = await cg.get_variable(config[CONF_HTTP_CLIENT])

    cg.add(var.set_topic(config[CONF_TOPIC]))
    cg.add(var.set_mqtt(mqttClient))
    cg.add(var.set_display(screen))
    cg.add(var.set_http_client(httpClient))

    await cg.register_component(var, config)
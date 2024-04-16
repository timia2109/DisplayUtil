#pragma once

#include "esphome.h"
using namespace esphome;

namespace esphome
{
    namespace mqtt_screen
    {

        static const std::string BASE64_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                        "abcdefghijklmnopqrstuvwxyz"
                                        "0123456789+/";

        static inline bool is_base64(char c) { return (isalnum(c) || (c == '+') || (c == '/')); }

        std::vector<char> *base64_decode(std::string const &);

        class Position {
            public:
                Position(int width, int height) {
                    _width = width;
                    _height = height;
                };
                inline int get_x(){ return _x + 1; };
                inline int get_y(){ return _y + 1; };
                void add_pixels(int quantity);
                inline Position operator++(int i) {
                    add_pixels(1);
                    return *this;
                };
                inline Position& operator +=(const int& quantity) {
                    add_pixels(quantity);
                    return *this;
                };

            private:
                int _x{0};
                int _y{0};
                int _width;
                int _height;
        };

        class MqttScreen : public Component
        {
        public:
            void set_topic(const std::string &topic) { topic_ = topic; }
            void set_mqtt(mqtt::MQTTClientComponent *mqttClient) { mqtt_client_ = mqttClient; }
            void set_display(display::Display *display) { display_ = display; }
            void set_http_client(http_request::HttpRequestComponent *http_client) {
                http_client_ = http_client;
            }

            void process(int32_t status_code, uint32_t duration_ms);

            void on_message(const std::string &topic, const std::string &message);
            void draw(display::Display &display);

            void setup() override;
            void loop() override{};
            void dump_config() override;
            float get_setup_priority() const override;

        private:
            std::vector<unsigned short>* data_ {nullptr};

            http_request::HttpRequestResponseTrigger *trigger_;
            display::Display *display_;
            mqtt::MQTTClientComponent *mqtt_client_;
            http_request::HttpRequestComponent *http_client_;
            std::string topic_;
        };
    }
}
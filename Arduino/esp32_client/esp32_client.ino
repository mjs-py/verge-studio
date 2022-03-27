#include <WiFi.h>
#include <WebSocketsClient.h>

// initialize network parameters
const char* ssid = "YOUR_SSID";
const char* password = "YOUR_PASSWORD";

// declare websocket client class variable
WebSocketsClient webSocket;

void setup() {
  // connect to WiFi
  WiFi.begin(ssid,password);
  Serial.begin(115200);
  while(WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }
  Serial.println();
  Serial.print("IP Address: "); Serial.println(WiFi.localIP());

  // server address, port, and URL path
  webSocket.begin("LOCAL_IP", "PORT", "/");

  // try ever 5000 again if connection has failed
  webSocket.setReconnectInterval(5000);
}

void loop() {
  webSocket.loop();
}

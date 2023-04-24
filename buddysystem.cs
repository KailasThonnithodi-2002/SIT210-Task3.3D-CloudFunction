// This #include statement was automatically added by the Particle IDE.
#include <MQTT.h>

const int trigger = D0;
const int echo = D1;
const int led = D2;


MQTT client("broker.emqx.io", 1883, callback);

void callback(char* topic, byte* payload, unsigned int length) {
    
    String message = "";
    for (int i = 0; i < length; i++) {
    message += (char)payload[i];
    }
    
    if (message == "wave") {
    Serial.println("[Annoucement] WAVE Message Recieved.");
    client.publish("SIT210/wave", "Kailas Thonnithodi [WAVE]");
    blink_led(500);
    } else if (message == "pat") {
    Serial.println("[Annoucement] PAT Message Recieved.");
    client.publish("SIT210/wave", "Kailas Thonnithodi [PAT]");
    blink_led(1500);
    }
  
}


// MQTT client("broker.emqx.io", 1883, myHandler);

void setup() {
    
  Particle.connect();
  
  // connect to MQTT broker
  client.connect("e00fce68777387e21be98112", NULL, NULL);
  client.subscribe("SIT210/wave");
  client.subscribe("SIT210/pat");
  
  pinMode(trigger, OUTPUT);
  pinMode(led, OUTPUT);
  pinMode(echo, INPUT);
  
  Serial.begin(9600);
}

void loop() {
    
  // keep the connection alive
  client.loop();
    
  float distance_sense = distance();
  Serial.println("Distamce: " + String(distance_sense) + "cm");
  
  // if the system detects physical wave, the publish name 
  
  // waving
  if (distance_sense > 5.0 && distance_sense < 10.0) {
      client.publish("SIT210/wave", "Kailas Thonnithodi [WAVE]");
      Serial.println("[Annoucement] WAVE Message Published.");
      blink_led(500);
  }
  
  // patting
  if (distance_sense < 3.0) {
      client.publish("SIT210/wave", "Kailas Thonnithodi [PAT]");
      Serial.println("[Annoucement] PAT Message Published.");
      blink_led(1500); // patting is 3 times slower than the waving
  }
  
  delay(500);
}

// measure the distance in the form of float
float distance() {
  digitalWrite(trigger, LOW);
  delayMicroseconds(2);
  digitalWrite(trigger, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigger, LOW);
  
  long duration = pulseIn(echo, HIGH);
  float distance = duration * 0.034 / 2;
  return distance;
}

// Blinks the led based on the speed parameter
void blink_led(int speed) {
    digitalWrite(led, HIGH);
    delay(speed);
    digitalWrite(led, LOW);
    delay(speed);
    digitalWrite(led, HIGH);
    delay(speed);
    digitalWrite(led, LOW);
    delay(speed);
    digitalWrite(led, HIGH);
    delay(speed);
    digitalWrite(led, LOW);
}


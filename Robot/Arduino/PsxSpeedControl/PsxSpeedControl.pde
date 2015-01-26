#include "WProgram.h"
#include <Servo.h> 
#include "Psx_analog.h"      // Includes the Psx Library to access a Sony Playstation controller

#define c_MaxSpeed 45.0   // range is 0 ... 90 (half servo range)

// Sony Playstation 2 Controller
#define c_PsxDataPin 36
#define c_PsxCommandPin 35
#define c_PsxAttPin 33
#define c_PsxClockPin 34

Psx _Psx;
Servo _RightServo;  // create servo object to control right motor
Servo _LeftServo;  // create servo object to control left motor

void setup()
{
  Serial.begin(115200);
  _Psx.setupPins(c_PsxDataPin, c_PsxCommandPin, c_PsxAttPin, c_PsxClockPin);  // Defines what each pin is used (Data Pin #, Cmnd Pin #, Att Pin #, Clk Pin #)
  _Psx.initcontroller(psxAnalog);
  
  _RightServo.attach(2);  // attaches the servo on specified pin to the servo object 
  _LeftServo.attach(3);  // attaches the servo on specified pin to the servo object 

  delay(100);
}

void loop()
{
  Serial.print(_Psx.Controller_mode, DEC);     // prints value as string in hexadecimal (base 16) 
  Serial.print("\t");
  Serial.print(_Psx.digital_buttons, BIN);     // prints value as string in hexadecimal (base 16)  
  Serial.print("\t");
  Serial.print(_Psx.Right_x, DEC);     // prints value as string in hexadecimal (base 16)    
  Serial.print("\t");
  Serial.print(_Psx.Right_y, DEC);     // prints value as string in hexadecimal (base 16)    
  Serial.print("\t");
  Serial.print(_Psx.Left_x, DEC);     // prints value as string in hexadecimal (base 16)    
  Serial.print("\t");
  Serial.print(_Psx.Left_y, DEC);     // prints value as string in hexadecimal (base 16)      
  Serial.print("\n");

  IssueCommands();
  delay(10);   // waits for the servo to get there 
}

void IssueCommands()
{
  float rigthSpeed, leftSpeed;
  
  _Psx.poll(); // poll the Sony Playstation controller
  if (_Psx.Controller_mode == 115)
  {
    // analog mode; we use the right joystick to determine the desired speed

    float mainSpeed = -(_Psx.Right_y - 128.0);    
    float rightLeftRatio = -(_Psx.Right_x - 128) / 128.0;
    
    rigthSpeed = mainSpeed + rightLeftRatio * 128;
    leftSpeed = mainSpeed - rightLeftRatio * 128;
  }
  else
  {
    rigthSpeed = 0;
    leftSpeed = 0;
  }
  Serial.print("Speed: ");
  Serial.print(rigthSpeed);
  Serial.print("\t");
  Serial.print(leftSpeed);
  Serial.print("\n");
  
  float rightServoValue = map(rigthSpeed, -128.0, 127.0, 90.0 - c_MaxSpeed, 90.0 + c_MaxSpeed);     // scale it to use it with the servo (value between 0 and 180) 
  float leftServoValue = map(leftSpeed, -128.0, 127.0, 90.0 - c_MaxSpeed, 90.0 + c_MaxSpeed);     // scale it to use it with the servo (value between 0 and 180) 
 
  Serial.print("Servos: ");
  Serial.print(rightServoValue);
  Serial.print("\t");
  Serial.print(leftServoValue);
  Serial.print("\n");

  _RightServo.write(rightServoValue);     // sets the servo position according to the scaled value (0 ... 179)
  _LeftServo.write(leftServoValue);     // sets the servo position according to the scaled value (0 ... 179)
}



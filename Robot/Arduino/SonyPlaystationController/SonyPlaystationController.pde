/*  Analog PSX Controller Decoder Library (Psx_analog.pde)
	Written by: Kieran Levin Mar 8th, 2009
						KL: updated July 5 2010
						updated button defines to fix button mappings
			kieranlevin.com

	Based upon the work by Kevin Ahrendt June 22nd, 2008

	Analog protocal implemented with information from 
	http://www.curiousinventor.com/guides/ps2

	Fixed reverse shifting in (MSB->LSB) now reads analog values....
	This also has been tested to work with a generic wireless psx controller

	Controller protocol implemented using Andrew J McCubbin's analysis.
	http://www.gamesx.com/controldata/psxcont/psxcont.htm
	
	Shift command is based on tutorial examples for ShiftIn and ShiftOut
	functions both written by Carlyn Maw and Tom Igoe
	http://www.arduino.cc/en/Tutorial/ShiftIn
	http://www.arduino.cc/en/Tutorial/ShiftOut
	
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
--------------------------------------------------------------------------------
    Standard Digital Pad

    BYTE    CMND    DATA

     01     0x01    idle
     02     0x42    0x41
     03     idle    0x5A    Bit0 Bit1 Bit2 Bit3 Bit4 Bit5 Bit6 Bit7
     04     idle    data    SLCT           STRT UP   RGHT DOWN LEFT
     05     idle    data    L2   R2    L1  R1   /\   O    X    |_|

    All Buttons active low.

--------------------------------------------------------------------------------
    Analogue Controller in Red Mode

    BYTE    CMND    DATA

     01     0x01    idle
     02     0x42    0x73
     03     idle    0x5A    Bit0 Bit1 Bit2 Bit3 Bit4 Bit5 Bit6 Bit7
     04     idle    data    SLCT JOYR JOYL STRT UP   RGHT DOWN LEFT
     05     idle    data    L2   R2   L1   R1   /\   O    X    |_|
     06     idle    data    Right Joy 0x00 = Left  0xFF = Right
     07     idle    data    Right Joy 0x00 = Up    0xFF = Down
     08     idle    data    Left Joy  0x00 = Left  0xFF = Right
     09     idle    data    Left Joy  0x00 = Up    0xFF = Down
--------------------------------------------------------------------------------
*/

#include "WProgram.h"
#include "Psx_analog.h"                                          // Includes the Psx Library 

#define dataPin 36
#define cmndPin 35
#define attPin 33
#define clockPin 34

//#define motorup  5
//#define motordown 6
//#define motorleft  9
//#define motorright 10
//
//#define center  0x7F
//#define LEDPin 13

Psx Psx;                                                  // Initializes the library

void setup()
{
  Psx.setupPins(dataPin, cmndPin, attPin, clockPin);  // Defines what each pin is used
                                                      // (Data Pin #, Cmnd Pin #, Att Pin #, Clk Pin #)
                                                       
  Psx.initcontroller(psxAnalog);    
  
 
  Serial.begin(115200); 
  Serial.println("Raw controller values"); 

  
  // wait for the long string to be sent 
  delay(100);
}

void loop()
{
  Psx.poll();                                      // Psx.read() initiates the PSX controller and returns
  Serial.print("\n");                                            // the button data
  Serial.print(Psx.Controller_mode, DEC);     // prints value as string in hexadecimal (base 16) 
  Serial.print("\t");
  Serial.print(Psx.digital_buttons, BIN);     // prints value as string in hexadecimal (base 16)  
  Serial.print("\t");
  Serial.print(Psx.Right_x, DEC);     // prints value as string in hexadecimal (base 16)    
  Serial.print("\t");
  Serial.print(Psx.Right_y, DEC);     // prints value as string in hexadecimal (base 16)    
  Serial.print("\t");
  Serial.print(Psx.Left_x, DEC);     // prints value as string in hexadecimal (base 16)    
  Serial.print("\t");
  Serial.print(Psx.Left_y, DEC);     // prints value as string in hexadecimal (base 16)      
  
  delay(100); 
}


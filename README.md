# NodeMCU-Wixel
Low-cost Wifi connected wixel for use with xDrip.

## About
This is a low-cost do-it-yourself Wifi connected receiver for the Dexcom G4 transmitter. It is intended to be used together with [xDrip](https://github.com/StephenBlackWasAlreadyTaken/xDrip/wiki/xDrip-Beta). It is possible to use multiple NodeMCU-Wixels to cover a large area. Can be used together with a Bluetooth Wixel, if you do not want to carray around the Bluetooth Wixel at home.

![Blueprint](https://github.com/MrPsi/NodeMCU-Wixel/blob/master/img/blueprint.png?raw=true "Blueprint")
Example use

# What you need
* A [Wixel](http://www.hobbytronics.co.uk/wixel-usb-wireless-module) ~15 €
* A [NodeMCU](http://www.ebay.co.uk/itm/NodeMCU-LUA-WIFI-Internet-Development-Board-Based-on-ESP8266-/291505733201?hash=item43df187e51:g:iikAAOSwHPlWeoBr) ~4 €
* A micro usb charger (An old phone charger works great.)
* A stripboard or breadboard and cables depending on how it should be assembled.
* A box to put it in.
* A Wifi network to connect the nodes too.
* An Android device (phone/tablet/computer)

## Diagram

![Diagram](https://github.com/MrPsi/NodeMCU-Wixel/blob/master/img/diagram.png?raw=true "Diagram")
Diagram

![Diagram](https://github.com/MrPsi/NodeMCU-Wixel/blob/master/img/box1.jpg?raw=true "Diagram")
Finished NodeMCU-Wixel in box.

See bottom for more images.

## How-to

### Windows

1. Assemble the NodeMCU-Wixel by soldering or using a breadboard.
2. Download the [NodeMCU-Wixel repository](https://github.com/MrPsi/NodeMCU-Wixel) and extract the files.
3. Download the [wixel-xDrip repository](https://github.com/StephenBlackWasAlreadyTaken/wixel-xDrip) and extract the files.
4. Modify transmitter id in Wixel code from the NodeMCU-Wixel repository (NodeMCU-Wixel/Wixel/dexdrip.c) and set it to your transmitter id. Then save.
5. Copy dexdrip.c from the NodeMCU-Wixel repository (NodeMCU-Wixel/Wixel/dexdrip.c) to the wixel-xDrip repository (wixel-xDrip/apps/dexdrip/dexdrip.c).
6. Build the Wixel code in the wixel-xDrip repository and write it to the Wixel by following the instructions [here](https://github.com/StephenBlackWasAlreadyTaken/wixel-xDrip).
7. Flash the NodeMCU with the firmware from the NodeMCU-Wixel repository (NodeMCU-Wixel/Firmware) using the [NodeMCU Firmware Programmer](https://github.com/nodemcu/nodemcu-flasher).
..* Connect the NodeMCU to the computer.
..* Open the firmware programmer.
..* In the Config tab:
....* Browse for the firmware file.
....* Make sure that the new firmware file is the only selected firmware.
....* Set Offset to 0x00000.
..* In the Operation tab:
....* Select COM Port.
....* Click the Flash button.
..* Wait for the flash to finish.
8. Change Wifi settings in the NodeMCU-Wixel repository (NodeMCU-Wixel/NodeMCU/modWifi.lua). Change ip, netmask, gateway, wifiSsid and wifiPassword depending on your network configuration. Then save. If you have multiple NodeMCU-Wixels, then make sure they each have a different ip.
9. Upload code to the NodeMCU using [ESPlorer](http://esp8266.ru/esplorer/).
..* Select COM port.
..* Set Baud rate to 115200.
..* Click Open.
..* Make sure you are connected to the NodeMCU. You might need to press the reset button on the NodeMCU.
..* Upload all files for the NodeMCU from the NodeMCU-Wixel repository (NodeMCU-Wixel/NodeMCU/) by clicking "Upload..." and selecting a file. Repeat until all files are uploaded.
10. Restart the NodeMCU by pressing the reset button on it.
..* Wait until you see the text "Starting NodeMCU Wixel in 10 seconds...", then you know it is working.
11. Download and install [xDrip](https://github.com/StephenBlackWasAlreadyTaken/xDrip/wiki/xDrip-Beta) to your Android device (phone/tablet/computer).
12. Start xDrip and goto Settings.
..* Select "Wifi Wixel" as Hardware Data Source. (Or "Wifi Wixel + BT Wixel" if you also want to use a Bluetooth Wixel.)
..* Under "List of recievers" enter the ip-addresses and ports of all NodeMCU-Wixels you want to use. This is the ip-address that was configured when changing Wifi settings for the NodeMCU-Wixel. The port is 50005 It should be a comma separated list. Example: 192.168.0.100:50005,192.168.0.101:50005
13. Select "Start Sensor" in xDrip and wait for two readings, then enter a dual calibration.

## Images

![Diagram](https://github.com/MrPsi/NodeMCU-Wixel/blob/master/img/box2.jpg?raw=true "Diagram")
Finished NodeMCU-Wixel in box.

![Diagram](https://github.com/MrPsi/NodeMCU-Wixel/blob/master/img/solder.jpg?raw=true "Diagram")
Back of stripboard.

![Diagram](https://github.com/MrPsi/NodeMCU-Wixel/blob/master/img/breadboard.jpg?raw=true "Diagram")
NodeMCU-Wixel connected on breadboard.
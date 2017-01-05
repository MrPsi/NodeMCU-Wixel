
-- Timers
-- Print - 0
-- Wifi connect = 1
-- Wifi monitor = 1
-- Led = 2
-- Delayed start = 4

-- Globals
--modPrintText = nil

require("modCompile").compileFiles()
require("modPins").init()
modPrintText = require("modPrintText")
modPrintText.init()

-- Start execution.
tmr.alarm(4, 10000, tmr.ALARM_SINGLE,
    function ()
        require("modInitialize").initialize()
    end)
print("Starting NodeMCU Wixel in 10 seconds...")

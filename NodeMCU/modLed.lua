local modLed, modName = {}, ...

-- Globals
--isLedOn = false

function modLed.blink(interval)
    package.loaded[modName] = nil
    
    isLedOn = false
    tmr.alarm(2, interval, tmr.ALARM_AUTO,
        function ()
            gpio.write(1, isOn and 0 or 1)
            isOn = not isOn
        end)
end

function modLed.stop()
    package.loaded[modName] = nil
    
    tmr.unregister(2)
    gpio.write(1, 0)
end

return modLed

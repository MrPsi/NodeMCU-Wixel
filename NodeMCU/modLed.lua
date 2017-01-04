local modLed, modName = {}, ...

-- Globals
--isBlinking = false
--lastInterval = 0
--isPinHigh = false

function modLed.blink(interval)
    package.loaded[modName] = nil
    
    if isBlinking ~= nil then
        if lastInterval == interval then
            return
        end
        
        tmr.unregister(2)
    end
    
    isBlinking = true
    lastInterval = interval
    
    gpio.write(1, 1)
    isPinHigh = true
    
    tmr.alarm(2, interval, tmr.ALARM_AUTO,
        function ()
            gpio.write(1, isPinHigh and 0 or 1)
            isPinHigh = not isPinHigh
        end)
end

function modLed.stop()
    package.loaded[modName] = nil
    
    isBlinking = nil
    lastInterval = nil
    isPinHigh = nil
    tmr.unregister(2)
    gpio.write(1, 0)
end

return modLed

local modWifiMon, modName = {}, ...

-- Globals
--isPendingRestart = false

function modWifiMon.monitor()
    package.loaded[modName] = nil
    
    isPendingRestart = false
    wifi.sta.eventMonReg(wifi.STA_CONNECTING, 
        function()
            if isPendingRestart == false then
                isPendingRestart = true
                require("modLed").blink(1000)
                
                tmr.alarm(1, 60000, tmr.ALARM_SINGLE,
                    function()
                        -- If timer fires, connection has not been restored
                        node.restart()
                    end)
            end
        end)
    
    wifi.sta.eventMonReg(wifi.STA_GOTIP,
        function()
            -- Connected to AP
            isPendingRestart = false
            tmr.unregister(1)
            require("modLed").stop()
        end)
    
    wifi.sta.eventMonStart(1000)
end

return modWifiMon

local modWifiMon, modName = {}, ...

-- Globals
--gOnWifiConnected = nil
--hasBeenConnected = false

function modWifiMon.monitor(onConnected)
    package.loaded[modName] = nil
    
    require("modLed").blink(1000)
    hasBeenConnected = false
    gOnWifiConnected = onConnected
    
    --wifi.sta.eventMonReg(wifi.STA_IDLE, 
    --    function(previousState)
    --        modPrintText.print("Idle")
    --    end)
    
    wifi.sta.eventMonReg(wifi.STA_CONNECTING, 
        function(previousState)
            require("modLed").blink(1000)
            modPrintText.print("Connecting...")
        end)
    
    wifi.sta.eventMonReg(wifi.STA_WRONGPWD, 
        function(previousState)
            require("modLed").blink(125)
            modPrintText.print("Wrong password")
        end)
    
    wifi.sta.eventMonReg(wifi.STA_APNOTFOUND, 
        function(previousState)
            require("modLed").blink(500)
            modPrintText.print("Access point not found")
        end)
    
    wifi.sta.eventMonReg(wifi.STA_FAIL, 
        function(previousState)
            require("modLed").blink(125)
            modPrintText.print("Connection failure")
        end)
    
    wifi.sta.eventMonReg(wifi.STA_GOTIP, 
        function(previousState)
            modPrintText.print("Connected")
            
            require("modLed").stop()
            
            if hasBeenConnected == false then
                hasBeenConnected = true
                gOnWifiConnected()
                gOnWifiConnected = nil
            end
        end)
    
    wifi.sta.eventMonStart(1000)
end

return modWifiMon

local modInitialize, modName = {}, ...

function modInitialize.initialize()
    package.loaded[modName] = nil
    
    require("modWifi").connectToAp(
        function()
            -- Connected to wifi
            require("modWifiMon").monitor()
            require("modStart").start()
        end,
        function()
            -- Connection failed
            require("modLed").blink(125)
        end)
end

return modInitialize

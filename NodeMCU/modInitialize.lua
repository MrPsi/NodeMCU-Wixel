local modInitialize, modName = {}, ...

function modInitialize.initialize()
    package.loaded[modName] = nil
    
    require("modWifi").connectToAp(
        function()
            -- Connected to wifi
            require("modStart").start()
        end)
end

return modInitialize

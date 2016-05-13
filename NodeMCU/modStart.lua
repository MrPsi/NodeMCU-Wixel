local modStart, modName = {}, ...

-- Globals
--list = nil

function modStart.start()
    package.loaded[modName] = nil
    
    -- Disable debug prints
    allowPrint = false
    
    list = require("modList")
    -- Max 5 in list if it should be sent in one package to the client.
    list.init(5)
    require("modUart").start()
    require("modServer").listen()
end

return modStart

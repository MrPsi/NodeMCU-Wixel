local modClient, modName = {}, ...

function modClient.dataReceived(socket, json)
    package.loaded[modName] = nil
    
    local isDecodeSuccess, data = pcall(cjson.decode, json)
    if isDecodeSuccess == false then
        modPrintText.print("Error decoding received data")
        socket:close()
        return
    end
    
    local jsonList = {}
    if data["includeInfo"] == true then
        local json = require("modInfo").getInfo()
        if json == nil then
            socket:close()
            return
        end
        jsonList[#jsonList + 1] = json
        jsonList[#jsonList + 1] = "\n"
    end
    
    if data["version"] ~= 1 then
        socket:close()
        return
    end

    local numberOfRecords = (data["numberOfRecords"])
    if type(numberOfRecords) == "number" and numberOfRecords > 200 or numberOfRecords < 1 then
        socket:close()
        return
    end

    local numberToSend = math.min(numberOfRecords, list.size())
    if numberToSend == 0 and #jsonList == 0 then
        socket:close()
        return
    end
    
    local converter = require("modConverter")
    converter.init()
    for i = 1, numberToSend do
        local json = converter.convert(list.getFromEnd(i))
        if json == nil then
            socket:close()
            return
        end
        jsonList[#jsonList + 1] = json
        jsonList[#jsonList + 1] = "\n"
    end
    converter = nil
    local toSend = table.concat(jsonList)
    socket:send(toSend)
end

return modClient

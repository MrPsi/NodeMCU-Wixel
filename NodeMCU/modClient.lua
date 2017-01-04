local modClient, modName = {}, ...

function modClient.dataReceived(socket, receivedData)
    package.loaded[modName] = nil
    
    local isDecodeSuccess, data = pcall(cjson.decode, receivedData)
    if isDecodeSuccess == false then
        modPrintText.print("Error decoding received data")
        socket:close()
        return
    end
    
    local jsonList = {}
    if data["includeInfo"] == true then
        local infoJson = require("modInfo").getInfo()
        if infoJson == nil then
            socket:close()
            return
        end
        jsonList[#jsonList + 1] = infoJson
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
        local listItemJson = converter.convert(list.getFromEnd(i))
        if listItemJson == nil then
            socket:close()
            return
        end
        jsonList[#jsonList + 1] = listItemJson
        jsonList[#jsonList + 1] = "\n"
    end
    converter = nil
    local toSend = table.concat(jsonList)
    socket:send(toSend)
end

return modClient

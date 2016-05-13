local modConverter, modName = {}, ...

function modConverter.init()
    package.loaded[modName] = nil
end

function modConverter.convert(item)
    local i = 0
    local values = {}
    for value in string.gmatch(item.data, "%S+") do
        values[i] = value
        i = i + 1
    end
    
    local serverFormat = {
        TransmissionId = tonumber(values[5]),
        TransmitterId = values[0],
        RawValue = tonumber(values[1]),
        FilteredValue = tonumber(values[2]),
        BatteryLife = tonumber(values[3]),
        ReceivedSignalStrength = tonumber(values[4]),
        CaptureDateTime = 0,
        Uploaded = 0,
        UploadAttempts = 0,
        UploaderBatteryLife = 0,
        RelativeTime = "r" }
    
    local isEncodeSuccess, json = pcall(cjson.encode, serverFormat)
    if isEncodeSuccess == false then
        -- Unable to encode to json.
        return nil
    end
    
    -- Can't multiply to get milliseconds, will overflow. Use string manipulation.
    local sec = tmr.time()
    local relativeSec = sec - item.sec
    local relativeZeros = "000"
    if relativeSec == 0 then
        relativeZeros = ""
    end
    json = string.gsub(json, "\"r\"", relativeSec .. relativeZeros)
    
    return json
end

return modConverter

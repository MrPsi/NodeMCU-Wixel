local modInfo, modName = {}, ...

function modInfo.getInfo()
    package.loaded[modName] = nil
    
    local info = {
        uptime = tmr.time()
    }
    
    local isEncodeSuccess, json = pcall(cjson.encode, info)
    if isEncodeSuccess == false then
        -- Unable to encode to json.
        return nil
    end
    
    return json
end

return modInfo

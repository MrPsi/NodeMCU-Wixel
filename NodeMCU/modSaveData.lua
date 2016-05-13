local modSaveData, modName = {}, ...

function modSaveData.save(data)
    package.loaded[modName] = nil
    
    local sec = tmr.time()
    local listItem = {
        sec = sec,
        data = data }
    list.push(listItem)
end

return modSaveData

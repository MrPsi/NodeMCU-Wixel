local modCompile, modName = {}, ...

function modCompile.compileFiles()
    package.loaded[modName] = nil
    
    local hasCompiled = false
    local files = file.list()
    for name, size in pairs(files) do
        if name ~= "init.lua" and name ~= "modCompile.lua" and string.sub(name, -3) == "lua" then
            hasCompiled = true
            node.compile(name)
            file.remove(name)
        end
    end
    
    if hasCompiled then
        -- Compiled file, restart.
        node.restart()
    end
end

return modCompile

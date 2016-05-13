local modUart, modName = {}, ...

function modUart.start()
    package.loaded[modName] = nil
    
    uart.setup(0, 9600, 8, uart.PARITY_NONE, uart.STOPBITS_1, 0)
    uart.alt(1)
    uart.on("data", "\n",
        function(line)
            local trimmedLine = string.gsub(line, " \r\n", "")
            if #trimmedLine < 13 then
                -- Should never be shorter than 13 characters
                return
            end
            
            modPrintText.print("Uart received '" .. trimmedLine .. "'")
            require("modSaveData").save(trimmedLine)
        end, 0)
end

return modUart

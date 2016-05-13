local modPrintText, modName = {}, ...

-- Globals
isUsb = true
allowPrint = true

function modPrintText.init()
    package.loaded[modName] = nil
end

function modPrintText.print(text)
    if allowPrint == false then
        return
    end
    
    if isUsb == false then
        isUsb = true
        uart.alt(0)
        uart.setup(0, 115200, 8, uart.PARITY_NONE, uart.STOPBITS_1, 0)
    end
    tmr.alarm(0, 1000, tmr.ALARM_SINGLE,
        function ()
            isUsb = false
            uart.setup(0, 9600, 8, uart.PARITY_NONE, uart.STOPBITS_1, 0)
            uart.alt(1)
        end)
    print(text)
end

return modPrintText

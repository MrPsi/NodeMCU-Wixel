local modPins, modName = {}, ...

function modPins.init()
    package.loaded[modName] = nil
    
	-- Led pin.
	gpio.mode(1, gpio.OUTPUT)
	gpio.write(1, gpio.LOW)
	
    -- Start Wixel.
	gpio.mode(2, gpio.OUTPUT)
	gpio.write(2, gpio.HIGH)
end

return modPins

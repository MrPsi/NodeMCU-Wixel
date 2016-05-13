local modWifi, modName = {}, ...
local cfg = {
    ip = "192.168.0.2",
    netmask = "255.255.255.0",
    gateway = "192.168.0.1"
}
local wifiSsid = "SSID"
local wifiPassword = "PASSWORD"

-- Globals
--gOnWifiConnected = nil
--gOnWifiFail = nil

function modWifi.connectToAp(onConnected, onFail)
    package.loaded[modName] = nil
    
    gOnWifiConnected = onConnected
    gOnWifiFail = onFail
    wifi.setmode(wifi.STATION)
    -- Use 802.11b for maximum range
    wifi.setphymode(wifi.PHYMODE_B)
    wifi.sleeptype(wifi.NONE_SLEEP)
    wifi.sta.setip(cfg)
    wifi.sta.config(wifiSsid, wifiPassword)
    wifi.sta.connect()
    
    tmr.alarm(1, 1000, tmr.ALARM_AUTO, 
        function()
            local status = wifi.sta.status()
            if status == wifi.STA_CONNECTING then
                -- Connecting
                modPrintText.print("Connecting...")
                return
            end
            
            tmr.unregister(1)
            
            if status == wifi.STA_GOTIP then
                -- Connected
                modPrintText.print("Wifi connected")
                gOnWifiConnected()
            else
                -- Connection failed
                modPrintText.print("Wifi connection failed " .. status)
                gOnWifiFail()
            end
            
            gOnWifiConnected = nil
            gOnWifiFail = nil
        end)
end

return modWifi

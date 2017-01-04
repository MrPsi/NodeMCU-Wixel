local modWifi, modName = {}, ...
local cfg = {
    ip = "192.168.0.2",
    netmask = "255.255.255.0",
    gateway = "192.168.0.1"
}
local wifiSsid = "SSID"
local wifiPassword = "PASSWORD"

function modWifi.connectToAp(onConnected)
    package.loaded[modName] = nil
    
    wifi.setmode(wifi.STATION)
    -- Use 802.11n (Can also be set to PHYMODE_B or PHYMODE_G)
    wifi.setphymode(wifi.PHYMODE_N)
    wifi.sleeptype(wifi.NONE_SLEEP)
    wifi.sta.setip(cfg)
    wifi.sta.config(wifiSsid, wifiPassword)
    wifi.sta.connect()
    
    require("modWifiMon").monitor(onConnected)
end

return modWifi

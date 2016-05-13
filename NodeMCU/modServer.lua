local modServer, modName = {}, ...

function modServer.listen()
    package.loaded[modName] = nil
    
    local server = net.createServer(net.TCP, 10) 
    server:listen(50005,
        function(conn)
            --conn:on("connection",
            --    function(socket)
            --    end)
            
            conn:on("receive",
                function(socket, data)
                    modPrintText.print("Server received '" .. data .. "'")
                    local dataLength = #data
                    -- TODO? Keep global buffer of received data and concatinate?
                    if dataLength < 33 then
                        socket:close()
                        return
                    end
                    
                    if dataLength > 56 then
                        socket:close()
                        return
                    end
                    
                    require("modClient").dataReceived(socket, data)
                end)
            
            --conn:on("sent",
            --    function(socket)
            --    end)
            
            --conn:on("reconnection",
            --    function(socket)
            --    end)
            
            --conn:on("disconnection",
            --    function(socket)
            --    end)
        end)
end

return modServer

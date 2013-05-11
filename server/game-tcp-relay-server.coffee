net = require 'net'

PORT = 5566

sockets = [undefined, undefined]
    
net.createServer (socket) ->

    if sockets[0]?

        if sockets[1]?
    
            socket.destroy()
            console.log "No more clients"
            return
    
        else
            sockets[1] = socket
            console.log "Client2 Connected"
    
    else
    
        sockets[0] = socket
        console.log "Client1 Connected"
    
    console.log socket.address()['address']
        
    socket.on 'data', (data) ->

        if sockets[0] == socket

            if sockets[1]?
                sockets[1].write data
    
        else

            if sockets[0]?
                sockets[0].write data
    
        console.log "Sending Data : " + data
 
    socket.on 'end',  () ->

        if sockets[0] == socket
            sockets[0] = undefined
        else
            sockets[1] = undefined
    
        console.log 'server disconnected'
    
.listen PORT, () ->
    console.log 'server listening on ' + PORT

net = require 'net'

socks = []
PORT = 5566
    
net.createServer (socket) ->

    console.log('server connected');    
        
    socket.on 'data', (data) ->
        socket.write data
        console.log socket.address()['address']
 
    socket.on 'end',  () ->
        console.log 'server disconnected'
    

.listen PORT, () ->
    console.log 'server listening on ' + PORT

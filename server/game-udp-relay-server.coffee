PORT = 5567

dgram = require 'dgram'

server = dgram.createSocket 'udp4'

server.bind PORT, ->
    console.log "SOCKET BINDED"

server.on "message", (msg, rinfo) ->
    console.log "server got: " + msg + " from " + rinfo.address + ":" + rinfo.port
    server.send msg, 0, msg.length, rinfo.port, rinfo.address, (err, bytes) ->
        console.log "sent #{bytes} bytes of data"

server.on "listening", ->
    address = server.address()
    console.log "server listening "+ address.address + ":" + address.port

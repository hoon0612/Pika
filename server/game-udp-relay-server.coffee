ProtoBuf = require "protobufjs"
builder  = ProtoBuf.protoFromFile "../protocol/Control.proto"
Pika     = builder.build "Pika"
Control  = Pika.Game.Control
ByteBuffer = require "bytebuffer"
        
PORT = 5567

dgram = require 'dgram'

server = dgram.createSocket 'udp4'

clients = new Array()

server.bind PORT, ->
    console.log "SOCKET BINDED"

lookup_client = (rinfo) ->

    result = [client for client in clients when (client.address == rinfo.address and client.port == rinfo.port)]
    
    if result.length != 1
        return undefined
    
    return result[0]

add_client = (rinfo) ->
    clients.push
        "port": rinfo.port
        "address": rinfo.address

server.on "message", (msg, rinfo) ->

    console.log "server got: #{ msg }(#{ msg.length } bytes) from #{ rinfo.address }:#{ rinfo.port }"

    buf = ByteBuffer.wrap(msg)

    try
        
        myMessage = Control.decode(buf)
        
    catch e

        if e.msg
            console.log "decoded message with missing required fields" + e.msg
        else
            console.log "something is weird"

    client = lookup_client rinfo

    console.log myMessage        
    
    if  client == undefined
        client = add_client rinfo
            
#    server.send msg, 0, msg.length, rinfo.port, rinfo.address, (err, bytes) ->
#        console.log "sent #{bytes} bytes of data"

server.on "listening", ->
    address = server.address()
    console.log "server listening "+ address.address + ":" + address.port


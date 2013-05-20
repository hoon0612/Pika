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

    result = undefined
    result = client for client in clients when (client.address == rinfo.address and client.port == rinfo.port)

    if result == undefined
        return undefined

    return result

enemy_client = (rinfo) ->

    result = undefined
    result = client for client in clients when (client.address != rinfo.address or client.port != rinfo.port)
    
    if result == undefined
        return undefined
    
    return result

add_client = (rinfo) ->

    if clients.length < 2
        clients.push
            "port": rinfo.port
            "address": rinfo.address

        console.log "[+] Num of client(s) : #{ clients.length }  "
    
        return clients[ clients.length - 1 ]
    
    else

        console.log "[+] Too much clients"
    
        return undefined

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

    if  client == undefined
        client = add_client rinfo

    enemy = enemy_client client

    if enemy != undefined
    
        server.send msg, 0, msg.length, enemy.port, enemy.address, (err, bytes) ->
            console.log "sent #{bytes} bytes of data"

server.on "listening", ->
    address = server.address()
    console.log "server listening "+ address.address + ":" + address.port
    

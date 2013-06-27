ProtoBuf = require "protobufjs"
builder  = ProtoBuf.protoFromFile "../protocol/Control.proto"
Pika     = builder.build "Pika"
Control  = Pika.Game.Control
ByteBuffer = require "bytebuffer"
Config   = require "./config"    

net   = require 'net'
dgram = require 'dgram'

server = dgram.createSocket 'udp4'

clients = new Array()

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

    if client == undefined
        client = add_client rinfo

    # client == undefined if add_client failed
        
    if client == undefined
        enemy = undefined
    else
        enemy = enemy_client client

    if enemy != undefined
    
        server.send msg, 0, msg.length, enemy.port, enemy.address, (err, bytes) ->
            console.log "sent #{bytes} bytes of data"

connected = true

connect_to_match_server = () ->
    match_server = net.connect
        port: Config.MATCH_PORT
        address: '127.0.0.1'
        , ->
            connected = true
            console.log '[+] Match server connected'
            match_server.write 'ping'

            match_server.on 'data', (data) ->
    
                dataStr = data.toString()
    
                if dataStr == 'pong'
                    console.log '[+] Match server ready\n'

            match_server.on 'end', ->
    
                connected = false
                reconnect_to_match_server()
    
    match_server.on 'error', (err) ->

        if connected == true
            connected = false
            reconnect_to_match_server()

reconnect_to_match_server = () ->
    console.log "[-] Trying to reconnect to match server... "

    setTimeout ->
        if connected == false
            connect_to_match_server()
            reconnect_to_match_server()
    , Config.RETRY_INTERVAL
    
server.on "listening", ->
    address = server.address()
    console.log "server listening "+ address.address + ":" + address.port

    connect_to_match_server()

server.bind Config.GAME_PORT, ->
    console.log "SOCKET BINDED"

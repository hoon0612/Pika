ProtoBuf = require "protobufjs"
builder  = ProtoBuf.protoFromFile "../protocol/Game.proto"
ProtoBuf.protoFromFile "../protocol/Match.proto", builder    
Pika       = builder.build "Pika"
GameProtocol  = Pika.Game
MatchProtocol = Pika.Match
gp = GameProtocol
mp = MatchProtocol    
    
inet       = require "inet"    
        
ByteBuffer = require "bytebuffer"
Config     = require "./config"

net    = require 'net'
dgram  = require 'dgram'

libClients  = require './game.clients'
Clients = new libClients()

server = dgram.createSocket 'udp4'
        
server.on "message", (msg, rinfo) ->

    console.log "server got: #{ msg }(#{ msg.length } bytes) from #{ rinfo.address }:#{ rinfo.port }"

    buf = ByteBuffer.wrap(msg)

    try
        
        myMessage = gp.GameProtocol.decode(buf)

    catch e

        if e.msg
            console.log "decoded message with missing required fields" + e.msg
        else
            console.log "something is weird"


    if myMessage.type == gp.ProtocolType.GAME_REGISTER_REQUEST

        key    = myMessage.registerRequest.key

        client = Clients.lookup_client rinfo, key

        console.log "[*] client : " + client

        if client == undefined
            num_clients = Clients.add_client rinfo, key

            console.log "[+] Client added : " + num_clients

            if num_clients == 2
                client_list = Clients.get_clients key

                for client in client_list

                    request = new gp.GameProtocol
                        type : gp.ProtocolType.GAME_START
                        startProtocol :
                            delay : 5000
    
                    buf = request.encode().toBuffer()

                    server.send buf, 0, buf.length, client.port, client.address, (err, bytes) ->
                        console.log "Start game!\n"

    else if myMessage.type == gp.ProtocolType.GAME_CONTROL

        key    = myMessage.controlProtocol.key

        client = Clients.lookup_client rinfo, key

        if client == undefined
            return
        else
            enemy = Clients.enemy_client client, key

            if enemy != undefined
    
                server.send msg, 0, msg.length, enemy.port, enemy.address, (err, bytes) ->
                    console.log "sent #{bytes} bytes of data"

        console.log "Control"
    
    else
        console.log "Error"
        

connected = true

connect_to_match_server = () ->
    match_server = net.connect
        port: Config.MATCH_PORT
        address: '127.0.0.1'
        , ->
            connected = true
            console.log '[+] Match server connected'

            #ip = get_ipaddress() 

            request = new mp.MatchProtocol
                type : mp.ProtocolType.MATCH_SERVERREGISTER
                registerProtocol :
                    ip   : 0 #inet.aton(server.address().address)
                    port : server.address().port

            buf = request.encode().toBuffer().toString('binary')
            match_server.write buf, 'binary'
    
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

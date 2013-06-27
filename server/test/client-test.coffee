net   = require 'net'
Config = require '../config'        


ProtoBuf    = require "protobufjs"
builder     = ProtoBuf.protoFromFile "../../protocol/Lobby.proto"
    
lobby_proto = builder.build "Pika.Lobby"
lp          = lobby_proto

                
client = net.connect
    port: Config.MAIN_PORT
    address: '127.0.0.1',
    ->
        console.log '[+] match server connected'
        
        request = new lp.LobbyRequest
            type : lp.ProtocolType.JOIN_MATCH
            joinRequest :
                joinType : lp.Join.JoinRequest.JoinType.RAND_MATCH
            sessionID : "Test Session"

        buf = request.encode().toBuffer().toString('binary')
        client.write buf

client.on 'data', (data) ->
    dataStr = data.toString()
    console.log lp.LobbyResponse.decode(new Buffer(dataStr, 'binary'))
    client.end()

client.on "listening", ->
    address = server.address()
    console.log "server listening "+ address.address + ":" + address.port

ProtoBuf    = require "protobufjs"
builder     = ProtoBuf.protoFromFile "../protocol/Lobby.proto"
    
lobby_proto = builder.build "Pika.Lobby"
lp          = lobby_proto

console.log new lp.LobbyResponse
    type       : lp.ProtocolType.JOIN_MATCH
    error_code : 0
    joinResponse : 
        ip   : 44
        port : 55
        room : "random"
    

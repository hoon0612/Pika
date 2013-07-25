ProtoBuf = require "protobufjs"
builder  = ProtoBuf.protoFromFile "../../protocol/Game.proto"
Pika     = builder.build("Pika");
GameProtocol  = Pika.Game
gp = GameProtocol

PORT = 5569

dgram = require 'dgram'

client = dgram.createSocket("udp4")

stdin = process.stdin

stdin.setRawMode true
stdin.resume()
stdin.setEncoding 'utf8'

room_key = "test"        

console.log "[+] udp-server test\n"

stdin.on 'data', (key) ->

    if key == '\u0003'
    	process.exit();
    
    register = new gp.GameProtocol
        "type" : gp.ProtocolType.GAME_REGISTER_REQUEST
        "registerRequest" :
            "key": room_key
            "id" : "test1"

    buf = register.encode().toBuffer()

    client.send buf, 0, buf.length, PORT, "localhost", (err, bytes) ->
        console.log "Register Data!\n"

    control = new gp.GameProtocol
        "type": gp.ProtocolType.GAME_CONTROL
        "controlProtocol" :
            "id": "Hello"
            "time": 56
            "Character" :
                "loc_x": 4.1
                "loc_y": 4.3
                "vel_x": 4.5
                "vel_y": 4.7
            "Ball" :
                "loc_x": 4.1
                "loc_y": 4.3
                "vel_x": 4.5
                "vel_y": 4.7
            "key" : room_key
    
    buf = control.encode().toBuffer()
    client.send buf, 0, buf.length, PORT, "localhost", (err, bytes) ->
        console.log "Sent Data!\n"

    process.stdout.write key

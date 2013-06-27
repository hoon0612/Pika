ProtoBuf = require "protobufjs"
builder  = ProtoBuf.protoFromFile "../../protocol/Control.proto"
Pika     = builder.build("Pika");
Control  = Pika.Game.Control

PORT = 5567

dgram = require 'dgram'

client = dgram.createSocket("udp4")

stdin = process.stdin

stdin.setRawMode true
stdin.resume()

console.log "[+] udp-server test\n"

stdin.on 'data', (key) ->

    if key == 'q'
        client.close()
    
    myMessage = new Control
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
    


    buf = myMessage.encode().toBuffer()

    client.send buf, 0, buf.length, PORT, "localhost", (err, bytes) ->
        console.log "Sent Data!\n"

    process.stdout.write key

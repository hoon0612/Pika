# TODO : Split lobby.coffee to lobby.coffee and match.coffee

ProtoBuf    = require "protobufjs"
builder     = ProtoBuf.protoFromFile "../protocol/Lobby.proto"
ProtoBuf.protoFromFile "../protocol/Match.proto", builder

Pika       = builder.build "Pika"
LobbyProtocol = Pika.Lobby
MatchProtocol = Pika.Match
lp = LobbyProtocol
mp = MatchProtocol    
        
ByteBuffer = require "bytebuffer"
inet       = require "inet"    
Config     = require "./config"

net = require 'net'

game_servers = new Array()

add_game_server = (ip, port) ->

    result = undefined
    result = server for server in game_servers when (server.address == ip and server.port == port)

    if result == undefined
        console.log ip
        console.log port
        game_servers.push
            "address": ip
            "port": port

get_game_server = ->

    num_game_servers = game_servers.length

    if num_game_servers == 0
        return undefined
    else
        selected_game_server = Math.floor((Math.random()*num_game_servers))
        return game_servers[selected_game_server]

game_server = net.createServer (socket) ->

    socket.setEncoding('binary')

    console.log socket.address()['address']

    socket.on 'data', (data) ->

        request = mp.MatchProtocol.decode(new Buffer(data.toString(), 'binary'))

        if request.type == mp.ProtocolType.MATCH_SERVERREGISTER
            add_game_server inet.aton(socket.address().address) , request.registerProtocol.port

            request = new mp.MatchProtocol
                type : mp.ProtocolType.MATCH_REGISTEROK

            buf = request.encode().toBuffer().toString('binary')
            socket.write buf, 'binary'
        
    socket.on 'end', () ->
        console.log 'server disconnected'


join_handler = (request) ->
    
    server_info = get_game_server()

    if server_info == undefined

        response =  new lp.LobbyResponse
            error_code : 1
            type : lp.ProtocolType.JOIN_MATCH
        
    else
        
        response = new lp.LobbyResponse
            error_code : 0
            type : lp.ProtocolType.JOIN_MATCH
            joinResponse : 
                ip   : server_info.address
                port : server_info.port
                room : "something random key"


str2hex = (s) ->
    [k.charCodeAt(0).toString(16) for k in s].join(" ")

match_server = net.createServer (socket) ->

    socket.setEncoding('binary')
    

    console.log socket.address()['address']

        
    socket.on 'data', (data) ->

        request = lp.LobbyRequest.decode(new Buffer(data.toString(), 'binary'))
        response = join_handler request
        
        buf = response.encode().toBuffer().toString('binary')
        socket.write buf, 'binary'
    
    socket.on 'end', () ->
        console.log 'client disconnected'

game_server.listen Config.MATCH_PORT, () ->
    console.log 'listening for game servers on ' + Config.MATCH_PORT

match_server.listen Config.MAIN_PORT, () ->
    console.log 'listening for clients on  ' + Config.MAIN_PORT

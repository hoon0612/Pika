# TODO LIST
# 1. Connection with gameserver
# 2. 
        
ProtoBuf    = require "protobufjs"
builder     = ProtoBuf.protoFromFile "../protocol/Lobby.proto"
lobby_proto = builder.build "Pika.Lobby"
lp          = lobby_proto    
        
ByteBuffer = require "bytebuffer"
inet       = require "inet"    
Config     = require "./config"

net = require 'net'

game_servers = new Array()

add_game_server = (rinfo) ->

    result = undefined
    result = server for server in game_servers when (server.address == rinfo.address and server.port == rinfo.port)

    if result == undefined
        game_servers.push
            "port": rinfo.port
            "address": rinfo.address

get_game_server = ->

    num_game_servers = game_servers.length

    if num_game_servers == 0
        return undefined
    else
        selected_game_server = Math.floor((Math.random()*num_game_servers))
        return game_servers[selected_game_server]

game_server = net.createServer (socket) ->

    socket.setEncoding('ascii')

    console.log socket.address()['address']

    socket.on 'data', (data) ->

        strData = data.toString()

        if strData == "ping" or strData == "ping\n"
            add_game_server socket.address()
            socket.write "pong"
        
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
                ip   : inet.aton(server_info.address)
                port : server_info.port
                room : "something random key"


str2hex = (s) ->
    [k.charCodeAt(0).toString(16) for k in s].join(" ")

match_server = net.createServer (socket) ->

    socket.setEncoding('ascii')

    console.log socket.address()['address']
        
    socket.on 'data', (data) ->

        request = lp.LobbyRequest.decode(new Buffer(data.toString(), 'binary'))

        response = join_handler request
    
        console.log response.encode().toBuffer().toString('binary').length

        buf = response.encode().toBuffer().toString('binary')
        socket.write buf
    
    socket.on 'end', () ->
        console.log 'client disconnected'

game_server.listen Config.MATCH_PORT, () ->
    console.log 'listening for game servers on ' + Config.MATCH_PORT

match_server.listen Config.MAIN_PORT, () ->
    console.log 'listening for clients on  ' + Config.MAIN_PORT

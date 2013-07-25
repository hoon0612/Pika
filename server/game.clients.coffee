class Clients        
    constructor: () ->
        @clients = new Array()

    add_room: (key) ->
        
        console.log "[+] added a room : "  + key
        
        @clients[key] =
            "clients"      : new Array()
            "created_time" : new Date().getTime()

    check_room: (key) ->
        
        if not (@clients.hasOwnProperty key)
            @add_room key

        return true

        #return (@clients.hasOwnProperty key)

    add_client: (rinfo, key) ->

        if not (@clients.hasOwnProperty key)
            return undefined

        console.log "[+] very good " + key + " " + @clients[key].clients.length

        if @clients[key].clients.length < 2
            @clients[key].clients.push
                "port": rinfo.port
                "address": rinfo.address

            console.log "[+] Num of client(s) : #{ @clients[key].clients.length }  "
        
        else
        
            console.log "[-] Too much clients"

        return @clients[key].clients.length

    lookup_client: (rinfo, key) ->

        if not (@check_room key)
            return undefined

        result = undefined
        result = client for client in @clients[key].clients when (client.address == rinfo.address and client.port == rinfo.port)

        return result

    enemy_client: (rinfo, key) ->

        if not (@check_room key)
            return undefined
        
        result = undefined
        result = client for client in @clients[key].clients when (client.address != rinfo.address or client.port != rinfo.port)
        
        return result

    get_clients: (key) ->

        if not (@check_room key)
            return undefined
        
        return @clients[key].clients

module.exports = Clients


class Rooms
    constructor: ->        
        @.rooms = new Array()

    lookup_room: (key) ->

        result = undefined
        result = room for room in this.rooms when (room.key == key)

        if result == undefined
            return undefined

        return result

    add_room: (key) ->

        if @lookup_room key == undefined

            @rooms.push
                "key"       : key
                "timestamp" : new Date().getTime()
                "cnt"       : 0
                
            console.log "[+] Added room : " + key
        
        else

            console.log "[-] Already existing room"
                
        return @rooms.length


module.exports = Rooms

        
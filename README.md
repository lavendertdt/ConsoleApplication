# ConsoleApplication
REST API(F#)

## Requirements

- FSharp.Core.4.5.2
- Newtonsoft.Json.12.0.2
- Suave.2.5.4
- System.ValueTuple.4.4.0

## Server

That will launch a server on *localhost*, with port *8080*. You can use command line option `--port=PORT` to specify a difference port.

## Some operations

1. Show all stored information
```Bash
GET localhost:8080/radios  
```
2. Show i's location
```Bash
GET localhost:8080/radios/i/location 
```
3. Set i's location to loc
```Bash
GET localhost:8080/radios/i/location/loc
```


## Initial stored data 
```Bash
let item = {
            device_id = 100
            alias = "Radio100"
            allowed_locations = ["CPH-1";"CPH-2";"CPH-3"]
            location = Some({location="CPH-1"})
        }
        let result = todoRepositoryDb.Add item

let item2 = {
            device_id = 101
            alias = "Radio101"
            allowed_locations =  ["CPH-1"; "CPH-3"]
            location = None
        }
```

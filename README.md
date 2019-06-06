# ConsoleApplication
REST API(F#)

Show all stored information
```Bash
GET localhost:8080/radios  
```
Show i's location
```Bash
GET localhost:8080/radios/i/location 
```
Set i's location to loc
```Bash
GET localhost:8080/radios/i/location/loc
```


Initial data is show below:
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

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

namespace SuaveRestApi
//open System
open SuaveRestApi.Controllers
open SuaveRestApi.Controllers.TodoController
open SuaveRestApi.Models
open SuaveRestApi.Models.TodoRepositoryDb
open Suave


module App =  

    [<EntryPoint>]
    let main argv = 
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
        let result2 = todoRepositoryDb.Add item2

        startWebServer defaultConfig (todoController todoRepositoryDb)
        0 // return an integer exit code





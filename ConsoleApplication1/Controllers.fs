namespace SuaveRestApi.Controllers

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Json
open System.Runtime.Serialization
open Suave.Filters
open Suave.Operators
open SuaveRestApi.Models



module Controller = 
    let fromJson<'a> json =
        let obj = JsonConvert.DeserializeObject(json, typeof<'a>) 
        if isNull obj then
            None
        else
            Some(obj :?> 'a)

    let getResourceFromReq<'a> (req : HttpRequest) =
        let getString rawForm =
            System.Text.Encoding.UTF8.GetString(rawForm)
        req.rawForm |> getString |> fromJson<'a>

    let JSON value =
        let settings = new JsonSerializerSettings()
        settings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(value, settings)
        |> Successful.OK
        >=> Writers.setMimeType "application/json; charset=utf-8"
    
    let handleResource f requestError = function
        | Some r -> r |> f
        | _ -> requestError

    let handleResourceBADREQUEST = 
        (fun f -> handleResource f (RequestErrors.BAD_REQUEST "No Resource from request"))

    let handleResourceNOTFOUND = 
        (fun f -> handleResource f (RequestErrors.NOT_FOUND "404 NOT FOUND"))

    let handleResourceFORBIDDEN = 
        (fun f -> handleResource f (RequestErrors.FORBIDDEN "403 FORBIDDEN"))

    let handleResourceCONFLICT = 
        (fun f -> handleResource f (RequestErrors.CONFLICT "Resource already exists"))
  
module TodoController = 
    let getAll db =
        warbler (fun _ -> db.GetAll() |> Controller.JSON)
    
    let find db =
        db.Find 
        >> (Controller.handleResourceNOTFOUND Controller.JSON)

    let add db =
        let addDb =
            db.Add 
            >> (Controller.handleResourceCONFLICT Controller.JSON)
        request (Controller.getResourceFromReq >> (Controller.handleResourceBADREQUEST addDb))

    let findloc db =
        db.Findloc 
        >> (Controller.handleResourceNOTFOUND Controller.JSON)   
        
    let setloc db =
        db.Setloc 
        >> (Controller.handleResourceFORBIDDEN Controller.JSON)  

    let todoController (db:TodoRepository) = 
        pathStarts "/radios" >=> choose [
            POST >=> path "/radios" >=> (add db)
            GET >=> pathScan "/radios/%i/location/%s" (setloc db)
            GET >=> path "/radios" >=> (getAll db)
            GET >=> pathScan "/radios/%i" (find db) 
            GET >=> pathScan "/radios/%i/location" (findloc db)
        ]
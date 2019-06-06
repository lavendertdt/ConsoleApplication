namespace SuaveRestApi.Models

open System
open System.Collections.Generic
open System.Collections.Concurrent

type Loc = {location: string}

type TodoItem = {
    device_id : int
    alias : string
    allowed_locations : string list
    location: Loc option
}

type TodoRepository = {
    Add : TodoItem -> Option<TodoItem>
    Addde : int -> TodoItem -> Option<TodoItem>
    GetAll : unit -> seq<TodoItem>
    Find : int -> Option<TodoItem>
    Findloc : int -> Option<Loc>
    Setloc : (int* string) -> Option<Loc>
}

module TodoRepositoryDb =
    let private todos = new ConcurrentDictionary<int, TodoItem>()

    let add todo =
        let copy = todo//{ todo with Key = Guid.NewGuid().ToString() }
        if todos.TryAdd(copy.device_id, copy) then
            Some(copy)
        else 
            None

    

    let getAll (key:unit) = todos.Values |> Seq.cast

    let find key =
        let (success, value) = todos.TryGetValue(key)
        if success then
            Some(value)
        else 
            None

    let addde key todo =
        match find key with
        |None   -> None
        |_     ->add todo
        

    let findloc key =
        let (success, value) = todos.TryGetValue(key)
        if success then
            value.location
        else 
            None   

    let setloc (key, loc)=
        let (success, value) = todos.TryGetValue(key)
        if success then
            match (List.tryFind (fun i -> i = loc) value.allowed_locations) with
            |None -> None//failwith "not in list"
            |Some(r) -> let loc1 = {location = loc}
                        (value.location = Some({location = loc}))|>ignore
                        let copy = {device_id = value.device_id; alias = value.alias; allowed_locations = value.allowed_locations;location = Some({location = loc})}
                        todos.TryUpdate(copy.device_id, copy, value)|>ignore
                        //Some({location = loc})
                        copy.location
        else 
            None 

    (*let remove key =
        let (success, value) = todos.TryRemove(key)
        if success then
            Some(value)
        else 
            None
    
    let update todo =
        let tryupdate current =
            if todos.TryUpdate(todo.id, todo, current) then
                Some(todo)
            else 
                None
        find todo.id |> Option.bind tryupdate*)

    let todoRepositoryDb = {
        Add = add
        Addde = addde
        GetAll = getAll
        Find = find
        Findloc = findloc
        Setloc = setloc
    }
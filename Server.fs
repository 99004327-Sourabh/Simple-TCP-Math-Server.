open System
open System.Net
open System.Net.Sockets
open System.Text

let clientCount = ref 1
let clientCountLock = new obj()
let clientSockets = ref []    // Mutable reference list for client sockets
let terminate = ref false     // Mutable boolean flag for termination

let calculateOperation operation (args: string[]) =
    try
        match operation with
        | "add" when args.Length >= 2 && args.Length <= 4 -> 
            args |> Array.map Int32.Parse |> Array.sum |> Some
        | "subtract" when args.Length = 2 -> 
            Some ((Int32.Parse args.[0]) - (Int32.Parse args.[1]))
        | "multiply" when args.Length >= 2 && args.Length <= 4 ->
            args |> Array.map Int32.Parse |> Array.reduce (*) |> Some
        | _ -> Some (-1)
    with
    | :? FormatException -> Some (-4)
    | _ -> Some (-1)

let processClient (clientSocket: Socket) =
    async {
        // Add clientSocket to the list
        clientSockets := clientSocket :: !clientSockets
        let clientId =
            lock clientCountLock (fun () ->
                let id = !clientCount
                clientCount := id + 1
                id)
        let greeting = sprintf "Hello client %d!" clientId
        clientSocket.Send(Encoding.ASCII.GetBytes(greeting)) |> ignore
        let buffer = Array.zeroCreate<byte> 1024
        while true do
            Array.Clear(buffer, 0, buffer.Length)
            let bytesRead = clientSocket.Receive(buffer)
            if bytesRead > 0 then
                let message = Encoding.ASCII.GetString(buffer, 0, bytesRead)
                printfn "Received message from client %d: %s" clientId message
                let parts = message.Split(' ')
                let command = parts.[0]
                let rest = parts.[1..] |> Array.ofSeq
                let response = 
                    match command with
                    | "terminate" | "bye" -> "-5"
                    | _ -> match calculateOperation command rest with
                           | Some result -> string result
                           | None -> "-1"
                clientSocket.Send(Encoding.ASCII.GetBytes(response)) |> ignore
                if command = "terminate" then
                    if command = "terminate" then
                        terminate := true
                        for socket in !clientSockets do
                            try
                                socket.Send(Encoding.ASCII.GetBytes("-5")) |> ignore
                                socket.Close()
                            with
                            | ex -> printfn "Error broadcasting termination signal: %s" ex.Message
                        Environment.Exit(0)
    }

[<EntryPoint>]
let main _ =
    let runServer port =
        let listener = new TcpListener(IPAddress.Any, port)
        listener.Start()
        printfn "Server is running and listening on port %d." port
        while true do
            let clientSocket = listener.AcceptSocket()
            // For every client connection, start an independent async computation
            Async.Start (processClient clientSocket)

    runServer 12345
    0 // Return an integer exit code
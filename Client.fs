open System
open System.Net
open System.Net.Sockets
open System.Text

let clientFunction (serverIPAddress: string) (serverPort: int) =
    use client = new TcpClient()
    client.Connect(IPAddress.Parse(serverIPAddress), serverPort)

    let stream = client.GetStream()

    let rec loop () =
        
        let bytesReceived = Array.zeroCreate<byte> 256
        let bytesRead = stream.Read(bytesReceived, 0, bytesReceived.Length)
        let response = Encoding.ASCII.GetString(bytesReceived, 0, bytesRead)
        if response = "-5" then  // Removed extra space before '-5'
            Console.WriteLine("Exit")
            client.Close()
        else
            Console.WriteLine("Received from server: " + response)
            
            Console.Write("Enter a message to send (or 'bye' to quit): ")
            let message = Console.ReadLine()
            let bytesToSend = Encoding.ASCII.GetBytes(message)  // Assuming message is of type string
            stream.Write(bytesToSend, 0, bytesToSend.Length)
            loop ()
    loop ()

[<EntryPoint>]
let main argv =
    clientFunction "127.0.0.1" 12345
    0  // Return an integer exit code
# Simple-TCP-Math-Server.# Simple TCP Math Server

## Overview

Welcome to the `SimpleTCPCalculator` project! This project provides a basic example of a TCP server and a client in F#. The server can perform simple arithmetic operations, such as addition, subtraction, and multiplication. The client connects to the server, sends math-related commands, and gets back results.

## Features

- **Server Capabilities**:
  - Listens on a specified port for incoming client connections.
  - Processes multiple clients asynchronously.
  - Performs basic arithmetic operations (addition, subtraction, multiplication).
  - Can broadcast a termination signal to all connected clients and shut itself down.

- **Client Capabilities**:
  - Connects to a given server IP and port.
  - Sends commands to the server and displays the results.
  - Disconnects upon receiving a termination signal from the server.


### Requirements

- .NET Core SDK (Version 5.0 or later)


### Usage

Once both the server and client are running:

1. The server will display a message indicating it's listening for connections.
2. The client will connect and get a greeting from the server.
3. The client can send commands to the server in the following format:
- `add <number1> <number2> [..]`: Adds the provided numbers. Supports 2 to 4 numbers.
- `subtract <number1> <number2>`: Subtracts the second number from the first.
- `multiply <number1> <number2> [..]`: Multiplies the provided numbers. Supports 2 to 4 numbers.
- `bye`: Disconnects the client from the server.
- `terminate`: Commands the server to disconnect all clients and shut down.


## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

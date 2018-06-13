# Wpf_Tic-Tac Toe
### Client-server application (using TcpClient) written in C # using WPF technology.

Technology TcpClient is similar to Sokets, but a bit on the higher level. This desktop application for Windows running on the network.
It implements the Tic-tac-toe game for a large number pairs of players.

## How it works

I.e. a server is created on one computer, with the specifying IP address and port. After this, any (even) number of clients can connect to the server (specifying IP and port on which the server was started) and start the game with each other.
You can play this game locally and via the internet.

## Get Started (To start playing this game (on Windows))

### NOTE: To launch this game you have to have open port on your Windows.
1.  Download Wpf_Tic_Tac_Toe
2. Open Wpf_Tic_Tac_Toe.sln in Visual Studio and Build->Rebuild Solution. Shut down Visual Studio.
#### Let's start server:
3. Launch Wpf_Tic_Tac_Toe.exe (in Wpf_Tic_Tac_Toe-master->Wpf_Tic_Tac_Toe->bin->Debug directory)
4. In cmd type ipconfig. The Configuring IP for Windows must appears.
5. Copy IPv4-address value and paste it to "IP" input in  Wpf_Tic-Tac Toe  app.
6. Close cmd.
7. Type your port number (which you should open before this) into "Port" input in Wpf_Tic-Tac Toe  app.
8. Click "Server" button (to start as Server)
#### So server is started, now you can join to it as client:
9. Repeat actions from 3, 5, 7
10. Start as Client  
Now only one client have joined to game. To start game should be at least 2 clients (players). So join at least one more client.So the game started :)  
You can join to game any even number of clients (players), to do it repeat actions from 3,5,7,10.

Thanks!

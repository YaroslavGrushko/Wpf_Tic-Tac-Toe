# Wpf_Tic-Tac Toe
### Client-server application (using TcpClient) written in C # using WPF technology.

Technology TcpClient is similar to Sokets, but a bit on the higher level. This desktop application for Windows running on the network.
It implements the Tic-tac-toe game for a large number pairs of players.

## How it works

I.e. a server is created on one computer, with the specifying IP address and port. After this, any (even) number of clients can connect to the server (specifying IP and port on which the server was started) and start the game with each other.
You can play this game locally and in the internet.

## Get Started

### To start playing this game (on Windows):
### NOTE To launch this game you have to have open port on your Operation System.
1.  Download Wpf_Tic_Tac_Toe
2. Open Wpf_Tic_Tac_Toe.sln in Visual Studio and rebuild project. Shut down visual Studio.
Lets start server:
3. Launch Wpf_Tic_Tac_Toe.exe (in Wpf_Tic_Tac_Toe-master->Wpf_Tic_Tac_Toe->bin->Debug directory)
4. in cmd type ipconfig. The Configuring IP for Windows must appears.
5. Copy IPv4-address value and past it to IP input in  Wpf_Tic-Tac Toe  app.
6. Close cmd.
7. Type your port number (which you should open before this) into Port input in Wpf_Tic-Tac Toe  app.
8. Start Server
So Server is started, now you can join to it as client:
9. Repeate actions from 3, 5, 7
10. Start as Client
Now only one cliennt joined to game. To start play should be at least 2 clients. So join one more client. So game is on.
You can join to game any even number of clients (players), to do it repeate actions from 3,5,7,10.

Thanks!

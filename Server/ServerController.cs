using Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ServerController
    {
        public List<Player> players;
        public ServerController()
        {
            players = new List<Player>();
        }

        public void StartServer()
        {
            TcpListener tcpListener = new TcpListener(
                IPAddress.Parse("127.0.01"), 7777);
            tcpListener.Start();
            Console.WriteLine("Started");

            while(true)
            {
                if (tcpListener.Pending())
                {
                    Console.WriteLine("Pending connection");
                    tcpListener.BeginAcceptTcpClient(AcceptPlayer, tcpListener);
                }

                foreach (var player in players)
                {
                    switch (player.GameState)
                    {
                        case GameState.Connecting:
                            Connecting(player);
                            break;
                        case GameState.Sync:
                            Sync(player);
                            break;
                        case GameState.GameStarted:
                            GameStarted(player);
                            break;
                    }
                }
            }
        }

        private void GameStarted(Player player)
        {
            if (player.DataAvailable())
            {
                Console.WriteLine("New player position ");
                Message message = player.ReadMessage();
                if (message.MessageType == MessageType.PlayerMovement)
                {
                    //players.Where(s => s.GameState == GameState.GameStarted).
                    //    ToList().ForEach(p => p.SendMessage(message));
                    foreach (var p in players)
                    {
                        if (p.GameState == GameState.GameStarted)
                        {
                            p.SendMessage(message);
                        }
                    }
                }
            }
        }

        private void Sync(Player player)
        {
            // 1. processar todos os NeWPlayer
            SyncNewPlayers(player);
            // 2. processar todos os PlayerMovement
            SyncPlayerMovement(player);

            // 3. enviar mensagem ao servidor com FinishedSync
            Message msg = new Message();
            msg.MessageType = MessageType.FinishedSync;
            player.SendMessage(msg);
            player.GameState = GameState.GameStarted;
        }

        private void SyncPlayerMovement(Player player)
        {
            foreach (Player p in players)
            {
                if (p.GameState == GameState.GameStarted)
                {
                    var last = p.Messages.LastOrDefault(mt => mt.MessageType == MessageType.PlayerMovement);
                    if (last != null)
                    {
                        Message m = new Message();
                        m.PlayerInfo = last.PlayerInfo;
                        player.SendMessage(m);
                    }
                }
            }
        }

        private void SyncNewPlayers(Player player)
        {
            foreach (var p in players)
            {
                if (p.GameState == GameState.GameStarted)
                {
                    Message m = new Message();
                    m.MessageType = MessageType.NewPlayer;
                    PlayerInfo info = p.Messages.FirstOrDefault(mt => mt.MessageType == MessageType.NewPlayer).PlayerInfo;
                    m.PlayerInfo = info;

                    player.SendMessage(m);
                }
            }
        }

        // envia msg ao cliente (unity) a informar da ligação ao servidor e posicao inicial
        private void Connecting(Player player)
        {
            if (player.DataAvailable())
            {
                Console.WriteLine("New player registering");
                Player msg = player.ReadPlayer();
                player.Name = msg.Name;

                // comunica aos outros players novo jogador
                foreach (var p in players)
                {
                    Message message = new Message();
                    message.MessageType = MessageType.NewPlayer;
                    message.Description = (p == player) ?
                        "Successfully joined" :
                        $"Player {player.Name} has joined";
                    PlayerInfo info = new PlayerInfo();
                    info.Id = player.Id;
                    info.Name = player.Name;
                    info.X = 0;
                    info.Y = 0;
                    info.Z = 0;
                    info.lookX = 0;
                    info.lookY = 0;
                    info.lookZ = 0;

                    info.C1x = 0;
                    info.C1y = 0;
                    info.C1z = 0;

                    info.C2x = 0;
                    info.C2y = 0;
                    info.C2z = 0;

                    info.C3x = 0;
                    info.C3y = 0;
                    info.C3z = 0;

                    info.P1x = 0;
                    info.P1y = 0;
                    info.P1z = 0;

                    info.P2x = 0;
                    info.P2y = 0;
                    info.P2z = 0;
                   
                    message.PlayerInfo = info;

                    p.SendMessage(message);
                    p.Messages.Add(message);
                }

                player.GameState = GameState.Sync;
            }
        }

        private void AcceptPlayer(IAsyncResult result)
        {
            TcpListener tcpListener = (TcpListener)result.AsyncState;
            TcpClient client = tcpListener.EndAcceptTcpClient(result);

            if(client.Connected)
            {
                Console.WriteLine("Accepted new player");
                Player p = new Player();
                p.Messages = new List<Message>();
                p.Id = Guid.NewGuid();
                p.GameState = GameState.Connecting;
                p.TcpClient = client;
                players.Add(p);

                p.SendPlayer(p);
            }
            else
            {
                Console.WriteLine("Connection refused");
            }
        }
    }
}

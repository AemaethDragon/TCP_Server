using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public TcpClient TcpClient { get; set; }
        public GameState GameState { get; set; }
        public List<Message> Messages { get; set; }

        private BinaryWriter _writer;
        private BinaryReader _reader;

        public void SendMessage(Message msg)
        {
            if (_writer == null)
            {
                _writer = new BinaryWriter(TcpClient.GetStream());
            }
            _writer.Write(JsonConvert.SerializeObject(msg));
        }

        public void SendPlayer(Player p)
        {
            if (_writer == null)
            {
                _writer = new BinaryWriter(TcpClient.GetStream());
            }
            _writer.Write(JsonConvert.SerializeObject(p));
        }

        public bool DataAvailable()
        {
            return TcpClient.GetStream().DataAvailable;
        }

        public Player ReadPlayer()
        {
            if (_reader == null)
                _reader = new BinaryReader(TcpClient.GetStream());

            string msg = _reader.ReadString();
            return JsonConvert.DeserializeObject<Player>(msg);
        }

        public Message ReadMessage()
        {
            string msg = _reader.ReadString();
            return JsonConvert.DeserializeObject<Message>(msg);
        }

    }
}

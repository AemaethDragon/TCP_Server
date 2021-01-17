using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{

    public class Message
    {
        public string Description { get; set; }
        public PlayerInfo PlayerInfo { get; set; }
        public MessageType MessageType { get; set; }
    }

    public class PlayerInfo
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public enum MessageType
    {
        PlayerName,
        NewPlayer,
        PlayerMovement,
        FinishedSync
    }
}

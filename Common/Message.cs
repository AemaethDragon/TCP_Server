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
        public Cube cube1 { get; set; }
        public MessageType MessageType { get; set; }
    }

    public class PlayerInfo
    {
        public Cube cube1 { get; set; }
        //public Cube cube2 { get; set; }
        //public Cube cube3 { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float lookX { get; set; }
        public float lookY { get; set; }
        public float lookZ { get; set; }
       

        public Guid Id { get; set; }
        public string Name { get; set; }

    }

    public class Cube 
    {
        public float Cx;
        public float Cy;
        public float Cz;
    }


    public enum MessageType
    {
        PlayerName,
        NewPlayer,
        PlayerMovement,
        FinishedSync
    }
}

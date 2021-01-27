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
        #region CubePositions
        public float C1x { get; set; }
        public float C1y { get; set; }
        public float C1z { get; set; }
        #endregion
        #region portalPositions
        public float P1x { get; set; }
        public float P2x { get; set; }
        public float P3x { get; set; }
        public float P4x { get; set; }
        public float P1y { get; set; }
        public float P2y { get; set; }
        public float P3y { get; set; }
        public float P4y { get; set; }
        public float P1z { get; set; }
        public float P2z { get; set; } 
        public float P3z { get; set; }  
        public float P4z { get; set; }
        #endregion
        public float lookX { get; set; }
        public float lookY { get; set; }
        public float lookZ { get; set; }
       

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

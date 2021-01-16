using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public bool Playable;
    private Vector3 _oldPosition;
    private float _horizontal;
    private float _vertical;
    public TcpClientController TcpClient;
    void Update () {
        if (!Playable) return;

        _horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        _vertical = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
	}

    void FixedUpdate()
    {
        if (!Playable) return;

        transform.Translate(_horizontal, 0, _vertical);
        if (transform.position != _oldPosition)
        {
            Message m = new Message();
            m.MessageType = MessageType.PlayerMovement;
            PlayerInfo info = new PlayerInfo();
            info.Id = TcpClient.Player.Id;
            info.Name = TcpClient.Player.Name;
            info.X = transform.position.x;
            info.Y = transform.position.y;
            info.Z = transform.position.z;
            m.PlayerInfo = info;
            TcpClient.Player.SendMessage(m);
        }
        _oldPosition = transform.position;
    }
}

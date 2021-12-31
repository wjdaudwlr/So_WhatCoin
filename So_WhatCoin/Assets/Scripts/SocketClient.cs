using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Socket.Quobject.SocketIoClientDotNet.Client;

public class SocketClient : MonoBehaviour
{
    private QSocket socket;

    private void Start()
    {
        socket = IO.Socket("http://10.120.74.70:3001/");

        socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log("���� �����.");
            socket.Emit("msgToServer", "�Ǥ�");
        });
        socket.On(QSocket.EVENT_DISCONNECT, () =>
        {
            Debug.Log("���� ����.");
        });
        socket.On(QSocket.EVENT_RECONNECT_ATTEMPT, () =>
        {
            Debug.Log("�翬�� �õ�.");
        });

        socket.On("msgToClient", data =>
        {
            Debug.Log("data : " + data);
        });
    }

    private void OnDestroy()
    {
        socket.Disconnect();
    }
}

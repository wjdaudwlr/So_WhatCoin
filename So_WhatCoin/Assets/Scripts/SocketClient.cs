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
            Debug.Log("¼­¹ö ¿¬°áµÊ.");
            socket.Emit("msgToServer", "¤Ç¤§");
        });
        socket.On(QSocket.EVENT_DISCONNECT, () =>
        {
            Debug.Log("¿¬°á ²÷±è.");
        });
        socket.On(QSocket.EVENT_RECONNECT_ATTEMPT, () =>
        {
            Debug.Log("Àç¿¬°á ½Ãµµ.");
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

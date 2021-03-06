using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Socket.Quobject.SocketIoClientDotNet.Client;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SocketClient : MonoBehaviour
{
    private QSocket socket;
    public List<CoinData> coinDatas = new List<CoinData>();
    public CoinManager coinManager;

    private void Start()
    {
        socket = IO.Socket("http://louis7308.iptime.org:3001/");

        socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log("서버 연결됨.");
            socket.Emit("msgToServer", "ㅗㄷ");
        });
        socket.On(QSocket.EVENT_DISCONNECT, () =>
        {
            Debug.Log("연결 끊김.");
        });
        socket.On(QSocket.EVENT_RECONNECT_ATTEMPT, () =>
        {
            Debug.Log("재연결 시도.");
        });

        socket.On("msgToClient", data =>
        {
            coinDatas.Clear();
            Debug.Log("데이터 전송 완료");


            JObject parseObj = new JObject();

            parseObj = JObject.Parse(data.ToString());

            JArray jArray = new JArray();
            jArray = JArray.Parse(parseObj["data"].ToString());

            Debug.Log(jArray);
            foreach (JObject jo in jArray)
            {
                CoinData obj = new CoinData();

                obj.id = System.Int32.Parse(jo["id"].ToString());
                obj.name = jo["name"].Value<string>();
                obj.price = int.Parse(jo["price"].ToString());
                coinDatas.Add(obj);
            }

            foreach (var item in coinDatas)
            {
                Debug.Log("id : " + item.id + "      name : " + item.name + "      price : " + item.price);
            }

        });
    }

    private void OnDestroy()
    {
        socket.Disconnect();
    }


    [System.Serializable]
    public class CoinData
    {
        public int id;
        public string name;
        public int price;
    }

}

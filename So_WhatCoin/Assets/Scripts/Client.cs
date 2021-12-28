
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Net.Sockets;
//using System.IO;
//using System;

//public class Client : MonoBehaviour
//{
//	public InputField IPInput, PortInput, NickInput;
//	string clientName;

//	bool socketReady;
//	TcpClient socket;
//	NetworkStream stream;
//	StreamWriter writer;
//	StreamReader reader;


//	public void ConnectToServer()
//	{
//		// �̹� ����Ǿ��ٸ� �Լ� ����
//		if (socketReady) return;

//		// �⺻ ȣ��Ʈ/ ��Ʈ��ȣ
//		string ip = IPInput.text == "" ? "127.0.0.1" : IPInput.text;
//		int port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);

//		// ���� ����
//		try
//		{
//			socket = new TcpClient(ip, port);
//			stream = socket.GetStream();
//			writer = new StreamWriter(stream);
//			reader = new StreamReader(stream);
//			socketReady = true;
//		}
//		catch (Exception e)
//		{
//			Chat.instance.ShowMessage($"���Ͽ��� : {e.Message}");
//		}
//	}

//	void Update()
//	{
//		if (socketReady && stream.DataAvailable)
//		{
//			string data = reader.ReadLine();
//			if (data != null)
//				OnIncomingData(data);
//		}
//	}

//	void OnIncomingData(string data)
//	{
//		if (data == "%NAME")
//		{
//			clientName = NickInput.text == "" ? "Guest" + UnityEngine.Random.Range(1000, 10000) : NickInput.text;
//			Send($"&NAME|{clientName}");
//			return;
//		}

//		Chat.instance.ShowMessage(data);
//	}

//	void Send(string data)
//	{
//		if (!socketReady) return;

//		writer.WriteLine(data);
//		writer.Flush();
//	}

//	public void OnSendButton(InputField SendInput)
//	{
//#if (UNITY_EDITOR || UNITY_STANDALONE)
//		if (!Input.GetButtonDown("Submit")) return;
//		SendInput.ActivateInputField();
//#endif
//		if (SendInput.text.Trim() == "") return;

//		string message = SendInput.text;
//		SendInput.text = "";
//		Send(message);
//	}


//	void OnApplicationQuit()
//	{
//		CloseSocket();
//	}

//	void CloseSocket()
//	{
//		if (!socketReady) return;

//		writer.Close();
//		reader.Close();
//		socket.Close();
//		socketReady = false;
//	}
//}
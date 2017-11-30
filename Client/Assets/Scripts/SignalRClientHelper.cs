using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SignalR.Client._20.Hubs;
using SignalR.Client._20.Transports;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.UI;

public class SignalRClientHelper : MonoBehaviour {
    public string SignalRUrl = "http://localhost:52527";

    public Text ChatMessages;
    public InputField MessageForChat;
    private HubConnection _hubConnection = null;
    private IHubProxy _hubProxy;
    private string ComingMessage;

    IEnumerator Start () {
        Debug.Log("Start()");
        yield return new  WaitForSeconds(1);
        Debug.Log("Strart() wait 1 second");

        StratSignalR();
	}

    private void StratSignalR()
    {
        Debug.Log("StartSignalR()");
        if (_hubConnection == null)
        {
            _hubConnection = new HubConnection(SignalRUrl);
            Debug.Log(SignalRUrl);
            _hubConnection.Error += HubConnection_Error;

            _hubProxy = _hubConnection.CreateProxy("ChatHub");
            Subscription sudscription = _hubProxy.Subscribe("broadcastMessage");
            sudscription.Data += Subscription_data;

            Debug.Log("_hubConnection.Start();");
            _hubConnection.Start();
        }
        else
        {
            Debug.Log("SignalR already connected...");
        }
    }

    /// Пришел ответ сервера

    private void Update()
    {
        ChatMessages.text = ComingMessage;
    }

    private void Subscription_data(object[] obj)
    {
        Debug.Log(obj[0].ToString() + "-" + obj[1].ToString());
        ComingMessage += obj[0].ToString() + ":" + obj[1].ToString() + Environment.NewLine;
    }

    private void HubConnection_Error(System.Exception obj)
    {
        Debug.Log("Hub Error - " + obj.Message + 
            Environment.NewLine + obj.InnerException+
            Environment.NewLine + obj.Data +
            Environment.NewLine + obj.StackTrace+
            Environment.NewLine +obj.TargetSite
            );
    }

    private void HubConnectionClosed()
    {
        Debug.Log("HubConnectionClosed()");
    }

    public void SendMessage()
    {
        _hubProxy.Invoke("Send", "Unity", MessageForChat.text);
    }
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit()" + Time.time +" seconds");
        _hubConnection.Error -= HubConnection_Error;
        _hubConnection.Stop();
    }

}

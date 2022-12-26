using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    #region Kinda Singleton implementation
    public static Client Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public NetworkDriver driver;
    private NetworkConnection connection;

    private bool isActive = false;

    public Action connectionDropped;

    #region Methods
    public void Init(string ip,ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endPoint = NetworkEndPoint.Parse(ip, port);

        Debug.Log("Attempting to connect to server on " + endPoint.Address);

        connection = driver.Connect(endPoint);

        isActive = true;

        RegisterToEvent();
    }
    public void Shutdown()
    {
        if (isActive)
        {
            UnregisterToEvent();
            driver.Dispose();
            isActive = false;
            connection = default(NetworkConnection);
        }
    }
    public void OnDestroy()
    {
        Shutdown();
    }
    #endregion

    #region Client Logic
    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        driver.ScheduleUpdate().Complete();
        CheckAlive();

        UpdateMessagePump();
    }

    private void CheckAlive() 
    {
        if(!connection.IsCreated && isActive)
        {
            Debug.Log("Something went wrong, lost connection to server");
            connectionDropped?.Invoke();
            Shutdown();
        }
    }

    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                //SendToServer(new NetWelcome());
                Debug.Log("We're connected");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NetUtility.OnData(stream, default(NetworkConnection));
            }
            else if(cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
                Shutdown();
            }
        }

    }
    
    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    #endregion

    #region Event Parsing
    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }
    private void UnregisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }
    private void OnKeepAlive(NetMessage nm)
    {
        SendToServer(nm);
    }
    #endregion
}

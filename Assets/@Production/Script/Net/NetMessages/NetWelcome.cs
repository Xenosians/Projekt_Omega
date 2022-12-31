﻿using Unity.Networking.Transport;
using UnityEngine;

public class NetWelcome : NetMessage
{
    public int AssignedTeam { get; set; }
    public NetWelcome()
    {
        Code = OpCode.WELCOME;
    }
    public NetWelcome(DataStreamReader reader)
    {
        Code= OpCode.WELCOME;
        Deserialize(reader);
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(AssignedTeam);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        AssignedTeam = reader.ReadInt();
    }
    public override void RecivedOnClient()
    {
        NetUtility.C_WELCOME?.Invoke(this);
    }
    public override void RecivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_WELCOME?.Invoke(this, cnn);
    }

}

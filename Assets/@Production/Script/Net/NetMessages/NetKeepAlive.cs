using Unity.Networking.Transport;

public class NetKeepAlive : NetMessage
{
    public NetKeepAlive() 
    {
        code = OpCode.KEEP_ALIVE;
    }

    public NetKeepAlive(DataStreamReader reader)
    {
        code = OpCode.KEEP_ALIVE;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        
    }

    public override void RecivedOnClient()
    {
        NetUtility.C_KEEP_ALIVE?.Invoke(this);
    }

    public override void RecivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_KEEP_ALIVE?.Invoke(this, cnn);
    }
}

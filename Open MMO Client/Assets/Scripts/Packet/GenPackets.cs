using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
    C_Chat = 1,
	S_Chat = 2,
	
}

interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}


class C_Chat : IPacket
{
    public string chat;

    public ushort Protocol { get { return (ushort)PacketID.C_Chat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort chatLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.chat = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, chatLen);
		count += chatLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> sendBufferSegment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Chat), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        
        ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, chat.Length,
		sendBufferSegment.Array, sendBufferSegment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(chatLen), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += chatLen;

        Array.Copy(BitConverter.GetBytes(count), 0, sendBufferSegment.Array, sendBufferSegment.Offset, sizeof(ushort));    // Add Packet Size

        return SendBufferHelper.Close(count);
    }
}

class S_Chat : IPacket
{
    public int playerId;
	public string chat;

    public ushort Protocol { get { return (ushort)PacketID.S_Chat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		
		ushort chatLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.chat = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, chatLen);
		count += chatLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> sendBufferSegment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_Chat), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, chat.Length,
		sendBufferSegment.Array, sendBufferSegment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(chatLen), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += chatLen;

        Array.Copy(BitConverter.GetBytes(count), 0, sendBufferSegment.Array, sendBufferSegment.Offset, sizeof(ushort));    // Add Packet Size

        return SendBufferHelper.Close(count);
    }
}


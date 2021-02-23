using System;
using System.Collections.Generic;
using System.Text;

namespace PacketGenerator
{
    class PacketFormat
    {
        // {0} resister packet
        public static string managerFormat =
@"using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance {{ get {{ return _instance; }} }}
    #endregion

    PacketManager()
    {{
        Register();
    }}

    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {{
        {0}
    }}

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>> action = null;
        if (_onRecv.TryGetValue(id, out action))
            action.Invoke(session, buffer);
    }}

    void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {{
        T pkt = new T();
        pkt.Read(buffer);   // Deserialize

        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(pkt.Protocol, out action))
        {{
            action.Invoke(session, pkt);
        }}
    }}
}}";

        // {0} packet name
        public static string managerResisterFormat =
@"_onRecv.Add((ushort)PacketID.{0}, MakePacket<{0}>);
        _handler.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);";

        // {0} Packet name / number list
        // {1} Packet Classes list
        public static string fileFormat =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{{
    {0}
}}

interface IPacket
{{
	ushort Protocol {{ get; }}
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}}

{1}
";
        // {0} : Packet name
        // {1} : Packet number
        public static string packetEnumFormat =
@"{0} = {1},";

        
        // {0} : packet name
        // {1} : member
        // {2} : read member
        // {3} : write member

        public static string packetFormat =
@"
class {0} : IPacket
{{
    {1}

    public ushort Protocol {{ get {{ return (ushort)PacketID.{0}; }} }}

    public void Read(ArraySegment<byte> segment)
    {{
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> sendBufferSegment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.{0}), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        
        {3}

        Array.Copy(BitConverter.GetBytes(count), 0, sendBufferSegment.Array, sendBufferSegment.Offset, sizeof(ushort));    // Add Packet Size

        return SendBufferHelper.Close(count);
    }}
}}
";

        // SkillInfo
        // {0} : member type
        // {1} : member name
        public static string memberFormat =
@"public {0} {1};";

        // {0} : list name [Upper case]
        // {1} : list name [Loser case]
        // {2} : member
        // {2} : read member
        // {3} : write member
        public static string memberListFormat =
@"
public List<{0}> {1}s = new List<{0}>();
public struct {0}
{{
    {2}

    public void Read(ArraySegment<byte> segment, ref ushort count)
    {{
        {3}
    }}

    public bool Write(ArraySegment<byte> segment, ref ushort count)
    {{
        bool success = true;
        {4}
        return success;
    }}
}}";

        // {0} : member name
        // {1} : To~data type
        // {2} : member type
        public static string readFormat =
@"this.{0} = BitConverter.{1}(segment.Array, segment.Offset + count);
count += sizeof({2});
";

        // {0} : member name
        // {1} : member type
        public static string readByteFormat =
@"this.{0} = ({1})segment.Array[segment.Offset + count];
count += sizeof({1});";

        // {0} : member name
        public static string readStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, {0}Len);
count += {0}Len;";

        // {0} : list name [Upper case]
        // {1} : list name [lower case]
        public static string readListFormat =
@"this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
count += sizeof(ushort);
for (int i = 0; i < {1}Len; ++i)
{{
    {0} {1} = new {0}();
    {1}.Read(s, ref count);
    {1}s.Add({1});
}}";

        // {0} : member name
        // {1} : member type
        public static string writeFormat =
@"Array.Copy(BitConverter.GetBytes(this.{0}), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof({1}));
count += sizeof({1});";

        // {0} : member name
        // {1} : member type
        public static string writeByteFormat =
@"sendBufferSegment.Array[sendBufferSegment.Offset + count] = (byte)this.{0};
count += sizeof({1});";

        // {0} : member name
        public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, {0}.Length,
sendBufferSegment.Array, sendBufferSegment.Offset + count + sizeof(ushort));
Array.Copy(BitConverter.GetBytes({0}Len), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
count += sizeof(ushort);
count += {0}Len;";

        // {0} : list name [Upper case]
        // {1} : list name [lower case]
        public static string writeListFormat =
@"Array.Copy(BitConverter.GetBytes((ushort)this.{1}s.Count), 0, sendBufferSegment.Array, sendBufferSegment.Offset + count, sizeof(ushort));
count += sizeof(ushort);
foreach({0} {1} in this.{1}s)
{{
    success &= {1}.Write(sendBufferSegment, ref count);
}}";
    }
}

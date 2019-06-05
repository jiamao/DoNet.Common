using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DoNet.Common.Net
{
    /// <summary>
    /// socket通过协议包
    /// </summary>
    public class SocketPacket
    {
        /// <summary>
        /// 包字节数据
        /// </summary>
        public byte[] PacketData
        {
            get;set;
        }

        /// <summary>
        /// 包的协义类型
        /// </summary>
        public enum PacketProtocol
        {
            ICMP = 1,
            IGMP = 2,
            TCP=6,
            UDP=17
        }

        /// <summary>
        /// 当前协义类型
        /// </summary>
        public PacketProtocol Protocol
        {
            get;
            set;
        }

        public ushort GetChecksum(ref byte[] Packet, int start, int end)
        {
            uint CheckSum = 0;
            int i;
            for (i = start; i < end; i += 2) CheckSum += (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, i));
            if (i == end) CheckSum += (ushort)System.Net.IPAddress.NetworkToHostOrder((ushort)Packet[end]);
            while (CheckSum >> 16 != 0) CheckSum = (CheckSum & 0xFFFF) + (CheckSum >> 16);
            return (ushort)~CheckSum;
        }

        /// <summary>
        /// 解析包数据
        /// </summary>
        /// <param name="SourceAddress"></param>
        /// <param name="DestinationAddress"></param>
        /// <returns></returns>
        public virtual byte[] GetBytes(ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
        {
            return PacketData;
        }
    }

    public class IPPacket: SocketPacket // RFC791
    {
        public byte Version;
        public byte HeaderLength;
        public byte TypeOfService;
        public ushort TotalLength;
        public ushort Identification;
        public byte Flags;
        public ushort FragmentOffset;
        public byte TimeToLive;
        //public byte Protocol;
        public ushort HeaderChecksum;
        public System.Net.IPAddress SourceAddress;
        public System.Net.IPAddress DestinationAddress;

        /// <summary>
        /// 包，tcp/udp/icmp
        /// </summary>
        public SocketPacket Packet;

        public IPPacket() : base() { }

        public IPPacket(byte[] data)
            : base()
        {
            try
            {
                Version = (byte)(data[0] >> 4);
                HeaderLength = (byte)((data[0] & 0x0F) * 4);
                TypeOfService = data[1];
                TotalLength = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 2));
                Identification = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 4));
                Flags = (byte)((data[6] & 0xE0) >> 5);
                FragmentOffset = (ushort)(System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 6)) & 0x1FFF);
                TimeToLive = data[8];
                Protocol = (SocketPacket.PacketProtocol)data[9];
                HeaderChecksum = (ushort)(System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 10)));
                SourceAddress = new System.Net.IPAddress(System.BitConverter.ToInt32(data, 12) & 0x00000000FFFFFFFF);
                DestinationAddress = new System.Net.IPAddress(System.BitConverter.ToInt32(data, 16) & 0x00000000FFFFFFFF);
                PacketData = new byte[TotalLength - HeaderLength];
                System.Buffer.BlockCopy(data, HeaderLength, PacketData, 0, PacketData.Length);
            }
            catch { }

            switch (Protocol)
            {
                case PacketProtocol.ICMP: Packet = new ICMPPacket(PacketData); break;
                case PacketProtocol.TCP: Packet = new TCPPacket(PacketData); break;
                case PacketProtocol.UDP: Packet = new UDPPacket(PacketData); break;
            }
        }

        public byte[] GetBytes()
        {
            if (Packet != null) { Protocol = Packet.Protocol; PacketData = Packet.GetBytes(ref SourceAddress, ref DestinationAddress); }

            if (PacketData == null) PacketData = new byte[0];
            if (Version == 0) Version = 4;
            if (HeaderLength == 0) HeaderLength = 20;
            TotalLength = (ushort)(HeaderLength + PacketData.Length);
            byte[] data = new byte[TotalLength];
            if (TimeToLive == 0) TimeToLive = 128;
            data[0] = (byte)(((Version & 0x0F) << 4) | ((HeaderLength / 4) & 0x0F));
            data[1] = TypeOfService;
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)TotalLength)), 0, data, 2, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Identification)), 0, data, 4, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)((FragmentOffset & 0x1F) | ((Flags & 0x03) << 13)))), 0, data, 6, 2);
            data[8] = TimeToLive;
            data[9] = (byte)Protocol.GetHashCode();
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, data, 10, 2);
            System.Buffer.BlockCopy(SourceAddress.GetAddressBytes(), 0, data, 12, 4);
            System.Buffer.BlockCopy(DestinationAddress.GetAddressBytes(), 0, data, 16, 4);
            System.Buffer.BlockCopy(PacketData, 0, data, HeaderLength, PacketData.Length);
            HeaderChecksum = GetChecksum(ref data, 0, HeaderLength - 1);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)HeaderChecksum)), 0, data, 10, 2);
            return data;
        }
    }

    /// <summary>
    /// tcp数据包
    /// </summary>
    public class TCPPacket : SocketPacket //rfc793
    {
        public ushort SourcePort;
        public ushort DestinationPort;
        public uint SequenceNumber;
        public uint AcknowledgmentNumber;
        public byte DataOffset;
        public byte ControlBits;
        public ushort Window;
        public ushort Checksum;
        public ushort UrgentPointer;
        public byte[] Options;

        public TCPPacket() : base() {
            this.Protocol = PacketProtocol.TCP;
        }

        public TCPPacket(byte[] data)
            : base()
        {
            try
            {
                this.Protocol = PacketProtocol.TCP;
                SourcePort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 0));
                DestinationPort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 2));
                SequenceNumber = (uint)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 4));
                AcknowledgmentNumber = (uint)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 8));
                DataOffset = (byte)((data[12] >> 4) * 4);
                ControlBits = (byte)((data[13] & 0x3F));
                Window = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 14));
                Checksum = (ushort)(System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 16)));
                UrgentPointer = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(data, 18));
                if (DataOffset >= 20)
                {
                    Options = new byte[DataOffset - 20];
                    System.Buffer.BlockCopy(data, 20, Options, 0, Options.Length);
                    PacketData = new byte[data.Length - DataOffset];
                    System.Buffer.BlockCopy(data, DataOffset, PacketData, 0, data.Length - DataOffset);
                }
                
            }
            catch {
            }
        }

        public override byte[] GetBytes(ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
        {
            if (PacketData == null) PacketData = new byte[0];
            if (Options == null) Options = new byte[0];
            int OptionsLength = ((int)((Options.Length + 3) / 4)) * 4;
            DataOffset = (byte)(20 + OptionsLength);
            byte[] Packet = new byte[20 + OptionsLength + PacketData.Length];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)SourcePort)), 0, Packet, 0, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)DestinationPort)), 0, Packet, 2, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((int)SequenceNumber)), 0, Packet, 4, 4);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((int)AcknowledgmentNumber)), 0, Packet, 8, 4);
            Packet[12] = (byte)(((Packet[12] & 0x0F) | ((DataOffset & 0x0F) << 4)) / 4);
            Packet[13] = (byte)(((Packet[13] & 0xC0) | (ControlBits & 0x3F)));
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Window)), 0, Packet, 14, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 16, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)UrgentPointer)), 0, Packet, 18, 2);
            System.Buffer.BlockCopy(Options, 0, Packet, 20, Options.Length);
            if (OptionsLength > Options.Length) System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)0), 0, Packet, 20 + Options.Length, OptionsLength - Options.Length);
            System.Buffer.BlockCopy(PacketData, 0, Packet, DataOffset, PacketData.Length);
            Checksum = GetChecksum(ref Packet, 0, DataOffset - 1, ref SourceAddress, ref DestinationAddress);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Checksum), 0, Packet, 16, 2);
            return PacketData;
        }

        public ushort GetChecksum(ref byte[] Packet, int start, int end, ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
        {
            byte[] PseudoPacket;
            PseudoPacket = new byte[12 + Packet.Length];
            System.Buffer.BlockCopy(SourceAddress.GetAddressBytes(), 0, PseudoPacket, 0, 4);
            System.Buffer.BlockCopy(DestinationAddress.GetAddressBytes(), 0, PseudoPacket, 4, 4);
            PseudoPacket[8] = 0;
            PseudoPacket[9] = 6;
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Packet.Length)), 0, PseudoPacket, 10, 2);
            System.Buffer.BlockCopy(Packet, 0, PseudoPacket, 12, Packet.Length);
            return GetChecksum(ref PseudoPacket, 0, PseudoPacket.Length - 1);
        }
    }

    /// <summary>
    /// udp协议包
    /// </summary>
    public class UDPPacket:SocketPacket // rfc768
    {
        public ushort SourcePort;
        public ushort DestinationPort;
        public ushort Length;
        public ushort Checksum;

        public UDPPacket() : base() { this.Protocol = PacketProtocol.UDP; }

        public UDPPacket(byte[] Packet)
            : base()
        {
            this.Protocol = PacketProtocol.UDP;
            try
            {
                SourcePort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 0));
                DestinationPort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 2));
                Length = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 4));
                Checksum = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 6));
                PacketData = new byte[Packet.Length - 8];
                System.Buffer.BlockCopy(Packet, 8, PacketData, 0, Packet.Length - 8);
            }
            catch { }
        }

        public override byte[] GetBytes(ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
        {
            if (PacketData == null) PacketData = new byte[0];
            byte[] Packet = new byte[8 + PacketData.Length];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)SourcePort)), 0, Packet, 0, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)DestinationPort)), 0, Packet, 2, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Length)), 0, Packet, 4, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 6, 2);
            System.Buffer.BlockCopy(PacketData, 0, Packet, 8, PacketData.Length);
            Checksum = GetChecksum(ref Packet, 0, 8 - 1, ref SourceAddress, ref DestinationAddress);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Checksum), 0, Packet, 6, 2);
            return PacketData;
        }

        public ushort GetChecksum(ref byte[] Packet, int start, int end, ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
        {
            byte[] PseudoPacket;
            PseudoPacket = new byte[12 + Packet.Length];
            System.Buffer.BlockCopy(SourceAddress.GetAddressBytes(), 0, PseudoPacket, 0, 4);
            System.Buffer.BlockCopy(DestinationAddress.GetAddressBytes(), 0, PseudoPacket, 4, 4);
            PseudoPacket[8] = 0;
            PseudoPacket[9] = 17;
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Packet.Length)), 0, PseudoPacket, 10, 2);
            System.Buffer.BlockCopy(Packet, 0, PseudoPacket, 12, Packet.Length);
            return GetChecksum(ref PseudoPacket, 0, PseudoPacket.Length - 1);
        }
    }

    public class ICMPPacket: SocketPacket // rfc792
    {
        public byte Type;
        public byte Code;
        public ushort Checksum;
        public ICMPMessage Message;

        public ICMPPacket() : base() { this.Protocol = PacketProtocol.ICMP; }

        public ICMPPacket(byte[] Packet)
            : base()
        {
            this.Protocol = PacketProtocol.ICMP;
            try
            {
                Type = (byte)Packet[0];
                Code = (byte)Packet[1];
                Checksum = (ushort)System.BitConverter.ToInt16(Packet, 2);
                PacketData = new byte[Packet.Length - 4];
                System.Buffer.BlockCopy(Packet, 4, PacketData, 0, Packet.Length - 4);
            }
            catch { }

            switch (Type)
            {
                case 0: Message = new ICMPEchoReply(PacketData); break;
                case 3: Message = new ICMPDestinationUnreachable(PacketData); break;
                case 4: Message = new ICMPSourceQuench(PacketData); break;
                case 5: Message = new ICMPRedirect(PacketData); break;
                case 8: Message = new ICMPEcho(PacketData); break;
                case 11: Message = new ICMPTimeExceeded(PacketData); break;
                case 12: Message = new ICMPParameterProblem(PacketData); break;
                case 13: Message = new ICMPTimestamp(PacketData); break;
                case 14: Message = new ICMPTimestampReply(PacketData); break;
                case 15: Message = new ICMPInformationRequest(PacketData); break;
                case 16: Message = new ICMPInformationReply(PacketData); break;
            }
        }

        public override byte[] GetBytes(ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
        {
            if (Message != null) PacketData = Message.GetBytes();
            if (Message is ICMPEchoReply) Type = 0;
            else
                if (Message is ICMPDestinationUnreachable) Type = 3;
                else
                    if (Message is ICMPSourceQuench) Type = 4;
                    else
                        if (Message is ICMPRedirect) Type = 5;
                        else
                            if (Message is ICMPEcho) Type = 8;
                            else
                                if (Message is ICMPTimeExceeded) Type = 11;
                                else
                                    if (Message is ICMPParameterProblem) Type = 12;
                                    else
                                        if (Message is ICMPTimestamp) Type = 13;
                                        else
                                            if (Message is ICMPTimestampReply) Type = 14;
                                            else
                                                if (Message is ICMPInformationRequest) Type = 15;
                                                else
                                                    if (Message is ICMPInformationReply) Type = 16;

            if (PacketData == null) PacketData = new byte[0];
            byte[] Packet = new byte[4 + PacketData.Length];
            Packet[0] = Type;
            Packet[1] = Code;
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 2, 2);
            System.Buffer.BlockCopy(PacketData, 0, Packet, 4, PacketData.Length);
            Checksum = GetChecksum(ref Packet, 0, Packet.Length - 1);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Checksum), 0, Packet, 2, 2);
            return Packet;
        }

        //public ushort GetChecksum(ref byte[] Packet, int start, int end)
        //{
        //    uint CheckSum = 0;
        //    int i;
        //    for (i = start; i < end; i += 2) CheckSum += (ushort)System.BitConverter.ToInt16(Packet, i);
        //    if (i == end) CheckSum += (ushort)Packet[end];
        //    while (CheckSum >> 16 != 0) CheckSum = (CheckSum & 0xFFFF) + (CheckSum >> 16);
        //    return (ushort)~CheckSum;
        //}
    }

    public abstract class ICMPMessage
    {
        public abstract byte[] GetBytes();
    }

    public class ICMPIPHeaderReply : ICMPMessage
    {
        public byte[] Data;
        public IPPacket IP;

        public ICMPIPHeaderReply() : base() { }

        public ICMPIPHeaderReply(ref byte[] Packet)
            : base()
        {
            try
            {
                Data = new byte[Packet.Length - 4];
                System.Buffer.BlockCopy(Packet, 4, Data, 0, Data.Length);
                IP = new IPPacket(Data);
            }
            catch { }
        }

        public override byte[] GetBytes()
        {
            if (Data == null) Data = new byte[0];
            byte[] Packet = new byte[4 + Data.Length];
            System.Buffer.BlockCopy(Data, 0, Packet, 4, Data.Length);
            return Packet;
        }
    }

    public class ICMPEcho : ICMPMessage
    {
        public ushort Identifier;
        public ushort SequenceNumber;
        public string Data;

        public ICMPEcho() : base() { }

        public ICMPEcho(byte[] Packet)
            : base()
        {
            try
            {
                Identifier = (ushort)System.BitConverter.ToInt16(Packet, 0);
                SequenceNumber = (ushort)System.BitConverter.ToInt16(Packet, 2);
                Data = System.Text.Encoding.ASCII.GetString(Packet, 4, Packet.Length - 4);
            }
            catch { }
        }

        public override byte[] GetBytes()
        {
            if (Data == null) Data = "";
            byte[] Packet = new byte[4 + Data.Length];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Identifier), 0, Packet, 0, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)SequenceNumber), 0, Packet, 2, 2);
            System.Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(Data), 0, Packet, 4, Data.Length);
            return Packet;
        }
    }

    public class ICMPEchoReply : ICMPEcho
    {
        public ICMPEchoReply() : base() { }
        public ICMPEchoReply(byte[] Packet) : base(Packet) { }
    }

    public class ICMPRedirect : ICMPMessage
    {
        public ulong GatewayInternetAddress;
        public byte[] Data;

        public enum CodeEnum
        {
            RedirectDatagramsForTheNetwork = 0,
            RedirectDatagramsForTheHost = 1,
            RedirectDatagramsForTheTypeOfServiceAndNetwork = 2,
            RedirectDatagramsForTheTypeOfServiceAndHost = 3
        }

        public ICMPRedirect() : base() { }

        public ICMPRedirect(byte[] Packet)
            : base()
        {
            try
            {
                GatewayInternetAddress = (ulong)System.BitConverter.ToInt32(Packet, 0);
                Data = new byte[Packet.Length - 4];
                System.Buffer.BlockCopy(Packet, 0, Data, 4, Packet.Length);
            }
            catch { }
        }

        public override byte[] GetBytes()
        {
            if (Data == null) Data = new byte[0];
            byte[] Packet = new byte[4 + Data.Length];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)GatewayInternetAddress), 0, Packet, 0, 4);
            System.Buffer.BlockCopy(Data, 0, Packet, 4, Data.Length);
            return Packet;
        }
    }

    public class ICMPDestinationUnreachable : ICMPIPHeaderReply
    {
        public enum CodeEnum
        {
            NetUnreachable = 0,
            HostUnreachable = 1,
            ProtocolUnreachable = 2,
            PortUnreachable = 3,
            FragmentationNeededAndDFSet = 4,
            SourceRouteFailed = 5
        }

        public ICMPDestinationUnreachable() : base() { }
        public ICMPDestinationUnreachable(byte[] Packet) : base(ref Packet) { }
    }

    public class ICMPSourceQuench : ICMPIPHeaderReply
    {
        public ICMPSourceQuench() : base() { }
        public ICMPSourceQuench(byte[] Packet) : base(ref Packet) { }
    }

    public class ICMPTimeExceeded : ICMPIPHeaderReply
    {
        public enum CodeEnum
        {
            TimeToLiveExceededInTransit = 0,
            FragmentReassemblyTimeExceeded = 1
        }

        public ICMPTimeExceeded() : base() { }
        public ICMPTimeExceeded(byte[] Packet) : base(ref Packet) { }
    }

    public class ICMPParameterProblem : ICMPMessage
    {
        public byte Pointer;
        public byte[] Data;

        public ICMPParameterProblem() : base() { }

        public ICMPParameterProblem(byte[] Packet)
            : base()
        {
            try
            {
                Pointer = Packet[0];
                Data = new byte[Packet.Length - 4];
                System.Buffer.BlockCopy(Packet, 0, Data, 4, Packet.Length);
            }
            catch { }
        }

        public override byte[] GetBytes()
        {
            if (Data == null) Data = new byte[0];
            byte[] Packet = new byte[4 + Data.Length];
            Packet[0] = Pointer;
            System.Buffer.BlockCopy(Data, 0, Packet, 4, Data.Length);
            return Packet;
        }
    }

    public class ICMPTimestamp : ICMPMessage
    {
        public ushort Identifier;
        public ushort SequenceNumber;
        public ulong OriginateTimestamp;
        public ulong ReceiveTimestamp;
        public ulong TransmitTimestamp;

        public ICMPTimestamp() : base() { }

        public ICMPTimestamp(byte[] Packet)
            : base()
        {
            try
            {
                Identifier = (ushort)System.BitConverter.ToInt16(Packet, 0);
                SequenceNumber = (ushort)System.BitConverter.ToInt16(Packet, 2);
                OriginateTimestamp = (ulong)System.BitConverter.ToInt32(Packet, 4);
                ReceiveTimestamp = (ulong)System.BitConverter.ToInt32(Packet, 8);
                TransmitTimestamp = (ulong)System.BitConverter.ToInt32(Packet, 12);
            }
            catch { }
        }

        public override byte[] GetBytes()
        {
            byte[] Packet = new byte[16];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Identifier), 0, Packet, 0, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)SequenceNumber), 0, Packet, 2, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)OriginateTimestamp), 0, Packet, 4, 4);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)ReceiveTimestamp), 0, Packet, 8, 4);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)TransmitTimestamp), 0, Packet, 12, 4);
            return Packet;
        }
    }

    public class ICMPTimestampReply : ICMPTimestamp
    {
        public ICMPTimestampReply() : base() { }
        public ICMPTimestampReply(byte[] Packet) : base(Packet) { }
    }

    public class ICMPInformationRequest : ICMPMessage
    {
        public ushort Identifier;
        public ushort SequenceNumber;

        public ICMPInformationRequest() : base() { }

        public ICMPInformationRequest(byte[] Packet)
            : base()
        {
            try
            {
                Identifier = (ushort)System.BitConverter.ToInt16(Packet, 0);
                SequenceNumber = (ushort)System.BitConverter.ToInt16(Packet, 2);
            }
            catch { }
        }

        public override byte[] GetBytes()
        {
            byte[] Packet = new byte[4];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Identifier), 0, Packet, 0, 2);
            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)SequenceNumber), 0, Packet, 2, 2);
            return Packet;
        }
    }

    public class ICMPInformationReply : ICMPInformationRequest
    {
        public ICMPInformationReply() : base() { }
        public ICMPInformationReply(byte[] Packet) : base(Packet) { }
    }
}

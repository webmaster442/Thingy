using System;
using System.Net;

namespace Thingy.Engineering.Domain.IP
{
    internal static class IpExtensions
    {
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static uint GetUint(this IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();
            uint ret = 0;
            ret = (uint)bytes[0] << 24;
            ret += (uint)bytes[1] << 16;
            ret += (uint)bytes[2] << 8;
            ret += (uint)bytes[3];
            return ret;
        }

        public static IPAddress SetUint(this IPAddress adress, uint value)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)((value & 0xFF000000) >> 24);
            bytes[1] = (byte)((value & 0x00FF0000) >> 16);
            bytes[2] = (byte)((value & 0xFF00FF00) >> 8);
            bytes[3] = (byte)(value & 0xFF0000FF);
            return new IPAddress(bytes);
        }
    }
}

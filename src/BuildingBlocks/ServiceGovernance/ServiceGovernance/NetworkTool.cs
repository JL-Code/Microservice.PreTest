﻿using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Microservice.PreTest.src.BuildingBlocks.Service.Governance
{
    public class NetworkTool
    {
        /// <summary>
        /// 获取本地IP地址
        /// </summary>
        public static string LocalIPAddress
        {
            get
            {
                UnicastIPAddressInformation mostSuitableIp = null;
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var network in networkInterfaces)
                {
                    if (network.OperationalStatus != OperationalStatus.Up)
                        continue;
                    var properties = network.GetIPProperties();
                    if (properties.GatewayAddresses.Count == 0)
                        continue;

                    foreach (var address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        if (IPAddress.IsLoopback(address.Address))
                            continue;
                        return address.Address.ToString();
                    }
                }

                return mostSuitableIp != null
                    ? mostSuitableIp.Address.ToString()
                    : "";
            }
        }
    }
}

using System;

namespace Microservice.PreTest.src.BuildingBlocks.ServiceGovernance
{
    /// <summary>
    /// 服务条目
    /// </summary>
    public class ServiceEntry
    {
        public int Port { get; set; }

        public string IP { get; set; }

        public string ServiceName { get; set; }

        public string ConsulIP { get; set; }

        public int ConsulPort { get; set; }
    }
}

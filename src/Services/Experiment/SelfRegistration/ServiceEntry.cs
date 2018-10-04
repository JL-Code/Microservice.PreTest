using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service01
{
    //服务条目
    public class ServiceEntry
    {
        #region MyRegion

        public int Port { get; set; }

        public string IP { get; set; }

        public string ServiceName { get; set; }

        public string ConsulIP { get; set; }

        public int ConsulPort { get; set; }
        #endregion
    }
}

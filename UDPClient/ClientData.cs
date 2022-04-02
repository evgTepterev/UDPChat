using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    [Serializable]
    public class ClientData
    {
        public ClientData(string myProperty1, string myProperty2, string myProperty3, string myProperty4)
        {
            MyProperty1 = myProperty1;
            MyProperty2 = myProperty2;
            MyProperty3 = myProperty3;
            MyProperty4 = myProperty4;
        }

        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string MyProperty3 { get; set; }
        public string MyProperty4 { get; set; }

    }
}

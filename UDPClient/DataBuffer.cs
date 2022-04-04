using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    public static class DataBuffer
    {
        public static readonly List<string> dataBuffer = new List<string>();
        public static void GetData(string data)
        {
            dataBuffer.Add(data);
        }
    }
}

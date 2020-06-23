using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;


namespace AsyncTcpClient
{
    //用于回调参数
    class ReadObject
    {
        public NetworkStream netStream;
        public byte[] bytes;
        public ReadObject(NetworkStream NS, int buffsize)
        {
            this.netStream = NS;
            this.bytes = new byte[buffsize];
        }
    }
}

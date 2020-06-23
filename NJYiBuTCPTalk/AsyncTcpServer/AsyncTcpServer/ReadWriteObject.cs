using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AsyncTcpServer
{
    class ReadWriteObject
    {
        public TcpClient client;
        public NetworkStream netStream;
        public byte[] readByte;
        public byte[] writeByte;

        public ReadWriteObject(TcpClient client)
        {
            this.client = client;
            netStream = client.GetStream();
            readByte = new byte[client.ReceiveBufferSize];
            writeByte = new byte[client.SendBufferSize];
        }

        public void InitReadArray()
        {
            readByte = new byte[client.ReceiveBufferSize];
        }

        public void InitWriteArray()
        {
            writeByte = new byte[client.SendBufferSize];
        }
    }
}

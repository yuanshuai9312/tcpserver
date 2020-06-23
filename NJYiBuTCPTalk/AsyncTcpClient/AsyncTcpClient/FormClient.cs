using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//添加的名称空间
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AsyncTcpClient
{
    public partial class FormClient : Form
    {
        private bool isExit = false;
        //用于在一个线程操作另外一个线程的控件
        private delegate void SetListBoxCallback(string str);
        SetListBoxCallback setListBoxCallback;

        private delegate void SetTextBoxReceiveCallback(string str);
        SetTextBoxReceiveCallback setTextBoxReceiveCallback;

        private TcpClient client;
        private NetworkStream networkStream;

        //用于线程同步，初始状态设为非终止状态，使用手动重置方式
        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);

        
        public FormClient()
        {
            InitializeComponent();
            listBox_Status.HorizontalScrollbar = true;
            setListBoxCallback = new SetListBoxCallback(SetListBox);
            setTextBoxReceiveCallback = new SetTextBoxReceiveCallback(SetTextBoxReceive);

        }

        private void button_Stop_Listen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_ServerIp.Text))
            {
                try
                {
                    //使用IPV4
                    client = new TcpClient(AddressFamily.InterNetwork);

                    //实际使用时要将Dns.gEThostname()变为服务器域名或IP地址
                    IPAddress serviceIp = IPAddress.Parse(textBox_ServerIp.Text);
                    //创建一个委托，并指明在异步操作完成的方法
                    //AsyncCallback callBack = new AsyncCallback(RequestCallback);
                    //将事件的状态设为非终止状态
                    allDone.Reset();
                    //开始一个对远程主机的异步请求
                    client.BeginConnect(serviceIp, 58000, RequestCallback, client);
                    //listBox_Status.Invoke(setListBoxCallback, string.Format("本机EndPoint：{0}", client.Client.LocalEndPoint));
                    //listBox_Status.Invoke(setListBoxCallback,"开始与服务器建立连接");
                    ListBoxAdd(string.Format("本机EndPoint：{0}", client.Client.LocalEndPoint));
                    ListBoxAdd("开始与服务器建立连接");
                    //阻塞当前线程，即窗体界面不在做任何响应用户操作，等待BeginConnect完成
                    //这样做的目的是为了保证与服务器连接有结果（成功或失败）时，才能继续
                    //当BeginConnect完成时，会自动调用RequestCallback,
                    //通过在RequestCallback中调用Set方法解除阻塞
                    allDone.WaitOne();
                }
                catch (System.Exception ex)
                {
                    textBox_ServerIp.Clear();
                    listBox_Status.Invoke(setListBoxCallback,string.Format("连接[{0}]失败",textBox_ServerIp.Text));
                }
            }
            


        }

        //ar是IAsyncResult类型的接口，表示异步操作的状态
        //是由Listner。beginaccepttcpclient（callback，listener）传递过来的
        private void RequestCallback(IAsyncResult ar)
        {
            //异步操作能执行到此处，说明调用beginconnect已经完成
            //并得到了IAsyncResult类型的状态参数ar，但beginconnect尚未结束
             //此时需要解除阻塞，以便能调用Endconnect
            allDone.Set();
            //调用Set后，事件状态变为终止状态，当前线程继续
            //buttonconnect_client执行结束
            //同时窗体可以响应用户操作
            try
            {
                //获取连接成功后得到的参数状态
                client = (TcpClient)ar.AsyncState;
                //异步接收传入的连接尝试，使beginconnect正常结束
                client.EndConnect(ar);
                ListBoxAdd(string.Format("连接[{0}]成功", client.Client.RemoteEndPoint));
                //listBox_Status.Invoke(setListBoxCallback,string.Format("连接[{0}]成功",client.Client.RemoteEndPoint));
                //获取接收和发送数据的网络流
                networkStream = client.GetStream();
                //异步接收服务器发送的数据，Beginread完成后，会自动调用readcallback
                ReadObject readObj = new ReadObject(networkStream, client.ReceiveBufferSize);
                networkStream.BeginRead(readObj.bytes, 0, readObj.bytes.Length, ReadCallback, readObj);
            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback, ex.Message);
                return;
            }

        }

        private void ReadCallback(IAsyncResult ar)
        {
            //异步操作能执行到此处，说明调用beginread已经完成
            try
            {
                ReadObject readobj = (ReadObject)ar.AsyncState;
                int count = readobj.netStream.EndRead(ar);
                //textBox_Receive.Invoke(setTextBoxReceiveCallback, System.Text.Encoding.Unicode.GetString(readobj.bytes, 0, count));
                TextBoxAdd("[服务器]:"+System.Text.Encoding.Unicode.GetString(readobj.bytes, 0, count));
                if (isExit==false)
                {
                    //重新调用beginread进行异步读取
                    readobj = new ReadObject(networkStream,client.ReceiveBufferSize);
                    networkStream.BeginRead(readobj.bytes, 0, readobj.bytes.Length, ReadCallback, readobj);
                }

            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback,ex.Message);
            }
        }

        private void SendString(string str)
        {
            try
            {
                byte[] byteDate = Encoding.Unicode.GetBytes(str + "\r\n");
                networkStream.BeginWrite(byteDate, 0, byteDate.Length, new AsyncCallback(SendBack), networkStream);
                networkStream.Flush();
                ListBoxAdd(">>服务器>>"+str);
            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback,ex.Message);
            }
        }

        private void SendBack(IAsyncResult ar)
        {
            try
            {
                networkStream.EndWrite(ar);
            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback,ex.Message);
            }
        }

        private void SetListBox(string str)
        {
            listBox_Status.Items.Add(str);
            listBox_Status.SelectedIndex = listBox_Status.Items.Count - 1;
            listBox_Status.ClearSelected();
        }


        private void SetTextBoxReceive(string str)
        {
            textBox_Receive.AppendText(str);

        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            SendString(textBox_Send.Text);
            textBox_Send.Clear();
        }

        private void FormClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            isExit = true;
            allDone.Set();
        }

        private void ListBoxAdd(string str)
        {
            listBox_Status.Invoke(setListBoxCallback,str);
            listBox_Status.Invoke(setListBoxCallback, "\r\n");
        }

        private void TextBoxAdd(string str)
        {
            textBox_Receive.Invoke(setTextBoxReceiveCallback, str);
            textBox_Receive.Invoke(setTextBoxReceiveCallback, "\r\n");

        }



    }
}

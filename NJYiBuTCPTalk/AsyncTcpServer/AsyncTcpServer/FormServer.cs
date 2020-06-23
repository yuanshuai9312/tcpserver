using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//添加的命名空间
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace AsyncTcpServer
{
    public partial class FormServer : Form
    {
        private bool isExit = false;
        //保存连接的所有客户端
        System.Collections.ArrayList clientList = new System.Collections.ArrayList();
        TcpListener listener;
        //用于一个线程操作另外一个线程的控件
        private delegate void SetListBoxCallback(string str);
        private SetListBoxCallback setListBoxCallback;

        private delegate void SetRichTextBoxCallback(string str);
        private SetRichTextBoxCallback setRichTextBoxCallback;

        private delegate void SetComboBoxCallback(string str);
        private SetComboBoxCallback setComboBoxCallback;

        private delegate void RemoveComboBoxItemsCallback(ReadWriteObject  readWriteObject);
        private RemoveComboBoxItemsCallback removeComboBoxItemsCallback;

        //用于线程同步，初始状态设为非终止状态，使用手动重置方式
        private EventWaitHandle allDone = new EventWaitHandle(false,EventResetMode.ManualReset);

        IPAddress selectIP;  

        public FormServer()
        {
            InitializeComponent();
            listBox_Status.HorizontalScrollbar = true;
            setListBoxCallback = new SetListBoxCallback(SetListBox);
            setRichTextBoxCallback = new SetRichTextBoxCallback(SetReceiveText);
            setComboBoxCallback = new SetComboBoxCallback(SetComboBox);
            removeComboBoxItemsCallback = new RemoveComboBoxItemsCallback(RemoveComboBoxItems);
            button_Start_Listen.Enabled = true;
            button_Stop_Listen.Enabled = false;

            //获取本机所有IP地址
            IPAddress[] ipAdress = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress i in ipAdress)
            {
                comboBox_IpAdress.Items.Add(i.ToString());
            }
            comboBox_IpAdress.SelectedIndex = 0;
        }


        #region 按钮事件处理函数
        //【停止监听】按钮
        private void button_Stop_Listen_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < clientList.Count; ++i)
            //{
            //    ((ReadWriteObject)clientList[i]).client.Close();
            //}
            //使线程自动结束
            isExit = true;
            //停止监听，不停止的话，下次点击开始监听会报错
           

            comboBox1.Items.Clear();
            //将事件状态置为终止状态，允许一个或者多个线程继续
            //从而使线程正常结束
            //listener.Stop();
            allDone.Set();
            button_Start_Listen.Enabled = true;
            button_Stop_Listen.Enabled = false;
        }

        //【开始监听】按钮
        private void button_Start_Listen_Click(object sender, EventArgs e)
        {
            isExit = false;
            //由于服务器要为多个客户端服务，所以需要创建一个线程监听客户端的链接请求
            selectIP = IPAddress.Parse(comboBox_IpAdress.Text.ToString());
            ThreadStart ts = new ThreadStart(AcceptConnect);
            Thread myThread = new Thread(ts);
            myThread.Start();
            button_Start_Listen.Enabled = false;
            button_Stop_Listen.Enabled = true;
        }

        //【发送】按钮
        private void button_Send_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("请先选择接收方，再单击[发送]");
            }
            else
            {
                ReadWriteObject obj = (ReadWriteObject)clientList[index];
                SendString(obj, textBox_Send.Text);
                textBox_Send.Clear();
            }

        } 
        #endregion

        #region 监听线程
        /// <summary>
        /// 监听线程，循环监听客户端请求
        /// </summary>
        private void AcceptConnect()
        {
            
            listener = new TcpListener(selectIP, 58000);
            listener.Start();
            while (isExit == false)
            {
                try
                {
                    //将事件的状态设为非终止
                    allDone.Reset();
                    //引用在异步操作完成时调用的回调方法，就是一个委托
                    AsyncCallback callback = new AsyncCallback(AcceptTcpClientCallback);
                    listBox_Status.Invoke(setListBoxCallback, "开始等待客户接入");
                    //开始一个异步操作接受传入的链接尝试
                    listener.BeginAcceptTcpClient(callback, listener);
                    //阻塞当前线程，直到收到客户链接信号
                    allDone.WaitOne();
                }
                catch (System.Exception ex)
                {
                    listBox_Status.Invoke(setListBoxCallback, ex.Message);
                    break;
                }
            }
            listener.Stop();


        } 
        #endregion

        #region 接收TCPclient的回调方法
        //ar是IAsyncResult类型的接口，表示异步操作的状态
        //是由lister.beginaccepttcpclient(callback,listner)传递过来的
        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            if (isExit==false)
            {
                try
                {
                    //将事件状态设为终止状态，允许一个或多个等待链接线程继续
                    allDone.Set();
                    TcpListener myListener = (TcpListener)ar.AsyncState;
                    //异步接收传入的连接，并创建新的TCPclient对象处理远程主机通信
                    TcpClient client = myListener.EndAcceptTcpClient(ar);
                    listBox_Status.Invoke(setListBoxCallback, "已接受客户连接：" + client.Client.RemoteEndPoint);
                    comboBox1.Invoke(setComboBoxCallback, client.Client.RemoteEndPoint.ToString());
                    ReadWriteObject readWriteObject = new ReadWriteObject(client);
                    clientList.Add(readWriteObject);
                    SendString(readWriteObject, "服务器已经接收连接，请通话！");
                    readWriteObject.netStream.BeginRead(readWriteObject.readByte, 0, readWriteObject.readByte.Length, ReadCallback, readWriteObject);
                }
                catch (System.Exception ex)
                {
                    listBox_Status.Invoke(setListBoxCallback, ex.Message);
                    return;
                }
            }
            
        } 
        #endregion

        #region 读客户端发送信息的回调方法
        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                ReadWriteObject readWriteObject = (ReadWriteObject)ar.AsyncState;
                int count = readWriteObject.netStream.EndRead(ar);
                textBox_Receive.Invoke(setRichTextBoxCallback, string.Format("[来自{0}]{1}", readWriteObject.client.Client.RemoteEndPoint, System.Text.Encoding.Unicode.GetString(readWriteObject.readByte, 0, count)));
                if (isExit == false)
                {
                    readWriteObject.InitReadArray();
                    readWriteObject.netStream.BeginRead(readWriteObject.readByte, 0, readWriteObject.readByte.Length, ReadCallback, readWriteObject);
                }
            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback, ex.Message);
            }
        } 
        #endregion

        #region 向客户端异步发送信息
        private void SendString(ReadWriteObject readWriteObject, string str)
        {
            try
            {
                readWriteObject.writeByte = System.Text.Encoding.Unicode.GetBytes(str + "\r\n");
                readWriteObject.netStream.BeginWrite(readWriteObject.writeByte, 0, readWriteObject.writeByte.Length, new AsyncCallback(SendCallback), readWriteObject);//这里的callback委托为什么要新建一个对象呢？而不直接使用SENDCALLback？

                readWriteObject.netStream.Flush();
                listBox_Status.Invoke(setListBoxCallback, string.Format("向{0}发送：{1}", readWriteObject.client.Client.RemoteEndPoint,str));


            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback, ex.Message);
            }

        } 
        #endregion

        #region 客户端发送信息的回调方法
        private void SendCallback(IAsyncResult ar)
        {
            ReadWriteObject readWriteObject = (ReadWriteObject)ar.AsyncState;
            try
            {
                readWriteObject.netStream.EndWrite(ar);
            }
            catch (System.Exception ex)
            {
                listBox_Status.Invoke(setListBoxCallback, ex.Message);
                comboBox1.Invoke(removeComboBoxItemsCallback, readWriteObject);//如果发送出现异常，就把ComboBox中指定的项删掉
            }
        } 
        #endregion

        #region 别的线程操作控件
        private void RemoveComboBoxItems(ReadWriteObject readWriteObject)
        {
            int index = clientList.IndexOf(readWriteObject);
            comboBox1.Items.RemoveAt(index);
        }

        private void SetListBox(string str)
        {
            listBox_Status.Items.Add(str);
            listBox_Status.SelectedIndex = listBox_Status.Items.Count - 1;
            listBox_Status.ClearSelected();

        }

        private void SetReceiveText(string str)
        {
            textBox_Receive.AppendText(str);
        }

        private void SetComboBox(object obj)
        {
            comboBox1.Items.Add(obj);
        }
        
        #endregion
      

        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            button_Stop_Listen_Click(null,null);
        } 












    }
}

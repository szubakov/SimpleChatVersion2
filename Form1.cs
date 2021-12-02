using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;


namespace SimpleChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // устанавливаем параметры по умолчанию
            UserName = "DefName";
            Ip = getIP();
            Port = "4665";

            if (System.IO.File.Exists(@"config.cfg"))
            {
                foreach (string line in System.IO.File.ReadLines(@"config.cfg"))
                {
                    var items = parseLine(line);
                    if (items.Item1 == "Name")
                    {
                        UserName = items.Item2;
                    }
                    else if (items.Item1 == "Ip")
                    {
                        Ip = items.Item2;
                    }
                    else if (items.Item1 == "Port")
                    {
                        Port = items.Item2;
                    }
                }
            }
            InitializeComponent();
            // запускаем сервер в отдельном потоке
            new Thread(() =>
            {
                try
                {
                    if (!RunServer(Ip, Port))
                    {
                        lock (loglock)
                        {
                            log.Log("Не удалось запустить сервер");
                        }
                    }
                }
                catch (Exception ex)
                {
                    lock (loglock)
                    {
                        log.Log(ex.Message);
                    }
                }
            }).Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textName.Text = UserName;
            textIP.Text = Ip;
            textPort.Text = Port;
            isOptionsChanged = false;
        }

        private string UserName;
        private string Ip;
        private string Port;
        private TcpClient client;
        private TcpListener ServerSocket;
        private static Logger log = new Logger();
        private static bool isActive = true;
        // см https://stackoverflow.com/a/40492829
        private bool invokeInProgress = false;
        private bool stopInvoking = false;
        private bool isOptionsChanged = false;

        private Tuple<string, string> parseLine(String str)
        {
            var parts = str.Split('=');
            if (parts.Length != 2)
            {
                return new Tuple<string, string>("", "");
            }
            return new Tuple<string, string>(parts[0].Trim(), parts[1].Trim());
        }

        enum EventType
        {
            NewConnection,
            Message,
            Disconnection
        }

        static readonly object loglock = new object();
        static readonly object listlock = new object();

        private uint clientCount;
        // клиент и его id
        private Dictionary<uint, TcpClient> clients = new Dictionary<uint, TcpClient>();

        // очередь сообщений
        private readonly ConcurrentQueue<Tuple<EventType, object, string>> queue =
            new ConcurrentQueue<Tuple<EventType, object, string>>();

        public bool RunServer(string ip, string port)
        {
            clientCount = 0;
            ServerSocket = new TcpListener(IPAddress.Parse(ip), int.Parse(port));
            try
            {
                // на этом порту сервер уже запущен
                ServerSocket.Start();
            }
            catch
            {
                return false;
            }

            // ждем подключения клиентов в отдельном потоке
            new Thread(() =>
            {
                try
                {
                    while (isActive)
                    {
                        TcpClient client = ServerSocket.AcceptTcpClient();
                        queue.Enqueue(new Tuple<EventType, object, string>(EventType.NewConnection, client, ""));
                    }
                }
                catch
                {

                }
            }).Start();

            // ждем сообщений и реагируем соответственно
            while (isActive)
            {
                // пришло сообщение
                if (queue.Count != 0)
                {
                    Tuple<EventType, object, string> item;
                    queue.TryDequeue(out item);

                    if (item.Item1 == EventType.NewConnection)
                    {
                        lock (listlock)
                        {
                            clients.Add(clientCount, (TcpClient)item.Item2);
                        }

                        WriteTextSafe("Новый клиент подключен" + Environment.NewLine);
                        // каждый клиент слушается в своем потоке
                        Thread t = new Thread(handle_client);
                        t.Start(clientCount);
                        clientCount++;
                    }
                    else if (item.Item1 == EventType.Disconnection)
                    {
                        lock (listlock)
                        {
                            clients.Remove(uint.Parse(item.Item3));
                        }
                        if (isActive)
                        {
                            WriteTextSafe("Клиент отключен " + Environment.NewLine);
                        }
                    }
                    else
                    {
                        DateTime now = DateTime.Now;

                        byte[] buffer = Encoding.UTF8.GetBytes(
                            String.Format("{0} | {1}", now.ToString("dd.MM.yyyy HH:mm:ss"), item.Item3));
                        foreach (KeyValuePair<uint, TcpClient> entry in clients)
                        {
                            NetworkStream stream = entry.Value.GetStream();
                            try
                            {
                                stream.Write(buffer, 0, buffer.Length);
                            }
                            catch
                            {

                            }
                        }
                    }

                }
            }
            return true;
        }

        private void bSent_Click(object sender, EventArgs e)
        {
            if (messageArea.Text.Length != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(textName.Text);
                sb.Append(" > ");
                sb.Append(messageArea.Text);
                sb.Append(Environment.NewLine);
                SendMessage(sb.ToString());
                messageArea.Text = "";
            }
            else
            {
                MessageBox.Show("Введите текст сообщения");
            }
        }

        public void WriteTextSafe(string text)
        {
            if (stopInvoking || !isActive) // don't start new invokes if the flag is set
            {
                return;
            }

            if (textArea.InvokeRequired)
            {
                if (stopInvoking != true)
                {
                    invokeInProgress = true;  // let the form know if an invoke has started
                    Action safeWrite = delegate
                    {
                        WriteTextSafe(text);
                    };
                    textArea.Invoke(safeWrite);
                    invokeInProgress = false;  // the invoke is complete
                }
            }
            else
                textArea.Text += text;
        }

        public void SendMessage(string text)
        {
            try
            {
                NetworkStream ns = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                ns.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                lock (loglock)
                {
                    log.Log(e.Message);
                }
                WriteTextSafe("Не удалось отправить сообщение, сервер не доступен" + Environment.NewLine);
            }
        }

        public void OnReceive()
        {
            while (isActive)
            {
                try
                {
                    NetworkStream ns = client.GetStream();
                    if (ns.DataAvailable)
                    {
                        byte[] receivedBytes = new byte[1024];
                        int byte_count;

                        StringBuilder sb = new StringBuilder();

                        while (ns.DataAvailable && (byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
                        {
                            sb.Append(Encoding.UTF8.GetString(receivedBytes, 0, byte_count));
                        }

                        if (sb.Length != 0)
                        {
                            WriteTextSafe(sb.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    WriteTextSafe("Сервер отключил соединение" + Environment.NewLine);
                    lock (loglock)
                    {
                        log.Log(e.Message);
                        return;
                    }
                }
            }
        }

        public void handle_client(object id)
        {
            TcpClient client;

            lock (listlock)
            {
                client = clients[(uint)id];
            }

            while (isActive)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                try
                {
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }

                    string data = Encoding.UTF8.GetString(buffer, 0, byte_count);
                    queue.Enqueue(new Tuple<EventType, object, string>(EventType.Message, client, data));
                }
                catch (Exception e)
                {
                    lock (loglock)
                    {
                        log.Log(e.Message);
                    }
                    break;
                }
            }

            queue.Enqueue(new Tuple<EventType, object, string>(EventType.Disconnection, client, id.ToString()));
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private async void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (invokeInProgress)
            {
                e.Cancel = true;  // cancel the original event 

                stopInvoking = true; // advise to stop taking new work

                // now wait until current invoke finishes
                await System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    while (invokeInProgress) ;
                });

                // now close the form
                this.Close();
                return;
            }
            // клиент был подключен
            if(bSent.Enabled)
            {
                client.Client.Shutdown(SocketShutdown.Send);
                client.Close();
            }
     
            ServerSocket.Stop();
            isActive = false;

            if (isOptionsChanged)
            {
                DialogResult dialogResult = MessageBox.Show
                    ("Параметры были измененs, перезаписать конфиг?", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    System.IO.File.WriteAllText(@"config.cfg",
                        string.Format("Name = {0}\nIp = {1}\nPort = {2}\n", textName.Text, textIP.Text, textPort.Text));
                }
            }

            Hide();
            Thread.Sleep(1000);// даем возможность корректно закрыть все соединения
        }

        // https://stackoverflow.com/a/27376368
        private static string getIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                try
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address.ToString();
                }
                catch
                {
                    return "127.0.0.1";
                }
            }
        }

        private void OptionsChanged(object sender, EventArgs e)
        {
            if(
                UserName != textName.Text ||
                Ip != textIP.Text ||
                Port != textPort.Text)
            {
                isOptionsChanged = true;
            }
        }

        private void connect_Click(object sender, EventArgs e)
        {
            UserName = textName.Text;
            Ip = textIP.Text;
            Port = textPort.Text;

            try
            {
                // запускаем клиент
                client = new TcpClient();
                client.Connect(IPAddress.Parse(Ip), int.Parse(Port));
                // ответы от сервера обрабатываем в отдельном потоке
                // чтобы не блокировать UI-поток
                Thread thread = new Thread(OnReceive);
                thread.Start();

                bSent.Enabled = true;
                bConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                WriteTextSafe("Не удалось подключиться к серверу" + Environment.NewLine);
                lock (loglock)
                {
                    log.Log(ex.Message);
                }
            }
        }
    }
}
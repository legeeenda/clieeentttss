using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;



namespace clieeentttss
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public Form1()
        {
            InitializeComponent();
            client = new TcpClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string serverIP = "";

            try
            {
                client = new TcpClient();
                client.Connect(serverIP, 8888);

                if (client.Connected)
                {
                    MessageBox.Show("Ты был подключен к серверу ");

                    // Получение потока данных
                    NetworkStream serverStream = client.GetStream();


                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }

                Thread clientThread = new Thread(new ThreadStart(ReceiveMessages));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
        private void ReceiveMessages()
        {
            NetworkStream serverStream = client.GetStream();
            byte[] receiveBuffer = new byte[1024];

            while (true)
            {
                try
                {

                    int bytesRead = serverStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    string receivedMessage = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

                    textBoxChat.Invoke((MethodInvoker)(() =>
                    {
                        textBoxChat.Text += "Вы: " + receivedMessage + Environment.NewLine;
                    }));
                }
                catch (Exception)
                {
                    break;
                }
            }

            client.Close();
        }
        private void btnsend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!client.Connected)
                {
                    client.Connect("", 8888);
                }

                string message = textBoxMessage.Text;
                byte[] sendBuffer = Encoding.UTF8.GetBytes(message);

                NetworkStream serverStream = client.GetStream();
                serverStream.Write(sendBuffer, 0, sendBuffer.Length);
                serverStream.Flush();

                textBoxChat.Text += "Вы: " + message + Environment.NewLine;
                textBoxMessage.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChat_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

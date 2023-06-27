using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Client
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isConnected = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(nameTextBox.Text) || nameTextBox.Text != "SERVER")
            {
                try
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", 6666);
                    reader = new StreamReader(client.GetStream());
                    writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(nameTextBox.Text);
                    writer.Flush();
                    Task.Run(() => ServerListen());

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Name empty");
            }
        }

        private void ServerListen()
        {
            try
            {
                while (true)
                {
                    string msgObj = reader.ReadLine();
                    Message msg = JsonConvert.DeserializeObject<Message>(msgObj);
                    messagesListBox.Items.Add($"[{msg.TimeStamp}] {msg.SenderName}: {msg.Content}");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(messageTextBox.Text))
            {
                writer.WriteLine(messageTextBox.Text);
                writer.Flush();
                messageTextBox.Clear();
            }
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                sendMessageButton_Click(sendMessageButton, new EventArgs());
            }
        }
    }
}
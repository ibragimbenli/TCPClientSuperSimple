using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPSERVERSuperSimple
{
    public partial class Server : Form
    {
        SimpleTcpServer server;
        public Server()
        {
            InitializeComponent();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtServerIp.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived; ;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var strIncomingData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
                txtInfo.Text += strIncomingData;
            });
          
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
            lstClientIp.Items.Add(e.IpPort);
        }
        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            txtInfo.Text += $"{e.IpPort} DisConnected.{Environment.NewLine}";
            lstClientIp.Items.Remove(e.IpPort);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"başlatılıyor...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (!string.IsNullOrEmpty(txtMessage.Text) && lstClientIp.SelectedItem != null)//check message and seelct client ip from listbox
                {
                    server.Send(lstClientIp.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server:{Environment.NewLine}{txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }
    }
}

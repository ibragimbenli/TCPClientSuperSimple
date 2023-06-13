using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPClientSuperSimple
{
    public partial class Client : Form
    {
        SimpleTcpClient client;
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient(txtIpAdres.Text + ":" + txtPort.Text);
            client.Events.Connected += Events_Connected;
            client.Events.Disconnected += Events_Disconnected;
            client.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var strIncomingData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
                //txtInMessage.Text += strIncomingData;
                txtInMessage.Text += $"Gelen: {txtOutMessage.Text}{Environment.NewLine} " + strIncomingData;
            });
        }

        private void Events_Disconnected(object sender, ConnectionEventArgs e)
        {
            txtInMessage.Text += $"{e.IpPort} DisConnected.{Environment.NewLine}";
        }

        private void Events_Connected(object sender, ConnectionEventArgs e)
        {
            txtInMessage.Text += $"{e.IpPort} Connected.{Environment.NewLine}";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
            }
            catch (Exception)
            {

                MessageBox.Show("Bağlantı Zaman aşımına Uğradı!.", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendData(txtOutMessage.Text);
            txtInMessage.Text += $"->: {Environment.NewLine}{txtOutMessage.Text}{Environment.NewLine}";
            txtOutMessage.Text = string.Empty;
        }
        private void SendData(string data)
        {
            byte[] vOut = Encoding.UTF8.GetBytes(data);
            client.Send(vOut);
        }

        private void btnGetIp_Click(object sender, EventArgs e)
        {
            txtInMessage.Text = string.Empty;
            string hostName = Dns.GetHostName();

            string myIp = Dns.GetHostByName(hostName).AddressList[0].ToString();
            txtInMessage.Text += myIp;
        }
    }
}

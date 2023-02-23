using SimpleTCP;
using System;
using System.Text;
using System.Windows.Forms;
using EasySave_G8_UI;
using EasySave_G8_UI.View_Models;

namespace EasySave_Server
{
    public partial class ES_Server : Form
    {
        public ES_Server()
        {
            InitializeComponent();
            lblLang.Text = $"{View_Model.VM_GetString_Language("SrvLang")}";
        }

        SimpleTcpServer server;
        private void Server_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;//enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            //Update mesage to txtStatus
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
                e.ReplyLine(string.Format("Connection etablished"));
            });

            if (e.MessageString == "paolo")
            {
                MessageBox.Show(e.MessageString, "bonjour");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Start server host
            txtStatus.Text += "Server starting...";
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(txtHost.Text);
            server.Start(ip, Convert.ToInt32(txtPort.Text));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
                txtStatus.Text += "Server stopping...";
            server.BroadcastLine("Server is stopped");
            server.Stop();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            server.Broadcast(txtMessage.Text);
        }
    }
}
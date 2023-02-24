using System.Text;
using SimpleTCP;

namespace EasySave_Client
{
    public partial class ES_Client : Form
    {
        public ES_Client()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            //Connect to server
            client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
            client.WriteLineAndGetReply("I am the client", TimeSpan.FromSeconds(3));
        }

        private void Client_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            //Update message to txtStatus
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
            });
        }

        private void btnPaolo_Click(object sender, EventArgs e)
        {
            client.Write("paolo");
        }
    }
}
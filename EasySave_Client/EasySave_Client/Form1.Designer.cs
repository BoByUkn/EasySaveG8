namespace EasySave_Client
{
    partial class ES_Client
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnPaolo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(91, 101);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(580, 253);
            this.txtStatus.TabIndex = 11;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(314, 66);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(91, 27);
            this.txtPort.TabIndex = 10;
            this.txtPort.Text = "8080";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(91, 66);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(125, 27);
            this.txtHost.TabIndex = 9;
            this.txtHost.Text = "127.0.0.1";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(257, 69);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(42, 20);
            this.lblPort.TabIndex = 8;
            this.lblPort.Text = "Port :";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(35, 69);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(47, 20);
            this.lblHost.TabIndex = 7;
            this.lblHost.Text = "Host :";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(577, 65);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(94, 29);
            this.btnConnect.TabIndex = 12;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnPaolo
            // 
            this.btnPaolo.Location = new System.Drawing.Point(91, 390);
            this.btnPaolo.Name = "btnPaolo";
            this.btnPaolo.Size = new System.Drawing.Size(94, 29);
            this.btnPaolo.TabIndex = 13;
            this.btnPaolo.Text = "Paolo";
            this.btnPaolo.UseVisualStyleBackColor = true;
            this.btnPaolo.Click += new System.EventHandler(this.btnPaolo_Click);
            // 
            // ES_Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnPaolo);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblHost);
            this.Name = "ES_Client";
            this.Text = "EasySave Client";
            this.Load += new System.EventHandler(this.Client_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtStatus;
        private TextBox txtPort;
        private TextBox txtHost;
        private Label lblPort;
        private Label lblHost;
        private Button btnConnect;
        private Button btnPaolo;
    }
}
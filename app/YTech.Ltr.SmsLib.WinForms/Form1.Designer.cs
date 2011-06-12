namespace YTech.Ltr.SmsLib.WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cboTimeout = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.gbConnect = new System.Windows.Forms.GroupBox();
            this.dtSalesDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefreshPort = new System.Windows.Forms.Button();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.timerSms = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtSalesDateManual = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtManual = new System.Windows.Forms.TextBox();
            this.timerTrans = new System.Windows.Forms.Timer(this.components);
            this.gbConnect.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(6, 52);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(459, 235);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.WordWrap = false;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(41, 23);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(91, 23);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "Baca SMS";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // lblTimeout
            // 
            this.lblTimeout.Location = new System.Drawing.Point(16, 64);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(100, 23);
            this.lblTimeout.TabIndex = 19;
            this.lblTimeout.Text = "Ti&meout (ms):";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.Location = new System.Drawing.Point(16, 40);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(100, 23);
            this.lblBaudRate.TabIndex = 17;
            this.lblBaudRate.Text = "&Baud rate:";
            this.lblBaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboTimeout
            // 
            this.cboTimeout.Location = new System.Drawing.Point(128, 64);
            this.cboTimeout.Name = "cboTimeout";
            this.cboTimeout.Size = new System.Drawing.Size(104, 21);
            this.cboTimeout.TabIndex = 20;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(16, 16);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(88, 23);
            this.lblPort.TabIndex = 15;
            this.lblPort.Text = "&COM-Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPort
            // 
            this.cboPort.Location = new System.Drawing.Point(128, 16);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(104, 21);
            this.cboPort.TabIndex = 16;
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.Location = new System.Drawing.Point(128, 40);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(104, 21);
            this.cboBaudRate.TabIndex = 18;
            // 
            // btnTest
            // 
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTest.Location = new System.Drawing.Point(40, 131);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "&Test";
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(128, 131);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "Connect";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(298, 23);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(103, 23);
            this.btnDisconnect.TabIndex = 23;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Visible = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // gbConnect
            // 
            this.gbConnect.Controls.Add(this.dtSalesDate);
            this.gbConnect.Controls.Add(this.label1);
            this.gbConnect.Controls.Add(this.btnRefreshPort);
            this.gbConnect.Controls.Add(this.lblPort);
            this.gbConnect.Controls.Add(this.cboBaudRate);
            this.gbConnect.Controls.Add(this.btnTest);
            this.gbConnect.Controls.Add(this.cboPort);
            this.gbConnect.Controls.Add(this.btnOK);
            this.gbConnect.Controls.Add(this.cboTimeout);
            this.gbConnect.Controls.Add(this.lblTimeout);
            this.gbConnect.Controls.Add(this.lblBaudRate);
            this.gbConnect.Location = new System.Drawing.Point(12, 12);
            this.gbConnect.Name = "gbConnect";
            this.gbConnect.Size = new System.Drawing.Size(471, 160);
            this.gbConnect.TabIndex = 24;
            this.gbConnect.TabStop = false;
            this.gbConnect.Text = "Koneksi Modem";
            // 
            // dtSalesDate
            // 
            this.dtSalesDate.CustomFormat = "dd-MMM-yyyy";
            this.dtSalesDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtSalesDate.Location = new System.Drawing.Point(128, 91);
            this.dtSalesDate.Name = "dtSalesDate";
            this.dtSalesDate.Size = new System.Drawing.Size(104, 20);
            this.dtSalesDate.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 24;
            this.label1.Text = "Tanggal :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefreshPort
            // 
            this.btnRefreshPort.Location = new System.Drawing.Point(238, 16);
            this.btnRefreshPort.Name = "btnRefreshPort";
            this.btnRefreshPort.Size = new System.Drawing.Size(105, 23);
            this.btnRefreshPort.TabIndex = 23;
            this.btnRefreshPort.Text = "Refresh Port";
            this.btnRefreshPort.UseVisualStyleBackColor = true;
            this.btnRefreshPort.Click += new System.EventHandler(this.btnRefreshPort_Click);
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.btnStop);
            this.gbStatus.Controls.Add(this.btnDisconnect);
            this.gbStatus.Controls.Add(this.btnRead);
            this.gbStatus.Controls.Add(this.txtOutput);
            this.gbStatus.Location = new System.Drawing.Point(12, 178);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(471, 293);
            this.gbStatus.TabIndex = 25;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "SMS";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(156, 23);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(91, 23);
            this.btnStop.TabIndex = 24;
            this.btnStop.Text = "Berhenti";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // timerSms
            // 
            this.timerSms.Interval = 5000;
            this.timerSms.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtSalesDateManual);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtManual);
            this.groupBox1.Location = new System.Drawing.Point(508, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(515, 459);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manual Input";
            // 
            // dtSalesDateManual
            // 
            this.dtSalesDateManual.CustomFormat = "dd-MMM-yyyy";
            this.dtSalesDateManual.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtSalesDateManual.Location = new System.Drawing.Point(121, 19);
            this.dtSalesDateManual.Name = "dtSalesDateManual";
            this.dtSalesDateManual.Size = new System.Drawing.Size(104, 20);
            this.dtSalesDateManual.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 26;
            this.label2.Text = "Tanggal :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(220, 359);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Simpan";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtManual
            // 
            this.txtManual.Location = new System.Drawing.Point(6, 45);
            this.txtManual.Multiline = true;
            this.txtManual.Name = "txtManual";
            this.txtManual.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtManual.Size = new System.Drawing.Size(503, 307);
            this.txtManual.TabIndex = 1;
            this.txtManual.WordWrap = false;
            // 
            // timerTrans
            // 
            this.timerTrans.Interval = 7000;
            this.timerTrans.Tick += new System.EventHandler(this.timerTrans_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 483);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.gbConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gbConnect.ResumeLayout(false);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cboTimeout;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.GroupBox gbConnect;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.Button btnRefreshPort;
        private System.Windows.Forms.Timer timerSms;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.DateTimePicker dtSalesDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtManual;
        private System.Windows.Forms.DateTimePicker dtSalesDateManual;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timerTrans;
    }
}


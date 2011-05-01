using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GsmComm.GsmCommunication;

namespace YTech.Ltr.Sms.WinForms
{
    public partial class FormConnection : Form
    {
        private int port;
        private int baudRate;
        private int timeout;

        public FormConnection()
        {
            InitializeComponent();

            btnTest.Click += new EventHandler(btnTest_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
        }

        public void SetData(int port, int baudRate, int timeout)
        {
            this.port = port;
            this.baudRate = baudRate;
            this.timeout = timeout;
        }

        public void GetData(out int port, out int baudRate, out int timeout)
        {
            port = this.port;
            baudRate = this.baudRate;
            timeout = this.timeout;
        }

        private bool EnterNewSettings()
        {
            int newPort;
            int newBaudRate;
            int newTimeout;

            try
            {
                newPort = int.Parse(cboPort.Text.Replace("COM",""));
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Invalid port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboPort.Focus();
                return false;
            }

            try
            {
                newBaudRate = int.Parse(cboBaudRate.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Invalid baud rate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboBaudRate.Focus();
                return false;
            }

            try
            {
                newTimeout = int.Parse(cboTimeout.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Invalid timeout value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboTimeout.Focus();
                return false;
            }

            SetData(newPort, newBaudRate, newTimeout);

            return true;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (!EnterNewSettings())
                DialogResult = DialogResult.None;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region Display all available COM Ports
            string[] ports = SerialPort.GetPortNames();

            // Add all port names to the combo box:
            foreach (string p in ports)
            {
                this.cboPort.Items.Add(p);
            }
            #endregion
            cboPort.Text = port.ToString();

            cboBaudRate.Items.Add("9600");
            cboBaudRate.Items.Add("19200");
            cboBaudRate.Items.Add("38400");
            cboBaudRate.Items.Add("57600");
            cboBaudRate.Items.Add("115200");
            cboBaudRate.Text = baudRate.ToString();

            cboTimeout.Items.Add("150");
            cboTimeout.Items.Add("300");
            cboTimeout.Items.Add("600");
            cboTimeout.Items.Add("900");
            cboTimeout.Items.Add("1200");
            cboTimeout.Items.Add("1500");
            cboTimeout.Items.Add("1800");
            cboTimeout.Items.Add("2000");
            cboTimeout.Text = timeout.ToString();
        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            if (!EnterNewSettings())
                return;

            Cursor.Current = Cursors.WaitCursor;
            GsmCommMain comm = new GsmCommMain(port, baudRate, timeout);
            try
            {
                comm.Open();
                while (!comm.IsConnected())
                {
                    Cursor.Current = Cursors.Default;
                    if (MessageBox.Show(this, "No phone connected.", "Connection setup",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        comm.Close();
                        return;
                    }
                    Cursor.Current = Cursors.WaitCursor;
                }

                comm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Connection error: " + ex.Message, "Connection setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show(this, "Successfully connected to the phone.", "Connection setup", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }
    }
}

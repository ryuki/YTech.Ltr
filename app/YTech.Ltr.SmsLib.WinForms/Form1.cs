using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using System.Threading;
using org.smslib;
using org.smslib.modem;
using System.Text.RegularExpressions;
using YTech.Ltr.ApplicationServices.Helper;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;
using YTech.Ltr.Enums;

namespace YTech.Ltr.SmsLib.WinForms
{
    public partial class Form1 : Form
    {
        private readonly ITSalesRepository _tSalesRepository;
        private readonly ITSalesDetRepository _tSalesDetRepository;
        private readonly IMGameRepository _mGameRepository;
        private readonly IMAgentRepository _mAgentRepository;
        private readonly ITMsgRepository _tMsgRepository;
        Service srv;
        private Comm2IP.Comm2IP com1;
        private IPModemGateway gateway;

        public Form1(ITSalesRepository tSalesRepository, ITSalesDetRepository tSalesDetRepository, IMGameRepository mGameRepository, IMAgentRepository mAgentRepository, ITMsgRepository tMsgRepository)
        {
            _tSalesRepository = tSalesRepository;
            _tSalesDetRepository = tSalesDetRepository;
            _mGameRepository = mGameRepository;
            _mAgentRepository = mAgentRepository;
            _tMsgRepository = tMsgRepository;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FillPorts();
            //ModemPort = "COM20";

            cboBaudRate.Items.Add("9600");
            cboBaudRate.Items.Add("19200");
            cboBaudRate.Items.Add("38400");
            cboBaudRate.Items.Add("57600");
            cboBaudRate.Items.Add("115200");
            ModemBaudRate = 115200;

            cboTimeout.Items.Add("150");
            cboTimeout.Items.Add("300");
            cboTimeout.Items.Add("600");
            cboTimeout.Items.Add("900");
            cboTimeout.Items.Add("1200");
            cboTimeout.Items.Add("1500");
            cboTimeout.Items.Add("1800");
            cboTimeout.Items.Add("2000");
            ModemTimeOut = 300;

            gbConnect.Enabled = true;
            gbStatus.Enabled = false;

            // Create new Service object - the parent of all and the main interface to you.

            srv = Service.getInstance();

            //set default date to today
            dtSalesDate.Value = DateTime.Today;
            dtSalesDateManual.Value = DateTime.Today;
        }

        #region Display all available COM Ports
        private void FillPorts()
        {
            cboPort.Items.Clear();
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            // Add all port names to the combo box:
            foreach (string p in ports)
            {
                this.cboPort.Items.Add(p);
            }
        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            try
            {
                com1.Stop();
                srv.stopService();
            }
            catch (Exception)
            {

                //throw;
            }

        }

        private void PreparePort()
        {
            Output("Persiapan port untuk membaca sms.");
            Output(Library.getLibraryDescription());
            Output("Version: " + Library.getLibraryVersion());

            // *** The tricky part ***
            // *** Comm2IP Driver ***
            // Create (and start!) as many Comm2IP threads as the modems you are using.
            // Be careful about the mappings - use the same mapping in the Gateway definition.
            com1 = new Comm2IP.Comm2IP(new byte[] { 127, 0, 0, 1 }, 12000, ModemPort, ModemBaudRate);

            // Start the COM listening thread.
            new Thread(new ThreadStart(com1.Run)).Start();

            // Create the Gateway representing the serial GSM modem.
            // Due to the Comm2IP bridge, in SMSLib for .NET all modems are considered IP modems.
            gateway = new IPModemGateway("modem." + ModemPort, "127.0.0.1", 12000, "Wavecom", "M1306B");
            gateway.setIpProtocol(ModemGateway.IPProtocols.BINARY);

            // Set the modem protocol to PDU (alternative is TEXT). PDU is the default, anyway...
            gateway.setProtocol(AGateway.Protocols.PDU);

            // Do we want the Gateway to be used for Inbound messages?
            gateway.setInbound(true);

            // Do we want the Gateway to be used for Outbound messages?
            gateway.setOutbound(true);

            // Add the Gateway to the Service object.
            srv.addGateway(gateway);

            // Start! (i.e. connect to all defined Gateways)
            srv.startService();
        }

        private void ReadSMS()
        {
            InboundMessage[] msgList = srv.readMessages(org.smslib.InboundMessage.MessageClasses.ALL);
            foreach (InboundMessage m in msgList)
            {
                OutputSMS(m);
                //System.Threading.Thread.Sleep(1000);
                //save message
                SaveMessage(m);
            }
        }

        private void SaveMessage(InboundMessage m)
        {
            string pesan = string.Empty;
            bool? isSuccess = null;
            try
            {
                _tMsgRepository.DbContext.BeginTransaction();

                //check sms for mencegah double data
                if (_tMsgRepository.GetMsgLikes(DateTime.Today, m.getText()))
                {
                    throw new Exception("SMS ini sudah pernah diinput.");
                }

                // Received message
                TMsg msg = new TMsg();
                msg.SetAssignedIdTo(Guid.NewGuid().ToString());
                msg.MsgDate = DateTime.Now;// Convert.ToDateTime(m.getDate().toGMTString());
                msg.MsgFrom = m.getOriginator();
                msg.MsgText = m.getText();
                msg.DataStatus = EnumDataStatus.New.ToString();
                msg.CreatedBy = Environment.UserName;
                msg.CreatedDate = DateTime.Now;
                // save both stores, this saves everything else via cascading
                _tMsgRepository.Save(msg);
                isSuccess = true;
                pesan = "\nBerhasil.";
            }
            catch (Exception ex)
            {
                _tMsgRepository.DbContext.RollbackTransaction();
                //throw ex;
                isSuccess = false;
                pesan = "\nGagal.\n" + ex.GetBaseException().Message;
            }
            finally
            {
                if (isSuccess.HasValue)
                {
                    //delete sms
                    bool deleteSuccess = gateway.deleteMessage(m);
                }
            }
        }

        private bool SendMessage(string txt, string sender, string status)
        {
            if (txt.Length >= 140)
            {
                txt = txt.Substring(0, 100) + "...";
            }
            OutboundMessage msg = new OutboundMessage(sender, txt + status);
            return srv.sendMessage(msg);
        }

        private void OutputSMS(InboundMessage m)
        {
            //Output("inbox : " + m);
            Output("Tanggal : \n" + m.getDate());//.toString());
            //Output("getPduUserDataHeader : " + m.getPduUserDataHeader());
            //Output("getDstPort : " + m.getDstPort().ToString());
            //Output("getGatewayId : " + m.getGatewayId());
            //Output("getId : " + m.getId());
            //Output("getMemIndex : " + m.getMemIndex().ToString());
            //Output("getMemLocation : " + m.getMemLocation());
            //Output("getMessageId : " + m.getMessageId());
            //Output("getMpMaxNo : " + m.getMpMaxNo());
            //Output("getMpMemIndex : " + m.getMpMemIndex());
            //Output("getMpRefNo : " + m.getMpRefNo());
            //Output("getMpSeqNo : " + m.getMpSeqNo());
            Output("Pengirim : \n" + m.getOriginator());
            //Output("getSmscNumber : " + m.getSmscNumber());
            //Output("getSrcPort : " + m.getSrcPort());
            Output("Isi SMS : \n" + m.getText());
        }
        private void Output(string text)
        {
            //if (this.txtOutput.InvokeRequired)
            //{
            //    SetTextCallback stc = new SetTextCallback(Output);
            //    this.Invoke(stc, new object[] { text });
            //}
            //else
            {
                txtOutput.AppendText(text);
                txtOutput.AppendText("\r\n");
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            btnRead.Enabled = false;
            btnStop.Enabled = true;
            timerSms.Start();
            timerTrans.Start();
            //btnRead.Enabled = false;
            //txtOutput.Clear();
            //ReadSMS();
            //btnRead.Enabled = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            gbConnect.Enabled = false;
            PreparePort();

            gbStatus.Enabled = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                com1.Stop();
                if (srv.getServiceStatus() == org.smslib.Service.ServiceStatus.STARTED || srv.getServiceStatus() == org.smslib.Service.ServiceStatus.STARTING)
                    srv.stopService();
            }
            catch (Exception)
            {

                //throw;
            }
            gbConnect.Enabled = true;
            gbStatus.Enabled = false;

        }

        private void btnRefreshPort_Click(object sender, EventArgs e)
        {
            FillPorts();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            Output(string.Format("--------- Start {0} ------------", DateTime.Now));
            timerSms.Stop();
            ReadSMS();
            timerSms.Start();
            Output(string.Format("--------- Stop {0} ------------", DateTime.Now));
            btnStop.Enabled = true;
        }


        #region properties
        public string ModemPort
        {
            get
            {
                return cboPort.Text;
            }
            set
            {
                cboPort.Text = value;
            }
        }
        public int ModemBaudRate
        {
            get
            {
                return Convert.ToInt32(cboBaudRate.Text);
            }
            set
            {
                cboBaudRate.Text = value.ToString();
            }
        }
        public int ModemTimeOut
        {
            get
            {
                return Convert.ToInt32(cboTimeout.Text);
            }
            set
            {
                cboTimeout.Text = value.ToString();
            }
        }
        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerSms.Stop();
            timerTrans.Stop();
            btnRead.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _tSalesRepository.DbContext.BeginTransaction();
                //split string
                SaveTransHelper hlp = new SaveTransHelper(_tSalesRepository, _tSalesDetRepository, _mGameRepository, _mAgentRepository, _tMsgRepository);
                hlp.SaveToTrans(null, txtManual.Text, dtSalesDateManual.Value);

                _tSalesRepository.DbContext.CommitTransaction();
                MessageBox.Show("Input manual Berhasil disimpan.");
                txtManual.ResetText();
            }
            catch (Exception ex)
            {
                _tSalesRepository.DbContext.RollbackTransaction();
                MessageBox.Show("Input manual gagal disimpan. Error : " + ex.GetBaseException().Message);
                //throw;
            }

        }

        private void timerTrans_Tick(object sender, EventArgs e)
        {
            IList<TMsg> listMsg = _tMsgRepository.GetMsgNotRead();
            foreach (TMsg msg in listMsg)
            {
                //save trans
                SaveTrans(msg);
            }
        }

        private void SaveTrans(TMsg msg)
        {
            string pesan = string.Empty;
            bool? isSuccess = null;
            //MessageBox.Show(msg.MsgText);
            //MessageBox.Show(msg.MsgFrom);
            try
            {
                _tSalesRepository.DbContext.BeginTransaction();

                //split string
                SaveTransHelper hlp = new SaveTransHelper(_tSalesRepository, _tSalesDetRepository, _mGameRepository, _mAgentRepository, _tMsgRepository);
                hlp.SaveToTrans(msg, msg.MsgText, dtSalesDate.Value);
                
                _tSalesRepository.DbContext.CommitTransaction();
                isSuccess = true;
                pesan = "\nBerhasil.";
            }
            catch (Exception ex)
            {
                _tSalesRepository.DbContext.RollbackTransaction();
                isSuccess = false;
                pesan = "\nGagal.\n" + ex.GetBaseException().Message;
                //throw ex;
            }
            finally
            {
                if (isSuccess.HasValue)
                {
                    _tSalesRepository.DbContext.BeginTransaction();
                    //update msg status
                    msg.MsgStatus = string.Format("Read|{0}", isSuccess.Value.ToString());
                    msg.DataStatus = EnumDataStatus.Updated.ToString();
                    msg.ModifiedBy = Environment.UserName;
                    msg.ModifiedDate = DateTime.Now;
                    _tMsgRepository.Update(msg);
                    _tSalesRepository.DbContext.CommitTransaction();

                    string txt = msg.MsgText;
                    string sender = "+" + msg.MsgFrom;
                    bool replySuccess = SendMessage(txt, sender, pesan);
                }
            }
        }
    }
}

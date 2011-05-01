using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using YTech.Ltr.ApplicationServices.Helper;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;
using YTech.Ltr.Enums;

namespace YTech.Ltr.Sms.WinForms
{
    public partial class FormServerMain : Form
    {
        private readonly ITSalesRepository _tSalesRepository;
        private readonly ITSalesDetRepository _tSalesDetRepository;
        private readonly IMGameRepository _mGameRepository;
        private readonly IMAgentRepository _mAgentRepository;
        private readonly ITMsgRepository _tMsgRepository;

        private delegate void SetTextCallback(string text);
        private CommSetting comm_settings = new CommSetting();

        public FormServerMain(ITSalesRepository tSalesRepository, ITSalesDetRepository tSalesDetRepository, IMGameRepository mGameRepository, IMAgentRepository mAgentRepository, ITMsgRepository tMsgRepository)
        {
            _tSalesRepository = tSalesRepository;
            _tSalesDetRepository = tSalesDetRepository;
            _mGameRepository = mGameRepository;
            _mAgentRepository = mAgentRepository;
            _tMsgRepository = tMsgRepository;
            InitializeComponent();

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        void btnEnd_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Enabled = true;
            btnEnd.Enabled = false;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            InsertToDb();
        }

        private void InsertToDb()
        {
            timer1.Stop();
            GetSmsRead();
            timer1.Start();
        }

        private void GetSmsRead()
        {
            // Cursor.Current = Cursors.WaitCursor;
            string storage = GetMessageStorage();

            try
            {
                // Read all SMS messages from the storage

                DecodedShortMessage[] unreadmessages = CommSetting.comm.ReadMessages(PhoneMessageStatus.ReceivedUnread, storage);
                DecodedShortMessage[] messages = CommSetting.comm.ReadMessages(PhoneMessageStatus.ReceivedRead, storage);

                foreach (DecodedShortMessage message in messages)
                {
                    try
                    {
                        Output(string.Format("Message status = {0}, Location = {1}/{2}",
                                               StatusToString(message.Status), message.Storage, message.Index));
                        ShowMessage(message.Data);
                        Output("");

                        System.Threading.Thread.Sleep(1000);
                        //save message
                        SaveMessage(message.Data);
                    }
                    catch (Exception)
                    {

                    }
                    System.Threading.Thread.Sleep(1000);
                    CommSetting.comm.DeleteMessage(message.Index, storage);

                }
                Output(string.Format("{0,9} messages read.", messages.Length.ToString()));
                Output("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //  Cursor.Current = Cursors.Default;
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnEnd.Enabled = true;
            timer1.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // Clean up comm object
            if (CommSetting.comm != null)
            {
                // Unregister events
                CommSetting.comm.PhoneConnected -= new EventHandler(comm_PhoneConnected);
                CommSetting.comm.MessageReceived -= new MessageReceivedEventHandler(comm_MessageReceived);

                // Close connection to phone
                if (CommSetting.comm != null && CommSetting.comm.IsOpen())
                    CommSetting.comm.Close();

                CommSetting.comm = null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Prompt user for connection settings
            int port = GsmCommMain.DefaultPortNumber;
            int baudRate = 115200; // We Set 9600 as our Default Baud Rate
            int timeout = GsmCommMain.DefaultTimeout;

            FormConnection dlg = new FormConnection();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.SetData(port, baudRate, timeout);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                dlg.GetData(out port, out baudRate, out timeout);
                CommSetting.Comm_Port = port;
                CommSetting.Comm_BaudRate = baudRate;
                CommSetting.Comm_TimeOut = timeout;
            }
            else
            {
                Close();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            CommSetting.comm = new GsmCommMain(port, baudRate, timeout);
            Cursor.Current = Cursors.Default;
            CommSetting.comm.PhoneConnected += new EventHandler(comm_PhoneConnected);
            CommSetting.comm.MessageReceived += new MessageReceivedEventHandler(comm_MessageReceived);
            btnStart.Enabled = true;
            btnEnd.Enabled = false;

            bool retry;
            do
            {
                retry = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    CommSetting.comm.Open();
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception)
                {
                    Cursor.Current = Cursors.Default;
                    if (MessageBox.Show(this, "Unable to open the port.", "Error",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                        retry = true;
                    else
                    {
                        Close();
                        return;
                    }
                }
            }
            while (retry);
        }


        private delegate void ConnectedHandler(bool connected);

        private void OnPhoneConnectionChange(bool connected)
        {
            lbl_phone_status.Text = "CONNECTED";
        }


        private void comm_MessageReceived(object sender, GsmComm.GsmCommunication.MessageReceivedEventArgs e)
        {
            MessageReceived();
        }

        private void comm_PhoneConnected(object sender, EventArgs e)
        {
            this.Invoke(new ConnectedHandler(OnPhoneConnectionChange), new object[] { true });
        }


        private string GetMessageStorage()
        {
            string storage = string.Empty;
            storage = PhoneStorageType.Sim;

            if (storage.Length == 0)
                throw new ApplicationException("Unknown message storage.");
            else
                return storage;
        }


        private void MessageReceived()
        {
            Cursor.Current = Cursors.WaitCursor;
            string storage = GetMessageStorage();

            DecodedShortMessage[] messages = CommSetting.comm.ReadMessages(PhoneMessageStatus.ReceivedUnread, storage);
            foreach (DecodedShortMessage message in messages)
            {
                Output(string.Format("Message status = {0}, Location = {1}/{2}",
                    StatusToString(message.Status), message.Storage, message.Index));
                ShowMessage(message.Data);
                Output("");
            }

            Output(string.Format("{0,9} messages read.", messages.Length.ToString()));
            Output("");
        }


        private string StatusToString(PhoneMessageStatus status)
        {
            // Map a message status to a string
            string ret;
            switch (status)
            {
                case PhoneMessageStatus.All:
                    ret = "All";
                    break;
                case PhoneMessageStatus.ReceivedRead:
                    ret = "Read";
                    break;
                case PhoneMessageStatus.ReceivedUnread:
                    ret = "Unread";
                    break;
                case PhoneMessageStatus.StoredSent:
                    ret = "Sent";
                    break;
                case PhoneMessageStatus.StoredUnsent:
                    ret = "Unsent";
                    break;
                default:
                    ret = "Unknown (" + status.ToString() + ")";
                    break;
            }
            return ret;
        }


        private void Output(string text)
        {
            if (this.txtOutput.InvokeRequired)
            {
                SetTextCallback stc = new SetTextCallback(Output);
                this.Invoke(stc, new object[] { text });
            }
            else
            {
                txtOutput.AppendText(text);
                txtOutput.AppendText("\r\n");
            }
        }


        private void ShowMessage(SmsPdu pdu)
        {
            if (pdu is SmsSubmitPdu)
            {
                // Stored (sent/unsent) message
                SmsSubmitPdu data = (SmsSubmitPdu)pdu;
                Output("SENT/UNSENT MESSAGE");
                Output("Recipient: " + data.DestinationAddress);
                Output("Message text: " + data.UserDataText);
                Output("-------------------------------------------------------------------");
                return;
            }
            if (pdu is SmsDeliverPdu)
            {
                // Received message
                SmsDeliverPdu data = (SmsDeliverPdu)pdu;
                Output("RECEIVED MESSAGE");
                Output("Sender: " + data.OriginatingAddress);
                Output("Sent: " + data.SCTimestamp.ToString());
                Output("Message text: " + data.UserDataText);
                Output("-------------------------------------------------------------------");
                return;
            }
            if (pdu is SmsStatusReportPdu)
            {
                // Status report
                SmsStatusReportPdu data = (SmsStatusReportPdu)pdu;
                Output("STATUS REPORT");
                Output("Recipient: " + data.RecipientAddress);
                Output("Status: " + data.Status.ToString());
                Output("Timestamp: " + data.DischargeTime.ToString());
                Output("Message ref: " + data.MessageReference.ToString());
                Output("-------------------------------------------------------------------");
                return;
            }
            Output("Unknown message type: " + pdu.GetType().ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var productCount = _mGameRepository.GetAll();

            foreach (MGame mGame in productCount)
            {
                MessageBox.Show("Complete, # of products: " + mGame.Id + " name " + mGame.GameName);
            }

            MessageBox.Show("Complete, # of products: " + productCount.Count);
        }

        private void SaveMessage(SmsPdu smsPdu)
        {
            string pesan = string.Empty;
            SmsDeliverPdu data = (SmsDeliverPdu)smsPdu;
            try
            {
                _tMsgRepository.DbContext.BeginTransaction();

                // Received message
                TMsg msg = new TMsg();
                msg.SetAssignedIdTo(Guid.NewGuid().ToString());
                msg.MsgDate = data.SCTimestamp.ToDateTime();
                msg.MsgFrom = data.OriginatingAddress;
                msg.MsgTo = "";
                msg.MsgText = data.UserDataText;

                // save both stores, this saves everything else via cascading
                _tMsgRepository.Save(msg);

                //split string
                SaveToTrans(data.UserDataText);

                _tMsgRepository.DbContext.CommitTransaction();
                pesan = "\nBerhasil.";
            }
            catch (Exception ex)
            {
                _tMsgRepository.DbContext.RollbackTransaction();
                pesan = "\nGagal.\n" + ex.GetBaseException().Message;
                //throw ex;
            }
            if (data.UserDataText.Length >= 150)
            {
                SendMessage(data.OriginatingAddress, data.UserDataText.Substring(0, 150) + pesan);
            }
            else
            {
                SendMessage(data.OriginatingAddress, data.UserDataText + pesan);
            }

            //make delay to send sms

        }

        private void SendMessage(string no, string msg)
        {
            System.Threading.Thread.Sleep(2000);
            SmsSubmitPdu pdu = new SmsSubmitPdu(msg, no, "");
            CommSetting.comm.SendMessage(pdu);
        }

        private IDictionary<string, MGame> GetDictGame()
        {
            IDictionary<string, MGame> dictGame = new Dictionary<string, MGame>();
            IList<MGame> listGame = _mGameRepository.GetAll();
            foreach (MGame game in listGame)
            {
                dictGame.Add(new KeyValuePair<string, MGame>(game.Id, game));
            }
            return dictGame;
        }

        private void SaveToTrans(string msg)
        {
            //many line breaks, every handphone have different string
            string[] separator = new string[] { "\n", "\r", "\r\n", "\n\r" };
            string[] lines = msg.ToUpper().Split(separator, StringSplitOptions.RemoveEmptyEntries);// Regex.Split(msg.ToUpper(), "\n");
            string agentId = string.Empty;
            string salesNo = string.Empty;

            IList<DetailMessage> listDet = new List<DetailMessage>();
            DetailMessage detMsg = null;
            foreach (string line in lines)
            {
                if (line.Contains("A="))
                {
                    agentId = line.Replace("A=", "");
                }
                else if (line.Contains("KE="))
                {
                    salesNo = line.Replace("KE=", "");
                }
                else
                {
                    //search game and value
                    //det[0] = number list
                    //det[1] = value and game (for BB)
                    string[] dets = Regex.Split(line, "=");
                    string det = dets[0];
                    //check if games is BB
                    if (dets[1].Contains("BB"))
                    {
                        decimal? value = decimal.Parse(dets[1].Replace("BB", ""));
                        string[] sep = new string[] { "." };
                        string[] numbers = det.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string num in numbers)
                        {
                            detMsg = new DetailMessage();
                            detMsg.GameId = string.Format("D{0}", num.Length);
                            detMsg.SalesNumber = num;
                            detMsg.SalesValue = value;
                            detMsg.IsBB = true;
                            listDet.Add(detMsg);
                        }
                    }
                    //if not, just do it
                    else
                    {
                        decimal? value = decimal.Parse(dets[1]);
                        //cannot use regex .(dot), it use for other functionality
                        // string[] numbers = Regex.Split(det, ".");

                        string[] sep = new string[] { "." };
                        string[] numbers = det.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string num in numbers)
                        {
                            detMsg = new DetailMessage();
                            //check if numbers contain x string 
                            //(this for game WING)
                            if (num.Contains("X"))
                            {
                                detMsg.GameId = EnumGame.WING.ToString();
                            }
                            //if not, use regular games
                            else
                            {
                                detMsg.GameId = string.Format("D{0}", num.Length);
                            }
                            detMsg.SalesNumber = num;
                            detMsg.SalesValue = value;
                            listDet.Add(detMsg);
                        }
                    }

                }
            }
            //save trans and details
            TSales sales = SaveTrans(agentId, salesNo);
            SaveSalesDets(sales, listDet);
        }

        private TSales SaveTrans(string agentId, string salesNo)
        {
            TSales sales = new TSales();
            sales.SetAssignedIdTo(Guid.NewGuid().ToString());
            sales.SalesDate = DateTime.Today;
            sales.SalesNo = salesNo;
            if (!string.IsNullOrEmpty(agentId))
            {
                sales.AgentId = _mAgentRepository.Get(agentId);
            }

            sales.CreatedDate = DateTime.Now;
            sales.CreatedBy = Environment.UserName;
            sales.DataStatus = EnumDataStatus.New.ToString();
            sales.SalesDets.Clear();
            _tSalesRepository.Save(sales);
            return sales;
        }

        private void SaveDetsForBB(TSales sales, string detNumber, decimal? detValue, MGame gameId, decimal? comm, int leng)
        {
            var result = detNumber.AllPermutations().Where(x => x.Length == leng);
            foreach (var res in result)
            {
                SaveSalesDet(sales, res, detValue, gameId, comm, string.Format("BB : {0}", detNumber));
            }
        }

        private void SaveSalesDets(TSales sales, IList<DetailMessage> listDet)
        {
            var agentComms = (from agentComm in sales.AgentId.AgentComms
                              select agentComm).ToList();
            decimal? comm = null;

            IDictionary<string, MGame> dictGame = GetDictGame();
            MGame game = null;
            foreach (DetailMessage detailMessage in listDet)
            {
                dictGame.TryGetValue(detailMessage.GameId, out game);
                if (agentComms.Count > 0)
                {
                    comm = (from agentComm in agentComms
                            where agentComm.GameId == game
                            select agentComm.CommValue).First();
                }
                if (detailMessage.IsBB)
                {
                    SaveDetsForBB(sales, detailMessage.SalesNumber, detailMessage.SalesValue, game, comm, detailMessage.SalesNumber.Length);
                }
                else
                {
                    SaveSalesDet(sales, detailMessage.SalesNumber, detailMessage.SalesValue, game, comm, null);
                }

            }
        }

        private void SaveSalesDet(TSales sales, string detNumber, decimal? detValue, MGame gameId, decimal? comm, string desc)
        {
            TSalesDet det = new TSalesDet(sales);
            det.SetAssignedIdTo(Guid.NewGuid().ToString());
            det.SalesDetNumber = detNumber;
            det.SalesDetValue = detValue;
            det.GameId = gameId;
            det.SalesDetComm = comm;
            det.SalesDetDesc = desc;
            det.CreatedDate = DateTime.Now;
            det.CreatedBy = Environment.UserName;
            det.DataStatus = EnumDataStatus.New.ToString();
            _tSalesDetRepository.Save(det);
        }
    }

    public class DetailMessage
    {
        public string GameId { get; set; }
        public string SalesNumber { get; set; }
        public decimal? SalesValue { get; set; }
        public bool IsBB { get; set; }
    }
}

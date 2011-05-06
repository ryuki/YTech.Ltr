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
    public class SaveTransHelper
    {
        private readonly ITSalesRepository _tSalesRepository;
        private readonly ITSalesDetRepository _tSalesDetRepository;
        private readonly IMGameRepository _mGameRepository;
        private readonly IMAgentRepository _mAgentRepository;
        private readonly ITMsgRepository _tMsgRepository;
        public SaveTransHelper(ITSalesRepository tSalesRepository, ITSalesDetRepository tSalesDetRepository, IMGameRepository mGameRepository, IMAgentRepository mAgentRepository, ITMsgRepository tMsgRepository)
        {
            _tSalesRepository = tSalesRepository;
            _tSalesDetRepository = tSalesDetRepository;
            _mGameRepository = mGameRepository;
            _mAgentRepository = mAgentRepository;
            _tMsgRepository = tMsgRepository;

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

        public void SaveToTrans(string msg)
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
                        string[] sep = new string[] { ".", "," };
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

                        string[] sep = new string[] { ".", "," };
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
}

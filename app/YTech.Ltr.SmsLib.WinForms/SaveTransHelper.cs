using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using YTech.Ltr.ApplicationServices.Helper;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;
using YTech.Ltr.Enums;

namespace YTech.Ltr.SmsLib.WinForms
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

        public void SaveToTrans(TMsg m, string msg, DateTime salesDate)
        {
            //many line breaks, every handphone have different string
            string[] separator = new string[] { "\n", "\r", "\r\n", "\n\r" };
            string[] lines = msg.Replace(" ", "").ToUpper().Split(separator, StringSplitOptions.RemoveEmptyEntries);// Regex.Split(msg.ToUpper(), "\n");
            string agentId = string.Empty;
            string salesNo = string.Empty;

            IList<DetailMessage> listDet = new List<DetailMessage>();
            DetailMessage detMsg = null;
            decimal factor = 1;
            bool isHBR = false;
            
            //set TH as default game
            bool isTH = true;
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
                //if message contain HRB string, the game is HBR
                else if (line.Contains("HBR"))
                {
                    isHBR = true;
                    isTH = false;
                }
                ////if message contain TH string, the game is TH
                //else if (line.Contains("TH"))
                //{
                //    isTH = true;
                //}
                else
                {
                    //search game and value
                    //det[0] = number list
                    //det[1] = value and game (for BB)
                    string[] dets = Regex.Split(line, "=");
                    string det = dets[0].Trim();
                    //check if games is BB
                    if (dets[1].Contains("BB"))
                    {
                        decimal value = 0;
                        //replace comma with dot to identify decimal number, ex : 0,5
                        if (!decimal.TryParse(dets[1].Replace("BB", "").Replace(",","."), out value))
                        {
                            throw new Exception("Format angka salah!!!");
                        }

                        string[] sep = new string[] { "." };
                        string[] numbers = det.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string num in numbers)
                        {
                            detMsg = new DetailMessage();
                            
                            if (isTH && num.Length == 4)
                            {
                                detMsg.GameId = EnumGame.D4TH.ToString();
                            }
                            else
                            {
                               detMsg.GameId = string.Format("D{0}", num.Length); 
                            }
                            detMsg.SalesNumber = num;
                            detMsg.SalesValue = value * factor;
                            detMsg.IsBB = true;
                            detMsg.IsHBR = false;
                            //HBR with factor = 2 is for only game D4 only
                            if (isHBR && num.Length == 4)
                            {
                                detMsg.SalesValue = value * 2;
                                detMsg.IsHBR = true;
                            }
                            listDet.Add(detMsg);
                        }
                    }
                    //if not, just do it
                    else
                    {
                        decimal value = 0;
                        if (!decimal.TryParse(dets[1].Replace(",", "."), out value))
                        {
                            throw new Exception("Format angka salah!!!");
                        }

                        //cannot use regex .(dot), it use for other functionality
                        // string[] numbers = Regex.Split(det, ".");

                        string[] sep = new[] { "." };
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
                            else if (num.Contains("/"))
                            {
                                detMsg.GameId = EnumGame.PAKET.ToString();
                            }
                            //if not, use regular games
                            else
                            {
                               
                                if (isTH && num.Length == 4)
                                {
                                    detMsg.GameId = EnumGame.D4TH.ToString();
                                }
                                else
                                {
                                     detMsg.GameId = string.Format("D{0}", num.Length);
                                }
                            }
                            detMsg.SalesNumber = num;
                            detMsg.SalesValue = value * factor;
                            detMsg.IsHBR = false;

                            //HBR with factor = 2 is for only game D4 only
                            if (isHBR && num.Length == 4)
                            {
                                detMsg.SalesValue = value * 2;
                                detMsg.IsHBR = true;
                            }
                            listDet.Add(detMsg);
                        }
                    }

                }
            }
            //save trans and details
            TSales sales = SaveTrans(m, agentId, salesNo, salesDate);
            SaveSalesDets(sales, listDet);
        }

        private TSales SaveTrans(TMsg m, string agentId, string salesNo, DateTime salesDate)
        {
            TSales sales = new TSales();
            sales.SetAssignedIdTo(Guid.NewGuid().ToString());
            sales.SalesDate = salesDate;
            sales.SalesNo = salesNo;
            if (!string.IsNullOrEmpty(agentId))
            {
                MAgent agent = _mAgentRepository.Get(agentId);
                if (agent == null)
                {
                    throw new Exception("Kode Agen salah atau tidak terdaftar");
                }
                sales.AgentId = agent;
            }
            else
            {
                throw new Exception("Kode Agen kosong.");
            }
            sales.MsgId = m;

            sales.CreatedDate = DateTime.Now;
            sales.CreatedBy = Environment.UserName;
            sales.DataStatus = EnumDataStatus.New.ToString();
            sales.SalesDets.Clear();
            _tSalesRepository.Save(sales);
            return sales;
        }

        private void SaveDetsForBB(TSales sales, string detNumber, decimal? detValue, MGame gameId, decimal? comm, int leng, string desc)
        {
            var result = detNumber.AllPermutations().Where(x => x.Length == leng);
            foreach (var res in result)
            {
                SaveSalesDet(sales, res, detValue, gameId, comm, string.Format("{0} BB : {1}", desc, detNumber));
            }
        }

        private void SaveSalesDets(TSales sales, IList<DetailMessage> listDet)
        {
            var agentComms = (from agentComm in sales.AgentId.AgentComms
                              select agentComm).ToList();
            decimal? comm = null;

            IDictionary<string, MGame> dictGame = GetDictGame();
            MGame game = null;
            string desc = string.Empty;
            foreach (DetailMessage detailMessage in listDet)
            {
                dictGame.TryGetValue(detailMessage.GameId, out game);
                if (detailMessage.IsHBR)
                {
                    desc = string.Format("HBR : {0}", detailMessage.SalesNumber);
                }
                if (agentComms.Count > 0)
                {
                    var getComms = (from agentComm in agentComms
                                    where agentComm.GameId == game
                                    select agentComm.CommValue);
                    if (getComms.Count() > 0)
                    {
                        comm = getComms.First();
                    }
                }
                if (detailMessage.IsBB)
                {
                    SaveDetsForBB(sales, detailMessage.SalesNumber, detailMessage.SalesValue, game, comm, detailMessage.SalesNumber.Length, desc);
                }
                else
                {
                    SaveSalesDet(sales, detailMessage.SalesNumber, detailMessage.SalesValue, game, comm, desc);
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

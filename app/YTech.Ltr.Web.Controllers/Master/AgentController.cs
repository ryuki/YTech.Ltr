using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Data.Repository;
using YTech.Ltr.Enums;
using YTech.Ltr.Web.Controllers.ViewModel;

namespace YTech.Ltr.Web.Controllers.Master
{
    [HandleError]
    public class AgentController : Controller
    {
        private readonly IMAgentRepository _mAgentRepository;
        private readonly IMAgentCommRepository _mAgentCommRepository;
        private readonly IMGameRepository _mGameRepository;
        public AgentController(IMAgentRepository mAgentRepository, IMAgentCommRepository mAgentCommRepository, IMGameRepository mGameRepository)
        {
            Check.Require(mAgentRepository != null, "mAgentRepository may not be null");
            Check.Require(mAgentCommRepository != null, "mAgentCommRepository may not be null");
            Check.Require(mGameRepository != null, "mGameRepository may not be null");

            this._mAgentRepository = mAgentRepository;
            this._mAgentCommRepository = mAgentCommRepository;
            this._mGameRepository = mGameRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var Agents = _mAgentRepository.GetPagedAgentList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from Agent in Agents
                    select new
                    {
                        i = Agent.Id,
                        cell = new string[] {
                            string.Empty,
                            Agent.Id, 
                            Agent.AgentName, 
                          Agent.AgentDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MAgent viewModel, FormCollection formCollection)
        {

            MAgent mItemCatToInsert = new MAgent();
            TransferFormValuesTo(mItemCatToInsert, viewModel);
            mItemCatToInsert.SetAssignedIdTo(viewModel.Id);
            mItemCatToInsert.CreatedDate = DateTime.Now;
            mItemCatToInsert.CreatedBy = User.Identity.Name;
            mItemCatToInsert.DataStatus = EnumDataStatus.New.ToString();
            _mAgentRepository.Save(mItemCatToInsert);

            try
            {
                _mAgentRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mAgentRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MAgent viewModel, FormCollection formCollection)
        {
            MAgent mItemCatToDelete = _mAgentRepository.Get(viewModel.Id);

            if (mItemCatToDelete != null)
            {
                _mAgentRepository.Delete(mItemCatToDelete);
            }

            try
            {
                _mAgentRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mAgentRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MAgent viewModel, FormCollection formCollection)
        {
            MAgent mItemCatToUpdate = _mAgentRepository.Get(viewModel.Id);
            TransferFormValuesTo(mItemCatToUpdate, viewModel);
            mItemCatToUpdate.ModifiedDate = DateTime.Now;
            mItemCatToUpdate.ModifiedBy = User.Identity.Name;
            mItemCatToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            _mAgentRepository.Update(mItemCatToUpdate);

            try
            {
                _mAgentRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mAgentRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MAgent mItemCatToUpdate, MAgent mItemCatFromForm)
        {
            mItemCatToUpdate.AgentName = mItemCatFromForm.AgentName;
            mItemCatToUpdate.AgentDesc = mItemCatFromForm.AgentDesc;
        }

        [Transaction]
        public ActionResult Commission(string agentId)
        {
            AgentCommViewModel viewModel = AgentCommViewModel.Create(_mGameRepository, agentId);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Commission(AgentCommViewModel viewModel, FormCollection formCollection, string agentId)
        {
            _mAgentCommRepository.DbContext.BeginTransaction();
            //delete agent commission first
            _mAgentCommRepository.DeleteByAgent(agentId);
            IList<MGame> listGame = _mGameRepository.GetAll();
            MGame game = null;
            MAgentComm comm = null;
            MAgent agent = _mAgentRepository.Get(agentId);
            string gameComm;
            for (int i = 0; i < listGame.Count; i++)
            {
                game = listGame[i];
                comm = new MAgentComm(agent);
                comm.SetAssignedIdTo(Guid.NewGuid().ToString());

                gameComm = formCollection[string.Format("txtComm_{0}", game.Id)];
                //set comm value if comm value is not empty
                if (!string.IsNullOrEmpty(gameComm))
                {
                    comm.CommValue = decimal.Parse(gameComm);
                }
                comm.GameId = game;
                comm.DataStatus = EnumDataStatus.New.ToString();
                comm.CreatedBy = User.Identity.Name;
                comm.CreatedDate = DateTime.Now;
                _mAgentCommRepository.Save(comm);
            }

            bool Success = true;
            string Message = string.Empty;
            try
            {
                _mAgentCommRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                Success = true;
                Message = "Komisi Agen berhasil disimpan.";
            }
            catch (Exception ex)
            {
                _mAgentCommRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
                Success = false;
                Message = ex.GetBaseException().Message;
            }

            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult ListAgentCommForSubGrid(string id)
        {
            var agentComms = _mAgentCommRepository.GetByAgentId(id);

            var jsonData = new
            {
                rows = (
                    from comm in agentComms
                    select new
                    {
                        i = comm.Id.ToString(),
                        cell = new string[] {
                           //itemCat.Id, 
                           //itemCat.PacketId != null ? itemCat.PacketId.Id : null, 
                           comm.GameId != null ? comm.GameId.GameName : null,
                            comm.CommValue.HasValue ?  comm.CommValue.Value.ToString(Helper.CommonHelper.NumberFormat) : null
                        }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}

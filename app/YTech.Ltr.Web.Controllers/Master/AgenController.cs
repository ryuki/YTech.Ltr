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
        public AgentController(IMAgentRepository mAgentRepository)
        {
            Check.Require(mAgentRepository != null, "mAgentRepository may not be null");

            this._mAgentRepository = mAgentRepository;
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
    }
}

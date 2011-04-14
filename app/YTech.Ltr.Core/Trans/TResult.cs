using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Core.Trans
{
    public class TResult : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public TResult()
        {
            InitMembers();
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers()
        {
            ResultDets = new List<TResultDet>();
        }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual DateTime? ResultDate { get; set; }
        public virtual string ResultStatus { get; set; }
        public virtual string ResultDesc { get; set; }

        public virtual IList<TResultDet> ResultDets { get; protected set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}

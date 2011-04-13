using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Core.Trans
{
    public class TSalesDet : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual TSales SalesId { get; set; }
        public virtual MGame GameId { get; set; }
        public virtual string SalesDetNumber { get; set; }
        public virtual decimal? SalesDetValue { get; set; }
        public virtual decimal? SalesDetComm { get; set; }
        public virtual string SalesDetStatus { get; set; }
        public virtual string SalesDetDesc { get; set; }

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

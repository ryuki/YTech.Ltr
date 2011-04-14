using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.Trans;
using YTech.Ltr.Enums;
using YTech.Ltr.Data.Repository;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core;

namespace YTech.Ltr.Web.Controllers.Helper
{
    public class CommonHelper
    {
        public static string DateFormat
        {
            get { return "dd-MMM-yyyy"; }
        }
        public static string DateTimeFormat
        {
            get { return "dd-MMM-yyyy HH:mm"; }
        }
        public static string TimeFormat
        {
            get { return "HH:mm"; }
        }
        public static string NumberFormat
        {
            get { return "N2"; }
        }

        /// <summary>
        /// get list of enum for jqgrid combobox
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="defaultText">default text for display</param>
        /// <returns>string</returns>
        public static string GetEnumListForGrid<T>(string defaultText)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Type object must enum");
            }
            var lists = from T e in Enum.GetValues(typeof(T))
                        select new { ID = e, Name = e.ToString() };
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1};", string.Empty, defaultText);

            for (int i = 0; i < lists.Count(); i++)
            {
                var obj = lists.ElementAt(i);
                sb.AppendFormat("{0}:{1}", obj.ID, obj.Name);
                if (i < lists.Count() - 1)
                    sb.Append(";");
            }
            return (sb.ToString());
        }
    }
}

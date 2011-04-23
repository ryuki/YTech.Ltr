using System;
using System.Collections.Generic;
using System.Linq;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Web.Controllers.Helper
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> list)
        {
            if (list.Count() == 1)
                return new List<IEnumerable<T>> { list };

            return list.Select((a, i1) => Permute(list.Where((b, i2) => i2 != i1)).Select(b => (new List<T> { a }).Union(b)))
                       .SelectMany(c => c);
        }

        public static IEnumerable<string> AllPermutations(this IEnumerable<char> s)
        {
            return s.SelectMany(x =>
            {
                var index = Array.IndexOf(s.ToArray(), x);
                return s.Where((y, i) => i != index).AllPermutations()
                        .Select(y => new string(new[] { x }.Concat(y).ToArray()))
                        .Union(new[] { new string(new[] { x }) });
            }).Distinct();
        }


        //public static IEnumerable<T> Traverse(IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        //{
        //    foreach (T item in source)
        //    {
        //        yield return item;

        //        IEnumerable<T> seqRecurse = fnRecurse(item);

        //        if (seqRecurse != null)
        //        {
        //            foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
        //            {
        //                yield return itemRecurse;
        //            }
        //        }
        //    }
        //}
    }
}

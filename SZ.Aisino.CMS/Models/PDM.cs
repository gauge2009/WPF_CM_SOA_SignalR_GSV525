using System.Collections.Generic;
using JR.Common;
 
namespace SZ.Aisino.CMS.Models
{
    public class PDM<T> where T : class
    {

        public T Data
        {
            get;
            set;
        }

        public Pager Pager
        {
            get;
            set;
        }

        private List<Pager> pagers = new List<Pager>();
        public List<Pager> Pagers
        {
            get
            {
                return this.pagers;
            }
        }
    }

    public class PDM<TData, TSearch> : PDM<TData>
        where TData : class
        where TSearch : class
    {

        public TSearch Condition
        {
            get;
            set;
        }

    }

    public static class PDM
    {
        public static PDM<T> Create<T>(T data, Pager pager = null) where T : class
        {
            return new PDM<T>()
            {
                Data = data,
                Pager = pager ?? new Pager()
            };
        }

        public static PDM<TData, TCondition> Create<TData, TCondition>(TData data, TCondition search, Pager pager = null)
            where TData : class
            where TCondition : class
        {

            return new PDM<TData, TCondition>()
            {
                Data = data,
                Condition = search,
                Pager = pager ?? new Pager()
            };
        }
    }
}
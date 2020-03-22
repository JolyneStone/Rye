using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx.Filter
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T>
    {
        public PageData() : this(new List<T>(), 0)
        { 
        }

        public PageData(List<T> data, int total)
        {
            Data = data;
            Total = total;
        }

        public List<T> Data { get; set; }

        public int Total { get; set; }


        /// <summary>
        /// 转换为指定类型的分页数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public PageData<TResult> ToPageResult<TResult>(Func<T, TResult> func)
        {
            var d = new List<TResult>();
            if(Data!=null)
            {
                foreach(var item in Data)
                {
                    d.Add(func(item));
                }
            }
            return new PageData<TResult>(d, Total);
        }
    }
}

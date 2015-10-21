using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class ItemSet<T>
    {
        /// <summary>
        /// 项
        /// </summary>
        public List<T> Items
        {
            get
            {
                return items;
            }
        }
        private List<T> items;

        public ItemSet()
        {
            items = new List<T>();
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="i">待添加的项</param>
        public void AddItem(T i)
        {
            try
            {
                items.Add(i);
            }
            catch
            {
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class Transaction<T> where T :IComparable<T>
    {
        /// <summary>
        /// 项集
        /// </summary>
        public T[] Items
        {
            get
            {
                return items;
            }
        }
        private T[] items;

        public Transaction(T[] i)
        {
            items = i;
        }
    }
}

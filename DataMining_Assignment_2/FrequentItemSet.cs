using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class FrequentItemSet<T>
    {
        /// <summary>
        /// 频繁项中的数量(frequent k-itemset)
        /// </summary>
        public int ItemCount
        {
            get
            {
                return itemCount;
            }
        }
        private int itemCount;

        private LinkedList<ItemSet<T>> itemSets;

       
        public FrequentItemSet(int k)
        {
            itemCount = k;
        }


    }
}

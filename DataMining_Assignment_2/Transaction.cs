using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class Transaction<T>
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

        /*
        private T[] itemName;
        private int itemCount;

        public Transaction()
        {
            itemSets = new LinkedList<ItemSet<T>>();
        }
        
        /// <summary>
        /// 设置item名称及数量
        /// </summary>
        /// <param name="n">每个item set中的item数</param>
        /// <param name="names">item的名字</param>
        public void SetItemName(int n, T[] names)
        {
            itemCount = n;
            itemName = names;
        }
        
        /// <summary>
        /// 添加一个item
        /// </summary>
        /// <param name="value">添加的item</param>
        public void AddItem(T item)
        {
            
            ItemSet<T> newItemSet = new ItemSet<T>();
            for (int i = 0; i < itemCount; i++)
            {
                if (value[i] == 1) newItemSet.AddItem(itemName[i]);
            }
            itemSets.AddLast(newItemSet);
            
            items.AddLast(item);
        }
        */
    }
}

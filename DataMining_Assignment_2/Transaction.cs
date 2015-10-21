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
        public LinkedList<ItemSet<T>> ItemSets
        {
            get
            {
                return itemSets;
            }
        }
        private LinkedList<ItemSet<T>> itemSets;

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
        /// 添加一个item set
        /// </summary>
        /// <param name="value">item set的值，0表示不存在对应的item，1表示存在</param>
        public void AddItemSet(int[] value)
        {
            ItemSet<T> newItemSet = new ItemSet<T>();
            for (int i = 0; i < itemCount; i++)
            {
                if (value[i] == 1) newItemSet.AddItem(itemName[i]);
            }
            itemSets.AddLast(newItemSet);
        }

    }
}

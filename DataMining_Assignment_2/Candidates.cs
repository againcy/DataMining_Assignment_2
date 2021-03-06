﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class Candidates<T> where T: System.IComparable<T>
    {
        /// <summary>
        /// itemset类型，data为项集，value为support counting
        /// </summary>
        public struct DataType
        {
            public T[] data;
            public int value;
        }
        /// <summary>
        /// 候选项集
        /// </summary>
        public LinkedList<DataType> ItemSets
        {
            get
            {
                return itemSets;
            }
        }
        private LinkedList<DataType> itemSets;
    
        public Candidates()
        {
            itemSets = new LinkedList<DataType>();
            //itemCount = k;
        }

        /// <summary>
        /// 添加一个候选项
        /// </summary>
        /// <param name="newItemSet">候选项</param>
        /// <param name="value">候选项的support counting</param>
        public void AddCandidate(T[] newItemSet, int value)
        {
            DataType newData = new DataType();
            newData.data = newItemSet;
            newData.value = value;
            itemSets.AddLast(newData);
        }

        /// <summary>
        /// 添加一个候选项
        /// </summary>
        /// <param name="d">包含候选项集和support counting的元素</param>
        public void AddCandidate(DataType d)
        {
            itemSets.AddLast(d);
        }

        /// <summary>
        /// 根据Ck生成Ck+1候选集
        /// </summary>
        /// <param name="ck">拥有项个数为k的项集的候选集</param>
        public LinkedList<T[]> GenerateNewCandidates()
        {
            LinkedList<T[]> newCandidates = new LinkedList<T[]>();
            for(var head = this.itemSets.First;head!=null;head=head.Next)
            {
                for (var tail = head.Next; tail != null; tail = tail.Next)
                {
                    int count = head.Value.data.Count();
                    int k = 0;
                    while (k < count && object.Equals(head.Value.data[k], tail.Value.data[k]))
                    {
                        k++;
                    }
                    if (k == count - 1)
                    {
                        //head和tail前k个元素相同，最后一个元素不同，将两者合并成新的候选项
                        T[] newSet = new T[count + 1];
                        int j;
                        for (j = 0; j < k; j++) newSet[j] = head.Value.data[j];
                        newSet[j] = head.Value.data[k];
                        newSet[j + 1] = tail.Value.data[k];
                        newCandidates.AddLast(newSet);
                    }
                }
            }
            return newCandidates;
        }

        /// <summary>
        /// 判断两个数组是否相等
        /// </summary>
        /// <param name="a1">比较数组1</param>
        /// <param name="a2">比较数组2</param>
        /// <returns>true 相等; false 不等</returns>
        public bool ArrayEqual(T[] a1,T[] a2)
        {
            if (a1 == null || a2 == null) return false;
            if (a1.Count() != a2.Count()) return false;
            for(int i =0;i<a1.Count();i++)
            {
                if (object.Equals(a1[i],a2[i])==false) return false;
            }
            return true;
        }

        /// <summary>
        /// 剪枝，对于每一个候选的项集，若其所有k项子集有未在ck中出现的，则去掉
        /// </summary>
        /// <param name="downwards">ck的所有项集</param>
        public void Pruning(LinkedList<DataType> checkList)
        {
            //List<T[]> checkList = new List<T[]>();
            //checkList = downwards.ToList();

            int count = this.itemSets.First.Value.data.Count();
            LinkedList<DataType> newItemSets = new LinkedList<DataType>();
            foreach (var tmp in itemSets)
            {
                bool isAvailable=true;
                //枚举每一个itemset的k项子集
                foreach(var deleteItem in tmp.data)
                {
                    //因为所有的tmp均为k+1项，所以通过枚举不在k项子集中的item生成k项子集
                    T[] newSet = new T[count - 1];
                    int i = 0;
                    foreach (var item in tmp.data)
                    {
                        if (object.Equals(item, deleteItem) == false)
                        {
                            newSet[i] = item;
                            i++;
                        }
                    }
                    //判断子集是否在checklist中
                    isAvailable = false;
                    foreach(var d in checkList)
                    {
                        if (ArrayEqual(d.data, newSet))
                        {
                            isAvailable = true;//子集在checklist中
                            break;
                        }
                    }
                    if (isAvailable==false)
                    {
                        //有子集不在checklist中则跳出
                        break;
                    }
                }
                //若所有子集都在checklist中，则加入新生成的ItemSets
                if (isAvailable==true)
                {
                    newItemSets.AddLast(tmp);
                }
            }
            itemSets = newItemSets;
        }

        
    }
}

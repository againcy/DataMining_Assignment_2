using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class TrieNode<T> where T:System.IComparable<T>
    {
        public LinkedList<TrieNode<T>> children;
        /// <summary>
        /// 节点值
        /// </summary>
        public T data;
        /// <summary>
        /// 节点的support counting
        /// </summary>
        public int v;
        /// <summary>
        /// 判断节点是否到达过
        /// </summary>
        public bool tagArrival;

        public TrieNode()
        {
            children = null;
            v = -1;
            tagArrival = false;
        }
    }
    class TrieTree<T> where T : System.IComparable<T>
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public TrieNode<T> Root
        {
            get
            {
                return root;
            }
        }
        private TrieNode<T> root;

        public TrieTree()
        {
            root = new TrieNode<T>();
        }

        /// <summary>
        /// 添加新的节点
        /// </summary>
        /// <param name="newData">新节点的值</param>
        /// <param name="value">新节点的support counting</param>
        public void AddNode(T[] newData, int value)
        {
            TrieNode<T> cur=root;
            int depth = 0;
            while (depth<newData.Count())
            {
                if (cur.children == null)
                {
                    cur.children = new LinkedList<TrieNode<T>>();
                    TrieNode<T> newNode = new TrieNode<T>();
                    newNode.data = newData[depth];
                    cur.children.AddFirst(newNode);
                    cur = newNode;
                }
                else
                {
                    LinkedListNode<TrieNode<T>> node = cur.children.First;
                    LinkedListNode<TrieNode<T>> prev = null;
                    while (node != null && node.Value.data.CompareTo(newData[depth]) < 0)
                    {
                        prev = node;
                        node = node.Next;
                    }
                    if (node != null && node.Value.data.CompareTo(newData[depth]) == 0)
                    {
                        //到当前深度路径存在
                        cur = node.Value;
                    }
                    else
                    {
                        TrieNode<T> newNode = new TrieNode<T>();
                        newNode.data = newData[depth];
                        if (prev != null)
                        {
                            cur.children.AddAfter(prev, newNode);
                        }
                        else
                        {
                            cur.children.AddBefore(node, newNode);
                        }
                        cur = newNode;
                    }
                }
                depth++;
            }
            cur.v = value;
        }

        /// <summary>
        /// 清楚所有的到达标记
        /// </summary>
        /// <param name="cur">父节点</param>
        public void ClearTag(TrieNode<T> cur)
        {
            if (cur != null)
            {
                cur.tagArrival = false;
                if (cur.children != null)
                {
                    foreach (var node in cur.children)
                    {
                        ClearTag(node);
                    }
                }
            }
        }

        /// <summary>
        /// 获取下一个字典中的词
        /// </summary>
        /// <param name="cur"></param>
        /// <returns></returns>
        public string GetNodeData(TrieNode<T> cur)
        {
            if (cur != null)
            {
                if (cur.tagArrival == false && cur.v != -1)
                {
                    cur.tagArrival = true;
                    return cur.data.ToString()+" "+cur.v.ToString();
                }
                else
                {
                    if (cur.children != null)
                    {
                        LinkedListNode<TrieNode<T>> node = cur.children.First;
                        while (node != null)
                        {
                            string str = GetNodeData(node.Value);
                            if (str != "") return cur.data.ToString() + " " + str;
                            node = node.Next;
                        }
                    }
                    return "";
                }
            }
            else return "";
        }
    }
}

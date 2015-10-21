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

        public TrieNode()
        {
            children = null;
            v = -1;
        }
    }
    class TrieTree<T> where T : System.IComparable<T>
    {
        public delegate void NodePrintFunction(string str);

        private NodePrintFunction NodePrint;
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

        public TrieTree(NodePrintFunction n)
        {
            root = new TrieNode<T>();
            NodePrint = n;
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
                        cur.children.AddAfter(prev, newNode);
                        cur = newNode;
                    }
                }
                depth++;
            }
            cur.v = value;
        }

        /// <summary>
        /// 先序遍历并输出
        /// </summary>
        /// <param name="cur"></param>
        public void Output(TrieNode<T> cur, string outputStr)
        {
            outputStr += cur.data.ToString()+" ";
            if (cur.v != -1) NodePrint(outputStr + ":"+cur.v.ToString()+"\n");
            if (cur.children != null)
            {
                LinkedListNode<TrieNode<T>> node = cur.children.First;
                while (node != null)
                {
                    Output(node.Value, outputStr);
                    node = node.Next;
                }
            }
        }
    }
}

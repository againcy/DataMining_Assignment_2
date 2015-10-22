using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    /// <summary>
    /// 二叉树节点
    /// </summary>
    /// <typeparam name="T">元素的类型</typeparam>
    class TreeNode<T> where T :IComparable<T>
    {
        public TreeNode(int l)
        {
            left = null;
            right = null;
            list = new LinkedList<Candidates<T>.DataType>();
            level = l;
        }
        public TreeNode<T> left;
        public TreeNode<T> right;
        /// <summary>
        /// 节点中的数据，value为其support值
        /// </summary>
        public LinkedList<Candidates<T>.DataType> list;
        public int level;//层数
        public bool tagArrival;//判断当前叶节点是否到达过
    }

    /// <summary>
    /// Hash树
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    class HashTree<T> where T : IComparable<T>
    {
        public delegate int HashFunction(T[] data, int level);//外部传入的hash函数
        public delegate string NodePrintFunction(T[] nodeData);//外部传入的打印节点的函数
        /// <summary>
        /// hash函数
        /// </summary>
        private HashFunction HashFunc;
        /// <summary>
        /// 用于打印节点
        /// </summary>
        private NodePrintFunction PrintFunc;
        /// <summary>
        /// hash值分界点，小于等于该值放左子树，大于该值放右子树
        /// </summary>
        private int seperation;
        /// <summary>
        /// 一个节点下最多存放几个值
        /// </summary>
        private int threshold;

        /// <summary>
        /// 树最深的层数
        /// </summary>
        private int bottom;

        public TreeNode<T> Root
        {
            get
            {
                return root;
            }
        }
        private TreeNode<T> root;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="h">hash函数</param>
        /// <param name="sep">hash值分界点，小于等于该值放左子树，大于该值放右子树</param>
        /// <param name="thres">一个节点下最多存放的值的个数</param>
        /// <param name="b">树最深的层数</param>
        /// <param name="n">用于打印节点的函数</param>
        public HashTree(HashFunction h, int sep, int thres, int b,NodePrintFunction n)
        {
            HashFunc = h;
            PrintFunc = n;
            seperation = sep;
            threshold = thres;
            bottom = b;
            root = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="h">hash函数</param>
        /// <param name="sep">hash值分界点，小于等于该值放左子树，大于该值放右子树</param>
        /// <param name="thres">一个节点下最多存放的值的个数</param>
        /// <param name="b">树最深的层数</param>
        public HashTree(HashFunction h, int sep, int thres, int b)
        {
            HashFunc = h;
            seperation = sep;
            threshold = thres;
            bottom = b;
            root = null;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="nodeData">加入的数据</param>
        public void AddNode(Candidates<T>.DataType nodeData)
        {
            if (root==null)
            {
                root = new TreeNode<T>(0);
                root.list.AddLast(nodeData);
            }
            else
            {
                TreeNode<T> cur = root;
                TreeNode<T> parent = null;
                int direction = -1;
                while (cur!=null && (cur.left!=null || cur.right!=null))
                {
                    //cur不是叶子节点
                    parent = cur;
                    int h = HashFunc(nodeData.data, cur.level);
                    if (h <= seperation)
                    {
                        cur = cur.left;
                        direction = -1;
                    }
                    else
                    {
                        cur = cur.right;
                        direction = 1;
                    }
                }
                //找到所属的叶子节点
                if (cur==null)
                {
                    //cur为新的节点，且一定不为根
                    cur = new TreeNode<T>(parent.level + 1);
                    cur.list.AddLast(nodeData);
                    if (direction == -1) parent.left = cur;
                    else parent.right = cur;
                }
                else
                {
                    cur.list.AddLast(nodeData);
                    if (cur.list.Count() > threshold && cur.level < bottom)
                    {
                        //需要扩展当前节点
                        while ((cur.left == null || cur.right == null) && cur.level<bottom)
                        {
                            TreeNode<T> leftNode = new TreeNode<T>(cur.level + 1);
                            TreeNode<T> rightNode = new TreeNode<T>(cur.level + 1);
                            foreach (var v in cur.list)
                            {
                                if (HashFunc(v.data, cur.level) <= seperation) leftNode.list.AddLast(v);
                                else rightNode.list.AddLast(v);
                            }
                            if (leftNode.list.Count() > 0) cur.left = leftNode;
                            if (rightNode.list.Count() > 0) cur.right = rightNode;
                            cur.list.Clear();
                            //防止出现扩展后所有数据仍在一边的情况
                            if (cur.left == null) cur = cur.right;
                            else if (cur.right == null) cur = cur.left;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将所有节点的到达标记置为false
        /// </summary>
        /// <param name="cur">起始节点</param>
        public void ClearTag(TreeNode<T> cur)
        {
            if (cur == null) return;
            cur.tagArrival = false;
            ClearTag(cur.left);
            ClearTag(cur.right);
        }

        /// <summary>
        /// 计算对于给定的transaction其包含的子集
        /// </summary>
        /// <param name="transaction">待计算的transaction</param>
        /// <param name="cur">当前算到的节点</param>
        /// <param name="index">对transaction中的第index个位置进行hash</param>
        public void SupportCounting(Transaction<T> transaction,TreeNode<T> cur, int index)
        {
            if (cur == null) return;
            if (cur.left==null && cur.right==null)
            {
                if (cur.tagArrival == true) return;
                //到达叶子节点，且之前未到达过
                LinkedList<Candidates<T>.DataType> subset = new LinkedList<Candidates<T>.DataType>();
                foreach(var nodeData in cur.list)
                {
                    //判断data是否为transaction的一个子集，所有数据均为字典序
                    int i_data = 0;
                    int i_trans = 0;
                    while (i_trans<transaction.Items.Count() && i_data<nodeData.data.Count())
                    {
                        if (object.Equals(transaction.Items[i_trans], nodeData.data[i_data]))
                        {
                            i_data++;
                            i_trans++;
                        }
                        else
                        {
                            i_trans++;
                        }
                    }
                    
                    if (i_data >= nodeData.data.Count()) subset.AddLast(nodeData);
                }
                //将子集的value加1
                var node = cur.list.First;
                while (node!=null)
                {
                    if (subset.Find(node.Value) != null)
                    {
                        Candidates<T>.DataType d = node.Value;
                        d.value++;
                        node.Value = d;
                    }
                    node = node.Next;
                }
                
                cur.tagArrival = true;
            }
            else
            {
                //到达中间节点
                for(int i =index+1;i<transaction.Items.Count();i++)
                {
                    //对index之后的位置分别进行hash
                    if (transaction.Items.Count() > i)
                    {
                        if (HashFunc(transaction.Items, i) <= seperation) SupportCounting(transaction, cur.left, i);
                        else SupportCounting(transaction, cur.right, i);
                    }
                }
            }
        }

        /// <summary>
        /// 逐个返回叶节点
        /// </summary>
        /// <param name="cur">父亲节点</param>
        /// <returns>null或叶节点</returns>
        public TreeNode<T> GetLeaves(TreeNode<T> cur)
        {
            if (cur != null)
            {
                if (cur.left == null && cur.right == null && cur.tagArrival == false)
                {
                    cur.tagArrival = true;
                    return cur;
                }
                else
                {
                    TreeNode<T> l, r;
                    l = GetLeaves(cur.left);
                    if (l != null) return l;
                    else
                    {
                        r = GetLeaves(cur.right);
                        if (r != null) return r;
                    }
                    return null;
                }
            }
            else return null;
        }

        /// <summary>
        /// 打印树
        /// </summary>
        /// <param name="node"></param>
        public void PrintTree(TreeNode<T> node)
        {
            if (node == null) return;
            for (int i = 0; i < node.level; i++) Console.Write("  ");
            Console.Write("Level:" + node.level.ToString() + "; ");
            foreach (var d in node.list)
            {
                Console.Write(PrintFunc(d.data)+"("+d.value+"),");
            }
            Console.WriteLine();
            PrintTree(node.left);
            PrintTree(node.right);
        }

    }
}

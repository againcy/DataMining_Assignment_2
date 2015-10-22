using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataMining_Assignment_2
{
    class Program
    {
        static LinkedList<Transaction<int>> transactions;//项集的集合
        static LinkedList<Candidates<int>> frequentItemsets;//频繁项集合
        static int[] itemName;//项的名称
        static double minsup;//最小support counting
        static string addrRoot = @"G:\Data Mining\Assignment_2\";

        static void Input()
        {
            transactions = new LinkedList<Transaction<int>>();
            StreamReader sr = new StreamReader(addrRoot+ @"assignment2-data.txt");
            
            //读入项的名称
            string str = sr.ReadLine();
            itemName= str.Split(new char[] { ' ' }).Select<string, int>(x => Convert.ToInt32(x)).ToArray();

            //读入所有的transaction
            str = sr.ReadLine();
            while (str!=null)
            {
                string[] tmp = str.Split(new char[]{' '});
                LinkedList<int> data = new LinkedList<int>();
                for (int i = 0; i < itemName.Count(); i++)
                {
                    if (tmp[i] == "1") data.AddLast(itemName[i]);
                }
                
                Transaction<int> newTrans = new Transaction<int>(data.ToArray());
                transactions.AddLast(newTrans);
                str = sr.ReadLine();
            }
            sr.Close();
        }

        /// <summary>
        /// hash函数
        /// </summary>
        /// <param name="data">数组</param>
        /// <param name="level">对数组第level位置的数进行hash</param>
        /// <returns>对2取模</returns>
        static int HashCode(int[] data, int level)
        {
            return (data[level] % 2);
        }

        //hash树的节点打印函数
        static string NodePrint(int[] data)
        {
            string str = "";
            foreach (var i in data) str += i.ToString();
            return str;
        }
        
        /// <summary>
        /// 将candidate按字典序排好
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns>按字典序排好的新的candidate</returns>
        static Candidates<int> SortInTrie(Candidates<int> candidate)
        {
            //建树
            TrieTree<int> trieTree = new TrieTree<int>();
            foreach (var itemset in candidate.ItemSets)
            {
                trieTree.AddNode(itemset.data, itemset.value);
            }
            //取出
            Candidates<int> result = new Candidates<int>();
            trieTree.ClearTag(trieTree.Root);
            foreach (var node in trieTree.Root.children)
            {
                string str = trieTree.GetNodeData(node);
                while (str != "")
                {
                    string[] strData = str.Split(new char[] { ' ' });
                    int[] data = new int[strData.Length - 1];
                    for (int i = 0; i < strData.Length - 1; i++) data[i] = Convert.ToInt32(strData[i]);
                    result.AddCandidate(data, Convert.ToInt32(strData[strData.Length - 1]));
                    str = trieTree.GetNodeData(node);
                }
            }
            return result;
        }

        /// <summary>
        /// 对当前候选集中的项集求support counting
        /// </summary>
        /// <param name="candidate">候选项集</param>
        /// <param name="depth">当前的候选项集的项的个数</param>
        /// <returns>返回support counting大于等于minsup的项集</returns>
        static Candidates<int> DoSupportCounting(Candidates<int> candidate, int depth)
        {
            //建立hash树
            HashTree<int> tree = new HashTree<int>(HashCode, 0, 2, depth, NodePrint);
            foreach (var tmp in candidate.ItemSets)
            {
                tree.AddNode(tmp);
            }
            //计算候选集中的support counting
            foreach (var tmp in transactions)
            {
                tree.ClearTag(tree.Root);
                tree.SupportCounting(tmp, tree.Root, -1);
            }
            //将support counting大于minsup的加入频繁项的集合
            Candidates<int> tmpCandi = new Candidates<int>();
            tree.ClearTag(tree.Root);
            TreeNode<int> node = tree.GetLeaves(tree.Root);
            while (node != null)
            {
                foreach (var d in node.list)
                {
                    if (d.value >= minsup * transactions.Count()) tmpCandi.AddCandidate(d);
                }
                node = tree.GetLeaves(tree.Root);
            }
            return SortInTrie(tmpCandi);
        }

        /// <summary>
        /// Apriori算法初始化，生成含有一个项的频繁项集
        /// </summary>
        static void AprioriInitialize()
        {
            frequentItemsets = new LinkedList<Candidates<int>>();
            Candidates<int> newCandidate = new Candidates<int>();
            //生成项数为1的项集
            foreach (var tmp in itemName)
            {
                newCandidate.AddCandidate(new[] { tmp }, 0);
            }
            frequentItemsets.AddFirst(DoSupportCounting(newCandidate, 1));
        }
        
        /// <summary>
        /// Apriori算法
        /// </summary>
        static void Apriori()
        {
            //初始化，生成含有一个项的频繁项集
            AprioriInitialize();
            int depth = 1;
            Console.WriteLine(depth.ToString() + "层结束");

            Candidates<int> lastCandi = frequentItemsets.Last();
            while(lastCandi.ItemSets.Count>0)
            {
                Candidates<int> tmpCandi = new Candidates<int>();
                depth++;
                //根据上一次的频繁项集合并生成新的候选集
                foreach (var tmp in lastCandi.GenerateNewCandidates()) tmpCandi.AddCandidate(tmp, 0);
                if (tmpCandi.ItemSets.Count == 0) break;
                //判断当前候选集中的项集的所有子集是否都在上一层的频繁项集中
                tmpCandi.Pruning(lastCandi.ItemSets);
                //计算support counting留下大于minsup的项集
                Candidates<int> newCandi = DoSupportCounting(tmpCandi,depth);
                frequentItemsets.AddLast(newCandi);
                lastCandi = newCandi;
                Console.WriteLine(depth.ToString() + "层结束");
            }
            Console.WriteLine("按回车结束程序...");
        }

        /// <summary>
        /// 按字典序输出结果
        /// </summary>
        static void OutputInTrieTree()
        {
            Candidates<int> result = new Candidates<int>();
            TrieTree<int> trieTree = new TrieTree<int>();
            //将所有项集按字典序排序
            foreach(var candi in frequentItemsets)
            {
                foreach(var itemset in candi.ItemSets)
                {
                    result.AddCandidate(itemset);
                }
            }
            result = SortInTrie(result);
            //输出
            StreamWriter sw = new StreamWriter(addrRoot + @"result.txt");
            foreach(var itemset in result.ItemSets)
            {
                foreach (var item in itemset.data) sw.Write(item.ToString() + " ");
                sw.WriteLine(((double)itemset.value / (double)transactions.Count).ToString("0.000"));
            }
            sw.Close();
        }

        static void Main(string[] args)
        {
            minsup = 0.144;
            Input();
            Apriori();
            OutputInTrieTree();
            Console.ReadLine();
        }
    }
}

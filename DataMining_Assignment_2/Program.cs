using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataMining_Assignment_2
{
    class Program
    {
        /*
        static Candidates<char> c1;
        static Candidates<char> c2;
        static void test()
        {
            c1 = new Candidates<char>();
            char[] k1 = new char[2];
            k1[0] = 'a';
            k1[1] = 'b';
            char[] k2 = new char[2];
            k2[0] = 'a';
            k2[1] = 'c';
            char[] k3 = new char[2];
            k3[0] = 'a';
            k3[1] = 'd';
            char[] k4 = new char[2];
            k4[0] = 'b';
            k4[1] = 'c';

            c1.AddCandidate(k1);
            c1.AddCandidate(k2);
            c1.AddCandidate(k3);
            c1.AddCandidate(k4);
            
            c2 = new Candidates<char>();
            foreach(var tmp in c1.GenerateNewCandidates())
            {
                foreach (var c in tmp) Console.Write(c);
                c2.AddCandidate(tmp);
                Console.WriteLine();
            }
            Console.WriteLine("Pruning...");
            c2.Pruning(c1.ItemSets);
            foreach(var tmp in c2.ItemSets)
            {
                foreach (var c in tmp) Console.Write(c);
                Console.WriteLine();
            }

        }

        static int hashcode(int[] data, int level)
        {
            return data[level] % 2;
        }
        static string nodePrint(int[] data)
        {
            string str = "";
            foreach (var i in data) str += i.ToString();
            return str;
        }

        static void trieprint(string str)
        {
            Console.Write(str);
        }
        static void testTree()
        {
            HashTree<int> tree = new HashTree<int>(hashcode, 0, 2, 3,nodePrint);
            int[] a1 = { 0, 1, 2 };
            int[] a2 = { 0, 2, 3 };
            int[] a3 = { 1, 2, 4 };
            int[] a4 = { 1, 3, 4 };
            int[] a5 = { 2, 3, 4 };
            tree.AddNode(a1);
            tree.AddNode(a2);
            tree.AddNode(a3);
            tree.AddNode(a4);
            tree.AddNode(a5);

            int[] t1 = { 0, 1, 2, 3 };
            int[] t2 = { 0, 1, 2, 4 };
            Transaction<int> trans1 = new Transaction<int>(t1);
            Transaction<int> trans2 = new Transaction<int>(t2);

            tree.ClearTag(tree.Root);
            tree.SupportCounting(trans1, tree.Root, -1);
            tree.ClearTag(tree.Root);
            tree.SupportCounting(trans2, tree.Root, -1);
            tree.PrintTree(tree.Root);
        }
        static void testTrie()
        {
            TrieTree<int> tree = new TrieTree<int>(trieprint);
            int[] b1 = { 0 };
            int[] b2 = { 1 };
            int[] a1 = { 0, 1, };
            int[] a2 = { 0, 1, 2 };
            int[] a3 = { 1, 2, 4 };
            int[] a4 = { 1, 3, 4 };
            int[] a5 = { 2, 3, 4 };
            
            tree.AddNode(b1, 1);
            tree.AddNode(b2, 2);
            tree.AddNode(a1, 3);
            tree.AddNode(a2, 4);
            tree.AddNode(a3, 5);
            tree.AddNode(a4, 6);
            tree.AddNode(a5, 7);
            foreach (var node in tree.Root.children) tree.Output(node,"");

        }
        */
        static LinkedList<Transaction<int>> transactions;
        static LinkedList<Candidates<int>> candidates;
        static LinkedList<int> itemName;

        static void Input()
        {
            transactions = new LinkedList<Transaction<int>>();
            StreamReader sr = new StreamReader(@"G:\AgaIn_FieLd\大学_研究生\DataMining\assignment_2\assignment2-data.txt");
            itemName = new LinkedList<int>();
            //读入项的名称
            string str = sr.ReadLine();
            int id1 = str.IndexOf(' ');
            int id2 = 0;
            while (id1 != -1)
            {
                int tmp;
                int.TryParse(str.Substring(id2, id1 - id2), out tmp);
                itemName.AddLast(tmp);
                id2 = id1 + 1;
                id1 = str.IndexOf(' ', id2); 
            }
            int t;
            int.TryParse(str.Substring(id2), out t);
            itemName.AddLast(t);
            int[] name = new int[itemName.Count()];
            name = itemName.ToArray();
            //读入所有的transaction
            str = sr.ReadLine();
            while (str!=null)
            {
                string[] tmp = str.Split(new char[]{' '});
                LinkedList<int> data = new LinkedList<int>();
                for (int i = 0; i < itemName.Count(); i++)
                {
                    if (tmp[i] == "1") data.AddLast(name[i]);
                }
                
                Transaction<int> newTrans = new Transaction<int>(data.ToArray());
                transactions.AddLast(newTrans);
                str = sr.ReadLine();
            }

            sr.Close();
            StreamWriter sw = new StreamWriter(@"G:\AgaIn_FieLd\大学_研究生\DataMining\assignment_2\test.txt");
            sw.WriteLine(transactions.Count());
            foreach (var trans in transactions)
            {
                foreach (var d in trans.Items)
                {
                    sw.Write(d.ToString() + ' ');
                }
                sw.WriteLine();
            }
            sw.Close();
        }

        static int HashCode(int[] data, int level)
        {
            return (data[level] % 2);
        }
        static string NodePrint(int[] data)
        {
            string str = "";
            foreach (var i in data) str += i.ToString();
            return str;
        }
        static void AprioriInitialize()
        {
            Candidates<int> newCandidate = new Candidates<int>();
            foreach (var tmp in itemName)
            {
                newCandidate.AddCandidate(new int[] { tmp });
            }
            HashTree<int> tree = new HashTree<int>(HashCode, 0, 2, 1,NodePrint);
            foreach(var tmp in newCandidate.ItemSets)
            {
                tree.AddNode(tmp);
            }
            foreach(var tmp in transactions)
            {
                tree.ClearTag(tree.Root);
                tree.SupportCounting(tmp,tree.Root,-1);
            }
            tree.PrintTree(tree.Root);
        }
        static void Apriori()
        {
            
            
        }

        static void Main(string[] args)
        {
            Input();
            AprioriInitialize();
            //test();
            //testTree();
            //testTrie();
            Console.ReadLine();
        }
    }
}

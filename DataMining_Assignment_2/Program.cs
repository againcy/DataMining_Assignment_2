using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining_Assignment_2
{
    class Program
    {
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
        static void Main(string[] args)
        {
            //test();
            testTree();
            Console.ReadLine();
        }
    }
}

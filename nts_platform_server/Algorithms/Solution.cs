using System;
using System.Collections.Generic;

namespace nts_platform_server.Algorithms
{
    
  //Definition for a binary tree node.
  public class TreeNode {
     public int val;
       public TreeNode left;
       public TreeNode right;
       public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
           this.val = val;
           this.left = left;
           this.right = right;
       }
   }

    public static class Solution
    {
        private static bool _flag = false;

        public static bool IsSameTree(TreeNode p, TreeNode q)
        {
            
            if (p == null && q == null) return true;
            if (p == null || q == null) return false;
            if (q.val != p.val)
                return false;

            string s = "";
            return IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);

        }


        public static List<char> store = new List<char>();

        public static bool IsValid(string s)
        {

            foreach (var item in s)
            {
                if (item == '(' || item == '{' || item == '[')
                {
                    store.Add(item);
                }
                else
                {
                    if (store.Count > 0)
                    {
                        var cs = store[store.Count - 1];

                        if (cs == '(')
                        {
                            if (item == ')')
                            {
                                store.Remove(store[^1]);
                            }
                            else
                            {
                                return false;
                            }
                        }

                        if (cs == '{')
                        {
                            if (item == '}')
                            {
                                store.Remove(store[^1]);
                            }
                            else
                            {
                                return false;
                            }
                        }

                        if (cs == '[')
                        {
                            if (item == ']')
                            {
                                store.Remove(store[^1]);
                            }
                            else
                            {
                                return false;
                            }
                        }

                    }
                }
            }


            return store.Count == 0;
        }

    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace Zupagood.Models
{
    [Serializable]
    public class TrieNode
    {
        public ZupaNode ZupaNode { get; private set; }

        public char Value { get; set; }
        public List<TrieNode> Children { get; set; }
        public TrieNode Parent { get; set; }
        public int Depth { get; set; }

        public TrieNode(char value, int depth, TrieNode parent, string parentString)
        {
            Value = value;
            Children = new List<TrieNode>();
            Depth = depth;
            Parent = parent;

            TrieNode currentParent = Parent;
            StringBuilder wordBuilder = new StringBuilder(value);

            if (parentString == null)
            {
                while (currentParent != null)
                {
                    if(currentParent.Parent != null)
                        wordBuilder.Insert(0, currentParent.Value);

                    currentParent = currentParent.Parent;
                }

                ZupaNode = new ZupaNode(wordBuilder.ToString());
            }
            else
            {
                ZupaNode = new ZupaNode(parentString + value);
            }
        }

        public bool IsLeaf()
        {
            return Children.Count == 0;
        }

        public TrieNode FindChildNode(char c)
        {
            foreach (var child in Children)
                if (child.Value == c)
                    return child;

            return null;
        }

        public void DeleteChildNode(char c)
        {
            for (var i = 0; i < Children.Count; i++)
                if (Children[i].Value == c)
                    Children.RemoveAt(i);
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zupagood.Models
{
    [Serializable]
    public class ZupaTrie
    {
        private readonly TrieNode _root;

        public ZupaTrie()
        {
            _root = new TrieNode('^', 0, null, null);
        }

        public TrieNode Prefix(string s)
        {
            var currentNode = _root;
            var result = currentNode;

            foreach (var c in s)
            {
                currentNode = currentNode.FindChildNode(c);
                if (currentNode == null)
                    break;
                result = currentNode;
            }

            return result;
        }

        public bool Search(string s)
        {
            var prefix = Prefix(s);

            return prefix.Depth == s.Length && prefix.FindChildNode('$') != null;
        }

        public List<ZupaNode> InsertRange(List<string> items)
        {
            return items.Select(e => Insert(e.Trim())).ToList();
        }

        public ZupaNode Submit(string input)
        {
            ZupaNode found = Query(input);

            if (found != null) return found;

            List<string> tokens = input.Split(' ').Select(e => e.Trim()).ToList();

            var children = InsertRange(tokens);

            return new ZupaNode(children);
        }

        public ZupaNode Query(string input)
        {
            ZupaNode resultNode = null;

            List<string> tokens = input.Split(' ').Select(e => e.Trim()).ToList();

            List<ZupaNode> childNodes = tokens.Select(token => Prefix(token).ZupaNode).ToList();

            string pingGuid = Guid.NewGuid().ToString();
            bool shouldContinue = true;

            //Parallel.ForEach(childNodes, childNode => childNode.Ping(
            //    pingGuid,
            //    childNodes.Count,
            //    new Func<ZupaNode, ZupaNode>(foundNode =>
            //    {
            //        resultNode = foundNode;

            //        return resultNode;
            //    })
            //));

            //childNodes.ForEach(childNode => {
            //    if (shouldContinue)
            //    {
            //        childNode.Ping(
            //            pingGuid,
            //            childNodes.Count,
            //            new Func<ZupaNode, ZupaNode>(foundNode =>
            //            {
            //                resultNode = foundNode;
            //                shouldContinue = false;

            //                return resultNode;
            //            }),
            //            ref shouldContinue
            //        );
            //    }
            //});

            foreach (var childNode in childNodes)
                if (shouldContinue)
                    childNode.Ping(
                        pingGuid,
                        childNodes.Count,
                        new Func<ZupaNode, ZupaNode>(foundNode =>
                        {
                            resultNode = foundNode;
                            shouldContinue = false;

                            return resultNode;
                        }),
                        ref shouldContinue
                    );

            return resultNode;
        }

        public ZupaNode Insert(string s)
        {
            var commonPrefix = Prefix(s);
            var current = commonPrefix;

            for (var i = current.Depth; i < s.Length; i++)
            {
                var newNode = new TrieNode(s[i], current.Depth + 1, current, current.ZupaNode.Content);
                current.Children.Add(newNode);
                current = newNode;
            }

            current.Children.Add(new TrieNode('$', current.Depth + 1, current, current.ZupaNode.Content));

            return current.ZupaNode;
        }

        public void Delete(string s)
        {
            if (Search(s))
            {
                var node = Prefix(s).FindChildNode('$');

                while (node.IsLeaf())
                {
                    var parent = node.Parent;
                    parent.DeleteChildNode(node.Value);
                    node = parent;
                }
            }
        }

    }
}

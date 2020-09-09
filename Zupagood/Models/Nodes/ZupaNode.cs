using System;
using System.Collections.Generic;
using System.Linq;

namespace Zupagood.Models
{
    [Serializable]
    public class ZupaNode
    {
        private Dictionary<string, int> _pings;


        public string Content { get; private set; }
        public List<ZupaNode> Parents { get; private set; }
        public List<ZupaNode> Children { get; private set; }


        public ZupaNode(string word)
        {
            _pings = new Dictionary<string, int>();

            Content = word;
            Parents = new List<ZupaNode>();
            Children = new List<ZupaNode>();
        }

        public ZupaNode(List<ZupaNode> children)
        {
            _pings = new Dictionary<string, int>();

            Content = null;
            Parents = new List<ZupaNode>();
            Children = children;

            children.ForEach(e => e.Parents.Add(this));
        }

        public ZupaNode Ping(string guid, int count)
        {
            if (_pings.ContainsKey(guid))
            {
                _pings[guid] += 1;
            }
            else
            {
                _pings[guid] = 1;
            }

            if (_pings[guid] == count)
            {
                return this;
            }
            else
            {
                List<ZupaNode> result = this.Parents.Select(e => e.Ping(guid, count)).ToList();

                if (result.Any())
                    return result.ElementAt(0);
            }

            return null;
        }
    }
}

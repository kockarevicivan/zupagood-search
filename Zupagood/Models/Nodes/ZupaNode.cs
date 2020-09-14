using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zupagood.Models
{
    [Serializable]
    public class ZupaNode
    {
        private readonly Object _lockObject = new Object();
        //private ConcurrentDictionary<string, int> _pings;
        private Dictionary<string, int> _pings;


        public string Content { get; private set; }
        public List<ZupaNode> Parents { get; private set; }
        public List<ZupaNode> Children { get; private set; }


        public ZupaNode(string word)
        {
            //_pings = new ConcurrentDictionary<string, int>();
            _pings = new Dictionary<string, int>();

            Content = word;
            Parents = new List<ZupaNode>();
            Children = new List<ZupaNode>();
        }

        public ZupaNode(List<ZupaNode> children)
        {
            //_pings = new ConcurrentDictionary<string, int>();
            _pings = new Dictionary<string, int>();

            Content = null;
            Parents = new List<ZupaNode>();
            Children = children;

            children.ForEach(e => e.Parents.Add(this));
        }

        public void Ping(string guid, int count, Func<ZupaNode, ZupaNode> callback, ref bool shouldContinue)
        {
            int newPingValue = _pings.ContainsKey(guid) ? _pings[guid] + 1 : 1;

            lock (_lockObject)
            {
                // Increase the amount of pings for current ZupaNode.
                _pings[guid] = newPingValue;
            }

            // If current ZupaNode is a match, call the callback with it,
            // if not, ping the next layer of parents.
            if (_pings[guid] == count)
                callback(this);
            else
            {
                foreach (var parent in this.Parents)
                    if (shouldContinue)
                        parent.Ping(guid, count, callback, ref shouldContinue);

                //this.Parents.ForEach(e => e.Ping(guid, count, callback));

                //Parallel.ForEach(this.Parents, parent => parent.Ping(guid, count, callback));
            }

        }
    }
}

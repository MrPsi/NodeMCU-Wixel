using System;
using System.Collections.Generic;
using System.Linq;
using NodeMcuWixelMonitor.DataAccess;

namespace NodeMcuWixelMonitor
{
    public interface INodeRepository
    {
        void Add(Node node);
        IReadOnlyCollection<Node> GetAll();
        void Update(Node node);
        void Remove(Guid id);
    }

    public class NodeRepository : INodeRepository
    {
        private readonly IDatabase<List<Node>>  _database;
        private List<Node> _nodes;

        public NodeRepository(IDatabase<List<Node>> database)
        {
            _database = database;
        }

        public void Add(Node node)
        {
            Load();
            _nodes.Add(node);
            Save();
        }

        public IReadOnlyCollection<Node> GetAll()
        {
            Load();
            return _nodes;
        }

        public void Update(Node node)
        {
            Load();
            _nodes.RemoveAll(x => x.Id == node.Id);
            Add(node);
        }

        public void Remove(Guid id)
        {
            Load();
            var node = _nodes.First(x => x.Id == id);
            _nodes.Remove(node);
            Save();
        }

        private void Load()
        {
            if (_nodes != null)
            {
                return;
            }

            _nodes = _database.Load();

            _nodes.ForEach(x => x.ResetSessionVariables());
        }

        private void Save()
        {
            _database.Save(_nodes);
        }
    }
}
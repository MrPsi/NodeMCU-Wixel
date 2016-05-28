using System;

namespace NodeMcuWixelMonitor
{
    public enum NodeStatus
    {
        Available,
        Unavailable,
        Pending
    }

    [Serializable]
    public class Node
    {
        public Node()
        {
        }

        public Node(string name, string address, int port)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Port = port;
            ResetSessionVariables();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public DateTime? LastAccessed { get; set; }
        public DateTime? LastTry { get; set; }
        public NodeStatus CurrentStatus { get; set; }
        public DateTime? UpSince { get; set; }
        public DateTime? LastDataAvailableTime { get; set; }

        public void ResetSessionVariables()
        {
            CurrentStatus = NodeStatus.Unavailable;
            UpSince = null;
            LastDataAvailableTime = null;
        }
    }
}
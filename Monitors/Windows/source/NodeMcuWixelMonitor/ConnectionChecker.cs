using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;

namespace NodeMcuWixelMonitor
{
    public interface IConnectionChecker
    {
        void Check(IReadOnlyCollection<Node> nodes, Action<Node> oneBegin, Action<CheckConnectionResult> oneDone, Action allDone);
    }

    public class ConnectionChecker : IConnectionChecker
    {
        private readonly IDataDecoder _dataDecoder;
        private bool _isInProgress;
        private int _numberOfNodes;
        private int _nodesChecked;
        private Action<Node> _oneBegin;
        private Action<CheckConnectionResult> _oneDone;
        private Action _allDone;

        public ConnectionChecker(IDataDecoder dataDecoder)
        {
            _dataDecoder = dataDecoder;
        }

        public void Check(IReadOnlyCollection<Node> nodes, Action<Node> oneBegin, Action<CheckConnectionResult> oneDone, Action allDone)
        {
            if (_isInProgress)
            {
                throw new Exception("Already in progress");
            }

            _isInProgress = true;
            _oneBegin = oneBegin;
            _oneDone = oneDone;
            _allDone = allDone;
            _numberOfNodes = nodes.Count;
            _nodesChecked = 0;
            foreach (var node in nodes)
            {
                Check(node);
            }
        }

        private void Check(Node node)
        {
            _oneBegin(node);
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(node);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (CheckConnectionResult)e.Result;
            _nodesChecked++;
            _oneDone(result);

            if (_nodesChecked != _numberOfNodes)
            {
                return;
            }

            _isInProgress = false;
            _allDone();
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var receiveBuffer = new byte[4096];
            int receiveBytesCount;
            var node = (Node)doWorkEventArgs.Argument;
            using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Connect(node.Address, node.Port);
                }
                catch
                {
                    doWorkEventArgs.Result = CheckConnectionResult.CreateFail(node.Id);
                    return;
                }

                if (!socket.Connected)
                {
                    doWorkEventArgs.Result = CheckConnectionResult.CreateFail(node.Id);
                    return;
                }

                var messageToSend = Encoding.UTF8.GetBytes("{\"numberOfRecords\":200,\"version\":1,\"includeInfo\":true}");
                try
                {
                    socket.Send(messageToSend);
                }
                catch
                {
                    doWorkEventArgs.Result = CheckConnectionResult.CreateFail(node.Id);
                    return;
                }

                try
                {
                    receiveBytesCount = socket.Receive(receiveBuffer, 4096, SocketFlags.None);
                }
                catch
                {
                    doWorkEventArgs.Result = CheckConnectionResult.CreateFail(node.Id);
                    return;
                }
            }

            var receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, receiveBytesCount);
            var dataDecoderResult = _dataDecoder.Decode(receivedData);
            if (dataDecoderResult == null)
            {
                doWorkEventArgs.Result = CheckConnectionResult.CreateFail(node.Id);
                return;
            }

            doWorkEventArgs.Result = CheckConnectionResult.CreateSuccess(node.Id, dataDecoderResult);
        }
    }

    public class CheckConnectionResult
    {
        private CheckConnectionResult(Guid id, DataDecoderResult dataDecoderResult)
        {
            Id = id;
            DataDecoderResult = dataDecoderResult;
        }

        public static CheckConnectionResult CreateSuccess(Guid id, DataDecoderResult dataDecoderResult)
        {
            return new CheckConnectionResult(id, dataDecoderResult);
        }

        public static CheckConnectionResult CreateFail(Guid id)
        {
            return new CheckConnectionResult(id, null);
        }

        public Guid Id { get; }
        public DataDecoderResult DataDecoderResult { get; set; }
        public bool IsOnline => DataDecoderResult != null;
    }
}
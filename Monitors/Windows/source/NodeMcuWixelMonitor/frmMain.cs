using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NodeMcuWixelMonitor
{
    public partial class frmMain : Form
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IConnectionChecker _connectionChecker;

        private bool _isUpdateInProgress;

        public frmMain(INodeRepository nodeRepository, IConnectionChecker connectionChecker)
        {
            _nodeRepository = nodeRepository;
            _connectionChecker = connectionChecker;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var port = int.Parse(txtPort.Text.Trim());
            var address = txtAddress.Text.Trim();
            var name = txtName.Text.Trim();
            var node = new Node(name, address, port);
            _nodeRepository.Add(node);
            FillList();
            UpdateData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (_isUpdateInProgress)
            {
                return;
            }

            SetUpdateInProgress(true);
            var nodes = _nodeRepository.GetAll();
            _connectionChecker.Check(nodes, OneBegin, OneDone, AllDone);
        }

        private void OneBegin(Node node)
        {
            node.CurrentStatus = NodeStatus.Pending;
            FillList();
        }

        private void OneDone(CheckConnectionResult result)
        {
            var nodes = _nodeRepository.GetAll();
            var node = nodes.First(x => x.Id == result.Id);

            var now = DateTime.Now;
            if (result.IsOnline)
            {
                node.CurrentStatus = NodeStatus.Available;
                node.UpSince = result.DataDecoderResult.UpSince;
                node.LastDataAvailableTime = result.DataDecoderResult.LastDataAvailableTime;
                node.LastAccessed = now;
            }
            else
            {
                node.ResetSessionVariables();
            }
            node.LastTry = now;
            _nodeRepository.Update(node);
            FillList();
        }

        private void AllDone()
        {
            SetUpdateInProgress(false);
        }

        private void SetUpdateInProgress(bool inProgress)
        {
            _isUpdateInProgress = inProgress;
            var enabled = !inProgress;
            //btnAdd.Enabled = enabled;
            //btnDelete.Enabled = enabled;
            btnUpdate.Enabled = enabled;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            FillList();
            UpdateData();
        }

        private void FillList()
        {
            var items = lstNodes.Items;
            var nodes = _nodeRepository.GetAll();
            foreach (var node in nodes)
            {
                var existingItem = GetExisting(items, node);
                if (existingItem == null)
                {
                    AddListItem(node);
                }
                else
                {
                    UpdateItem(node, existingItem);
                }
            }

            var itemsToRemove = items.Cast<ListViewItem>().Where(x => nodes.All(y => y.Id != (Guid)x.Tag));
            foreach (var itemToRemove in itemsToRemove)
            {
                items.Remove(itemToRemove);
            }
        }

        private ListViewItem GetExisting(ListView.ListViewItemCollection items, Node node)
        {
            return items.Cast<ListViewItem>().FirstOrDefault(item => (Guid)item.Tag == node.Id);
        }

        private void AddListItem(Node node)
        {
            var item = new ListViewItem
            {
                Tag = node.Id
            };
            for (var i = 0; i < lstNodes.Columns.Count - 1; i++)
            {
                item.SubItems.Add(new ListViewItem.ListViewSubItem());
            }
            UpdateItem(node, item);
            lstNodes.Items.Add(item);
        }

        private void UpdateItem(Node node, ListViewItem item)
        {
            const int nameIndex = 0;
            const int addressIndex = 1;
            const int portIndex = 2;
            const int statusIndex = 3;
            const int lastAvaliableIndex = 4;
            const int lastDataIndex = 5;
            const int uptimeIndex = 6;

            var now = DateTime.Now;
            item.UseItemStyleForSubItems = false;
            item.SubItems[nameIndex].Text = node.Name;
            item.SubItems[addressIndex].Text = node.Address;
            item.SubItems[portIndex].Text = node.Port.ToString();
            item.SubItems[statusIndex].Text = node.CurrentStatus.ToString();
            item.SubItems[statusIndex].BackColor = GetStatusColor(node.CurrentStatus);
            var lastAccessed = now - node.LastAccessed;
            item.SubItems[lastAvaliableIndex].Text = GetNiceTimeSpanText(lastAccessed);
            var lastData = now - node.LastDataAvailableTime;
            item.SubItems[lastDataIndex].Text = GetNiceTimeSpanText(lastData);
            var upSince = now - node.UpSince;
            item.SubItems[uptimeIndex].Text = GetNiceTimeSpanText(upSince);
        }

        private Color GetStatusColor(NodeStatus status)
        {
            switch (status)
            {
                case NodeStatus.Unavailable:
                    return Color.Red;
                case NodeStatus.Pending:
                    return Color.Orange;
                case NodeStatus.Available:
                    return Color.Green;
                default:
                    throw new Exception("Unknown status");
            }
        }

        private static string GetNiceTimeSpanText(TimeSpan? timeSpan)
        {
            if (!timeSpan.HasValue)
            {
                return string.Empty;
            }

            var value = timeSpan.Value;
            if (value.Days > 0)
            {
                return string.Format("{0:%d} days {0:%h} hours", timeSpan);
            }

            return string.Format("{0:%h} hours {0:%m} minutes", timeSpan);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var items = lstNodes.SelectedItems;
            foreach (ListViewItem item in items)
            {
                var id = (Guid)item.Tag;
                _nodeRepository.Remove(id);
            }
            FillList();
        }

        private void tmrUpdateList_Tick(object sender, EventArgs e)
        {
            FillList();
        }

        private void tmrUpdateData_Tick(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void lstNodes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lnkHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = lnkHomepage.Text;
            try
            {
                Process.Start(url);
            }
            catch
            {
                Clipboard.SetText(url);
                MessageBox.Show(@"Error opening link. Link has been copied to clipboard.", Text);
            }
        }
    }
}

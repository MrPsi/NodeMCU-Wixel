using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NodeMcuWixelMonitor.DataAccess;

namespace NodeMcuWixelMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(GetMain());
        }

        private static Form GetMain()
        {
            //var database = new BinaryDatabase<List<Node>>("nodes.binary");
            var database = new XmlDatabase<List<Node>>("nodes.xml");
            var nodeRepository = new NodeRepository(database);
            var dataDecoder = new DataDecoder();
            var connectionChecker = new ConnectionChecker(dataDecoder);
            var frmMain = new frmMain(nodeRepository, connectionChecker);
            return frmMain;
        }
    }
}

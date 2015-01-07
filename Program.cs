/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace MetroGraphApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() //Yiyang: to enable the old Main, simply change __Main() to Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        [STAThread]
        static void __Main() //Yiyang: to disable the new entry, simply change Main() to __Main()
        {
            var mgv = new MetroGraphView();
            mgv.OpenFromFile(Application.StartupPath + "\\MetroGraph.xml");
            StringBuilder sb = new StringBuilder();
            int sizeOfStart = 1;
            int sizeOfEnd = mgv.Graph.Nodes.Count;// Yiyang:, since the size is very large, I comment out to avoid calculating all the paths, *mgv.Graph.Nodes.Count*;
            bool findShortest = true;
            for (int start = 0; start < sizeOfStart; start++)
                for (int end = start + 1; end < sizeOfEnd; end++)
                {
                    if (findShortest)
                    {
                        var pathList = mgv.FindShortestPaths(mgv.Graph.Nodes[start], mgv.Graph.Nodes[end], null);
                        var pathWithTransfer = pathList.Where(a => a.Transfers > 0);
                        if (pathWithTransfer.Count() > 0)
                            sb.AppendLine(string.Join("\n", pathWithTransfer.Select(b => JsonConvert.SerializeObject(b)).ToArray()));
                    }
                    else
                    {
                        var pathList = mgv.FindAllPaths(mgv.Graph.Nodes[start], mgv.Graph.Nodes[end]);
                        var pathWithTransfer = pathList.Where(a => a.Transfers > 0);
                        if (pathWithTransfer.Count() > 0)
                            sb.AppendLine(string.Join("\n", pathWithTransfer.Select(b => JsonConvert.SerializeObject(b)).ToArray()));
                    }
                }

            {
                System.IO.File.WriteAllText(Application.StartupPath + "\\PathOutput.txt", sb.ToString());
            }

        }
    }
}

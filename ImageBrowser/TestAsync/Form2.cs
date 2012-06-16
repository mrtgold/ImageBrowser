using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public partial class Form2 : Form
    {
        private readonly Dictionary<DirectoryInfo, FileSet> _dirs;

        public Form2()
        {
            InitializeComponent();
            _dirs = new Dictionary<DirectoryInfo, FileSet>();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            directoryTree1.InitDrives();
            directoryTree1.DirectorySelected += DirectorySelected;
        }

        private void DirectorySelected(DirectoryInfo dir)
        {
            toolStripStatusLabel1.Text = dir.FullName;

            if (!_dirs.ContainsKey(dir))
                _dirs[dir] = new FileSet(dir, "*.jpg");

            //listView1.View = View.LargeIcon;
            //var listViewItem = new ListViewItem("",,);
            //listView1.Items.Add(listViewItem);
        }

    }
}

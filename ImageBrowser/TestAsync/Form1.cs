using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserResources;

namespace TestAsync
{

    public partial class Form1 : Form
    {
        private readonly Dictionary<string, List<FileInfo>> _fileSets;
        private readonly List<string> _patterns;

        public Form1()
        {
            InitializeComponent();
            _patterns = new List<string> { "*.jpg", "*.png" };
            _fileSets = new Dictionary<string, List<FileInfo>>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // treeView1 = new DirectoryTree();
            //treeView1.CollapseAll();
           // treeView1.ImageList = DirectoryBrowserImageList.GetImageList();
            //treeView1.ImageKey = TreeViewImages.ClosedFolder.ToString();
            //treeView1.SelectedImageKey

            //InitDrives(treeView1);
            directoryTree1.InitDrives();

        }

        //public static void InitDrives(TreeView treeView)
        //{
        //    foreach (var d in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed))
        //    {
        //        var root = NewDriveRoot(d.Name);

        //        root.Nodes.Add(new TreeNode()); // add dummy node to allow expansion
        //        treeView.Nodes.Add(root);
        //    }
        //}


        //private class NewTreeNode : TreeNode
        //{
        //    public NewTreeNode(string text)
        //        : base(text)
        //    {

        //    }
        //}


        //private static TreeNode NewDriveRoot(string drive)
        //{
        //    var newDriveRoot = new NewTreeNode(drive)
        //                           {
        //                               Tag = new DirNode(drive, new DirectoryInfo(drive))
        //                                         {
        //                                             ExpandedImageKey = DirectoryBrowserImageList.TreeViewImages.Drive.ToString(),
        //                                             CollapsedImageKey = DirectoryBrowserImageList.TreeViewImages.Drive.ToString()
        //                                         }
        //                           };
        //    ((DirNode)newDriveRoot.Tag).SetCollapsedImage(newDriveRoot);

        //    return newDriveRoot;
        //}

        private static TreeNode NewDirNode(DirectoryInfo dir)
        {
            var dirNode = new TreeNode(dir.Name)
                                 {
                                     Tag = new DirNode(dir.Name, dir)
                                               {
                                                   ExpandedImageKey = DirectoryBrowserImageList.TreeViewImages.StuffedFolder.ToString(),
                                                   CollapsedImageKey = DirectoryBrowserImageList.TreeViewImages.ClosedFolder.ToString()
                                               }
                                 };
            ((DirNode)dirNode.Tag).SetCollapsedImage(dirNode);

            return dirNode;
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            NodeSelected(e.Node);

        }

        private void AddDirectoriesAndFiles(TreeNode node, IEnumerable<string> patterns)
        {
            node.Nodes.Clear(); // clear dummy node if exists
            var nodeInfo = (DirNode)node.Tag;

            try
            {
                var subdirs = nodeInfo.Dir.GetDirectories();

                foreach (var subdir in subdirs)
                {
                    var child = NewDirNode(subdir);
                    ((DirNode)child.Tag).SetCollapsedImage(child);

                    // TODO: Use some image for the node to show its a music file

                    child.Nodes.Add(new TreeNode()
                    {
                        ImageKey = DirectoryBrowserImageList.TreeViewImages.ClosedFolder.ToString(),
                        SelectedImageKey = DirectoryBrowserImageList.TreeViewImages.ClosedFolder.ToString()
                    }); // add dummy node to allow expansion
                    node.Nodes.Add(child);
                }


                LoadFiles(patterns, nodeInfo.Dir);

                nodeInfo.Done = true;
            }
            catch
            { // try to handle use each exception separately
                //  node.Tag = null; // clear tag
                nodeInfo.CollapsedImageKey = DirectoryBrowserImageList.TreeViewImages.Warning.ToString();
                nodeInfo.ExpandedImageKey = DirectoryBrowserImageList.TreeViewImages.Warning.ToString();
                nodeInfo.SetCollapsedImage(node);

            }
        }

        private void LoadFiles(IEnumerable<string> patterns, DirectoryInfo dir)
        {
            var fileSet = new List<FileInfo>();
            foreach (var pattern in patterns)
            {
                fileSet.AddRange(dir.GetFiles(pattern));
            }
            _fileSets[dir.FullName] = fileSet;
        }

        private void DisplaySelectedDirectoryFiles(DirectoryInfo dir)
        {
            listBox1.Items.Clear();
            if (_fileSets.ContainsKey(dir.FullName))
            {
                var fileset = _fileSets[dir.FullName];
                foreach (var file in fileset)
                {
                    listBox1.Items.Add(file.Name);
                }
            }
            else
            {
                listBox1.Items.Add(string.Format( "Files not loaded yet for {0}...", dir.FullName));
            }
        }

        private class DirNode
        {
            public string Name { get; set; }
            public DirectoryInfo Dir { get; set; }
            public bool Done { get; set; }

            public string ExpandedImageKey { get; set; }
            public string CollapsedImageKey { get; set; }

            public DirNode(string name, DirectoryInfo dir)
            {
                Name = name;
                Dir = dir;
                Done = false;
            }

            public void SetCollapsedImage(TreeNode node)
            {
                node.SelectedImageKey = CollapsedImageKey;
                node.ImageKey = CollapsedImageKey;
            }

            public void SetExpandedImage(TreeNode node)
            {
                node.SelectedImageKey = ExpandedImageKey;
                node.ImageKey = ExpandedImageKey;
            }
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            ((DirNode)e.Node.Tag).SetCollapsedImage(e.Node);
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            NodeSelected(e.Node);
        }

        private void NodeSelected(TreeNode node)
        {
            //if (!node.IsExpanded)
            //    node.Expand();
            //if (!node.IsSelected)
            //    node.TreeView.SelectedNode = node;

            if (node.Tag == null || !((DirNode)node.Tag).Done)
            {
                AddDirectoriesAndFiles(node, _patterns);
            }

            var nodeInfo = (DirNode)node.Tag;
            if (nodeInfo != null)
            {
                DisplaySelectedDirectoryFiles(nodeInfo.Dir);

                nodeInfo.SetExpandedImage(node);
            }

        }
    }
}

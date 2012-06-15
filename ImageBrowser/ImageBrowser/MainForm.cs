using System;
using System.Windows.Forms;
using ImageBrowserLogic;
using ImageBrowserPresenter;

namespace ImageBrowser
{
    public partial class MainForm : Form, IImageBrowserView
    {
        private ImageBrowserPresenter.ImageBrowserPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
            //dataGridView1.DataSource = bindingSource1.DataSource;
            _presenter = new ImageBrowserPresenter.ImageBrowserPresenter(this);
        }

        public event ImageBrowserViewEventDelegate DirectorySelected;
        public event ImageBrowserViewEventDelegate BrowserViewLoad;

        public IThumbnailDisplayer ThumbnailDisplayer
        {
            get
            {
                return new ThumbnailDisplayerWrapper(dataGridView1);
                //return new ThumbnailBindingSourceDisplayerWrapper(bindingSource1);
            }
        }

        public string Directory
        {
            get { return directoryName.Text; }
            set { directoryName.Text = value; }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (BrowserViewLoad != null)
                BrowserViewLoad(this, this);
        }

        private void directorySelected_Click(object sender, EventArgs e)
        {
            if (DirectorySelected != null)
                DirectorySelected(this, this);

            
        }

        class ThumbnailDisplayerWrapper : IThumbnailDisplayer
        {
            readonly DataGridView _dgv;

            public ThumbnailDisplayerWrapper(DataGridView dgview)
            {
                _dgv = dgview;
            }

            public Thumbnails DataSource
            {
                get { throw new NotImplementedException(); }
                set { _dgv.DataSource = value; }
            }
        }

        class ThumbnailBindingSourceDisplayerWrapper : IThumbnailDisplayer
        {
            readonly BindingSource _source;

            public ThumbnailBindingSourceDisplayerWrapper(BindingSource bindingSource)
            {
                _source = bindingSource;
            }

            public Thumbnails DataSource
            {
                get { throw new NotImplementedException(); }
                set { _source.DataSource = value; }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            //if (dataGridView1.ColumnCount != 1)
            //{
            //    DataGridViewColumn column = new DataGridViewTextBoxColumn();
            //    column.DataPropertyName = "FileName";
            //    column.Name = "fullPath";
            //    dataGridView1.Columns.Add(column);

            //    //dataGridView1.Columns.Add("Thumbnail", "thumbnail");
            //}

        }
    }
}

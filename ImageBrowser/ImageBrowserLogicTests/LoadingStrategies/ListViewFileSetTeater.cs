using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogicTests.Properties;

namespace ImageBrowserLogicTests.LoadingStrategies
{
    public class ListViewFileSetTeater : DirectoryTester
    {
        protected bool InitializeListView_WasCalled;
        protected string _file1;

        protected void InitializeListView(ListView listView)
        {
            InitializeListView_WasCalled = true;
        }

        protected string MakeImageFile()
        {
            var file1 = Path.Combine(TargetDirectory.FullName, Path.GetRandomFileName() + ".jpg");
            Resources.silver_laptop_icon.Save(file1, ImageFormat.Jpeg);

            return file1;
        }
    }
}
using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    public class DirectoryTester
    {
        protected DirectoryInfo TargetDirectory;

        [SetUp]
        public virtual void Setup()
        {
            var targetDirName = "TargetDirectory_"+ Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var dirTesterDirName = (GetType().Namespace ?? "NoNameSpace").Replace('.','_')+"_" + GetType().Name + "_DirectoryTester";
            var dirTesterPath = Path.Combine(Path.GetTempPath(), dirTesterDirName);
            TargetDirectory = Directory.CreateDirectory(Path.Combine(dirTesterPath, targetDirName));
        }
        [TearDown]
        public void TearDown()
        {
            TargetDirectory.Delete(true);
        }
    }
}
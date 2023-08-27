using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build_Assistant.Singletons
{
    public sealed class MoveBuildDirectory
    {
        private static MoveBuildDirectory instance = null;

        private MoveBuildDirectory() { }

        public static MoveBuildDirectory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MoveBuildDirectory();
                }
                return instance;
            }
        }

        public void Run()//D:\Visual Studio\Lakea-Stream-Lakea Stream Assistant\bin\Release\net6.0\Lakea Stream Assistant.exe
        {
            try
            {
                Console.WriteLine("Build Assistant: Moving & Renaming Release Folder...");
                string gitRootPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 22);
                string lakeaExePath = gitRootPath + "Lakea Stream Assistant\\bin\\Release\\net6.0\\Lakea Stream Assistant.exe";
                var versionInfo = FileVersionInfo.GetVersionInfo(lakeaExePath);
                string version = versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart + "." + versionInfo.ProductBuildPart;
                if(Directory.Exists(gitRootPath + "Output\\Lakea Stream Assistant " + version))
                {
                    Directory.Delete(gitRootPath + "Output\\Lakea Stream Assistant " + version, true);
                }
                Directory.Move(gitRootPath + "Lakea Stream Assistant\\bin\\Release\\net6.0", gitRootPath + "Output\\Lakea Stream Assistant " + version);
            }
            catch (Exception e)
            {
                Console.WriteLine("Build Assistant Error -> Couldn't Move Release Folder: " + e);
            }
        }
    }
}

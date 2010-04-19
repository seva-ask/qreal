using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace QReal.Web.Assemblies
{
    public class AssemblyManager
    {
        public Dictionary<string, byte[]> Assemblies { get; private set; }

        private static AssemblyManager instance = new AssemblyManager();

        public static AssemblyManager Instance
        {
            get
            {
                return instance;
            }
        }

        private AssemblyManager()
        {
            Assemblies = new Dictionary<string, byte[]>();
            string directory = AppDomain.CurrentDomain.RelativeSearchPath;
            string diagramsDirectory = directory + "\\Diagrams";
            if (Directory.Exists(diagramsDirectory))
            {
                string[] files = Directory.GetFiles(diagramsDirectory);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assemblies[file] = ReadFileToByteArray(file);
                    }
                }
            }
        }

        private byte[] ReadFileToByteArray(string filePath)
        {
            FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            int read = 0;
            while ((read = fs.Read(buffer, 0, 1024)) > 0) ms.Write(buffer, 0, read);
            fs.Close();
            return ms.ToArray();
        }
    }
}

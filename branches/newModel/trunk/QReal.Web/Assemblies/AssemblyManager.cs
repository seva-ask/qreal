using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QReal.Web.Assemblies
{
    public class AssemblyManager
    {
        public Dictionary<string, byte[]> Assemblies { get; private set; }

        private static readonly AssemblyManager myInstance = new AssemblyManager();

        public static AssemblyManager Instance
        {
            get
            {
                return myInstance;
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
                foreach (string file in files.Where(file => file.EndsWith(".dll")))
                {
                    Assemblies[file] = ReadFileToByteArray(file);
                }
            }
        }

        private static byte[] ReadFileToByteArray(string filePath)
        {
            FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            int read;
            while ((read = fs.Read(buffer, 0, 1024)) > 0) ms.Write(buffer, 0, read);
            fs.Close();
            return ms.ToArray();
        }
    }
}

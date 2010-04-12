using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using QReal.Web.Assemblies;
using System.Diagnostics;
using System.Threading;
using QReal.Web.Database;
using System.Linq;

namespace QReal.Ria.Types
{
    public delegate void LoadingComplete();

    public class TypeLoader
    {
        private List<Type> types;

        private static TypeLoader instance = new TypeLoader();

        public static TypeLoader Instance
        {
            get
            {
                return instance;
            }
        }

        private TypeLoader()
        {
            types = new List<Type>();
            AssembliesContext assembliesContext = new AssembliesContext();
            assembliesContext.GetAssemblies(operation =>
            {
                mReady = operation.Value.Length;
                foreach (string file in operation.Value)
                {
                    GetSerializedAssembly(file);
                }
            }, null);
        }

        private int mReady = -1;

        private void SetReady(int value)
        {
            mReady = value;
        }

        private void DecreaseReady()
        {
            mReady--;
            if (mReady == 0)
            {
                foreach (LoadingComplete action in actions)
                {
                    action();
                }
            }
        }

        private void GetSerializedAssembly(string file)
        {
            AssembliesContext assembliesContext = new AssembliesContext();
            assembliesContext.GetSerialiazedAssembly(file, operation =>
            {
                MemoryStream ms = new MemoryStream(operation.Value);
                AssemblyPart part = new AssemblyPart();
                Assembly assembly = part.Load(ms);
                Type[] availableTypes = assembly.GetTypes();
                types.AddRange(availableTypes);
                DecreaseReady();
            }, null);
        }

        public void Request(LoadingComplete action)
        {
            if (mReady != 0)
            {
	            actions.Add(action);
            }
            else
            {
                action();
            }
        }

        List<LoadingComplete> actions = new List<LoadingComplete>();

        public IEnumerable<Type> Types
        {
            get
            {
                return types;
            }
        }
    }
}

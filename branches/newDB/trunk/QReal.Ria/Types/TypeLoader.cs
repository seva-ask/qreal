using System;
using System.Windows;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using QReal.Web.Assemblies;

namespace QReal.Ria.Types
{
    public delegate void LoadingComplete();

    public class TypeLoader
    {
        private readonly List<Type> myTypes;

        private static readonly TypeLoader myInstance = new TypeLoader();

        public static TypeLoader Instance
        {
            get
            {
                return myInstance;
            }
        }

        private TypeLoader()
        {
            myTypes = new List<Type>();
            AssembliesContext assembliesContext = new AssembliesContext();
            assembliesContext.GetAssemblies(operation =>
            {
                myReady = operation.Value.Length;
                foreach (string file in operation.Value)
                {
                    GetSerializedAssembly(file);
                }
            }, null);
        }

        private int myReady = -1;

        private void DecreaseReady()
        {
            myReady--;
            if (myReady == 0)
            {
                foreach (LoadingComplete action in myActions)
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
                myTypes.AddRange(availableTypes);
                DecreaseReady();
            }, null);
        }

        public void Request(LoadingComplete action)
        {
            if (myReady != 0)
            {
	            myActions.Add(action);
            }
            else
            {
                action();
            }
        }

        readonly List<LoadingComplete> myActions = new List<LoadingComplete>();

        public IEnumerable<Type> Types
        {
            get
            {
                return myTypes;
            }
        }
    }
}

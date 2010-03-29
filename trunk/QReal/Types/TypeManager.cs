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
using ObjectTypes;
using System.Threading;

namespace QReal.Types
{
    public delegate void LoadingComplete();

    public class TypeManager
    {
        public Dictionary<string, DiagramWithTypes> Objects { get; private set; }

        private static TypeManager instance = new TypeManager();

        public static TypeManager Instance
        {
            get
            {
                return instance;
            }
        }

        private TypeManager()
        {
            Objects = new Dictionary<string, DiagramWithTypes>();
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
                Type diagram = null;
                Dictionary<string, Type> types = new Dictionary<string, Type>();
                foreach (Type type in availableTypes)
                {
                    if (type.GetInterface(typeof(IDiagram).FullName, false) != null)
                    {
                        diagram = type;
                    }
                    if (type.IsSubclassOf(typeof(ObjectType)))
                    {
                        ObjectType tmpType = (ObjectType)Activator.CreateInstance(type);
                        types[tmpType.TypeName] = type;
                    }
                }
                string diagramName = ((IDiagram)Activator.CreateInstance(diagram)).Name;
                Objects[diagramName] = new DiagramWithTypes(diagram, types);
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
    }
}

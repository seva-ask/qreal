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
        public List<Type> Types { get; private set; }

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
            Types = new List<Type>();
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
                Types.AddRange(availableTypes);
             //   Type diagram = null;
                //Dictionary<string, Type> types = new Dictionary<string, Type>();
                //foreach (Type type in availableTypes)
                //{
                //    if (type.GetInterface(typeof(IDiagram).FullName, false) != null)
                //    {
                //        diagram = type;
                //    }
                //    if (type.IsSubclassOf(typeof(ObjectType)))
                //    {
                //        ObjectType tmpType = (ObjectType)Activator.CreateInstance(type);
                //        types[tmpType.TypeName] = type;
                //    }
                //}
                //string diagramName = ((IDiagram)Activator.CreateInstance(diagram)).Name;
                //Objects[diagramName] = new DiagramWithTypes(diagram, types);
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

        public void InitProperties(GraphicInstance graphicInstance)
        {
            //Type type = TypeLoader.Instance.Objects["Kernel Diagram"][graphicInstance.LogicalInstance.Type];
            //var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            //foreach (var field in fields)
            //{
            //    if (field.FieldType == typeof(DependencyProperty))
            //    {
            //        string propertyName = field.Name.Substring(0, field.Name.LastIndexOf("Property"));
            //        int count = graphicInstance.LogicalInstance.InstanceProperties.Count(property => property.Name == propertyName);
            //        if (count == 0)
            //        {
            //            InstanceProperty instanceProperty = new InstanceProperty();
            //            instanceProperty.Name = propertyName;
            //            instanceProperty.LogicalInstance = graphicInstance.LogicalInstance;
            //            graphicInstance.LogicalInstance.InstanceProperties.Add(instanceProperty);
            //        }
            //    }
            //}
        }
    }
}

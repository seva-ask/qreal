using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using QReal.Web.Assemblies;

namespace QReal.Ria.Types
{
    public class TypeLoader : DependencyObject
    {
        public ObservableCollection<Type> Types
        {
            get { return (ObservableCollection<Type>) GetValue(TypesProperty); }
            set { SetValue(TypesProperty, value); }
        }

        public static readonly DependencyProperty TypesProperty =
            DependencyProperty.Register("Types", typeof (ObservableCollection<Type>), typeof (TypeLoader), new PropertyMetadata(new ObservableCollection<Type>()));

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
                foreach (Action action in myActions)
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
                foreach (var availableType in availableTypes)
                {
                    Types.Add(availableType);
                }
                DecreaseReady();
            }, null);
        }

        internal void Request(Action callback)
        {
            if (myReady != 0)
            {
	            myActions.Add(callback);
            }
            else
            {
                callback();
            }
        }

        readonly List<Action> myActions = new List<Action>();
    }
}

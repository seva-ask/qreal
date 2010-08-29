using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Browser;
using QReal.Ria.Types;
using System.Linq;
using ObjectTypes;
using QReal.Web.Database;
using System.Reflection;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace QReal.Types
{
    public class TypesManager : DependencyObject
    {
        private static readonly TypesManager myInstance = new TypesManager();

        public static TypesManager Instance
        {
            get
            {
                return myInstance;
            }
        }

        private static bool IsNotDesigner()
        {
            return HtmlPage.IsEnabled;
        }

        public TypesManager()
        {
            if (IsNotDesigner())
            {
                TypeLoader.Instance.Types.CollectionChanged += TypesCollectionChanged;
            }
        }

        private static readonly Func<Type,bool> myTypeIsInSelectedDiagram = type => 
                            ((type.Assembly == Instance.SelectedDiagram.GetType().Assembly) 
                                && (type.IsSubclassOf(typeof(ObjectType))));

        private void TypesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var newItem in e.NewItems)
            {
                Type type = newItem as Type;
                if (type.GetInterface(typeof(IDiagram).FullName, false) != null)
                {
                    IDiagram diagram = (IDiagram) Activator.CreateInstance(type);
                    if (!Diagrams.Any(item => item.GetType() == type))
                    {
                        Diagrams.Add(diagram);
                        if (SelectedDiagram == null)
                        {
                            SelectedDiagram = diagram;
                        }
                    }
                }
                else if (TypesManager.Instance.SelectedDiagram != null && myTypeIsInSelectedDiagram(type) && !TypesManager.Instance.SelectedDiagramTypes.Contains(type))
                {
                    TypesManager.Instance.SelectedDiagramTypes.Add(type);
                }
            }
        }

        public ObservableCollection<IDiagram> Diagrams
        {
            get { return (ObservableCollection<IDiagram>)GetValue(DiagramsProperty); }
            set { SetValue(DiagramsProperty, value); }
        }

        public static readonly DependencyProperty DiagramsProperty =
            DependencyProperty.Register("Diagrams", typeof(ObservableCollection<IDiagram>), typeof(TypesManager), new PropertyMetadata(new ObservableCollection<IDiagram>()));

        public IDiagram SelectedDiagram
        {
            get { return (IDiagram)GetValue(SelectedDiagramProperty); }
            set { SetValue(SelectedDiagramProperty, value); }
        }

        public static readonly DependencyProperty SelectedDiagramProperty =
            DependencyProperty.Register("SelectedDiagram", typeof(IDiagram), typeof(TypesManager), new PropertyMetadata(SelectedDiagramPropertyChangedCallback));

        private static void SelectedDiagramPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TypesManager.Instance.SelectedDiagramTypes = new ObservableCollection<Type>();
            var selectedDiagramTypes = TypeLoader.Instance.Types.Where(myTypeIsInSelectedDiagram);
            foreach (var selectedDiagramType in selectedDiagramTypes)
            {
                TypesManager.Instance.SelectedDiagramTypes.Add(selectedDiagramType);
            }
        }

        public ObservableCollection<Type> SelectedDiagramTypes
        {
            get { return (ObservableCollection<Type>) GetValue(SelectedDiagramTypesProperty); }
            set { SetValue(SelectedDiagramTypesProperty, value); }
        }

        public static readonly DependencyProperty SelectedDiagramTypesProperty =
            DependencyProperty.Register("SelectedDiagramTypes", typeof (ObservableCollection<Type>), typeof (TypesManager), new PropertyMetadata(new ObservableCollection<Type>()));

        public Type GetType(string typeName)
        {
            return TypeLoader.Instance.Types.Single(item => item.FullName == typeName);
        }

        public void InitProperties(LogicalInstance logicalInstance)
        {
            Type type = GetType(logicalInstance.Type);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields.Where(item => item.FieldType == typeof(DependencyProperty)))
            {
                string propertyName = field.Name.Substring(0, field.Name.LastIndexOf("Property"));
                int count = logicalInstance.InstanceProperties.Count(property => property.Name == propertyName);
                if (count == 0)
                {
                    InstanceProperty instanceProperty = new InstanceProperty
                                                            {
                                                                Name = propertyName,
                                                                LogicalInstance = logicalInstance
                                                            };
                    logicalInstance.InstanceProperties.Add(instanceProperty);
                }
            }
        }
    }
}

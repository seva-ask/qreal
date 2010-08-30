using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QReal.Ria.Types;
using System.Linq;
using ObjectTypes;
using QReal.Web.Database;
using System.Reflection;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;

namespace QReal.Types
{
    public class TypesManager : DependencyObject
    {
        private static TypesManager myInstance;

        public static TypesManager Instance
        {
            get
            {
                if (myInstance == null)
                {
                    myInstance = new TypesManager();
                }
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
                myInstance = this;
                TypeLoader.Instance.Types.CollectionChanged += TypesCollectionChanged;
            }
        }

        private void TypesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var newItem in e.NewItems)
            {
                Type type = newItem as Type;
                if (type.GetInterface(typeof (IDiagram).FullName, false) != null)
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
                else if (type.IsSubclassOf(typeof(ObjectType)))
                {
                    AddSelectedDiagramType(type);
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

        public ObservableCollection<Type> SelectedDiagramTypes
        {
            get { return (ObservableCollection<Type>)GetValue(SelectedDiagramTypesProperty); }
            set { SetValue(SelectedDiagramTypesProperty, value); }
        }

        public static readonly DependencyProperty SelectedDiagramTypesProperty =
            DependencyProperty.Register("SelectedDiagramTypes", typeof(ObservableCollection<Type>), typeof(TypesManager), new PropertyMetadata(new ObservableCollection<Type>()));

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
                Instance.AddSelectedDiagramType(selectedDiagramType);
            }
        }

        private static readonly Func<Type, bool> myTypeIsInSelectedDiagram = type =>
                            ((type.Assembly == Instance.SelectedDiagram.GetType().Assembly)
                                && (type.IsSubclassOf(typeof(ObjectType))));

        private readonly AutoResetEvent myIconCreatingIsAvailableEvent = new AutoResetEvent(true);

        private readonly AutoResetEvent myIconCreatedEvent = new AutoResetEvent(false);

        private void AddSelectedDiagramType(Type selectedDiagramType)
        {
            ThreadPool.QueueUserWorkItem(state =>
                                             {
                                                 myIconCreatingIsAvailableEvent.WaitOne();
                                                 UIManager.Instance.MainPage.Dispatcher.BeginInvoke(
                                                     () => CreateIcon(selectedDiagramType));
                                                 myIconCreatedEvent.WaitOne();
                                                 UIManager.Instance.MainPage.Dispatcher.BeginInvoke(
                                                     () =>
                                                     {
                                                         if (SelectedDiagram != null && myTypeIsInSelectedDiagram(selectedDiagramType) && !SelectedDiagramTypes.Contains(selectedDiagramType))
                                                         {
                                                             SelectedDiagramTypes.Add(selectedDiagramType);
                                                         }
                                                     });
                                                 myIconCreatingIsAvailableEvent.Set();
                                             });
        }

        private Type myCurrentTypeForCreatingIcon;

        private readonly Dictionary<Type, WriteableBitmap> myIcons = new Dictionary<Type, WriteableBitmap>();

        public WriteableBitmap GetIcon(Type type) // icons need to be bitmap because silverlight doesn't render many templates with equal x:Name correctly
        {
            return myIcons[type];
        }

        private const int ICON_SIZE = 35;

        private void CreateIcon(Type selectedDiagramType)
        {
            if (myIcons.ContainsKey(selectedDiagramType))
            {
                myIconCreatedEvent.Set();
                return;
            }
            myCurrentTypeForCreatingIcon = selectedDiagramType;
            Canvas canvas = new Canvas();
            ObjectType objectType = (Activator.CreateInstance(selectedDiagramType)) as ObjectType;
            if (objectType is NodeType)
            {
                double scaleCoefficient = ICON_SIZE / Math.Max(objectType.Width, objectType.Height);
                objectType.RenderTransform = new ScaleTransform { ScaleX = scaleCoefficient, ScaleY = scaleCoefficient };
                objectType.SetValue(Canvas.TopProperty, (ICON_SIZE - objectType.Height * scaleCoefficient) / 2);
                objectType.SetValue(Canvas.LeftProperty, (ICON_SIZE - objectType.Width * scaleCoefficient) / 2);
            }
            else
            {
                (objectType as EdgeType).X2 = ICON_SIZE;
                (objectType as EdgeType).Y2 = ICON_SIZE;
            }
            objectType.IsHitTestVisible = false;
            canvas.Children.Add(objectType);
            canvas.Loaded += new RoutedEventHandler(CanvasLoaded);
            UIManager.Instance.MainPage.sandboxForCreatingIcons.Content = canvas;
        }

        private void CanvasLoaded(object sender, RoutedEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            WriteableBitmap bitmap = new WriteableBitmap(ICON_SIZE, ICON_SIZE);
            bitmap.Render(canvas, null);
            bitmap.Invalidate();
            myIcons[myCurrentTypeForCreatingIcon] = bitmap;
            UIManager.Instance.MainPage.sandboxForCreatingIcons.Content = null;
            myIconCreatedEvent.Set();
        }

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

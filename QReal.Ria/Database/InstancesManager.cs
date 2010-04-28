using System;
using System.Windows;
using System.Linq;
using QReal.Web.Database;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Browser;

namespace QReal.Ria.Database
{
    public class InstancesManager : DependencyObject
    {
        private static InstancesManager myInstance;

        public static InstancesManager Instance
        {
            get
            {
                if (myInstance == null)
                {
                    myInstance = new InstancesManager();
                }
                return myInstance;
            }
        }

        public InstancesContext InstancesContext { get; set; }

        public InstancesManager()
	    {
            if (IsNotDesigner())
            {
                myInstance = this;
                InstancesContext = new InstancesContext();
                InstancesContext.EdgeInstances.PropertyChanged += GraphicInstances_PropertyChanged;
                InstancesContext.NodeInstances.PropertyChanged += GraphicInstances_PropertyChanged;
                DispatcherTimer timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 1000)};
                timer.Tick += new EventHandler(TimerTick);
                timer.Start();                
            }
        }

        private static bool IsNotDesigner()
        {
            return HtmlPage.IsEnabled;
        }

        private void GraphicInstances_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Count")
            {
                UpdateCanvasInstancesSource();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            InstancesContext.Load(InstancesContext.GetGeometryInformationsQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetInstancePropertiesQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetLogicalInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetGraphicInstancesQuery(), LoadBehavior.MergeIntoCurrent, 
                action =>
                    {
                        InstancesContext.Load(InstancesContext.GetParentableInstancesQuery(), LoadBehavior.MergeIntoCurrent,
                            action2 =>
                            {
                                InstancesContext.Load(InstancesContext.GetNodeInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
                                InstancesContext.Load(InstancesContext.GetRootInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
                            }, false);
                        InstancesContext.Load(InstancesContext.GetEdgeInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);                        
                    },false);
        }

        public IEnumerable<Entity> CanvasInstancesSource
        {
            get { return (IEnumerable<Entity>)GetValue(CanvasInstancesSourceProperty); }
            set { SetValue(CanvasInstancesSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasInstancesSourceProperty =
            DependencyProperty.Register("CanvasInstancesSource", typeof(IEnumerable<Entity>), typeof(InstancesManager), null);

        public RootInstance CanvasRootElement
        {
            get { return (RootInstance) GetValue(CanvasRootElementProperty); }
            set { SetValue(CanvasRootElementProperty, value); }
        }

        public static readonly DependencyProperty CanvasRootElementProperty =
            DependencyProperty.Register("CanvasRootElement", typeof (RootInstance), typeof (InstancesManager), new PropertyMetadata(OnCanvasRootElementPropertyChanged));

        private static void OnCanvasRootElementPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            InstancesManager.Instance.UpdateCanvasInstancesSource();
        }

        public void UpdateCanvasInstancesSource()
        {
            List<Entity> canvasSource = new List<Entity>();
            if (CanvasRootElement != null)
            {
                foreach (var entity in CanvasRootElement.GetParent<ParentableInstance>().NodeChildren)
                {
                    canvasSource.Add(entity);
                    AddChildren(canvasSource, entity);
                }
                foreach (var entity in CanvasRootElement.EdgeChildren)
                {
                    canvasSource.Add(entity);
                }
            }
            CanvasInstancesSource = canvasSource;
        }

        private static void AddChildren(List<Entity> canvasSource, NodeInstance nodeInstance)
        {
            if (nodeInstance.GetParent<ParentableInstance>() != null)
            {
                foreach (var entity in nodeInstance.GetParent<ParentableInstance>().NodeChildren)
                {
                    canvasSource.Add(entity);
                    AddChildren(canvasSource, entity);
                }
            }
        }

        public void SetCanvasRootItem(Entity selectedGraphicInstance)
        {
            if (selectedGraphicInstance is RootInstance)
            {
                CanvasRootElement = selectedGraphicInstance as RootInstance;
            }
            else if (selectedGraphicInstance is EdgeInstance)
            {
                CanvasRootElement = (selectedGraphicInstance as EdgeInstance).Parent;
            }
            else
            {
                CanvasRootElement = GetRootParent((selectedGraphicInstance as NodeInstance).Parent);
            }
        }

        private static RootInstance GetRootParent(ParentableInstance parentableInstance)
        {
            if (parentableInstance.RootInstanceInheritance.Any())
            {
                return parentableInstance.RootInstanceInheritance.Single();
            }
            else
            {
                return GetRootParent(parentableInstance.NodeInstanceInheritance.Single().Parent);
            }
        }
    }
}

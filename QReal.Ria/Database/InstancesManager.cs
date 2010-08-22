using System;
using System.Collections.ObjectModel;
using System.Windows;
using QReal.Web.Database;
using System.Windows.Threading;
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
                InstancesContext.GraphicInstances.EntityAdded += GraphicInstances_EntityAdded;
                InstancesContext.GraphicInstances.EntityRemoved += GraphicInstances_EntityRemoved;
                DispatcherTimer timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 1000)};
                timer.Tick += new EventHandler(TimerTick);
                timer.Start();                
            }
        }

        private static bool IsNotDesigner()
        {
            return HtmlPage.IsEnabled;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            InstancesContext.Load(InstancesContext.GetInstancePropertiesQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetLogicalInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetGraphicInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
        }

        public ObservableCollection<GraphicInstance> CanvasInstancesSource
        {
            get { return (ObservableCollection<GraphicInstance>)GetValue(CanvasInstancesSourceProperty); }
            set { SetValue(CanvasInstancesSourceProperty, value); }
        }

        public static readonly DependencyProperty CanvasInstancesSourceProperty =
            DependencyProperty.Register("CanvasInstancesSource", typeof(ObservableCollection<GraphicInstance>), typeof(InstancesManager), new PropertyMetadata(new ObservableCollection<GraphicInstance>()));

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

        public ObservableCollection<RootInstance> RootInstances
        {
            get { return (ObservableCollection<RootInstance>)GetValue(RootInstancesProperty); }
            set { SetValue(RootInstancesProperty, value); }
        }

        public static readonly DependencyProperty RootInstancesProperty =
            DependencyProperty.Register("RootInstances", typeof(ObservableCollection<RootInstance>), typeof(InstancesManager), new PropertyMetadata(new ObservableCollection<RootInstance>()));

        private void GraphicInstances_EntityRemoved(object sender, EntityCollectionChangedEventArgs<GraphicInstance> e)
        {
            if (e.Entity is RootInstance)
            {
                RootInstances.Remove(e.Entity as RootInstance);
                if (CanvasRootElement == e.Entity)
                {
                    UpdateCanvasInstancesSource();
                }
            }
            else
            {
                if (CanvasRootElement == GetRootParent(e.Entity))
                {
                    CanvasInstancesSource.Remove(e.Entity);
                }
            }
        }

        private void GraphicInstances_EntityAdded(object sender, EntityCollectionChangedEventArgs<GraphicInstance> e)
        {
            AddInstance(e.Entity);
        }

        public void UpdateCanvasInstancesSource()
        {
            CanvasInstancesSource.Clear();
            if (CanvasRootElement != null)
            {
                AddNodeChildren(CanvasRootElement);
                foreach (var entity in CanvasRootElement.EdgeChildren)
                {
                    CanvasInstancesSource.Add(entity);
                }
            }
        }

        private void AddNodeChildren(ParentableInstance parentableInstance)
        {
            foreach (var entity in parentableInstance.NodeChildren)
            {
                CanvasInstancesSource.Add(entity);
                AddNodeChildren(entity);
            }
        }

        public void SetCanvasRootItem(GraphicInstance selectedGraphicInstance)
        {
            CanvasRootElement = GetRootParent(selectedGraphicInstance);
        }

        public RootInstance GetRootParent(GraphicInstance graphicInstance)
        {
            if (graphicInstance == null)
            {
                return null;
            }
            else if (graphicInstance is RootInstance)
            {
                return graphicInstance as RootInstance;
            }
            else if (graphicInstance is EdgeInstance)
            {
                return (graphicInstance as EdgeInstance).Parent;
            }
            else
            {
                return GetRootParent((graphicInstance as NodeInstance).Parent);
            }
        }

        public void AddInstance(GraphicInstance graphicInstance)
        {
            if (graphicInstance is RootInstance)
            {
                if (!RootInstances.Contains(graphicInstance as RootInstance))
                {
                    RootInstances.Add(graphicInstance as RootInstance);
                    if (CanvasRootElement == graphicInstance)
                    {
                        UpdateCanvasInstancesSource();
                    }
                }
            }
            else
            {
                if (CanvasRootElement!= null && CanvasRootElement == GetRootParent(graphicInstance) && !CanvasInstancesSource.Contains(graphicInstance))
                {
                    CanvasInstancesSource.Add(graphicInstance);
                }
            }
        }
    }
}

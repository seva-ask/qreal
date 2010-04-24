using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Linq;
using System.Windows.Shapes;
using QReal.Web.Database;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;
using System.Reflection;
using System.Windows.Browser;

namespace QReal.Ria.Database
{
    public class InstancesManager : DependencyObject
    {
        private static InstancesManager instance = new InstancesManager();

        public static InstancesManager Instance
        {
            get
            {
                return instance;
            }
        }

        public InstancesContext InstancesContext = new InstancesContext();

        public InstancesManager()
	    {
            if (IsNotDesigner())
            {
                InstancesContext.GraphicInstances.PropertyChanged += new PropertyChangedEventHandler(GraphicInstances_PropertyChanged);
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
                timer.Tick += new EventHandler(timer_Tick);
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
                UpdateProperties();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            InstancesContext.Load(InstancesContext.GetLogicalInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetInstancePropertiesQuery(), LoadBehavior.MergeIntoCurrent, false);
            InstancesContext.Load(InstancesContext.GetGraphicInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
        }

        public IEnumerable<GraphicInstance> TreeviewInstancesSource
        {
            get { return (IEnumerable<GraphicInstance>)GetValue(TreeviewInstancesSourceProperty); }
            set { SetValue(TreeviewInstancesSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TreeviewInstancesSourceProperty =
            DependencyProperty.Register("TreeviewInstancesSource", typeof(IEnumerable<GraphicInstance>), typeof(InstancesManager), null);

        public IEnumerable<GraphicInstance> CanvasInstancesSource
        {
            get { return (IEnumerable<GraphicInstance>)GetValue(CanvasInstancesSourceProperty); }
            set { SetValue(CanvasInstancesSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasInstancesSourceProperty =
            DependencyProperty.Register("CanvasInstancesSource", typeof(IEnumerable<GraphicInstance>), typeof(InstancesManager), null);

        public GraphicInstance CanvasRootElement
        {
            get { return (GraphicInstance)GetValue(CanvasRootElementProperty); }
            set { SetValue(CanvasRootElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasRootElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasRootElementProperty =
            DependencyProperty.Register("CanvasRootElement", typeof(GraphicInstance), typeof(InstancesManager), new PropertyMetadata(OnCanvasRootElementPropertyChanged));

        private static void OnCanvasRootElementPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            InstancesManager.Instance.UpdateCanvasInstancesSource();
        }

        public void UpdateProperties()
        {
            TreeviewInstancesSource = InstancesContext.GraphicInstances.Where(instance => !(instance is GraphicVisualizedInstance));
            if (CanvasRootElement == null)
            {
                CanvasRootElement = InstancesContext.GraphicInstances.First(instance => !(instance is GraphicVisualizedInstance));    
            }
            UpdateCanvasInstancesSource();
        }

        public void UpdateCanvasInstancesSource()
        {
            List<GraphicInstance> canvasInstancesSource = new List<GraphicInstance>();
            AddGraphicInstancesRecursive(canvasInstancesSource, CanvasRootElement);
            CanvasInstancesSource = canvasInstancesSource;
        }

        private void AddGraphicInstancesRecursive(List<GraphicInstance> list, GraphicInstance graphicInstance)
        {
            if (graphicInstance != null)
            {
                foreach (var item in graphicInstance.Children)
                {
                    AddGraphicInstancesRecursive(list, item);
                    list.Add(item);
                }                
            }
        }

        public void SetCanvasRootElement(GraphicInstance graphicInstance)
        {
            if (!(graphicInstance is GraphicVisualizedInstance))
            {
                CanvasRootElement = graphicInstance;
            }
            else
            {
                SetCanvasRootElement((graphicInstance as GraphicVisualizedInstance).Parent);
            }
        }
    }
}

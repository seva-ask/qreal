using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using QReal.Controls;
using QReal.Web.Database;
using System.Collections.Generic;
using QReal.Ria.Database;
using System.Linq;

namespace QReal
{
    public delegate void SelectedItemChangedHandler(Entity newSelectedGraphicInstance);

    public class UIManager : DependencyObject
    {
        private static readonly UIManager myInstance = new UIManager();

        public static UIManager Instance
        {
            get
            {
                return myInstance;
            }
        }

        public MainPage MainPage { get; set; }

        private Canvas myCanvas;

        public Canvas Canvas
        {
            get
            {
                return myCanvas;
            }
            set
            {
                myCanvas = value;
                new AutoScroller(MainPage.scrollViewer, AutoScroller.Mode.Auto)
                    {
                        TargetCanvas = myCanvas,
                        AutoScroll = AutoScroller.Mode.Drag
                    };
            }
        }

        public GraphicInstance SelectedGraphicInstance
        {
            get { return (GraphicInstance)GetValue(SelectedGraphicInstanceProperty); }
            set { SetValue(SelectedGraphicInstanceProperty, value); }
        }

        public static readonly DependencyProperty SelectedGraphicInstanceProperty =
            DependencyProperty.Register("SelectedGraphicInstance", typeof(GraphicInstance), typeof(UIManager), new PropertyMetadata(null, OnSelectedGraphicInstancePropertyChanged));

        private static void OnSelectedGraphicInstancePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (UIManager.Instance.SelectedGraphicInstance != null)
            {
                InstancesManager.Instance.SetCanvasRootItem(UIManager.Instance.SelectedGraphicInstance);
            }
            if (UIManager.Instance.SelectedItemChanged != null)
            {
                UIManager.Instance.SelectedItemChanged(UIManager.Instance.SelectedGraphicInstance);
            }
            UIManager.Instance.InstancePropertiesSource = UIManager.Instance.SelectedGraphicInstance != null ? InstancesManager.Instance.InstancesContext.InstanceProperties.Where(item => item.LogicalInstance == UIManager.Instance.SelectedGraphicInstance.LogicalInstance) : null;
        }

        public event SelectedItemChangedHandler SelectedItemChanged;

        public IEnumerable<InstanceProperty> InstancePropertiesSource
        {
            get { return (IEnumerable<InstanceProperty>)GetValue(InstancePropertiesSourceProperty); }
            set { SetValue(InstancePropertiesSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InstancePropertiesSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InstancePropertiesSourceProperty =
            DependencyProperty.Register("InstancePropertiesSource", typeof(IEnumerable<InstanceProperty>), typeof(UIManager), null);

    }
}

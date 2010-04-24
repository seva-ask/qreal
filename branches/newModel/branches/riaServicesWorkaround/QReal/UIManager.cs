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
using QReal.Controls;
using QReal.Web.Database;
using System.Collections.Generic;
using QReal.Ria.Database;
using System.Linq;

namespace QReal
{
    public delegate void SelectedItemChangedHandler(GraphicInstance newSelectedGraphicInstance);

    public class UIManager : DependencyObject
    {
        private static UIManager instance = new UIManager();

        public static UIManager Instance
        {
            get
            {
                return instance;
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
                AutoScroller autoScroller = new AutoScroller(MainPage.scrollViewer, AutoScroller.Mode.Auto);
                autoScroller.TargetCanvas = myCanvas;
                autoScroller.AutoScroll = AutoScroller.Mode.Drag;
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
            UIManager uiManager = obj as UIManager;
            if (uiManager.SelectedItemChanged != null)
            {
                uiManager.SelectedItemChanged(uiManager.SelectedGraphicInstance);
            }
            if (uiManager.SelectedGraphicInstance != null)
            {
                uiManager.InstancePropertiesSource = InstancesManager.Instance.InstancesContext.InstanceProperties.Where(item => item.LogicalInstance == uiManager.SelectedGraphicInstance.LogicalInstance);
            }
            else
            {
                uiManager.InstancePropertiesSource = null;
            }
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

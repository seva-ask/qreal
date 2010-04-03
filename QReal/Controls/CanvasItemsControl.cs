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
using System.Windows.Markup;
using System.Linq;
using QReal.Types;
using QReal.Web.Database;
using ObjectTypes;
using System.Collections.Generic;
using QReal.Database;
using System.Reflection;
using System.Windows.Data;

namespace QReal.Controls
{
    public class CanvasItemsControl : ItemsControl
    {
        public CanvasItemsControl()
        {
            UIManager.Instance.SelectedItemChanged += new SelectedItemChangedHandler(Instance_SelectedItemChanged);
        }

        private void Instance_SelectedItemChanged(int newId)
        {
            foreach (var item in GetObjectTypes())
            {
                item.UnSelect();
            }
            ObjectType itemToSelect = GetObjectType(newId);
            if (itemToSelect != null)
            {
                itemToSelect.Select();
            }
        }

        private IEnumerable<ObjectType> GetObjectTypes()
        {
            var itemsPresenter = VisualTreeHelper.GetChild(this, 0);
            var canvas = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
            int itemsCount = VisualTreeHelper.GetChildrenCount(canvas);
            for (int i = 0; i < itemsCount; i++)
            {
                var contentPresenter = VisualTreeHelper.GetChild(canvas, i);
                var itemsCanvas = VisualTreeHelper.GetChild(contentPresenter, 0) as Canvas;
                var objectType = VisualTreeHelper.GetChild(itemsCanvas, 0) as ObjectType;
                yield return objectType;
            }
        }

        private ObjectType GetObjectType(int id)
        {
            foreach (var item in GetObjectTypes())
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GraphicInstance graphicInstance = item as GraphicInstance;
            ContentPresenter contentPresenter = element as ContentPresenter;
            if (graphicInstance.LogicalInstance != null)
            {
                TypeManager.Instance.Request(() =>
                contentPresenter.ContentTemplate = Create(TypeManager.Instance.Objects["Kernel Diagram"][graphicInstance.LogicalInstance.Type]));
            }
            contentPresenter.Loaded += new RoutedEventHandler(contentPresenter_Loaded);
        }

        private void contentPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            GraphicInstance graphicInstance = contentPresenter.Content as GraphicInstance;
            InstancesManager.Instance.InitProperties(graphicInstance);
            Canvas itemsCanvas = VisualTreeHelper.GetChild(contentPresenter, 0) as Canvas;
            ObjectType objectType = VisualTreeHelper.GetChild(itemsCanvas, 0) as ObjectType;
            SetPropertyBindings(graphicInstance, objectType);
            objectType.MouseLeftButtonDown += new MouseButtonEventHandler(objectType_MouseLeftButtonDown);
        }

        private void objectType_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIManager.Instance.SelectedGraphicInstanceId = (sender as ObjectType).Id;
            e.Handled = true;
        }

        private void SetPropertyBindings(GraphicInstance graphicInstance, ObjectType objectType)
        {
            Type type = objectType.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(DependencyProperty))
                {
                    string propertyName = field.Name.Substring(0, field.Name.LastIndexOf("Property"));
                    InstanceProperty instanceProperty = graphicInstance.LogicalInstance.InstanceProperties.Single(item => item.Name == propertyName);
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("Value");
                    binding.Mode = BindingMode.TwoWay;
                    binding.Source = instanceProperty;
                    DependencyProperty dependencyProperty = field.GetValue(objectType) as DependencyProperty;
                    objectType.SetBinding(dependencyProperty, binding);
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var itemsPresenter = VisualTreeHelper.GetChild(this, 0);
            var canvas = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
            UIManager.Instance.Canvas = canvas;
            return base.GetContainerForItemOverride();
        }

        public AutoScroller Autoscroller { get; set; }

        private DataTemplate Create(Type type)
        {
            string xaml = @"<DataTemplate 
                xmlns=""http://schemas.microsoft.com/client/2007""
                xmlns:controls=""clr-namespace:" + type.Namespace + @";assembly=" + type.Namespace + @""">
                <Canvas HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"">
                    <controls:" + type.Name + @" Canvas.Left=""{Binding X, Mode=TwoWay}"" Canvas.Top=""{Binding Y, Mode=TwoWay}"" Id=""{Binding Id, Mode=TwoWay}"" ElementName=""{Binding LogicalInstance.Name, Mode=TwoWay}"" " + GetSizeBinding(type) +  @" />
                </Canvas>
                </DataTemplate>";
            return (DataTemplate)XamlReader.Load(xaml);
        }

        private string GetSizeBinding(Type type)
        {
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                return @"Width=""{Binding Width, Mode=TwoWay}"" Height=""{Binding Height, Mode=TwoWay}""";
            }
            else
            {
                return @"X2=""{Binding Width, Mode=TwoWay}"" Y2=""{Binding Height, Mode=TwoWay}""";
            }
        }
    }
}

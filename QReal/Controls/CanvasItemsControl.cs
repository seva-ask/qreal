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
using QReal.Types;
using QReal.Web.Database;
using ObjectTypes;

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
            ProcessObjectTypes(delegate(ObjectType item)
            {
                (item.Content as Panel).Background = new SolidColorBrush(Colors.Transparent);
            });
            ObjectType itemToSelect = GetObjectType(newId);
            if (itemToSelect != null)
            {
                (itemToSelect.Content as Panel).Background = new SolidColorBrush(Colors.Blue);
            }
        }

        private delegate void ProcessObjectType(ObjectType item);

        private void ProcessObjectTypes(ProcessObjectType work)
        {
            var itemsPresenter = VisualTreeHelper.GetChild(this, 0);
            var canvas = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
            int itemsCount = VisualTreeHelper.GetChildrenCount(canvas);
            for (int i = 0; i < itemsCount; i++)
            {
                var contentPresenter = VisualTreeHelper.GetChild(canvas, i);
                var itemsCanvas = VisualTreeHelper.GetChild(contentPresenter, 0) as Canvas;
                var objectType = VisualTreeHelper.GetChild(itemsCanvas, 0) as ObjectType;
                work(objectType);
            }
        }

        private ObjectType GetObjectType(int id)
        {
            ObjectType result = null;
            ProcessObjectTypes(delegate(ObjectType item)
            {
                if (item.Id == id)
                {
                    result = item;
                }
            });
            return result;
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
                    <controls:" + type.Name + @" Canvas.Left=""{Binding X, Mode=TwoWay}"" Canvas.Top=""{Binding Y, Mode=TwoWay}"" Width=""{Binding Width, Mode=TwoWay}"" Height=""{Binding Height, Mode=TwoWay}"" Id=""{Binding Id, Mode=TwoWay}"" ElementName=""{Binding LogicalInstance.Name, Mode=TwoWay}""/>
                </Canvas>
                </DataTemplate>";
            return (DataTemplate)XamlReader.Load(xaml);
        }
    }
}

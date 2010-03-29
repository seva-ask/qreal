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

namespace QReal.Controls
{
    public class CanvasItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GraphicInstance graphicInstance = item as GraphicInstance;
            ContentPresenter contentPresenter = element as ContentPresenter;
            TypeManager.Instance.Request(() =>
            contentPresenter.ContentTemplate = Create(TypeManager.Instance.Objects["Kernel Diagram"][graphicInstance.LogicalInstance.Type])
            );
        }

        private DataTemplate Create(Type type)
        {
            string xaml = @"<DataTemplate 
                xmlns=""http://schemas.microsoft.com/client/2007""
                xmlns:controls=""clr-namespace:" + type.Namespace + @";assembly=" + type.Namespace + @""">
                <controls:" + type.Name + @" Margin=""{Binding Position, Mode=OneWay}""/></DataTemplate>";
            return (DataTemplate)XamlReader.Load(xaml);
        }
    }
}

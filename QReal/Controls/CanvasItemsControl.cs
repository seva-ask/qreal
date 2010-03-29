﻿using System;
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
            Autoscroller.TargetCanvas = canvas;
            Autoscroller.AutoScroll = AutoScroller.Mode.Drag;
            return base.GetContainerForItemOverride();
        }

        public AutoScroller Autoscroller { get; set; }

        private DataTemplate Create(Type type)
        {
            string xaml = @"<DataTemplate 
                xmlns=""http://schemas.microsoft.com/client/2007""
                xmlns:controls=""clr-namespace:" + type.Namespace + @";assembly=" + type.Namespace + @""">
                <Canvas HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"">
                    <controls:" + type.Name + @" Canvas.Left=""{Binding X, Mode=TwoWay}"" Canvas.Top=""{Binding Y, Mode=TwoWay}"" Width=""{Binding Width, Mode=TwoWay}"" Height=""{Binding Height, Mode=TwoWay}""/>
                </Canvas>
                </DataTemplate>";
            return (DataTemplate)XamlReader.Load(xaml);
        }
    }
}
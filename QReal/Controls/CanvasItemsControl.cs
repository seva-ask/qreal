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
using System.Linq;
using ObjectTypes;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Data;
using QReal.Web.Database;
using QReal.Ria.Types;
using QReal.Types;

namespace QReal.Controls
{
    public class CanvasItemsControl : ItemsControl
    {
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

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GraphicInstance graphicInstance = item as GraphicInstance;
            ContentPresenter contentPresenter = element as ContentPresenter;
            if (graphicInstance.LogicalInstance != null)
            {
                TypeLoader.Instance.Request(() =>
                contentPresenter.ContentTemplate = Create(TypesHelper.GetType(graphicInstance.LogicalInstance.Type)));
            }
            contentPresenter.Loaded += new RoutedEventHandler(contentPresenter_Loaded);
        }

        private void contentPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            GraphicInstance graphicInstance = contentPresenter.Content as GraphicInstance;
            TypesHelper.InitProperties(graphicInstance);
            Canvas itemsCanvas = VisualTreeHelper.GetChild(contentPresenter, 0) as Canvas;
            ObjectType objectType = VisualTreeHelper.GetChild(itemsCanvas, 0) as ObjectType;
            SetPropertyBindings(graphicInstance, objectType);
            objectType.MouseLeftButtonDown += new MouseButtonEventHandler(objectType_MouseLeftButtonDown);
            Binding bindingSelected = new Binding();
            bindingSelected.Mode = BindingMode.TwoWay;
            bindingSelected.Path = new PropertyPath("SelectedGraphicInstance");
            bindingSelected.Source = UIManager.Instance;
            bindingSelected.Converter = new IdToSelectedConverter(objectType);
            objectType.SetBinding(ObjectType.SelectedProperty, bindingSelected);
            objectType.ZIndexChanged += new ZIndexChangedHandler(objectType_ZIndexChanged);
        }

        private void objectType_ZIndexChanged(ObjectType objectType, int newZIndex)
        {
            foreach (var item in GetChildren(objectType))
            {
                item.SetZIndex(newZIndex);
            }
        }

        private IEnumerable<ObjectType> GetChildren(ObjectType parent)
        {
            NodeInstance nodeInstance = parent.DataContext as NodeInstance;
            if (nodeInstance != null)
            {
                foreach (var child in nodeInstance.Children)
                {
                    yield return GetObjectTypes().Single(item => item.DataContext == child);
                }                
            }
        }

        private void objectType_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIManager.Instance.SelectedGraphicInstance = (sender as ObjectType).DataContext as GraphicInstance;
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
                    <controls:" + type.Name + @" Canvas.Left=""{Binding X, Mode=TwoWay}"" Canvas.Top=""{Binding Y, Mode=TwoWay}"" " + GetTypeSpecificBinding(type) +  @" />
                </Canvas>
                </DataTemplate>";
            return (DataTemplate)XamlReader.Load(xaml);
        }

        private string GetTypeSpecificBinding(Type type)
        {
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                return @"Width=""{Binding Width, Mode=TwoWay}"" Height=""{Binding Height, Mode=TwoWay}"" LinksFrom=""{Binding LinksFrom, Mode=TwoWay}"" LinksTo=""{Binding LinksTo, Mode=TwoWay}""";
            }
            else
            {
                return @"X2=""{Binding Width, Mode=TwoWay}"" Y2=""{Binding Height, Mode=TwoWay}"" PortTo=""{Binding PortTo, Mode=TwoWay}"" PortFrom=""{Binding PortFrom, Mode=TwoWay}"" NodeTo=""{Binding NodeTo, Mode=TwoWay}"" NodeFrom=""{Binding NodeFrom, Mode=TwoWay}""";
            }
        }
    }
}

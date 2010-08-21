﻿using System;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System.Linq;
using ObjectTypes;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Data;
using QReal.Ria.Database;
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
            LogicalInstance logicalInstance = graphicInstance.LogicalInstance;
            if (logicalInstance != null)
            {
                TypeLoader.Instance.Request(() =>
                contentPresenter.ContentTemplate = Create(TypesHelper.GetType(logicalInstance.Type)));
            }
            contentPresenter.Loaded += new RoutedEventHandler(ContentPresenterLoaded);
        }

        private void ContentPresenterLoaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            GraphicInstance graphicInstance = contentPresenter.Content as GraphicInstance;
            LogicalInstance logicalInstance = graphicInstance.LogicalInstance;
            TypesHelper.InitProperties(logicalInstance);
            Canvas itemsCanvas = VisualTreeHelper.GetChild(contentPresenter, 0) as Canvas;
            ObjectType objectType = VisualTreeHelper.GetChild(itemsCanvas, 0) as ObjectType;
            SetPropertyBindings(logicalInstance, objectType);
            objectType.Clicked += new ClickHandler(ObjectTypeClicked);
            Binding bindingSelected = new Binding
                                          {
                                              Mode = BindingMode.TwoWay,
                                              Path = new PropertyPath("SelectedGraphicInstance"),
                                              Source = UIManager.Instance,
                                              Converter = new IdToSelectedConverter(objectType)
                                          };
            objectType.SetBinding(ObjectType.SelectedProperty, bindingSelected);
            objectType.ZIndexChanged += new ZIndexChangedHandler(ObjectTypeZIndexChanged);
        }

        private void ObjectTypeZIndexChanged(ObjectType objectType, int newZIndex)
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
                foreach (var child in nodeInstance.NodeChildren)
                {
                    yield return GetObjectTypes().Single(item => item.DataContext == child);
                }
            }
        }

        private static void ObjectTypeClicked(ObjectType sender)
        {
            UIManager.Instance.SelectedGraphicInstance = sender.DataContext as GraphicInstance;
        }

        private static void SetPropertyBindings(LogicalInstance logicalInstance, ObjectType objectType)
        {
            Type type = objectType.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(DependencyProperty))
                {
                    string propertyName = field.Name.Substring(0, field.Name.LastIndexOf("Property"));
                    InstanceProperty instanceProperty = logicalInstance.InstanceProperties.Single(item => item.Name == propertyName);
                    Binding binding = new Binding
                                          {
                                              Path = new PropertyPath("Value"),
                                              Mode = BindingMode.TwoWay,
                                              Source = instanceProperty
                                          };
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

        private static DataTemplate Create(Type type)
        {
            string xaml = @"
                <ResourceDictionary 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:d=""http://schemas.microsoft.com/expression/blend/2008"" 
                    xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
                    mc:Ignorable=""d"" 
                    xmlns:local=""clr-namespace:QReal""
                    xmlns:controls=""clr-namespace:" + type.Namespace + @";assembly=" + type.Namespace + @""">
                    <DataTemplate x:Key=""ObjectTypeTemplate"">
                    <Canvas HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"">
                        <controls:" + type.Name + @" Canvas.Left=""{Binding GeometryInformation.X, Mode=TwoWay}"" Canvas.Top=""{Binding GeometryInformation.Y, Mode=TwoWay}"" " + GetTypeSpecificBinding(type) + @" />
                    </Canvas>
                    </DataTemplate>
                </ResourceDictionary>";
            ResourceDictionary resourceDictionary = (ResourceDictionary)XamlReader.Load(xaml);
            return resourceDictionary["ObjectTypeTemplate"] as DataTemplate;
        }

        private static string GetTypeSpecificBinding(Type type)
        {
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                return @"Width=""{Binding GeometryInformation.Width, Mode=TwoWay}"" Height=""{Binding GeometryInformation.Height, Mode=TwoWay}""";
            }
            else
            {
                return @"X2=""{Binding GeometryInformation.Width, Mode=TwoWay}"" Y2=""{Binding GeometryInformation.Height, Mode=TwoWay}""";
            }
        }
    }
}
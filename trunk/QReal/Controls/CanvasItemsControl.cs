using System;
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
                contentPresenter.ContentTemplate = Create(TypesManager.Instance.GetType(logicalInstance.Type));
            }
            contentPresenter.Loaded += new RoutedEventHandler(ContentPresenterLoaded);
        }

        private void ContentPresenterLoaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter contentPresenter = sender as ContentPresenter;
            GraphicInstance graphicInstance = contentPresenter.Content as GraphicInstance;
            LogicalInstance logicalInstance = graphicInstance.LogicalInstance;
            TypesManager.Instance.InitProperties(logicalInstance);
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
            SetContextMenu(objectType);
        }

        private static void SetContextMenu(ObjectType objectType)
        {
            MenuItem deleteMenuItemItem = new MenuItem();
            deleteMenuItemItem.Header = "Удалить";
            deleteMenuItemItem.Click += DeleteMenuItemItemClick;
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Items.Add(deleteMenuItemItem);
            ContextMenuService.SetContextMenu(objectType, contextMenu);
        }

        private static void DeleteMenuItemItemClick(object sender, RoutedEventArgs e)
        {
            GraphicInstance graphicInstance = ((sender as MenuItem).DataContext as GraphicInstance);
            InstancesManager.Instance.DeleteInstance(graphicInstance);
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
            if (UIManager.Instance.Canvas == null)
            {
                var itemsPresenter = VisualTreeHelper.GetChild(this, 0);
                var canvas = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
                UIManager.Instance.Canvas = canvas;                
            }
            return base.GetContainerForItemOverride();
        }

        public AutoScroller Autoscroller { get; set; }

        private readonly Dictionary<Type, DataTemplate> myDataTemplates = new Dictionary<Type, DataTemplate>();

        private DataTemplate Create(Type type)
        {
            if (!myDataTemplates.ContainsKey(type))
            {
                string xaml =
                    @"
                <ResourceDictionary 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:d=""http://schemas.microsoft.com/expression/blend/2008"" 
                    xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
                    mc:Ignorable=""d"" 
                    xmlns:local=""clr-namespace:QReal""
                    xmlns:commonControls=""clr-namespace:QReal.Controls;assembly=QReal""
                    xmlns:controls=""clr-namespace:" +
                    type.Namespace + @";assembly=" + type.Namespace +
                    @"""
                    xmlns:ObjectTypes=""clr-namespace:ObjectTypes;assembly=ObjectTypes"">
                    <ObjectTypes:VisibilityConverter x:Key=""VisibilityConverter""/>
                    <commonControls:AbsOrNullConverter x:Key=""AbsOrNullConverter""/>
                    <DataTemplate x:Key=""ObjectTypeTemplate"">
                    <Canvas HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"">"
                    + GetEdgeTypeNegativeCoordinatesXaml(type) +
                    "<controls:" + type.Name +
                    @" Name=""MainControl"" X=""{Binding X, Mode=TwoWay}"" Y=""{Binding Y, Mode=TwoWay}""" +
                    GetTypeSpecificBinding(type) + @" />"
                    + GetSelectRectanglesXaml(type) +
                    @"</Canvas>
                    </DataTemplate>
                </ResourceDictionary>";
                ResourceDictionary resourceDictionary = (ResourceDictionary) XamlReader.Load(xaml);
                myDataTemplates[type] = resourceDictionary["ObjectTypeTemplate"] as DataTemplate;
            }
            return myDataTemplates[type];
        }

        private static string GetEdgeTypeNegativeCoordinatesXaml(Type type)
        {
            if (type.IsSubclassOf(typeof(EdgeType)))
            {
                return @"<Canvas.RenderTransform>
                    <TranslateTransform X=""{Binding Width, Converter={StaticResource AbsOrNullConverter}}"" 
                        Y=""{Binding Height, Converter={StaticResource AbsOrNullConverter}}""/>
                    </Canvas.RenderTransform>";
            }
            return string.Empty;
        }

        private static string GetTypeSpecificBinding(Type type)
        {
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                return @" Canvas.Left=""{Binding X, Mode=TwoWay}"" Canvas.Top=""{Binding Y, Mode=TwoWay}"" Width=""{Binding Width, Mode=TwoWay}"" Height=""{Binding Height, Mode=TwoWay}""";
            }
            else
            {
                return @" Canvas.Left=""{Binding Left, Mode=TwoWay}"" Canvas.Top=""{Binding Top, Mode=TwoWay}"" X2=""{Binding Width, Mode=TwoWay}"" Y2=""{Binding Height, Mode=TwoWay}""";
            }
        }

        private static string GetSelectRectanglesXaml(Type type)
        {
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                return @"<Grid
                    Canvas.Left=""{Binding X, Mode=TwoWay}"" Canvas.Top=""{Binding Y, Mode=TwoWay}""
                    Width=""{Binding Width, Mode=TwoWay}"" Height=""{Binding Height, Mode=TwoWay}""
                    Visibility=""{Binding Selected, ElementName=MainControl, Converter={StaticResource VisibilityConverter}}"">
                    <Rectangle Fill=""Blue"" Width=""5"" Height=""5"" VerticalAlignment=""Top"" HorizontalAlignment=""Left""/>
                    <Rectangle Fill=""Blue"" Width=""5"" Height=""5"" VerticalAlignment=""Top"" HorizontalAlignment=""Right""/>
                    <Rectangle Fill=""Blue"" Width=""5"" Height=""5"" VerticalAlignment=""Bottom"" HorizontalAlignment=""Left""/>
                    <Rectangle Fill=""Blue"" Width=""5"" Height=""5"" VerticalAlignment=""Bottom"" HorizontalAlignment=""Right""/>
                    </Grid>";
            }
            return string.Empty;
        }
    }
}

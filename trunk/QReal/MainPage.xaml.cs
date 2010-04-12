using System.Linq;
using System.Windows.Controls;
using QReal.Web.Database;
using QReal.Ria.Types;
using ObjectTypes;
using System.Windows.Data;
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using QReal.Ria.Database;
using System.Threading;
using System.Windows.Media;
using QReal.Controls;

namespace QReal
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeToolBox();
            UIManager.Instance.MainPage = this;
            UIManager.Instance.SelectedItemChanged += new SelectedItemChangedHandler(Instance_SelectedItemChanged);
        }

        private void Instance_SelectedItemChanged(GraphicInstance newSelectedGraphicInstance)
        {
            if (newSelectedGraphicInstance != null)
            {
                treeView.SelectItem(newSelectedGraphicInstance);
            }
            else
            {
                var selectedContainer = treeView.GetSelectedContainer();
                if (selectedContainer != null)
                {
                    selectedContainer.IsSelected = false;
                }
            }
        }

        private void InitializeToolBox()
        {
            TypeLoader.Instance.Request(delegate()
            {
                toolboxDiagramComboBox.ItemsSource = TypeLoader.Instance.Objects.Keys;
                toolboxDiagramComboBox.SelectedIndex = 0;
            });
        }

        private void toolboxDiagramComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string diagram = (string) toolboxDiagramComboBox.SelectedItem;
            toolboxObjectsDataGrid.ItemsSource = TypeLoader.Instance.Objects[diagram].TypeList;
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InstancesManager.Instance.InstancesContext.SubmitChanges();
        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InstancesManager.Instance.InstancesContext.RejectChanges();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            while (InstancesManager.Instance.InstancesContext.GraphicInstances.Count > 0)
            {
            InstancesManager.Instance.InstancesContext.GraphicInstances.Remove(InstancesManager.Instance.InstancesContext.GraphicInstances.First());
            }
            InstancesManager.Instance.InstancesContext.SubmitChanges();
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UIManager.Instance.SelectedGraphicInstance = treeView.SelectedItem as GraphicInstance;
        }
    }
}

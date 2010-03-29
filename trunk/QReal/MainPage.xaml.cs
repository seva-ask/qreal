using System.Linq;
using System.Windows.Controls;
using QReal.Web.Database;
using QReal.Types;
using ObjectTypes;
using System.Windows.Data;
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using QReal.Database;
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

        private void Instance_SelectedItemChanged(int newId)
        {
            if (newId != -1)
            {
                GraphicInstance instanceToSelect = InstancesManager.Instance.InstancesContext.GraphicInstances.Single(item => item.Id == newId);
                treeView.SelectItem(instanceToSelect);
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
            TypeManager.Instance.Request(delegate()
            {
                toolboxDiagramComboBox.ItemsSource = TypeManager.Instance.Objects.Keys;
                toolboxDiagramComboBox.SelectedIndex = 0;
            });
        }

        private void toolboxDiagramComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string diagram = (string) toolboxDiagramComboBox.SelectedItem;
            toolboxObjectsDataGrid.ItemsSource = TypeManager.Instance.Objects[diagram].TypeList;
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
            GraphicInstance graphicInstance = treeView.SelectedItem as GraphicInstance;
            int selectedId = -1;
            if (graphicInstance != null)
            {
                selectedId = graphicInstance.Id;
            }
            UIManager.Instance.SelectedGraphicInstanceId = selectedId;
        }
    }
}

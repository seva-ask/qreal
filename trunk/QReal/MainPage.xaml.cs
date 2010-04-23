using System.Linq;
using System.Windows.Controls;
using QReal.Web.Database;
using QReal.Ria.Types;
using System.Windows;
using QReal.Ria.Database;
using QReal.Types;

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
            TypeLoader.Instance.Request(() =>
                                            {
                                                toolboxDiagramComboBox.ItemsSource = TypesHelper.Diagrams;
                                                toolboxDiagramComboBox.SelectedIndex = 0;
                                            });
        }

        private void ToolboxDiagramComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string diagram = (string) toolboxDiagramComboBox.SelectedItem;
            toolboxObjectsDataGrid.ItemsSource = TypesHelper.GetDiagramTypes(diagram);
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            InstancesManager.Instance.InstancesContext.SubmitChanges();
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            InstancesManager.Instance.InstancesContext.RejectChanges();
        }

        private void Button3Click(object sender, RoutedEventArgs e)
        {
            while (InstancesManager.Instance.InstancesContext.GraphicInstances.Count > 0)
            {
            InstancesManager.Instance.InstancesContext.GraphicInstances.Remove(InstancesManager.Instance.InstancesContext.GraphicInstances.First());
            }
            InstancesManager.Instance.InstancesContext.SubmitChanges();
        }

        private void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UIManager.Instance.SelectedGraphicInstance = treeView.SelectedItem as GraphicInstance;
        }
    }
}

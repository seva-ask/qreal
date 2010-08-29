using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Controls;
using QReal.Web.Database;
using QReal.Ria.Types;
using System.Windows;
using QReal.Ria.Database;
using QReal.Types;
using System.Windows.Input;

namespace QReal
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            UIManager.Instance.MainPage = this;
            UIManager.Instance.SelectedItemChanged += new SelectedItemChangedHandler(Instance_SelectedItemChanged);
        }

        private void Instance_SelectedItemChanged(Entity newSelectedGraphicInstance)
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

        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                GraphicInstance selectedGraphicInstance = UIManager.Instance.SelectedGraphicInstance;
                if (selectedGraphicInstance != null)
                {
                    InstancesManager.Instance.DeleteInstance(selectedGraphicInstance);
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GraphicInstance graphicInstance = ((sender as MenuItem).DataContext as GraphicInstance);
            InstancesManager.Instance.DeleteInstance(graphicInstance);
        }
    }
}

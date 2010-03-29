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

namespace QReal
{
    public partial class MainPage : UserControl, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            InstancesContext.Instances.PropertyChanged += new PropertyChangedEventHandler(Instances_PropertyChanged);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            InitializeToolBox();
            TestCanvas();
        }

        #region Treeview content loading

        private void timer_Tick(object sender, EventArgs e)
        {
            InstancesContext.Load(InstancesContext.GetInstancesQuery(), LoadBehavior.MergeIntoCurrent, false);
        }

        private void Instances_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName=="Count") && (PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs("TreeviewInstancesSource"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private InstancesContext InstancesContext = new InstancesContext();

        public IEnumerable<Instance> TreeviewInstancesSource
        {
            get
            {
                return InstancesContext.Instances.Where(instance => instance.Parent == null);
            }
        }

        #endregion Treeview content loading

        private void TestCanvas()
        {
            TypeManager.Instance.Request(delegate()
            {
                ObjectType diagram = TypeManager.Instance.Objects["Kernel Diagram"]["Diagram"];
                canvas.Children.Add(diagram);
                ObjectType relation = TypeManager.Instance.Objects["Kernel Diagram"]["Relation"];
                relation.Margin = new System.Windows.Thickness(200,200,0,0);
                canvas.Children.Add(relation);
            });
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
            InstancesContext.SubmitChanges();
        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InstancesContext.RejectChanges();
        }
    }
}

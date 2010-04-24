using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using QReal.Ria.Database;

namespace QReal.Web.Database
{
    public sealed partial class ParentableInstance : Entity
    {
        public ObservableCollection<Entity> Children
        {
            get
            {
                if (myChildren == null)
                {
                    Create();
                }
                return myChildren;
            }
        }

        private void Create()
        {
            OnCreated();
            foreach (var nodeInstance in NodeChildren)
            {
                myChildren.Add(nodeInstance);
            }
            if (RootInstanceInheritance.Any())
            {
                foreach (var edgeInstance in RootInstanceInheritance.Single().EdgeChildren)
                {
                    myChildren.Add(edgeInstance);
                }
            }
        }

        private ObservableCollection<Entity> myChildren;

        partial void OnCreated()
        {
            myChildren = new ObservableCollection<Entity>();
            NodeChildren.EntityAdded += NodeChildren_EntityAdded;
            NodeChildren.EntityRemoved += NodeChildren_EntityRemoved;
            RootInstanceInheritance.EntityAdded += RootInstanceInheritance_EntityAdded;
        }

        private void RootInstanceInheritance_EntityAdded(object sender, EntityCollectionChangedEventArgs<RootInstance> e)
        {
            e.Entity.EdgeChildren.EntityAdded += EdgeChildren_EntityAdded;
            e.Entity.EdgeChildren.EntityRemoved += EdgeChildren_EntityRemoved;
        }

        void EdgeChildren_EntityRemoved(object sender, EntityCollectionChangedEventArgs<EdgeInstance> e)
        {
            Children.Remove(e.Entity);
        }

        private void EdgeChildren_EntityAdded(object sender, EntityCollectionChangedEventArgs<EdgeInstance> e)
        {
            Children.Add(e.Entity);
        }
        
        private void NodeChildren_EntityRemoved(object sender, EntityCollectionChangedEventArgs<NodeInstance> e)
        {
            Children.Remove(e.Entity);
        }

        private void NodeChildren_EntityAdded(object sender, EntityCollectionChangedEventArgs<NodeInstance> e)
        {
            Children.Add(e.Entity);
        }
    }

    public sealed partial class NodeInstance : Entity
    {
        public LogicalInstance LogicalInstance
        {
            get
            {
                return this.GetParent<GraphicInstance>().LogicalInstance;
            }
        }
    }

    public sealed partial class EdgeInstance : Entity
    {
        public LogicalInstance LogicalInstance
        {
            get
            {
                return this.GetParent<GraphicInstance>().LogicalInstance;
            }
        }
    }

    public sealed partial class RootInstance : Entity
    {
        public LogicalInstance LogicalInstance
        {
            get
            {
                return this.GetParent<GraphicInstance>().LogicalInstance;
            }
        }
    }
}

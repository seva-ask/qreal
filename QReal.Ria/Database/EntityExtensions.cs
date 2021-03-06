﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Client;
using QReal.Ria.Database;

namespace QReal.Web.Database
{
    public partial class ParentableInstance
    {
        public ObservableCollection<GraphicInstance> Children
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

        private ObservableCollection<GraphicInstance> myChildren;

        protected virtual void Create()
        {
            myChildren = new ObservableCollection<GraphicInstance>();
            foreach (var nodeInstance in NodeChildren)
            {
                myChildren.Add(nodeInstance);
            }
            NodeChildren.EntityAdded += NodeChildren_EntityAdded;
            NodeChildren.EntityRemoved += NodeChildren_EntityRemoved;
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

    public partial class RootInstance
    {
        protected override void Create()
        {
            base.Create();
            foreach (var edgeInstance in EdgeChildren)
            {
                Children.Add(edgeInstance);
            }
            EdgeChildren.EntityAdded += EdgeChildren_EntityAdded;
            EdgeChildren.EntityRemoved += EdgeChildren_EntityRemoved;
        }

        void EdgeChildren_EntityRemoved(object sender, EntityCollectionChangedEventArgs<EdgeInstance> e)
        {
            Children.Remove(e.Entity);
        }

        private void EdgeChildren_EntityAdded(object sender, EntityCollectionChangedEventArgs<EdgeInstance> e)
        {
            Children.Add(e.Entity);
        }
    }

    public partial class EdgeInstance
    {
        public double Top { get; private set; }
        public double Left { get; private set; }

        partial void OnHeightChanged()
        {
            Top = Y + (Height > 0 ? 0 : Height);
            OnPropertyChanged(new PropertyChangedEventArgs("Top"));
        }

        partial void OnWidthChanged()
        {
            Left = X + (Width > 0 ? 0 : Width);
            OnPropertyChanged(new PropertyChangedEventArgs("Left"));
        }

        partial void OnXChanged()
        {
            Left = X + (Width > 0 ? 0 : Width);
            OnPropertyChanged(new PropertyChangedEventArgs("Left"));
        }

        partial void OnYChanged()
        {
            Top = Y + (Height > 0 ? 0 : Height);
            OnPropertyChanged(new PropertyChangedEventArgs("Top"));
        }
    }
}

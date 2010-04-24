
namespace QReal.Web.Database
{
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;

    // Implements application logic using the QRealEntities context.
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess]
    public class InstancesService : LinqToEntitiesDomainService<QRealEntities>
    {

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<EdgeInstance> GetEdgeInstanceSet()
        {
            return this.ObjectContext.EdgeInstanceSet;
        }

        public void InsertEdgeInstance(EdgeInstance edgeInstance)
        {
            if ((edgeInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(edgeInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.EdgeInstanceSet.AddObject(edgeInstance);
            }
        }

        public void UpdateEdgeInstance(EdgeInstance currentEdgeInstance)
        {
            this.ObjectContext.EdgeInstanceSet.AttachAsModified(currentEdgeInstance, this.ChangeSet.GetOriginal(currentEdgeInstance));
        }

        public void DeleteEdgeInstance(EdgeInstance edgeInstance)
        {
            if ((edgeInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.EdgeInstanceSet.Attach(edgeInstance);
            }
            this.ObjectContext.EdgeInstanceSet.DeleteObject(edgeInstance);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<GeometryInformation> GetGeometryInformationSet()
        {
            return this.ObjectContext.GeometryInformationSet;
        }

        public void InsertGeometryInformation(GeometryInformation geometryInformation)
        {
            if ((geometryInformation.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(geometryInformation, EntityState.Added);
            }
            else
            {
                this.ObjectContext.GeometryInformationSet.AddObject(geometryInformation);
            }
        }

        public void UpdateGeometryInformation(GeometryInformation currentGeometryInformation)
        {
            this.ObjectContext.GeometryInformationSet.AttachAsModified(currentGeometryInformation, this.ChangeSet.GetOriginal(currentGeometryInformation));
        }

        public void DeleteGeometryInformation(GeometryInformation geometryInformation)
        {
            if ((geometryInformation.EntityState == EntityState.Detached))
            {
                this.ObjectContext.GeometryInformationSet.Attach(geometryInformation);
            }
            this.ObjectContext.GeometryInformationSet.DeleteObject(geometryInformation);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<GraphicInstance> GetGraphicInstances()
        {
            return this.ObjectContext.GraphicInstances;
        }

        public void InsertGraphicInstance(GraphicInstance graphicInstance)
        {
            if ((graphicInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(graphicInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.GraphicInstances.AddObject(graphicInstance);
            }
        }

        public void UpdateGraphicInstance(GraphicInstance currentGraphicInstance)
        {
            this.ObjectContext.GraphicInstances.AttachAsModified(currentGraphicInstance, this.ChangeSet.GetOriginal(currentGraphicInstance));
        }

        public void DeleteGraphicInstance(GraphicInstance graphicInstance)
        {
            if ((graphicInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.GraphicInstances.Attach(graphicInstance);
            }
            this.ObjectContext.GraphicInstances.DeleteObject(graphicInstance);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<InstanceProperty> GetInstanceProperties()
        {
            return this.ObjectContext.InstanceProperties;
        }

        public void InsertInstanceProperty(InstanceProperty instanceProperty)
        {
            if ((instanceProperty.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(instanceProperty, EntityState.Added);
            }
            else
            {
                this.ObjectContext.InstanceProperties.AddObject(instanceProperty);
            }
        }

        public void UpdateInstanceProperty(InstanceProperty currentInstanceProperty)
        {
            this.ObjectContext.InstanceProperties.AttachAsModified(currentInstanceProperty, this.ChangeSet.GetOriginal(currentInstanceProperty));
        }

        public void DeleteInstanceProperty(InstanceProperty instanceProperty)
        {
            if ((instanceProperty.EntityState == EntityState.Detached))
            {
                this.ObjectContext.InstanceProperties.Attach(instanceProperty);
            }
            this.ObjectContext.InstanceProperties.DeleteObject(instanceProperty);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<LogicalInstance> GetLogicalInstances()
        {
            return this.ObjectContext.LogicalInstances;
        }

        public void InsertLogicalInstance(LogicalInstance logicalInstance)
        {
            if ((logicalInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(logicalInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.LogicalInstances.AddObject(logicalInstance);
            }
        }

        public void UpdateLogicalInstance(LogicalInstance currentLogicalInstance)
        {
            this.ObjectContext.LogicalInstances.AttachAsModified(currentLogicalInstance, this.ChangeSet.GetOriginal(currentLogicalInstance));
        }

        public void DeleteLogicalInstance(LogicalInstance logicalInstance)
        {
            if ((logicalInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.LogicalInstances.Attach(logicalInstance);
            }
            this.ObjectContext.LogicalInstances.DeleteObject(logicalInstance);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<NodeInstance> GetNodeInstanceSet()
        {
            return this.ObjectContext.NodeInstanceSet;
        }

        public void InsertNodeInstance(NodeInstance nodeInstance)
        {
            if ((nodeInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nodeInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.NodeInstanceSet.AddObject(nodeInstance);
            }
        }

        public void UpdateNodeInstance(NodeInstance currentNodeInstance)
        {
            this.ObjectContext.NodeInstanceSet.AttachAsModified(currentNodeInstance, this.ChangeSet.GetOriginal(currentNodeInstance));
        }

        public void DeleteNodeInstance(NodeInstance nodeInstance)
        {
            if ((nodeInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.NodeInstanceSet.Attach(nodeInstance);
            }
            this.ObjectContext.NodeInstanceSet.DeleteObject(nodeInstance);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<ParentableInstance> GetParentableInstanceSet()
        {
            return this.ObjectContext.ParentableInstanceSet;
        }

        public void InsertParentableInstance(ParentableInstance parentableInstance)
        {
            if ((parentableInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(parentableInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ParentableInstanceSet.AddObject(parentableInstance);
            }
        }

        public void UpdateParentableInstance(ParentableInstance currentParentableInstance)
        {
            this.ObjectContext.ParentableInstanceSet.AttachAsModified(currentParentableInstance, this.ChangeSet.GetOriginal(currentParentableInstance));
        }

        public void DeleteParentableInstance(ParentableInstance parentableInstance)
        {
            if ((parentableInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.ParentableInstanceSet.Attach(parentableInstance);
            }
            this.ObjectContext.ParentableInstanceSet.DeleteObject(parentableInstance);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<RootInstance> GetRootInstanceSet()
        {
            return this.ObjectContext.RootInstanceSet;
        }

        public void InsertRootInstance(RootInstance rootInstance)
        {
            if ((rootInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(rootInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.RootInstanceSet.AddObject(rootInstance);
            }
        }

        public void UpdateRootInstance(RootInstance currentRootInstance)
        {
            this.ObjectContext.RootInstanceSet.AttachAsModified(currentRootInstance, this.ChangeSet.GetOriginal(currentRootInstance));
        }

        public void DeleteRootInstance(RootInstance rootInstance)
        {
            if ((rootInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.RootInstanceSet.Attach(rootInstance);
            }
            this.ObjectContext.RootInstanceSet.DeleteObject(rootInstance);
        }
    }
}



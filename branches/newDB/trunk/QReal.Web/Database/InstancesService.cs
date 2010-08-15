
namespace QReal.Web.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Implements application logic using the QRealEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class InstancesService : LinqToEntitiesDomainService<QRealEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'GeometryInformations' query.
        public IQueryable<GeometryInformation> GetGeometryInformations()
        {
            return this.ObjectContext.GeometryInformations;
        }

        public void InsertGeometryInformation(GeometryInformation geometryInformation)
        {
            if ((geometryInformation.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(geometryInformation, EntityState.Added);
            }
            else
            {
                this.ObjectContext.GeometryInformations.AddObject(geometryInformation);
            }
        }

        public void UpdateGeometryInformation(GeometryInformation currentGeometryInformation)
        {
            this.ObjectContext.GeometryInformations.AttachAsModified(currentGeometryInformation, this.ChangeSet.GetOriginal(currentGeometryInformation));
        }

        public void DeleteGeometryInformation(GeometryInformation geometryInformation)
        {
            if ((geometryInformation.EntityState == EntityState.Detached))
            {
                this.ObjectContext.GeometryInformations.Attach(geometryInformation);
            }
            this.ObjectContext.GeometryInformations.DeleteObject(geometryInformation);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'GraphicInstances' query.
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

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'InstanceProperties' query.
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

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'LogicalInstances' query.
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
    }
}



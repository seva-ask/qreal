using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using System.Linq;
using System.Threading;
using QReal.Web.Database;

namespace QReal.Ria.Database
{
    public static class CastHelper
    {
        public static TParent GetParent<TParent>(this Entity child) where TParent : Entity
        {
            PropertyInfo propertyParent = child.GetType().GetProperty("InheritanceParent");
            Entity parent = propertyParent.GetValue(child, null) as Entity;
            if (parent == null)
            {
                return null;
                //PropertyInfo propertyId = child.GetType().GetProperty("InheritanceId");
                //int inheritanceId = (int) propertyId.GetValue(child, null);
                //parent = InstancesManager.Instance.InstancesContext.EntityContainer.GetEntitySet<TParent>().Single(
                //    item =>
                //    {
                //        PropertyInfo property = item.GetType().GetProperty("Id");
                //        int id = (int) property.GetValue(item, null);
                //        return id == inheritanceId;
                //    });
            }
            TParent parentCasted = parent as TParent;
            if (parentCasted != null)
            {
                return parentCasted;
            }
            else
            {
                return parent.GetParent<TParent>();
            }
        }

        public static GeometryInformation GetGeometryInformation(this Entity child)
        {
            PropertyInfo property = child.GetType().GetProperty("GeometryInformation");
            return property.GetValue(child, null) as GeometryInformation;
        }

        public static LogicalInstance GetLogicalInstance(this Entity child)
        {
            return child.GetParent<GraphicInstance>().LogicalInstance;
        }
    }
}

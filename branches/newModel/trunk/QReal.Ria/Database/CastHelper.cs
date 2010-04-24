using System;
using System.Net;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using QReal.Web.Database;

namespace QReal.Ria.Database
{
    public static class CastHelper
    {
        public static TParent GetParent<TParent>(this Entity child) where TParent : Entity
        {
            PropertyInfo property = child.GetType().GetProperties().Single(item => item.Name == "InheritanceParent");
            Entity parent = property.GetValue(child, null) as Entity;
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
            PropertyInfo property = child.GetType().GetProperties().Single(item => item.Name == "GeometryInformation");
            return property.GetValue(child, null) as GeometryInformation;
        }

        public static LogicalInstance GetLogicalInstance(this Entity child)
        {
            return child.GetParent<GraphicInstance>().LogicalInstance;
        }
    }
}

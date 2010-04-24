using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Data;
using QReal.Ria.Database;
using QReal.Web.Database;

namespace QReal
{
    public class ChildrenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Entity> result = new List<Entity>();
            IEnumerable<ParentableInstance> parentableInstanceCollection =
                (value as Entity).GetParent<GraphicInstance>().ParentableInstanceInheritance;
            if (parentableInstanceCollection.Count() != 0)
            {
                ParentableInstance parentableInstance = parentableInstanceCollection.Single();
                foreach (var entity in parentableInstance.NodeChildren)
                {
                    result.Add(entity);
                }
                IEnumerable<RootInstance> rootInstanceCollection = parentableInstance.RootInstanceInheritance;
                if (rootInstanceCollection.Count() != 0)
                {
                    RootInstance rootInstance = rootInstanceCollection.Single();
                    foreach (var entity in rootInstance.EdgeChildren)
                    {
                        result.Add(entity);
                    }
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}

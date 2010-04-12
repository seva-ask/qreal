using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using QReal.Ria.Types;
using System.Linq;
using ObjectTypes;
using QReal.Web.Database;
using System.Reflection;

namespace QReal.Types
{
    public static class TypesProxy
    {
        public static IEnumerable<string> Diagrams
        {
            get
            {
                List<string> result = new List<string>();
                foreach (var item in TypeLoader.Instance.Types.Where(type => type.GetInterface(typeof(IDiagram).FullName, false) != null))
	            {
                    result.Add(((IDiagram)Activator.CreateInstance(item)).Name);
	            }
                return result;
            }
        }

        public static IEnumerable<Type> GetDiagramTypes(string diagramName)
        {
            Type diagram = TypeLoader.Instance.Types.Single(type =>
                {
                    if (type.GetInterface(typeof(IDiagram).FullName, false) != null)
	                {
                        return ((IDiagram)Activator.CreateInstance(type)).Name == diagramName;
	                }
                    return false;
                });
            return TypeLoader.Instance.Types.Where(type => ((type.Assembly == diagram.Assembly) && (type.IsSubclassOf(typeof(ObjectType)))));
        }

        public static Type GetType(string typeName)
        {
            return TypeLoader.Instance.Types.Single(item => item.FullName == typeName);
        }

        public static void InitProperties(GraphicInstance graphicInstance)
        {
            Type type = GetType(graphicInstance.LogicalInstance.Type);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(DependencyProperty))
                {
                    string propertyName = field.Name.Substring(0, field.Name.LastIndexOf("Property"));
                    int count = graphicInstance.LogicalInstance.InstanceProperties.Count(property => property.Name == propertyName);
                    if (count == 0)
                    {
                        InstanceProperty instanceProperty = new InstanceProperty();
                        instanceProperty.Name = propertyName;
                        instanceProperty.LogicalInstance = graphicInstance.LogicalInstance;
                        graphicInstance.LogicalInstance.InstanceProperties.Add(instanceProperty);
                    }
                }
            }
        }
    }
}

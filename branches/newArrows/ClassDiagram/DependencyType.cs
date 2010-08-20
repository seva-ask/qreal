using System;
using System.Windows.Media;
using ObjectTypes;
using ObjectTypes.Arrows;

namespace ClassDiagram
{
    public class DependencyType : EdgeType
    {
        public override string TypeName
        {
            get { return "Dependency"; }
        }

        protected override Arrow GetStartArrow()
        {
            return new AssociationArrow();
        }

        public DependencyType()
        {
            MainLine.StrokeDashArray = new DoubleCollection { 2.0, 2.0 };
        }
    }
}

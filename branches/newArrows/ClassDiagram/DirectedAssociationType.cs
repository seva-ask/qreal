using System;
using ObjectTypes;
using ObjectTypes.Arrows;

namespace ClassDiagram
{
    public class DirectedAssociationType : EdgeType
    {
        public override string TypeName
        {
            get { return "Directed Association"; }
        }

        protected override Arrow GetStartArrow()
        {
            return new AssociationArrow();
        }
    }
}

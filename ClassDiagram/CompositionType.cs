using System;
using ObjectTypes;
using ObjectTypes.Arrows;

namespace ClassDiagram
{
    public class CompositionType : EdgeType
    {
        public override string TypeName
        {
            get { return "Composition"; }
        }

        protected override Arrow GetStartArrow()
        {
            return new CompositionArrow();
        }
    }
}

using ObjectTypes;
using ObjectTypes.Arrows;

namespace ClassDiagram
{
    public class GeneralizationType : EdgeType
    {
        public override string TypeName
        {
            get { return "Generalization"; }
        }

        protected override Arrow GetStartArrow()
        {
            return new GeneralizationArrow();
        }
    }
}

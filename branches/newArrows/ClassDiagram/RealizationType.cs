using System.Windows.Media;
using ObjectTypes;
using ObjectTypes.Arrows;

namespace ClassDiagram
{
    public class RealizationType : EdgeType
    {
        public override string TypeName
        {
            get { return "Realization"; }
        }

        protected override Arrow GetStartArrow()
        {
            return new GeneralizationArrow();
        }

        public RealizationType()
        {
            MainLine.StrokeDashArray = new DoubleCollection { 2.0, 2.0 };
        }
    }
}

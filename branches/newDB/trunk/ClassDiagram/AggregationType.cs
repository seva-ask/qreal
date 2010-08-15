using ObjectTypes;
using ObjectTypes.Arrows;

namespace ClassDiagram
{
    public class AggregationType : EdgeType
    {
        public override string TypeName
        {
            get { return "Aggregation"; }
        }

        protected override Arrow GetStartArrow()
        {
            return new AggregationArrow();
        }
    }
}

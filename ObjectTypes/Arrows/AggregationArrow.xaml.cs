namespace ObjectTypes.Arrows
{
    public partial class AggregationArrow : Arrow
    {
        public AggregationArrow()
        {
            InitializeComponent();
        }

        public override bool IsMainLineVisibleUnderArrow
        {
            get
            {
                return false;
            }
        }
    }
}

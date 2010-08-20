namespace ClassDiagram
{
    public partial class ClassDiagramType
    {
        public ClassDiagramType()
        {
            InitializeComponent();
        }

        public override string TypeName
        {
            get { return "Class Diagram"; }
        }

        public override bool CanBeRootItem
        {
            get
            {
                return true;
            }
        }
    }
}

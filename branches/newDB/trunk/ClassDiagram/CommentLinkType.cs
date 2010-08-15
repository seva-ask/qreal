using System;
using System.Windows.Media;
using ObjectTypes;

namespace ClassDiagram
{
    public class CommentLinkType : EdgeType
    {
        public override string TypeName
        {
            get { return "Comment Link"; }
        }

        public CommentLinkType()
        {
            MainLine.StrokeDashArray = new DoubleCollection {2.0, 2.0};
        }
    }
}

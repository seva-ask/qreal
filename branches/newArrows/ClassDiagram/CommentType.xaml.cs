using System;
using System.Windows;

namespace ClassDiagram
{
    public partial class CommentType
    {
        public CommentType()
        {
            InitializeComponent();
        }

        public override string TypeName
        {
            get { return "Comment"; }
        }

        public string Body
        {
            get { return (string) GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof (string), typeof (CommentType), null);
    }
}

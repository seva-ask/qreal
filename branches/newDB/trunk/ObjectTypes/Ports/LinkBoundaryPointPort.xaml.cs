using System.Windows.Controls.Primitives;

namespace ObjectTypes.Ports
{
    public partial class LinkBoundaryPointPort : Port
    {
        public event DragCompletedEventHandler DragCompleted;
        public event DragDeltaEventHandler DragDelta;
        public event DragStartedEventHandler DragStarted;

        public LinkBoundaryPointPort()
        {
            InitializeComponent();
            thumb.DragCompleted += new DragCompletedEventHandler(ThumbDragCompleted);
            thumb.DragDelta += new DragDeltaEventHandler(ThumbDragDelta);
            thumb.DragStarted += new DragStartedEventHandler(ThumbDragStarted);
        }

        private void ThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            if (DragStarted != null)
            {
                DragStarted(sender, e);
            }
        }

        private void ThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (DragDelta != null)
            {
                DragDelta(sender, e);
            }
        }

        private void ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (DragCompleted != null)
            {
                DragCompleted(sender, e);
            }
        }
    }
}

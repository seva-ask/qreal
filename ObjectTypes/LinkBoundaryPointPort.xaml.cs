using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace ObjectTypes
{
    public partial class LinkBoundaryPointPort : Port
    {
        public event DragCompletedEventHandler DragCompleted;
        public event DragDeltaEventHandler DragDelta;
        public event DragStartedEventHandler DragStarted;

        public LinkBoundaryPointPort()
        {
            InitializeComponent();
            thumb.DragCompleted += new DragCompletedEventHandler(thumb_DragCompleted);
            thumb.DragDelta += new DragDeltaEventHandler(thumb_DragDelta);
            thumb.DragStarted += new DragStartedEventHandler(thumb_DragStarted);
        }

        private void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (DragStarted != null)
            {
                DragStarted(sender, e);
            }
        }

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (DragDelta != null)
            {
                DragDelta(sender, e);
            }
        }

        private void thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (DragCompleted != null)
            {
                DragCompleted(sender, e);
            }
        }
    }
}

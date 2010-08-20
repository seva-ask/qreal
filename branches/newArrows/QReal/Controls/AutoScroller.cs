using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace QReal.Controls
{

    /// <summary>
    /// Enables automatic scrolling on a targetted ScrollViewer. 
    /// </summary>
    public class AutoScroller
    {
        private System.Windows.Threading.DispatcherTimer myDispatcherTimer;
        private ScrollViewer myTargetScrollViewer;
        private ObservableCollection<FrameworkElement> myDraggableObjects;
        private double myTestoffset;
        private double myTestvoffset;

        private Canvas myTargetCanvas;
        /// <summary>
        /// Sets the canvas where objects are dragged/dropped. If null, it will default to the child of TargetScrollViewer when AutoScroll is set to Mode.Drag.
        /// </summary>
        public Canvas TargetCanvas
        {
            get
            {
                return myTargetCanvas;
            }
            set
            {
                myTargetCanvas = value;
                myDraggableObjects = new ObservableCollection<FrameworkElement>();
                foreach (FrameworkElement f in myTargetCanvas.Children)
                {
                    myDraggableObjects.Add(f);
                }
            }
        }

        /// <summary>
        /// Sets the mode of automatic scrolling for the targetted ScrollViewer. 
        /// </summary>
        public enum Mode
        {
            /// <summary>
            ///  Off: No automatic scrolling.
            /// </summary>
            Off,
            /// <summary>
            /// Auto: Scrolls when the cursor is at the edge of the ScrollViewer. 
            /// </summary>
            Auto,
            /// <summary>
            /// Drag: Scrolls when the the mouse is dragged at the edge of the ScrollViewer.
            /// </summary>
            Drag
        };

        #region properties

        /// <summary>
        /// Defines the width (in pixels) of the zone at the edge of the ScrollViewer that will trigger automatic scrolling. Default is 40.
        /// </summary>
        public double ScrollArea { get; set; }

        /// <summary>
        /// The number of pixels that will be scrolled per 100 milliseconds when scrolling is activated. Default is 5.
        /// </summary>
        public double ScrollPixelsPerTick { get; set; }

        private Mode myAutoScroll;
        /// <summary>
        /// Sets the mode of automatic scrolling for the targetted ScrollViewer. Auto: Scrolls when the cursor is at the edge of the ScrollViewer. Drag: Scrolls when the the mouse is dragged at the edge of the ScrollViewer.
        /// </summary>
        public Mode AutoScroll
        {
            get
            {
                return myAutoScroll;
            }
            set
            {
                if (myAutoScroll == Mode.Auto)
                {
                    myTargetScrollViewer.MouseMove -= AutoScrollViewer_MouseMove;
                    myTargetScrollViewer.MouseLeave -= AutoScrollViewer_MouseLeave;
                }
                if (myAutoScroll == Mode.Drag)
                {

                    foreach (FrameworkElement f in TargetCanvas.Children)
                    {
                        f.MouseLeftButtonDown -= new MouseButtonEventHandler(F_MouseLeftButtonDown);
                    }
                    TargetCanvas.LayoutUpdated -= new EventHandler(TargetCanvas_LayoutUpdated);
                }
                if (value == Mode.Auto)
                {
                    myTargetScrollViewer.MouseMove += AutoScrollViewer_MouseMove;
                    myTargetScrollViewer.MouseLeave += AutoScrollViewer_MouseLeave;
                }
                if (value == Mode.Drag)
                {
                    if (TargetCanvas == null)
                    {
                        if (myTargetScrollViewer.Content.GetType().ToString() == "System.Windows.Controls.Canvas")
                        {
                            TargetCanvas = ((Canvas)(myTargetScrollViewer.Content));
                        }
                    }
                    foreach (FrameworkElement f in TargetCanvas.Children)
                    {
                        f.MouseLeftButtonDown += new MouseButtonEventHandler(F_MouseLeftButtonDown);
                    }
                    TargetCanvas.LayoutUpdated += new EventHandler(TargetCanvas_LayoutUpdated);

                }
                myAutoScroll = value;

            }
        }

        void TargetCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            if (myDraggableObjects.Count != TargetCanvas.Children.Count)
            {
                foreach (FrameworkElement f in TargetCanvas.Children)
                {
                    if (myDraggableObjects.Contains(f) == false)
                    {
                        f.MouseLeftButtonDown += new MouseButtonEventHandler(F_MouseLeftButtonDown);
                        myDraggableObjects.Add(f);
                    }
                }
            }
        }

        private FrameworkElement DraggedObject
        { get; set; }

        private Boolean myScrollLeft;

        /// <summary>
        /// Causes the targetted ScrollViewer to scroll left, at a rate defined by the ScrollPixelsPerTick property.
        /// </summary>
        public Boolean ScrollLeft
        {
            get
            {
                return myScrollLeft;
            }
            set
            {
                if (value)
                {
                    if (ScrollUp == ScrollDown == ScrollRight == myScrollLeft == false)
                        StartTimer();
                }
                else
                {
                    if (ScrollUp == ScrollDown == ScrollRight == false)
                        StopTimer();
                }
                myScrollLeft = value;

            }
        }

        private Boolean myScrollRight;
        /// <summary>
        /// Causes the targetted ScrollViewer to scroll right, at a rate defined by the ScrollPixelsPerTick property.
        /// </summary>
        public Boolean ScrollRight
        {
            get
            {
                return myScrollRight;
            }
            set
            {
                if (value)
                {
                    if (ScrollUp == ScrollDown == ScrollRight == myScrollLeft == false)
                        StartTimer();
                }
                else
                {
                    if (ScrollUp == ScrollDown == ScrollLeft == false)
                        StopTimer();
                }
                myScrollRight = value;

            }
        }

        private Boolean myScrollUp;
        /// <summary>
        /// Causes the targetted ScrollViewer to scroll up, at a rate defined by the ScrollPixelsPerTick property.
        /// </summary>
        public Boolean ScrollUp
        {
            get
            {
                return myScrollUp;
            }
            set
            {
                if (value)
                {
                    if (ScrollUp == ScrollDown == ScrollRight == myScrollLeft == false)
                        StartTimer();
                }
                else
                {
                    if (ScrollRight == ScrollDown == ScrollLeft == false)
                        StopTimer();
                }
                myScrollUp = value;

            }
        }

        private Boolean myScrollDown;
        /// <summary>
        /// Causes the targetted ScrollViewer to scroll down, at a rate defined by the ScrollPixelsPerTick property.
        /// </summary>
        public Boolean ScrollDown
        {
            get
            {
                return myScrollDown;
            }
            set
            {
                if (value)
                {
                    if (ScrollUp == ScrollDown == ScrollRight == myScrollLeft == false)
                        StartTimer();
                }
                else
                {
                    if (ScrollUp == ScrollRight == ScrollLeft == false)
                        StopTimer();
                }
                myScrollDown = value;

            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Enables automatic scrolling on a targetted ScrollViewer. 
        /// </summary>
        /// <param name="targetScrollViewer">
        /// The ScrollViewer to use automatic scrolling.
        /// </param>
        public AutoScroller(ScrollViewer targetScrollViewer)
        {
            this.myTargetScrollViewer = targetScrollViewer;
            ScrollPixelsPerTick = 5;
            ScrollArea = 40;
        }

        /// <summary>
        /// Enables automatic scrolling on a targetted ScrollViewer. 
        /// </summary>
        /// <param name="targetScrollViewer">The ScrollViewer to use automatic scrolling.</param>
        /// <param name="autoScroll">The AutoScroll mode. Auto: Scrolls when the cursor is at the edge of the ScrollViewer. Drag: Scrolls when the the mouse is dragged at the edge of the ScrollViewer.</param>
        public AutoScroller(ScrollViewer targetScrollViewer, Mode autoScroll)
        {
            this.myTargetScrollViewer = targetScrollViewer;
            ScrollPixelsPerTick = 5;
            ScrollArea = 40;
            this.AutoScroll = autoScroll;
        }

        /// <summary>
        /// Enables automatic scrolling on a targetted ScrollViewer.
        /// </summary>
        /// <param name="targetScrollViewer">The ScrollViewer to use automatic scrolling.</param>
        /// <param name="scrollPixelsPerTick">The number of pixels per 100 milliseconds that the ScrollViewer moves while autoscrolling.</param>
        /// <param name="scrollArea">/// Defines the width (in pixels) of the zone at the edge of the ScrollViewer that will trigger automatic scrolling.</param>
        public AutoScroller(ScrollViewer targetScrollViewer, double scrollPixelsPerTick, double scrollArea)
        {
            this.myTargetScrollViewer = targetScrollViewer;
            this.ScrollPixelsPerTick = scrollPixelsPerTick;
            this.ScrollArea = scrollArea;
        }
        /// <summary>
        /// Enables automatic scrolling on a targetted ScrollViewer.
        /// </summary>
        /// <param name="targetScrollViewer">The ScrollViewer to use automatic scrolling.</param>
        /// <param name="scrollPixelsPerTick">The number of pixels per 100 milliseconds that the ScrollViewer moves while autoscrolling.</param>
        /// <param name="scrollArea">/// Defines the width (in pixels) of the zone at the edge of the ScrollViewer that will trigger automatic scrolling.</param>
        /// <param name="autoScroll">The AutoScroll mode. Auto: Scrolls when the cursor is at the edge of the ScrollViewer. Drag: Scrolls when the the mouse is dragged at the edge of the ScrollViewer.</param>
        public AutoScroller(ScrollViewer targetScrollViewer, double scrollPixelsPerTick, double scrollArea, Mode autoScroll)
        {
            this.myTargetScrollViewer = targetScrollViewer;
            myAutoScroll = autoScroll;
            this.ScrollPixelsPerTick = scrollPixelsPerTick;
            this.ScrollArea = scrollArea;
            this.AutoScroll = autoScroll;
        }

        #endregion

        #region events

        private void Each_Tick(object o, EventArgs sender)
        {
            if (ScrollRight)
            {
                if (myTargetScrollViewer.HorizontalOffset == myTargetScrollViewer.ScrollableWidth)
                    ScrollRight = false;
                else
                {
                    myTargetScrollViewer.ScrollToHorizontalOffset(myTargetScrollViewer.HorizontalOffset + ScrollPixelsPerTick);

                    if (DraggedObject != null && myTestoffset != myTargetScrollViewer.HorizontalOffset)
                    {
                        DraggedObject.SetValue(Canvas.LeftProperty, (double)(DraggedObject.GetValue(Canvas.LeftProperty)) + (ScrollPixelsPerTick));
                    }
                    myTestoffset = myTargetScrollViewer.HorizontalOffset;
                }
            }
            if (ScrollLeft)
            {
                if (myTargetScrollViewer.HorizontalOffset == 0)
                {
                    ScrollLeft = false;
                }
                else
                {
                    myTargetScrollViewer.ScrollToHorizontalOffset(myTargetScrollViewer.HorizontalOffset - ScrollPixelsPerTick);
                    if (DraggedObject != null && myTestoffset != myTargetScrollViewer.HorizontalOffset)
                    {
                        DraggedObject.SetValue(Canvas.LeftProperty, (double)(DraggedObject.GetValue(Canvas.LeftProperty)) - (ScrollPixelsPerTick));
                    }
                    myTestoffset = myTargetScrollViewer.HorizontalOffset;
                }
            }
            if (ScrollDown)
            {
                myTargetScrollViewer.ScrollToVerticalOffset(myTargetScrollViewer.VerticalOffset + ScrollPixelsPerTick);
                if (DraggedObject != null && myTestvoffset != myTargetScrollViewer.VerticalOffset)
                {
                    DraggedObject.SetValue(Canvas.TopProperty, (double)(DraggedObject.GetValue(Canvas.TopProperty)) + (ScrollPixelsPerTick));
                }
                myTestvoffset = myTargetScrollViewer.VerticalOffset;
            }
            if (ScrollUp)
            {
                if (myTargetScrollViewer.VerticalOffset == 0)
                {
                    ScrollUp = false;
                }
                else
                {
                    myTargetScrollViewer.ScrollToVerticalOffset(myTargetScrollViewer.VerticalOffset - ScrollPixelsPerTick);
                    if (DraggedObject != null && myTestvoffset != myTargetScrollViewer.VerticalOffset)
                    {
                        DraggedObject.SetValue(Canvas.TopProperty, (double)(DraggedObject.GetValue(Canvas.TopProperty)) - (ScrollPixelsPerTick));
                    }
                    myTestvoffset = myTargetScrollViewer.VerticalOffset;
                }
            }
        }

        private void AutoScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousepos = e.GetPosition(myTargetScrollViewer);

            if (mousepos.X < ScrollArea && myTargetScrollViewer.HorizontalOffset > 0)
                ScrollLeft = true;
            else if (ScrollLeft)
                ScrollLeft = false;

            if (mousepos.Y < ScrollArea && myTargetScrollViewer.VerticalOffset > 0)
                ScrollUp = true;
            else if (ScrollUp)
                ScrollUp = false;

            if (mousepos.X > (myTargetScrollViewer.ActualWidth - ScrollArea))
                ScrollRight = true;
            else if (ScrollRight)
                ScrollRight = false;

            if (mousepos.Y > (myTargetScrollViewer.ActualHeight - ScrollArea))
                ScrollDown = true;
            else if (ScrollDown)
                ScrollDown = false;

        }

        private void AutoScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollUp = ScrollDown = ScrollLeft = ScrollRight = false;
        }

        private void F_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myTestoffset = myTargetScrollViewer.HorizontalOffset;
            myTestvoffset = myTargetScrollViewer.VerticalOffset;
            DraggedObject = (FrameworkElement)sender;
            myTargetScrollViewer.MouseMove += AutoScrollViewer_MouseMove;
            myTargetScrollViewer.MouseLeave += AutoScrollViewer_MouseLeave;
            DraggedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AutoScrollViewer_MouseLeftButtonUp);
        }

        private void AutoScrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myTargetScrollViewer.MouseMove -= AutoScrollViewer_MouseMove;
            myTargetScrollViewer.MouseLeave -= AutoScrollViewer_MouseLeave;
            ScrollLeft = ScrollDown = ScrollUp = ScrollRight = false;
            DraggedObject = null;
        }

        #endregion

        #region Methods

        private void StartTimer()
        {
            myDispatcherTimer = new System.Windows.Threading.DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 100)};
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
            myDispatcherTimer.Start();
        }

        private void StopTimer()
        {
            if (myDispatcherTimer != null)
            {
                myDispatcherTimer.Stop();
                myDispatcherTimer.Tick -= Each_Tick;
            }
        }

        /// <summary>
        /// Clears all references so the object can be picked up by garbage collection.
        /// </summary>
        public void ClearControl()
        {
            this.AutoScroll = Mode.Off;
            ScrollUp = false;
            ScrollRight = false;
            ScrollLeft = false;
            ScrollDown = false;
            myTargetScrollViewer = null;
        }

        #endregion
    }
}
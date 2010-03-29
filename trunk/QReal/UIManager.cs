using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using QReal.Controls;

namespace QReal
{
    public class UIManager
    {
        private UIManager()
        {}

        private static UIManager instance = new UIManager();

        public static UIManager Instance
        {
            get
            {
                return instance;
            }
        }

        public MainPage MainPage { get; set; }

        private Canvas myCanvas;

        public Canvas Canvas
        {
            get
            {
                return myCanvas;
            }
            set
            {
                myCanvas = value;
                AutoScroller autoScroller = new AutoScroller(MainPage.scrollViewer, AutoScroller.Mode.Auto);
                autoScroller.TargetCanvas = myCanvas;
                autoScroller.AutoScroll = AutoScroller.Mode.Drag;
            }
        }
    }
}

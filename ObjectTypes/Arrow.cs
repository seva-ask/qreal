using System.Windows.Controls;

namespace ObjectTypes
{
    public abstract class Arrow : UserControl
    {
        public virtual bool IsMainLineVisibleUnderArrow
        {
            get
            {
                return true;
            }
        }
    }
}

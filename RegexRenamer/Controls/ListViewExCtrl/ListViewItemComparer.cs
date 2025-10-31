using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Controls.ListViewExCtrl
{
    public class ListViewItemComparer<T> : IEqualityComparer<T> where T : ListViewItem
    {
        public bool Equals(T x, T y)
        {
            return x.Tag.Equals(y.Tag);
        }

        public int GetHashCode(T obj)
        {
            return obj.Tag.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Ui.Common
{
    public interface ISelectable
    {
        Rectangle Rectangle { get; set; }
       
        bool Selected { get; set; }
        bool isSelectable { get; set; }
        bool isEnable { get; set; }
    }
}

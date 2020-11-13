using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    class FormSettings
    {
        private Point lastPoint;
        public void folding(Form F)
        {
            F.WindowState = FormWindowState.Minimized;
        }
        public void window_movement_move(Form F, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                F.Left += e.X - lastPoint.X;
                F.Top += e.Y - lastPoint.Y;
            }
        }
        public void window_movement_down(Form F, MouseEventArgs e)
        {
           lastPoint = new Point(e.X, e.Y);
        }
    }
}

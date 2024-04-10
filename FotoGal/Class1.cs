using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FotoGal
{
    internal class Class1
    {
        MiniForm miniForm = new MiniForm();
        public void mouse_left_click(object sender,MouseEventArgs e)
        {
           

                miniForm.Show();
          
        }


        public void mouse_click_right(object sender,MouseEventArgs e)
        {
             if (e.Button == MouseButtons.Right)
             {
                miniForm.Close();

             }
        }
    }
}

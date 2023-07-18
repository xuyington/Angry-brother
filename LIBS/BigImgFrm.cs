using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LIBS.Windows
{
    public partial class BigImgFrm : Form
    {
        public BigImgFrm(Image img)
        {
            InitializeComponent();
            //if (img != null)
            //{
            //    pictureBox1.Width = img.Width*3;
            //    pictureBox1.Height = img.Height*3;
            //    pictureBox1.Image = img;
            //}
            //Bitmap b = new Bitmap(img);
            //pictureBox1.DrawToBitmap(b,new Rectangle (0,0,200,200));
            pictureBox1.Image
                = img;

        }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YouTubeVideos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axShockwaveFlash1.Movie = "http://www.youtube.com/v/kg-z8JfOIKw?autoplay=1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axShockwaveFlash1.Movie = "https://www.youtube.com/v/yg2u_De8j5o?autoplay=1";
        }
    }
}

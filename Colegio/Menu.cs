﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Colegio
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 principal = new Form1();
            principal.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Profesor principal = new Profesor();
            principal.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Grado g = new Grado();
            g.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clase c = new clase();
            c.Show();
        }
    }
}

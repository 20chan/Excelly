using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Excelly
{
    public partial class FViewer : Form
    {
        private string[] _vars;
        public string[] Variables
        {
            get { return _vars; }
            set
            {
                _vars = value;
                RefreshChart();
            }
        }

        public FViewer(string[] vars)
        {
            InitializeComponent();

            Variables = vars;
        }

        private void RefreshChart()
        {
            Dictionary<float, int> table = new List<float>();
            foreach (string var in Variables)
            {
                float f = Convert.ToSingle(var);
                
            }
        }
    }
}
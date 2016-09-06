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
    public partial class FVars : Form
    {
        private List<Variable> vars = new List<Variable>();
        private readonly FMain mainForm;

        public FVars(FMain main)
        {
            InitializeComponent();
            mainForm = main;
            Variable.MainForm = main;
        }

        private void FVars_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void RefreshVars()
        {
            vars = new List<Variable>();
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    var cells = dataGridView1.Rows[i].Cells;
                    Variable v = new Variable(cells[0].Value.ToString(), Convert.ToInt32(cells[1].Value.ToString()), cells[2].Value.ToString(), cells[3].Value.ToString());
                    vars.Add(v);
                }
                catch { }
            }
        }

        public string[] GetVars(string name)
        {
            return vars.Where(v => v.Name == name).First().CalcVar();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            RefreshVars();
        }
    }
}

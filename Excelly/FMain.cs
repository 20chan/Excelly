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
    public partial class FMain : Form
    {
        private Dictionary<Point, string> funcCells = new Dictionary<Point, string>();
        private readonly string[] funcNames = new string[]
        {
            "AVERAGE", "SUM", "RAND", "RANDDOUBLE", "VAR", "DEV"
        };
        public FVars varsManager;
        
        public FMain()
        {
            InitializeComponent();
            SetRowsAndCells();
            varsManager = new FVars(this);
        }

        private void SetRowsAndCells()
        {
            this.dgvMain.Rows.Add(3);
        }

        public static string[] GetArgValue(string args, FMain form)
        {
            List<string> result = new List<string>();
            foreach (string arg in args.Split(null))
            {
                if (arg == "")
                    continue;
                else if (arg.StartsWith("{") && arg.EndsWith("}"))
                {
                    result.AddRange(form.varsManager.GetVars(arg.Substring(1, arg.Length - 2)));
                }
                else if (arg.StartsWith("'") && arg.EndsWith("'"))
                {
                    result.Add(arg.Substring(1, arg.Length - 2));
                }
                else if (arg.Length == 1 && char.IsLetter(arg[0]))
                {
                    result.AddRange(form.AllRowCells(arg).Select(a => GetArgValue(a, form)[0]));
                }
                else
                {
                    int x = Convert.ToChar(arg[0]) - 'A', y = Convert.ToInt32(arg.Substring(1));
                    result.Add(form.dgvMain.Rows[y].Cells[x].Value.ToString());
                }
            }

            return result.ToArray();
        }

        public string[] AllRowCells(string column)
        {
            string[] result = new string[dgvMain.Rows.Count-1];
            for (int i = 0; i < dgvMain.Rows.Count - 1; i++)
                result[i] = column + i.ToString();
            return result;
        }

        private string CalcFunc(string s)
        {
            string function = s.Substring(1, s.IndexOf("(") - 1);
            string _args = s.Substring(s.IndexOf("(") + 1, s.IndexOf(")") - s.IndexOf("(") - 1);
            string[] args = GetArgValue(_args, this);

            return CalcFunc(function, args);
        }

        public static string CalcFunc(string function, string[] args)
        {
            if (function == "SUM")
            {
                return CellFunc.Sum(args.Select(a => Convert.ToDouble(a)).ToArray()).ToString();
            }
            if (function == "AVERAGE")
            {
                return CellFunc.Average(args.Select(a => Convert.ToDouble(a)).ToArray()).ToString();
            }
            if (function == "RAND")
            {
                if (args.Length == 0)
                    return CellFunc.RandInt().ToString();
                else if (args.Length == 1)
                    return CellFunc.RandInt(Convert.ToInt32(args[0])).ToString();
                else
                    return CellFunc.RandInt(Convert.ToInt32(args[0]), Convert.ToInt32(args[1])).ToString();
            }
            if (function == "RANDDOUBLE")
            {
                if (args.Length == 0)
                    return CellFunc.RandDouble().ToString();
                else if (args.Length == 1)
                    return CellFunc.RandDouble(Convert.ToDouble(args[0])).ToString();
                else
                    return CellFunc.RandDouble(Convert.ToDouble(args[0]), Convert.ToDouble(args[1])).ToString();
            }
            if (function == "VAR")
            {
                return CellFunc.Variance(args.Select(a => Convert.ToDouble(a)).ToArray()).ToString();
            }
            if (function == "DEV")
            {
                return CellFunc.Deviation(args.Select(a => Convert.ToDouble(a)).ToArray()).ToString();
            }
            return "ERROR";
        }

        /// <summary>
        /// Include Random
        /// </summary>
        private void RefreshFuncCells()
        {
            foreach(var key in funcCells.Keys)
            {
                dgvMain.Rows[key.Y].Cells[key.X].Value = CalcFunc(funcCells[key]);
            }
        }

        /// <summary>
        /// Exclude Random
        /// </summary>
        private void ReCalcCells()
        {
            foreach (var key in funcCells.Keys)
            {
                if(!funcCells[key].StartsWith("=RAND"))
                    dgvMain.Rows[key.Y].Cells[key.X].Value = CalcFunc(funcCells[key]);
            }
        }
        
        private void dgvMain_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(e.ColumnIndex == dgvMain.Columns.Count - 1)
            {
                DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn()
                {
                    HeaderText = ((char)('A' + e.ColumnIndex)).ToString()
                };
                dgvMain.Columns.Insert(e.ColumnIndex, c);
                dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                e.Cancel = true;
            }
        }

        private void dgvMain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvMain.Columns.Count - 1)
            {
                dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex-1];
                dgvMain.BeginEdit(true);
            }
            else
            {
                try
                {
                    if (dgvMain.CurrentCell.Value.ToString().StartsWith("="))
                    {
                        string func = dgvMain.CurrentCell.Value.ToString();
                        Calculator c = new Calculator();
                        var res = c.ParseCode(func.Substring(1));
                        string function = func.Substring(1, func.IndexOf("(") - 1);
                        if (funcNames.Contains(function))
                        {
                            Point p = new Point(e.ColumnIndex, e.RowIndex);
                            if (!funcCells.ContainsKey(p))
                                funcCells.Add(p, func);
                            else
                                funcCells[p] = func;

                            dgvMain.CurrentCell.Value = CalcFunc(func);
                        }
                    }
                    else
                    {
                        if (funcCells.ContainsKey(new Point(e.ColumnIndex, e.RowIndex)))
                            funcCells.Remove(new Point(e.ColumnIndex, e.RowIndex));
                    }
                }
                catch { }
                finally { try { ReCalcCells(); } catch { } }
            }
        }

        private void dgvMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                RefreshFuncCells();
        }

        private void dgvMain_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMain.SelectedCells.Count == 0) return;
            Point p = new Point(dgvMain.CurrentCell.ColumnIndex, dgvMain.CurrentCell.RowIndex);
            if (funcCells.ContainsKey(p))
                cellTextbox.Text = funcCells[p];
            else if (dgvMain.CurrentCell.Value != null)
                cellTextbox.Text = dgvMain.CurrentCell.Value.ToString();
            else
                cellTextbox.Text = "";
        }

        private void cellTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                dgvMain.CurrentCell.Value = cellTextbox.Text;
                dgvMain_CellEndEdit(dgvMain, new DataGridViewCellEventArgs(dgvMain.CurrentCell.ColumnIndex, dgvMain.CurrentCell.RowIndex));
            }
        }

        private void 변수관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            varsManager.Show();
        }
    }
}

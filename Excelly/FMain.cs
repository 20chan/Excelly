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
        
        public FMain()
        {
            InitializeComponent();
            SetRowsAndCells();
        }

        private void SetRowsAndCells()
        {
            this.dgvMain.Rows.Add(3);
        }
        
        private void RefreshFuncCells(bool includeRandom)
        {
            foreach(var key in funcCells.Keys)
            {
                // TODO: includeRandom이 true일때 랜덤함수를 걸러내야 함
                if (includeRandom)
                    dgvMain.Rows[key.Y].Cells[key.X].Value = Execute(funcCells[key]);
            }
        }

        private string Execute(string expression)
        {
            throw new NotImplementedException();
        }

        #region 이벤트

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
                        throw new NotImplementedException("여기에 함수 계산을 추가");
                    }
                    else
                    {
                        if (funcCells.ContainsKey(new Point(e.ColumnIndex, e.RowIndex)))
                            funcCells.Remove(new Point(e.ColumnIndex, e.RowIndex));
                    }
                }
                catch { }
                finally { try { RefreshFuncCells(false); } catch { } }
            }
        }

        private void dgvMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                RefreshFuncCells(true);
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
            throw new NotImplementedException("변수 관리...차응ㄹ..띄우자..우앵우.ㅇ....");
        }

        #endregion
    }
}

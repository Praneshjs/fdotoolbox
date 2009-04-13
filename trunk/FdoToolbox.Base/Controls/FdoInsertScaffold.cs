#region LGPL Header
// Copyright (C) 2009, Jackie Ng
// http://code.google.com/p/fdotoolbox, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
//
// See license.txt for more/additional licensing information
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FdoToolbox.Core.Feature;
using OSGeo.FDO.Schema;
using OSGeo.FDO.Expression;
using FdoToolbox.Core;

namespace FdoToolbox.Base.Controls
{
    public partial class FdoInsertScaffold : ViewContent, IFdoInsertView
    {
        private FdoInsertScaffoldPresenter _presenter;

        internal FdoInsertScaffold()
        {
            InitializeComponent();
        }

        public FdoInsertScaffold(FdoConnection conn, string className)
            : this()
        {
            _presenter = new FdoInsertScaffoldPresenter(this, conn, className);
        }

        protected override void OnLoad(EventArgs e)
        {
            _presenter.Init();
            base.OnLoad(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.Cancel();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            _presenter.Insert();
        }

        public string ClassName
        {
            get
            {
                return lblTargetClass.Text;
            }
            set
            {
                lblTargetClass.Text = value;
            }
        }

        public void InitializeGrid()
        {
            grdProperties.Rows.Clear();
            grdProperties.Columns.Clear();
            DataGridViewColumn colName = new DataGridViewColumn();
            colName.Name = "COL_NAME";
            colName.HeaderText = "Name";
            colName.ReadOnly = true;
            colName.CellTemplate = new DataGridViewTextBoxCell();

            DataGridViewColumn colValue = new DataGridViewColumn();
            colValue.Name = "COL_VALUE";
            colValue.HeaderText = "Value";
            colValue.CellTemplate = new DataGridViewTextBoxCell();

            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grdProperties.Columns.Add(colName);
            grdProperties.Columns.Add(colValue);

        }

        public void AddDataProperty(DataPropertyDefinition dataDef)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
            nameCell.Value = dataDef.Name;
            nameCell.ToolTipText = "Type: " + dataDef.DataType;
            nameCell.Tag = dataDef;

            DataGridViewCell valueCell = null;
            if (dataDef.ValueConstraint != null && dataDef.ValueConstraint.ConstraintType == PropertyValueConstraintType.PropertyValueConstraintType_List)
            {
                PropertyValueConstraintList list = (dataDef.ValueConstraint as PropertyValueConstraintList);
                DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
                List<string> values = new List<string>();
                foreach (DataValue value in list.ConstraintList)
                {
                    values.Add(value.ToString());
                }
                cc.DataSource = values;
                valueCell = cc;
            }
            else
            {
                switch (dataDef.DataType)
                {
                    case DataType.DataType_BLOB:
                        {
                            DataGridViewTextBoxCell tc = new DataGridViewTextBoxCell();
                            tc.MaxInputLength = dataDef.Length;
                            valueCell = tc;
                        }
                        break;
                    case DataType.DataType_Boolean:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Byte:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_CLOB:
                        {
                            DataGridViewTextBoxCell tc = new DataGridViewTextBoxCell();
                            tc.MaxInputLength = dataDef.Length;
                            valueCell = tc;
                        }
                        break;
                    case DataType.DataType_DateTime:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Decimal:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Double:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Int16:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Int32:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Int64:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_Single:
                        valueCell = new DataGridViewTextBoxCell();
                        break;
                    case DataType.DataType_String:
                        {
                            DataGridViewTextBoxCell tc = new DataGridViewTextBoxCell();
                            tc.MaxInputLength = dataDef.Length;
                            valueCell = tc;
                        }
                        break;
                }
            }
            valueCell.Style.BackColor = dataDef.Nullable ? Color.YellowGreen : Color.White;
            valueCell.Value = dataDef.DefaultValue;
            valueCell.ToolTipText = dataDef.Description;


            row.Cells.Add(nameCell);
            row.Cells.Add(valueCell);

            nameCell.ReadOnly = true;

            grdProperties.Rows.Add(row);

        }

        public void AddGeometricProperty(GeometricPropertyDefinition geomDef)
        {
            if (geomDef.ReadOnly)
                return;

            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
            nameCell.Value = geomDef.Name;
            nameCell.Tag = geomDef;

            DataGridViewCell valueCell = new DataGridViewTextBoxCell();
            valueCell.ToolTipText = "Enter the FGF or WKB geometry text";

            row.Cells.Add(nameCell);
            row.Cells.Add(valueCell);

            nameCell.ReadOnly = true;

            grdProperties.Rows.Add(row);

        }

        public Dictionary<string, ValueExpression> GetValues()
        {
            Dictionary<string, ValueExpression> values = new Dictionary<string, ValueExpression>();
            foreach (DataGridViewRow row in grdProperties.Rows)
            {
                string name = row.Cells[0].Value.ToString();
                PropertyDefinition propDef = row.Cells[0].Tag as PropertyDefinition;
                if (row.Cells[1].Value != null)
                {
                    string str = row.Cells[1].Value.ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        ValueExpression expr = null;
                        if (propDef.PropertyType == PropertyType.PropertyType_DataProperty)
                        {
                            DataPropertyDefinition dp = propDef as DataPropertyDefinition;
                            switch (dp.DataType)
                            {
                                case DataType.DataType_Boolean:
                                    expr = new BooleanValue(Convert.ToBoolean(str));
                                    break;
                                case DataType.DataType_Byte:
                                    expr = new ByteValue(Convert.ToByte(str));
                                    break;
                                case DataType.DataType_DateTime:
                                    expr = new DateTimeValue(Convert.ToDateTime(str));
                                    break;
                                case DataType.DataType_Decimal:
                                    expr = new DecimalValue(Convert.ToDouble(str));
                                    break;
                                case DataType.DataType_Double:
                                    expr = new DoubleValue(Convert.ToDouble(str));
                                    break;
                                case DataType.DataType_Int16:
                                    expr = new Int16Value(Convert.ToInt16(str));
                                    break;
                                case DataType.DataType_Int32:
                                    expr = new Int32Value(Convert.ToInt32(str));
                                    break;
                                case DataType.DataType_Int64:
                                    expr = new Int64Value(Convert.ToInt64(str));
                                    break;
                                case DataType.DataType_Single:
                                    expr = new SingleValue(Convert.ToSingle(str));
                                    break;
                                case DataType.DataType_String:
                                    expr = new StringValue(str);
                                    break;
                                default:
                                    throw new NotSupportedException("Unsupported data type: " + dp.DataType);
                            }
                        }
                        else if (propDef.PropertyType == PropertyType.PropertyType_GeometricProperty)
                        {
                            FdoGeometryFactory fact = FdoGeometryFactory.Instance;
                            OSGeo.FDO.Geometry.IGeometry geom = fact.CreateGeometry(str);
                            byte[] fgf = fact.GetFgf(geom);
                            expr = new GeometryValue(fgf);
                            geom.Dispose();
                        }

                        if (expr != null)
                            values.Add(name, expr);
                    }
                }
            }
            return values;
        }

        public bool UseTransaction
        {
            get
            {
                return chkUseTransaction.Checked;
            }
            set
            {
                chkUseTransaction.Checked = value;
            }
        }

        public bool UseTransactionEnabled
        {
            get
            {
                return chkUseTransaction.Enabled;
            }
            set
            {
                chkUseTransaction.Enabled = value;
            }
        }
    }
}

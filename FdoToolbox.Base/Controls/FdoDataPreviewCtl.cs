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
using ICSharpCode.Core;
using FdoToolbox.Core.Feature;
using FdoToolbox.Base.Services;
using FdoToolbox.Core.ETL.Specialized;

namespace FdoToolbox.Base.Controls
{
    public partial class FdoDataPreviewCtl : UserControl, IViewContent, IFdoDataPreviewView
    {
        private FdoDataPreviewPresenter _presenter;

        internal FdoDataPreviewCtl()
        {
            InitializeComponent();
            ImageList list = new ImageList();
            list.Images.Add(ResourceService.GetBitmap("table"));
            list.Images.Add(ResourceService.GetBitmap("map"));
            resultTab.ImageList = list;
            TAB_GRID.ImageIndex = 0;
            TAB_MAP.ImageIndex = 1;
        }

        public FdoDataPreviewCtl(FdoConnection conn) : this()
        {
            _presenter = new FdoDataPreviewPresenter(this, conn);
        }

        private string _initSchema;
        private string _initClass;

        public FdoDataPreviewCtl(FdoConnection conn, string initialSchema, string initialClass)
            : this(conn)
        {
            _initSchema = initialSchema;
            _initClass = initialClass;
        }

        protected override void OnLoad(EventArgs e)
        {
            _presenter.Init(_initSchema, _initClass);
            base.OnLoad(e);
        }

        public Control ContentControl
        {
            get { return this; }
        }

        public string Title
        {
            get { return ResourceService.GetString("TITLE_DATA_PREVIEW"); }
        }

        public event EventHandler TitleChanged = delegate { };

        public bool CanClose
        {
            get { return true; }
        }

        public bool Close()
        {
            return true;
        }

        public bool Save()
        {
            return true;
        }

        public bool SaveAs()
        {
            return true;
        }

        public event EventHandler ViewContentClosing = delegate { };

        public List<QueryMode> QueryModes
        {
            set 
            { 
                cmbQueryMode.ComboBox.Items.Clear();
                foreach (QueryMode mode in value)
                {
                    cmbQueryMode.ComboBox.Items.Add(mode);
                }
                if (value.Count > 0)
                {
                    cmbQueryMode.SelectedIndex = 0;
                }
            }
        }

        public QueryMode SelectedQueryMode
        {
            get { return (QueryMode)cmbQueryMode.ComboBox.SelectedItem; }
        }

        public IQuerySubView QueryView
        {
            get
            {
                if (queryPanel.Controls.Count > 0)
                    return (IQuerySubView)queryPanel.Controls[0];
                return null;
            }
            set 
            {
                queryPanel.Controls.Clear();
                value.ContentControl.Dock = DockStyle.Fill;
                queryPanel.Controls.Add(value.ContentControl);
            }
        }

        private void cmbQueryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.QueryModeChanged();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            _presenter.ExecuteQuery();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelCurrentQuery();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _presenter.Clear();
        }

        public bool CancelEnabled
        {
            get
            {
                return btnCancel.Enabled;
            }
            set
            {
                btnCancel.Enabled = value;
            }
        }

        public bool ExecuteEnabled
        {
            get
            {
                return btnQuery.Enabled;
            }
            set
            {
                btnQuery.Enabled = value;
            }
        }

        public string ElapsedMessage
        {
            set 
            { 
                lblElapsedTime.Text = value;
                sepElapsed.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string CountMessage
        {
            set 
            { 
                lblResultCount.Text = value; 
                sepCount.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public bool ClearEnabled
        {
            get
            {
                return btnClear.Enabled;
            }
            set
            {
                btnClear.Enabled = value;
            }
        }

        private FdoFeatureTable _table;

        public FdoFeatureTable ResultTable
        {
            get
            {
                return _table;
            }
            set
            {
                if (value == null)
                {
                    _table = null;
                    grdResults.DataSource = null;
                    grdResults.Columns.Clear();
                    grdResults.Rows.Clear();
                    lblElapsedTime.Text = string.Empty;
                }
                else
                {
                    _table = value;
                    grdResults.DataSource = _table;
                    mapCtl.DataSource = _table;
                }
            }
        }

        public bool MapEnabled
        {
            set
            {
                if (value)
                {
                    if (!resultTab.TabPages.Contains(TAB_MAP))
                        resultTab.TabPages.Add(TAB_MAP);
                }
                else
                {
                    resultTab.TabPages.Remove(TAB_MAP);
                }
            }
        }

        public void DisplayError(Exception exception)
        {
            MessageService.ShowError(exception);
        }

        private void saveSdf_Click(object sender, EventArgs e)
        {
            string file = FileService.SaveFile(ResourceService.GetString("TITLE_SAVE_QUERY_RESULT"), ResourceService.GetString("FILTER_SDF_FILE"));
            if (file != null)
            {
                FdoFeatureTable table = this.ResultTable;

                //Ask for class name
                if (string.IsNullOrEmpty(table.TableName))
                {
                    string name = MessageService.ShowInputBox(ResourceService.GetString("TITLE_SAVE_QUERY_AS"), ResourceService.GetString("MSG_SAVE_QUERY_AS"), "QueryResult");
                    while(string.IsNullOrEmpty(name))
                        name = MessageService.ShowInputBox(ResourceService.GetString("TITLE_SAVE_QUERY_AS"), ResourceService.GetString("MSG_SAVE_QUERY_AS"), "QueryResult");

                    table.TableName = name;
                }

                EtlProcessCtl ctl = new EtlProcessCtl(new TableToFlatFile(table, file));
                Workbench.Instance.ShowContent(ctl, ViewRegion.Dialog);
            }
        }
    }
}

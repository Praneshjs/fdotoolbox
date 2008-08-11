#region LGPL Header
// Copyright (C) 2008, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.FDO.Commands.Schema;
using OSGeo.FDO.Schema;

namespace FdoToolbox.Core.Forms
{
    public partial class MultiClassPicker : Form
    {
        internal MultiClassPicker()
        {
            InitializeComponent();
        }

        public MultiClassPicker(ConnectionInfo connInfo)
            : this()
        {
            using (IDescribeSchema desc = connInfo.Connection.CreateCommand(OSGeo.FDO.Commands.CommandType.CommandType_DescribeSchema) as IDescribeSchema)
            {
                FeatureSchemaCollection schemas = desc.Execute();
                BindingSource bs = new BindingSource();
                bs.DataSource = schemas;
                cmbSchema.DataSource = bs;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void cmbSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeatureSchema schema = cmbSchema.SelectedItem as FeatureSchema;
            if (schema != null)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = schema.Classes;
                lstClasses.DataSource = bs;
            }
        }

        public List<ClassDefinition> GetSelectedClasses()
        {
            List<ClassDefinition> list = new List<ClassDefinition>();
            foreach(object obj in lstClasses.SelectedItems)
            {
                list.Add((ClassDefinition)obj);
            }
            return list;
        }

        public static List<ClassDefinition> GetClasses(string title, string prompt, ConnectionInfo connInfo)
        {
            MultiClassPicker diag = new MultiClassPicker(connInfo);
            diag.Text = title;
            diag.lblPrompt.Text = prompt;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                return diag.GetSelectedClasses();
            }
            return null;
        }
    }
}
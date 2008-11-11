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
using FdoToolbox.Lib.ClientServices;
using FdoToolbox.Core;
using FdoToolbox.Core.ClientServices;

namespace FdoToolbox.Lib.Forms
{
    public partial class TaskProgressDlg : Form
    {
        internal TaskProgressDlg()
        {
            InitializeComponent();
        }

        private ITask _Task;

        public TaskProgressDlg(ITask task)
            : this()
        {
            _Task = task;
            progressBar.Style = (_Task.IsCountable) ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
            switch (task.TaskType)
            {
                case TaskType.SpatialBulkCopy:
                    this.Text = "Bulk Copy in progress";
                    break;
                case TaskType.DbJoin:
                    this.Text = "Database join in progress";
                    break;
                case TaskType.DataTableToFeatureClass:
                    this.Text = "Saving table to feature class";
                    break;
                default:
                    this.Text = "Task executing";
                    break;
            }
            _Task.OnTaskMessage += new TaskProgressMessageEventHandler(OnTaskMessage);
            _Task.OnItemProcessed += new TaskPercentageEventHandler(OnItemProcessed);
            _Task.OnLogTaskMessage += new TaskProgressMessageEventHandler(OnLogTaskMessage);
            bgWorker.DoWork += new DoWorkEventHandler(DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            this.Disposed += delegate { bgWorker.Dispose(); };
        }

        void OnLogTaskMessage(object sender, EventArgs<string> e)
        {
            AppConsole.WriteLine("[{0}]: {1}", _Task.TaskType, e.Data);
        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        void OnItemProcessed(object sender, EventArgs<int> e)
        {
            bgWorker.ReportProgress(e.Data);
        }

        void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            if (e.Cancelled)
            {
                lblMessage.Text = "Bulk Copy Cancelled";
            }
            else
            {
                btnOK.Enabled = true;
                btnCancel.Enabled = false;
            }
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _Task.Execute();
            }
            catch (Exception ex)
            {
                OnTaskMessage(this, new EventArgs<string>(ex.Message));
                AppConsole.WriteException(ex);
                bgWorker.CancelAsync();
            }
        }

        void OnTaskMessage(object sender, EventArgs<string> e)
        {
            string msg = e.Data;
            if (lblMessage.InvokeRequired)
                lblMessage.Invoke(new EventHandler(delegate { lblMessage.Text = msg; }));
            else
                lblMessage.Text = msg;
        }

#if DEBUG || TEST
        public override string ToString()
        {
            return "progressdlg";
        }
#endif

        public DialogResult Run()
        {
            bool valid = true;
            try
            {
                _Task.ValidateTaskParameters();
                valid = true;
            }
            catch (TaskValidationException ex)
            {
                AppConsole.Alert("Error in Validating Task", ex.Message + "\n\nTask execution will not proceed");
                AppConsole.WriteException(ex);
                valid = false;
            }
            if (valid)
            {
                bgWorker.RunWorkerAsync();
                return this.ShowDialog();
            }
            else
            {
                return DialogResult.Cancel;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
            _Task.ExecutingThread.Abort();
            //_Task.ExecutingThread.Join();
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
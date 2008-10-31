namespace FdoToolbox.Lib.Forms
{
    partial class DataPropertyDefinitionDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.chkAutoGenerated = new System.Windows.Forms.CheckBox();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numPrecision = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.chkNullable = new System.Windows.Forms.CheckBox();
            this.txtDefault = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDataType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSystem = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpConstraints = new System.Windows.Forms.GroupBox();
            this.pnConstraint = new System.Windows.Forms.Panel();
            this.cmbConstraintType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.grpConstraints.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkReadOnly);
            this.groupBox2.Controls.Add(this.chkAutoGenerated);
            this.groupBox2.Controls.Add(this.numLength);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numPrecision);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numScale);
            this.groupBox2.Controls.Add(this.chkNullable);
            this.groupBox2.Controls.Add(this.txtDefault);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbDataType);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 153);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Extended Properties";
            // 
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.Location = new System.Drawing.Point(100, 47);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(76, 17);
            this.chkReadOnly.TabIndex = 12;
            this.chkReadOnly.Text = "Read-Only";
            this.chkReadOnly.UseVisualStyleBackColor = true;
            this.chkReadOnly.CheckedChanged += new System.EventHandler(this.chkReadOnly_CheckedChanged);
            // 
            // chkAutoGenerated
            // 
            this.chkAutoGenerated.AutoSize = true;
            this.chkAutoGenerated.Location = new System.Drawing.Point(275, 47);
            this.chkAutoGenerated.Name = "chkAutoGenerated";
            this.chkAutoGenerated.Size = new System.Drawing.Size(101, 17);
            this.chkAutoGenerated.TabIndex = 11;
            this.chkAutoGenerated.Text = "Auto-Generated";
            this.chkAutoGenerated.UseVisualStyleBackColor = true;
            this.chkAutoGenerated.CheckedChanged += new System.EventHandler(this.chkAutoGenerated_CheckedChanged);
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point(100, 96);
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size(93, 20);
            this.numLength.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Length";
            // 
            // numPrecision
            // 
            this.numPrecision.Location = new System.Drawing.Point(264, 124);
            this.numPrecision.Name = "numPrecision";
            this.numPrecision.Size = new System.Drawing.Size(112, 20);
            this.numPrecision.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(209, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Precision";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Scale";
            // 
            // numScale
            // 
            this.numScale.Location = new System.Drawing.Point(100, 122);
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(93, 20);
            this.numScale.TabIndex = 5;
            // 
            // chkNullable
            // 
            this.chkNullable.AutoSize = true;
            this.chkNullable.Location = new System.Drawing.Point(195, 47);
            this.chkNullable.Name = "chkNullable";
            this.chkNullable.Size = new System.Drawing.Size(64, 17);
            this.chkNullable.TabIndex = 4;
            this.chkNullable.Text = "Nullable";
            this.chkNullable.UseVisualStyleBackColor = true;
            this.chkNullable.CheckedChanged += new System.EventHandler(this.chkNullable_CheckedChanged);
            // 
            // txtDefault
            // 
            this.txtDefault.Location = new System.Drawing.Point(100, 70);
            this.txtDefault.Name = "txtDefault";
            this.txtDefault.Size = new System.Drawing.Size(276, 20);
            this.txtDefault.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Default Value";
            // 
            // cmbDataType
            // 
            this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.Location = new System.Drawing.Point(100, 20);
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.Size = new System.Drawing.Size(276, 21);
            this.cmbDataType.TabIndex = 1;
            this.cmbDataType.SelectedIndexChanged += new System.EventHandler(this.cmbDataType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Data Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkSystem);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 96);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // chkSystem
            // 
            this.chkSystem.AutoSize = true;
            this.chkSystem.Location = new System.Drawing.Point(100, 71);
            this.chkSystem.Name = "chkSystem";
            this.chkSystem.Size = new System.Drawing.Size(71, 17);
            this.chkSystem.TabIndex = 4;
            this.chkSystem.Text = "Is System";
            this.chkSystem.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(100, 45);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(276, 20);
            this.txtDescription.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(100, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(276, 20);
            this.txtName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(328, 429);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(247, 429);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // grpConstraints
            // 
            this.grpConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConstraints.Controls.Add(this.pnConstraint);
            this.grpConstraints.Controls.Add(this.cmbConstraintType);
            this.grpConstraints.Controls.Add(this.label8);
            this.grpConstraints.Location = new System.Drawing.Point(13, 275);
            this.grpConstraints.Name = "grpConstraints";
            this.grpConstraints.Size = new System.Drawing.Size(390, 148);
            this.grpConstraints.TabIndex = 7;
            this.grpConstraints.TabStop = false;
            this.grpConstraints.Text = "Constraints";
            // 
            // pnConstraint
            // 
            this.pnConstraint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnConstraint.AutoScroll = true;
            this.pnConstraint.Location = new System.Drawing.Point(6, 44);
            this.pnConstraint.Name = "pnConstraint";
            this.pnConstraint.Size = new System.Drawing.Size(378, 98);
            this.pnConstraint.TabIndex = 2;
            // 
            // cmbConstraintType
            // 
            this.cmbConstraintType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConstraintType.FormattingEnabled = true;
            this.cmbConstraintType.Location = new System.Drawing.Point(100, 17);
            this.cmbConstraintType.Name = "cmbConstraintType";
            this.cmbConstraintType.Size = new System.Drawing.Size(276, 21);
            this.cmbConstraintType.TabIndex = 1;
            this.cmbConstraintType.SelectedIndexChanged += new System.EventHandler(this.cmbConstraintType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Type";
            // 
            // DataPropertyDefinitionDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(415, 463);
            this.Controls.Add(this.grpConstraints);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "DataPropertyDefinitionDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Property Definition";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpConstraints.ResumeLayout(false);
            this.grpConstraints.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDefault;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numPrecision;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numScale;
        private System.Windows.Forms.CheckBox chkNullable;
        private System.Windows.Forms.CheckBox chkAutoGenerated;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkReadOnly;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSystem;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox grpConstraints;
        private System.Windows.Forms.Panel pnConstraint;
        private System.Windows.Forms.ComboBox cmbConstraintType;
        private System.Windows.Forms.Label label8;
    }
}
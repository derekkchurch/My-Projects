namespace SchemaMaster
{
    partial class SchemaMaster
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
            this.comboDB = new System.Windows.Forms.ComboBox();
            this.labelDB = new System.Windows.Forms.Label();
            this.labelStarting = new System.Windows.Forms.Label();
            this.comboOldRelease = new System.Windows.Forms.ComboBox();
            this.buttonView = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboOldRevision = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboNewRevision = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboNewRelease = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboDB
            // 
            this.comboDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDB.FormattingEnabled = true;
            this.comboDB.Location = new System.Drawing.Point(88, 21);
            this.comboDB.Name = "comboDB";
            this.comboDB.Size = new System.Drawing.Size(141, 28);
            this.comboDB.TabIndex = 0;
            this.comboDB.SelectedIndexChanged += new System.EventHandler(this.comboDB_SelectedIndexChanged);
            // 
            // labelDB
            // 
            this.labelDB.AutoSize = true;
            this.labelDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDB.Location = new System.Drawing.Point(3, 24);
            this.labelDB.Name = "labelDB";
            this.labelDB.Size = new System.Drawing.Size(83, 20);
            this.labelDB.TabIndex = 1;
            this.labelDB.Text = "Database:";
            this.labelDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStarting
            // 
            this.labelStarting.AutoSize = true;
            this.labelStarting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStarting.Location = new System.Drawing.Point(11, 18);
            this.labelStarting.Name = "labelStarting";
            this.labelStarting.Size = new System.Drawing.Size(72, 20);
            this.labelStarting.TabIndex = 2;
            this.labelStarting.Text = "Release:";
            // 
            // comboOldRelease
            // 
            this.comboOldRelease.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOldRelease.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboOldRelease.FormattingEnabled = true;
            this.comboOldRelease.Location = new System.Drawing.Point(80, 15);
            this.comboOldRelease.Name = "comboOldRelease";
            this.comboOldRelease.Size = new System.Drawing.Size(103, 28);
            this.comboOldRelease.TabIndex = 4;
            this.comboOldRelease.SelectedIndexChanged += new System.EventHandler(this.comboOldRelease_SelectedIndexChanged);
            // 
            // buttonView
            // 
            this.buttonView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonView.Location = new System.Drawing.Point(45, 286);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(138, 30);
            this.buttonView.TabIndex = 6;
            this.buttonView.Text = "View Differences";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(204, 286);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(173, 30);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save Differences";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboOldRevision);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboOldRelease);
            this.groupBox1.Controls.Add(this.labelStarting);
            this.groupBox1.Location = new System.Drawing.Point(244, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 51);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Old Release";
            // 
            // comboOldRevision
            // 
            this.comboOldRevision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOldRevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboOldRevision.FormattingEnabled = true;
            this.comboOldRevision.Location = new System.Drawing.Point(262, 15);
            this.comboOldRevision.Name = "comboOldRevision";
            this.comboOldRevision.Size = new System.Drawing.Size(88, 28);
            this.comboOldRevision.TabIndex = 6;
            this.comboOldRevision.SelectedIndexChanged += new System.EventHandler(this.comboOldRevision_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(190, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Revision:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboNewRevision);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboNewRelease);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(606, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 51);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "New Release";
            // 
            // comboNewRevision
            // 
            this.comboNewRevision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNewRevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboNewRevision.FormattingEnabled = true;
            this.comboNewRevision.Location = new System.Drawing.Point(262, 15);
            this.comboNewRevision.Name = "comboNewRevision";
            this.comboNewRevision.Size = new System.Drawing.Size(88, 28);
            this.comboNewRevision.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(190, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Revision:";
            // 
            // comboNewRelease
            // 
            this.comboNewRelease.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNewRelease.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboNewRelease.FormattingEnabled = true;
            this.comboNewRelease.Location = new System.Drawing.Point(80, 15);
            this.comboNewRelease.Name = "comboNewRelease";
            this.comboNewRelease.Size = new System.Drawing.Size(103, 28);
            this.comboNewRelease.TabIndex = 4;
            this.comboNewRelease.SelectedIndexChanged += new System.EventHandler(this.comboNewRelease_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Release:";
            // 
            // SchemaMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 337);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.labelDB);
            this.Controls.Add(this.comboDB);
            this.Name = "SchemaMaster";
            this.Text = "Schema Master";
            this.Load += new System.EventHandler(this.SchemaMaster_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboDB;
        private System.Windows.Forms.Label labelDB;
        private System.Windows.Forms.Label labelStarting;
        private System.Windows.Forms.ComboBox comboOldRelease;
        private System.Windows.Forms.Button buttonView;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboOldRevision;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboNewRevision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboNewRelease;
        private System.Windows.Forms.Label label3;
    }
}


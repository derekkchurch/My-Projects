namespace ReSearch_Spreadsheet_Builder
{
	partial class Form1
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
			this.comboSiteIDs = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnRun = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnTest = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.txtServerName = new System.Windows.Forms.TextBox();
			this.rdoSQLAuth = new System.Windows.Forms.RadioButton();
			this.rdoWindowsAuth = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtSaveLocation = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnLocation = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboSiteIDs
			// 
			this.comboSiteIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSiteIDs.FormattingEnabled = true;
			this.comboSiteIDs.Location = new System.Drawing.Point(12, 217);
			this.comboSiteIDs.Name = "comboSiteIDs";
			this.comboSiteIDs.Size = new System.Drawing.Size(322, 21);
			this.comboSiteIDs.TabIndex = 15;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 193);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(58, 20);
			this.label5.TabIndex = 14;
			this.label5.Text = "SiteID:";
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(70, 300);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(181, 23);
			this.btnRun.TabIndex = 12;
			this.btnRun.Text = "Generate Re:Search Spreadsheet";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnTest);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.Controls.Add(this.txtUser);
			this.groupBox1.Controls.Add(this.txtServerName);
			this.groupBox1.Controls.Add(this.rdoSQLAuth);
			this.groupBox1.Controls.Add(this.rdoWindowsAuth);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(322, 176);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ODY Server";
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(161, 149);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(136, 23);
			this.btnTest.TabIndex = 10;
			this.btnTest.Text = "Test/Update SiteID List";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.Enabled = false;
			this.txtPassword.Location = new System.Drawing.Point(99, 123);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(198, 20);
			this.txtPassword.TabIndex = 9;
			// 
			// txtUser
			// 
			this.txtUser.Enabled = false;
			this.txtUser.Location = new System.Drawing.Point(99, 96);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(198, 20);
			this.txtUser.TabIndex = 8;
			// 
			// txtServerName
			// 
			this.txtServerName.Location = new System.Drawing.Point(99, 24);
			this.txtServerName.Name = "txtServerName";
			this.txtServerName.Size = new System.Drawing.Size(197, 20);
			this.txtServerName.TabIndex = 7;
			this.txtServerName.TextChanged += new System.EventHandler(this.txtServerName_TextChanged);
			// 
			// rdoSQLAuth
			// 
			this.rdoSQLAuth.AutoSize = true;
			this.rdoSQLAuth.Location = new System.Drawing.Point(99, 73);
			this.rdoSQLAuth.Name = "rdoSQLAuth";
			this.rdoSQLAuth.Size = new System.Drawing.Size(151, 17);
			this.rdoSQLAuth.TabIndex = 6;
			this.rdoSQLAuth.Text = "SQL Server Authentication";
			this.rdoSQLAuth.UseVisualStyleBackColor = true;
			this.rdoSQLAuth.CheckedChanged += new System.EventHandler(this.rdoSQLAuth_CheckedChanged);
			// 
			// rdoWindowsAuth
			// 
			this.rdoWindowsAuth.AutoSize = true;
			this.rdoWindowsAuth.Checked = true;
			this.rdoWindowsAuth.Location = new System.Drawing.Point(99, 50);
			this.rdoWindowsAuth.Name = "rdoWindowsAuth";
			this.rdoWindowsAuth.Size = new System.Drawing.Size(140, 17);
			this.rdoWindowsAuth.TabIndex = 5;
			this.rdoWindowsAuth.TabStop = true;
			this.rdoWindowsAuth.Text = "Windows Authentication";
			this.rdoWindowsAuth.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(25, 123);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Password:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 99);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "User Name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(43, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Name:";
			// 
			// txtSaveLocation
			// 
			this.txtSaveLocation.Enabled = false;
			this.txtSaveLocation.Location = new System.Drawing.Point(12, 274);
			this.txtSaveLocation.Name = "txtSaveLocation";
			this.txtSaveLocation.Size = new System.Drawing.Size(296, 20);
			this.txtSaveLocation.TabIndex = 10;
			this.txtSaveLocation.Text = "D:\\ReSearch_Codes.xlsm";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 251);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 20);
			this.label1.TabIndex = 9;
			this.label1.Text = "Save Location:";
			// 
			// btnLocation
			// 
			this.btnLocation.Location = new System.Drawing.Point(310, 271);
			this.btnLocation.Name = "btnLocation";
			this.btnLocation.Size = new System.Drawing.Size(24, 23);
			this.btnLocation.TabIndex = 16;
			this.btnLocation.Text = "...";
			this.btnLocation.UseVisualStyleBackColor = true;
			this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(346, 330);
			this.Controls.Add(this.btnLocation);
			this.Controls.Add(this.comboSiteIDs);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnRun);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtSaveLocation);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Re:Search Spreadsheet Builder";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboSiteIDs;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.RadioButton rdoSQLAuth;
		private System.Windows.Forms.RadioButton rdoWindowsAuth;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtSaveLocation;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnLocation;
	}
}


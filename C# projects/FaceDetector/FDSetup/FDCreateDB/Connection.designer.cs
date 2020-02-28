namespace FDCreateDB
{
    partial class Connection
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
            this.cbServerName = new System.Windows.Forms.ComboBox();
            this.lServerName = new System.Windows.Forms.Label();
            this.lAuthentication = new System.Windows.Forms.Label();
            this.cbAuthentication = new System.Windows.Forms.ComboBox();
            this.lUserName = new System.Windows.Forms.Label();
            this.lPassword = new System.Windows.Forms.Label();
            this.chbRemPassword = new System.Windows.Forms.CheckBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.ePassword = new System.Windows.Forms.TextBox();
            this.eUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbServerName
            // 
            this.cbServerName.FormattingEnabled = true;
            this.cbServerName.Location = new System.Drawing.Point(136, 78);
            this.cbServerName.Name = "cbServerName";
            this.cbServerName.Size = new System.Drawing.Size(274, 21);
            this.cbServerName.TabIndex = 0;
            this.cbServerName.SelectedIndexChanged += new System.EventHandler(this.cbServerName_SelectedIndexChanged);
            // 
            // lServerName
            // 
            this.lServerName.AutoSize = true;
            this.lServerName.Location = new System.Drawing.Point(24, 81);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(70, 13);
            this.lServerName.TabIndex = 1;
            this.lServerName.Text = "Server name:";
            // 
            // lAuthentication
            // 
            this.lAuthentication.AutoSize = true;
            this.lAuthentication.Location = new System.Drawing.Point(24, 108);
            this.lAuthentication.Name = "lAuthentication";
            this.lAuthentication.Size = new System.Drawing.Size(78, 13);
            this.lAuthentication.TabIndex = 3;
            this.lAuthentication.Text = "Authentication:";
            // 
            // cbAuthentication
            // 
            this.cbAuthentication.FormattingEnabled = true;
            this.cbAuthentication.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cbAuthentication.Location = new System.Drawing.Point(136, 105);
            this.cbAuthentication.Name = "cbAuthentication";
            this.cbAuthentication.Size = new System.Drawing.Size(274, 21);
            this.cbAuthentication.TabIndex = 1;
            this.cbAuthentication.SelectedIndexChanged += new System.EventHandler(this.cbAuthentication_SelectedIndexChanged);
            // 
            // lUserName
            // 
            this.lUserName.AutoSize = true;
            this.lUserName.Location = new System.Drawing.Point(41, 135);
            this.lUserName.Name = "lUserName";
            this.lUserName.Size = new System.Drawing.Size(61, 13);
            this.lUserName.TabIndex = 5;
            this.lUserName.Text = "User name:";
            // 
            // lPassword
            // 
            this.lPassword.AutoSize = true;
            this.lPassword.Location = new System.Drawing.Point(41, 162);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(56, 13);
            this.lPassword.TabIndex = 7;
            this.lPassword.Text = "Password:";
            // 
            // chbRemPassword
            // 
            this.chbRemPassword.AutoSize = true;
            this.chbRemPassword.Location = new System.Drawing.Point(159, 186);
            this.chbRemPassword.Name = "chbRemPassword";
            this.chbRemPassword.Size = new System.Drawing.Size(125, 17);
            this.chbRemPassword.TabIndex = 4;
            this.chbRemPassword.Text = "Remember password";
            this.chbRemPassword.UseVisualStyleBackColor = true;
            // 
            // pBottom
            // 
            this.pBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pBottom.AutoSize = true;
            this.pBottom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pBottom.ForeColor = System.Drawing.SystemColors.Control;
            this.pBottom.Location = new System.Drawing.Point(0, 221);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(453, 1);
            this.pBottom.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(335, 242);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(239, 242);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Connect";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // ePassword
            // 
            this.ePassword.Location = new System.Drawing.Point(159, 159);
            this.ePassword.Name = "ePassword";
            this.ePassword.Size = new System.Drawing.Size(251, 20);
            this.ePassword.TabIndex = 3;
            this.ePassword.UseSystemPasswordChar = true;
            // 
            // eUserName
            // 
            this.eUserName.Location = new System.Drawing.Point(159, 132);
            this.eUserName.Name = "eUserName";
            this.eUserName.Size = new System.Drawing.Size(251, 20);
            this.eUserName.TabIndex = 2;
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 289);
            this.Controls.Add(this.eUserName);
            this.Controls.Add(this.ePassword);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.chbRemPassword);
            this.Controls.Add(this.lPassword);
            this.Controls.Add(this.lUserName);
            this.Controls.Add(this.lAuthentication);
            this.Controls.Add(this.cbAuthentication);
            this.Controls.Add(this.lServerName);
            this.Controls.Add(this.cbServerName);
            this.Name = "Connection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Server";
            this.Load += new System.EventHandler(this.Connection_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Connection_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbServerName;
        private System.Windows.Forms.Label lServerName;
        private System.Windows.Forms.Label lAuthentication;
        private System.Windows.Forms.ComboBox cbAuthentication;
        private System.Windows.Forms.Label lUserName;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.CheckBox chbRemPassword;
        private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox ePassword;
        private System.Windows.Forms.TextBox eUserName;
    }
}
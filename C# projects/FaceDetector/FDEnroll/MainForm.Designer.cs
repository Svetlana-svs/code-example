namespace FDEnroll
{
    partial class MainForm
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
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.btnDeletePerson = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnUpdatePerson = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtFname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gridImages = new System.Windows.Forms.DataGridView();
            this.lblValidator = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelImage = new System.Windows.Forms.Button();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.personIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.bindingSourceImage = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetMain1 = new FDEnroll.DataSetMain();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.templateDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourcePerson = new System.Windows.Forms.BindingSource(this.components);
            this.personTableAdapter = new FDEnroll.DataSetMainTableAdapters.PersonTableAdapter();
            this.imageTableAdapter = new FDEnroll.DataSetMainTableAdapters.ImageTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePerson)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.Location = new System.Drawing.Point(12, 87);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(115, 25);
            this.btnAddPerson.TabIndex = 3;
            this.btnAddPerson.Text = "Add";
            this.btnAddPerson.UseVisualStyleBackColor = true;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // btnDeletePerson
            // 
            this.btnDeletePerson.Enabled = false;
            this.btnDeletePerson.Location = new System.Drawing.Point(254, 87);
            this.btnDeletePerson.Name = "btnDeletePerson";
            this.btnDeletePerson.Size = new System.Drawing.Size(115, 25);
            this.btnDeletePerson.TabIndex = 5;
            this.btnDeletePerson.Text = "Delete";
            this.btnDeletePerson.UseVisualStyleBackColor = true;
            this.btnDeletePerson.Click += new System.EventHandler(this.btnDeletePerson_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.templateDataGridViewImageColumn,
            this.nameDataGridViewTextBoxColumn,
            this.fnameDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bindingSourcePerson;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 170);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(371, 302);
            this.dataGridView1.TabIndex = 11;
            // 
            // btnUpdatePerson
            // 
            this.btnUpdatePerson.Enabled = false;
            this.btnUpdatePerson.Location = new System.Drawing.Point(133, 87);
            this.btnUpdatePerson.Name = "btnUpdatePerson";
            this.btnUpdatePerson.Size = new System.Drawing.Size(115, 25);
            this.btnUpdatePerson.TabIndex = 4;
            this.btnUpdatePerson.Text = "Update";
            this.btnUpdatePerson.UseVisualStyleBackColor = true;
            this.btnUpdatePerson.Click += new System.EventHandler(this.btnUpdatePerson_Click);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Enabled = false;
            this.btnClearFilter.Location = new System.Drawing.Point(254, 49);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(114, 24);
            this.btnClearFilter.TabIndex = 7;
            this.btnClearFilter.Text = "Clear Filter";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(254, 17);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(114, 24);
            this.btnFilter.TabIndex = 6;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtFname
            // 
            this.txtFname.Location = new System.Drawing.Point(65, 52);
            this.txtFname.Name = "txtFname";
            this.txtFname.Size = new System.Drawing.Size(143, 20);
            this.txtFname.TabIndex = 2;
            this.txtFname.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "F.Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(65, 21);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(143, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name:";
            // 
            // btnAddImage
            // 
            this.btnAddImage.Enabled = false;
            this.btnAddImage.Location = new System.Drawing.Point(400, 219);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(71, 24);
            this.btnAddImage.TabIndex = 8;
            this.btnAddImage.Text = "Add photo";
            this.btnAddImage.UseVisualStyleBackColor = true;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(401, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(163, 201);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // gridImages
            // 
            this.gridImages.AllowUserToAddRows = false;
            this.gridImages.AllowUserToDeleteRows = false;
            this.gridImages.AutoGenerateColumns = false;
            this.gridImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn1,
            this.personIdDataGridViewTextBoxColumn,
            this.imageDataGridViewImageColumn});
            this.gridImages.DataSource = this.bindingSourceImage;
            this.gridImages.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridImages.Location = new System.Drawing.Point(401, 249);
            this.gridImages.Name = "gridImages";
            this.gridImages.ReadOnly = true;
            this.gridImages.Size = new System.Drawing.Size(163, 223);
            this.gridImages.TabIndex = 10;
            // 
            // lblValidator
            // 
            this.lblValidator.AutoSize = true;
            this.lblValidator.ForeColor = System.Drawing.Color.Red;
            this.lblValidator.Location = new System.Drawing.Point(9, 115);
            this.lblValidator.MaximumSize = new System.Drawing.Size(370, 0);
            this.lblValidator.Name = "lblValidator";
            this.lblValidator.Size = new System.Drawing.Size(48, 13);
            this.lblValidator.TabIndex = 0;
            this.lblValidator.Text = "Validator";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Persons in Black List:";
            // 
            // btnDelImage
            // 
            this.btnDelImage.Enabled = false;
            this.btnDelImage.Location = new System.Drawing.Point(481, 219);
            this.btnDelImage.Name = "btnDelImage";
            this.btnDelImage.Size = new System.Drawing.Size(83, 24);
            this.btnDelImage.TabIndex = 5;
            this.btnDelImage.Text = "Delete photo";
            this.btnDelImage.UseVisualStyleBackColor = true;
            this.btnDelImage.Click += new System.EventHandler(this.btnDeleteImage_Click);
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn1.HeaderText = "Photo";
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            this.idDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // personIdDataGridViewTextBoxColumn
            // 
            this.personIdDataGridViewTextBoxColumn.DataPropertyName = "Person_Id";
            this.personIdDataGridViewTextBoxColumn.HeaderText = "Person_Id";
            this.personIdDataGridViewTextBoxColumn.Name = "personIdDataGridViewTextBoxColumn";
            this.personIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.personIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // imageDataGridViewImageColumn
            // 
            this.imageDataGridViewImageColumn.DataPropertyName = "Image";
            this.imageDataGridViewImageColumn.HeaderText = "Image";
            this.imageDataGridViewImageColumn.Name = "imageDataGridViewImageColumn";
            this.imageDataGridViewImageColumn.ReadOnly = true;
            this.imageDataGridViewImageColumn.Visible = false;
            // 
            // bindingSourceImage
            // 
            this.bindingSourceImage.DataMember = "Image";
            this.bindingSourceImage.DataSource = this.dataSetMain1;
            this.bindingSourceImage.CurrentChanged += new System.EventHandler(this.bindingSourceImage_CurrentChanged);
            // 
            // dataSetMain1
            // 
            this.dataSetMain1.DataSetName = "DataSetMain";
            this.dataSetMain1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // templateDataGridViewImageColumn
            // 
            this.templateDataGridViewImageColumn.DataPropertyName = "Template";
            this.templateDataGridViewImageColumn.HeaderText = "Template";
            this.templateDataGridViewImageColumn.Name = "templateDataGridViewImageColumn";
            this.templateDataGridViewImageColumn.ReadOnly = true;
            this.templateDataGridViewImageColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fnameDataGridViewTextBoxColumn
            // 
            this.fnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fnameDataGridViewTextBoxColumn.DataPropertyName = "Fname";
            this.fnameDataGridViewTextBoxColumn.HeaderText = "Fname";
            this.fnameDataGridViewTextBoxColumn.Name = "fnameDataGridViewTextBoxColumn";
            this.fnameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bindingSourcePerson
            // 
            this.bindingSourcePerson.DataMember = "Person";
            this.bindingSourcePerson.DataSource = this.dataSetMain1;
            this.bindingSourcePerson.CurrentChanged += new System.EventHandler(this.bindingSourcePerson_CurrentChanged);
            // 
            // personTableAdapter
            // 
            this.personTableAdapter.ClearBeforeFill = true;
            // 
            // imageTableAdapter
            // 
            this.imageTableAdapter.ClearBeforeFill = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 484);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblValidator);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gridImages);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.txtFname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnUpdatePerson);
            this.Controls.Add(this.btnDelImage);
            this.Controls.Add(this.btnDeletePerson);
            this.Controls.Add(this.btnAddPerson);
            this.Name = "MainForm";
            this.Text = "Face Detector Enrollment";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePerson)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.Button btnDeletePerson;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DataSetMain dataSetMain1;
        private System.Windows.Forms.BindingSource bindingSourcePerson;
        private FDEnroll.DataSetMainTableAdapters.PersonTableAdapter personTableAdapter;
        private System.Windows.Forms.Button btnUpdatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn templateDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtFname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView gridImages;
        private System.Windows.Forms.Label lblValidator;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bindingSourceImage;
        private FDEnroll.DataSetMainTableAdapters.ImageTableAdapter imageTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn personIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn imageDataGridViewImageColumn;
        private System.Windows.Forms.Button btnDelImage;
    }
}


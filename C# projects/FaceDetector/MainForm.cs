using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace FDEnroll
{
    public partial class MainForm : Form
    {
        private DataSetMain.PersonRow PersonRow;
        private DataSetMain.ImageRow ImageRow;
        private VerilookUtils VerilookTools;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        { 
            this.personTableAdapter.Connection.ConnectionString = FDSettings.Settings.ConnectionString();
            this.imageTableAdapter.Connection.ConnectionString = this.personTableAdapter.Connection.ConnectionString;

            this.personTableAdapter.Fill(this.dataSetMain1.Person);
            this.lblValidator.Text = "";
            VerilookTools = new VerilookUtils();
            try
            {
                VerilookTools.Initialize();
            }
            catch (Exception ex)
            {
                this.lblValidator.Text = ex.Message;
            }
        }

        //
        // helpers
        //

        private bool DeleteConfirm(string strObject)
        {
            return MessageBox.Show(this, string.Format("Are You sure to delete this {0}?", strObject), string.Format("Deleting of {0}", strObject), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
        }

        private DataRow GetCurrentGridRow(object sender)
        {
            DataRowView rowView = (DataRowView)((BindingSource)sender).Current;
            return rowView != null ? rowView.Row : null;
        }

        private bool TextValidate(TextBox textBox, string lable)
        {
            string strDenied = " ,;";
            if (textBox.Text.Length == 0)
            {
                this.lblValidator.Text = string.Format("The {0} can not be empty", lable);
                return false;
            }
            else if (textBox.Text.IndexOfAny(strDenied.ToCharArray()) >= 0)
            {
                this.lblValidator.Text = string.Format("The {0} can not contain symbols '{1}'", lable, strDenied);
                return false;
            }
            else
                return true;
        }

        //
        // person management
        //

        private void CleaFilterPerson()
        {
            this.bindingSourcePerson.Filter = "";
            this.btnClearFilter.Enabled = false;
        }

        private void bindingSourcePerson_CurrentChanged(object sender, EventArgs e)
        {
            this.PersonRow = (DataSetMain.PersonRow)GetCurrentGridRow(sender);
            if (this.PersonRow != null)
            {
                this.txtName.Text = this.PersonRow.Name;
                this.txtFname.Text = this.PersonRow.Fname;
                this.btnUpdatePerson.Enabled = true;
                this.btnDeletePerson.Enabled = true;
                this.btnAddImage.Enabled = true;
                // fill person images
                this.imageTableAdapter.Fill(this.dataSetMain1.Image, this.PersonRow.Id);
            }
            else
            {
                this.btnUpdatePerson.Enabled = false;
                this.btnDeletePerson.Enabled = false;
                this.btnAddImage.Enabled = false;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (!TextValidate(this.txtName, "Name") || !TextValidate(this.txtFname, "F.Name"))
                return;
            CleaFilterPerson();
            DataSetMain.PersonRow row = this.dataSetMain1.Person.NewPersonRow();
            row.Template = new byte[0];
            row.Name = this.txtName.Text;
            row.Fname = this.txtFname.Text;
            this.dataSetMain1.Person.AddPersonRow(row);
            this.personTableAdapter.Update(row);
        }

        private void btnUpdatePerson_Click(object sender, EventArgs e)
        {
            if (this.PersonRow == null)
                return;
            // validation
            if (!TextValidate(this.txtName, "Name") || !TextValidate(this.txtFname, "F.Name"))
                return;
            DataSetMain.PersonRow row = this.PersonRow;
            string name = this.txtName.Text;
            string fname = this.txtFname.Text;
            string filter = this.bindingSourcePerson.Filter;
            this.bindingSourcePerson.Filter = "";
            row.Name = name;
            row.Fname = fname;
            this.personTableAdapter.Update(row);
            if (filter != null && filter.Length > 0)
                CleaFilterPerson();
        }

        private void btnDeletePerson_Click(object sender, EventArgs e)
        {
            if (this.PersonRow != null && DeleteConfirm("person"))
            {
                this.PersonRow.Delete();
                this.personTableAdapter.Update(this.dataSetMain1.Person);
                CleaFilterPerson();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.lblValidator.Text = "";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.bindingSourcePerson.Filter = string.Format("Name like '%{0}%' and Fname like '%{1}%'", this.txtName.Text, this.txtFname.Text);
            this.btnClearFilter.Enabled = true;
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            CleaFilterPerson();
        }

        //
        // image management
        //

        private void UpdatePersonTemplate()
        {
            if (this.PersonRow == null)
                return;

            this.PersonRow.Template = this.VerilookTools.ExtractTemplate(this.dataSetMain1.Image);
            this.personTableAdapter.Update(this.PersonRow);
        }

        private byte[] LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Stream stream;
                if ((stream = dlg.OpenFile()) != null)
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    stream.Close();
                    return buffer;
                }
            }
            return new byte[0];
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            DataSetMain.ImageRow row = this.dataSetMain1.Image.NewImageRow();
            row.Person_Id = this.PersonRow.Id;
            row.Image = LoadImage();
            if (row.Image.GetLength(0) > 0)
            {
                this.dataSetMain1.Image.AddImageRow(row);
                this.imageTableAdapter.Update(row);
                UpdatePersonTemplate();
            }
        }

        private void bindingSourceImage_CurrentChanged(object sender, EventArgs e)
        {
            this.ImageRow = (DataSetMain.ImageRow)GetCurrentGridRow(sender);
            if (this.ImageRow != null)
            {
                this.btnDelImage.Enabled = true;
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(this.ImageRow.Image));
            }
            else
            {
                this.pictureBox1.Image = null;
                this.ImageRow = null;
                this.btnDelImage.Enabled = false;
            }
        }

        private void btnDeleteImage_Click(object sender, EventArgs e)
        {
            if (this.ImageRow != null && DeleteConfirm("image"))
            {
                this.ImageRow.Delete();
                this.imageTableAdapter.Update(this.dataSetMain1.Image);
                UpdatePersonTemplate();
            }
        }
    }
}
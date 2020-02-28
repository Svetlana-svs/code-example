using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace FDSettings
{
    public partial class MainForm : Form
    {
        private const string fileName = "FDSettings.xml";
        private Hashtable treenodesMap;
        private Hashtable xmlattrMap;
        private XmlDocument xmlDocument;
        private DataSetSettings.TableSettingsRow rowCurrent;

        public MainForm()
        {
            InitializeComponent();
            this.treenodesMap = new Hashtable();
            this.xmlattrMap = new Hashtable();
            this.xmlDocument = new XmlDocument();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.xmlDocument.Load(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Settings file can not be loaded: " + ex.Message, "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.btnSave.Enabled = true;
            this.btnUpdate.Enabled = true;

            XmlElement rootSettings = this.xmlDocument.DocumentElement;
            FillTreeNode(this.xmlDocument.DocumentElement, this.treeView1.Nodes);
        }

        private void FillTreeNode(XmlNode xmlnode, TreeNodeCollection treechildren)
        {
            if (xmlnode.NodeType != XmlNodeType.Element || !xmlnode.HasChildNodes)
                return;
            foreach (XmlNode xmlchild in xmlnode.ChildNodes)
                if (xmlchild.Name != "add") // not setting item
                {
                    // add node to settings tree
                    TreeNode treenode = new TreeNode(xmlchild.Name);
                    treechildren.Add(treenode);
                    this.treenodesMap.Add(treenode, xmlchild); // store relationship between tree node and xml node
                    // fill subtree recursivelly
                    FillTreeNode(xmlchild, treenode.Nodes);
                }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string message;
            MessageBoxIcon icon;
            try
            {
                this.xmlDocument.Save(fileName);
                message = "Setting are successfully saved";
                icon = MessageBoxIcon.Information;
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                icon = MessageBoxIcon.Error;
            }
            MessageBox.Show(this, message, "Save Settings", MessageBoxButtons.OK, icon);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.xmlattrMap.Clear();
            this.dataSetSettings.TableSettings.Clear();
            XmlNode xmlnode = (XmlNode)this.treenodesMap[e.Node];
            if (xmlnode != null)
            {
                // load setting items for this setting container
                foreach (XmlNode xmlchild in xmlnode.ChildNodes)
                    if (xmlchild.Name == "add") // setting item
                    {
                        XmlAttribute attrName = (XmlAttribute)xmlchild.Attributes.GetNamedItem("stname");
                        XmlAttribute attrValue = (XmlAttribute)xmlchild.Attributes.GetNamedItem("stvalue");
                        XmlAttribute attrType = (XmlAttribute)xmlchild.Attributes.GetNamedItem("sttype");
                        XmlAttribute attrSelit = (XmlAttribute)xmlchild.Attributes.GetNamedItem("selitems");
                        if (attrName != null)
                        {
                            if (attrType == null)
                            {
                                attrType = this.xmlDocument.CreateAttribute("sttype");
                                attrType.Value = "text";
                                xmlchild.Attributes.Append(attrType);
                            }
                            if (attrValue == null)
                            {
                                attrValue = this.xmlDocument.CreateAttribute("stvalue");
                                attrValue.Value = "";
                                xmlchild.Attributes.Append(attrValue);
                            }
                            string selItems = attrSelit != null ? attrSelit.Value : null;
                            DataSetSettings.TableSettingsRow row = this.dataSetSettings.TableSettings.NewTableSettingsRow();
                            row.Name = attrName.Value;
                            row.Value = attrValue.Value;
                            row.Type = attrType.Value;
                            row.Selitems = selItems;
                            this.dataSetSettings.TableSettings.AddTableSettingsRow(row);
                            this.xmlattrMap.Add(row, attrValue);
                        }
                    }
            }
        }

        private void GetCurrentGridRow(object sender)
        {
            DataRowView rowView = (DataRowView)((BindingSource)sender).Current;
            this.rowCurrent = rowView != null ? (DataSetSettings.TableSettingsRow)rowView.Row : null;
        }

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            this.comboBoxValue.Items.Clear();
            GetCurrentGridRow(sender);
            if (this.rowCurrent != null)
            {
                this.lblValue.Text = this.rowCurrent.Name;
                switch (this.rowCurrent.Type)
                {
                    case "text":
                        this.checkBoxValue.Visible = false;
                        this.comboBoxValue.Visible = false;
                        this.textBoxValue.Visible = true;
                        this.textBoxValue.Text = this.rowCurrent.Value;
                        break;

                    case "bool":
                        this.checkBoxValue.Visible = true;
                        this.comboBoxValue.Visible = false;
                        this.textBoxValue.Visible = false;
                        this.checkBoxValue.Checked = this.rowCurrent.Value.Equals("true");
                        break;

                    case "select":
                    case "selectnull":
                        if (this.rowCurrent.Type.Equals("selectnull"))
                            this.comboBoxValue.Items.Add("");
                        string[] selItems = this.rowCurrent.Selitems != null && this.rowCurrent.Selitems.Length > 0 ? this.rowCurrent.Selitems.Split(new char[] { ';' }) : new string[0];
                        this.comboBoxValue.Items.AddRange(selItems);
                        this.checkBoxValue.Visible = false;
                        this.comboBoxValue.Visible = true;
                        this.textBoxValue.Visible = false;
                        this.comboBoxValue.SelectedIndex = this.comboBoxValue.FindString(this.rowCurrent.Value);
                        break;

                }
            }
            else
            {
                this.lblValue.Text = "";
                this.checkBoxValue.Visible = false;
                this.comboBoxValue.Visible = false;
                this.textBoxValue.Visible = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.rowCurrent == null)
                return;

            switch(this.rowCurrent.Type)
            {
                case "text":
                    this.rowCurrent.Value = this.textBoxValue.Text;
                    break;

                case "bool":
                    this.rowCurrent.Value = this.checkBoxValue.Checked ? "true" : "false";
                    break;

                case "select":
                    this.rowCurrent.Value = this.comboBoxValue.SelectedItem != null ? (string)this.comboBoxValue.SelectedItem : null;
                    break;

                case "selectnull":
                    this.rowCurrent.Value = this.comboBoxValue.SelectedItem != null && this.comboBoxValue.SelectedIndex > 0 ? (string)this.comboBoxValue.SelectedItem : "";
                    break;
            }

            XmlAttribute xmlAttrValue = (XmlAttribute)this.xmlattrMap[this.rowCurrent];
            if (xmlAttrValue != null)
                xmlAttrValue.Value = this.rowCurrent.Value;
        }
    }
}
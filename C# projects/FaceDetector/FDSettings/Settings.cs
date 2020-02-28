using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Runtime.InteropServices;

namespace FDSettings
{
    public partial class Settings : Form
    {
        private const string fileName = "FDSettings.xml";
        private Hashtable treenodesMap;
        private Hashtable xmlattrMap;
        private XmlDocument xmlDocument;
        private DataSetSettings.TableSettingsRow rowCurrent;
 
        enum HRESULT : long 
        {
            S_OK = 0x000000000L,
            E_INVALIDARG = 0x80070057L,
            E_OUTOFMEMORY = 0x8007000EL,
            E_UNEXPECTED = 0x8000FFFFL,
            E_FAIL = 0x80004005L
        };

       [DllImport("FDLicenceProvider.dll", PreserveSig=true)]
       unsafe public static extern uint Encrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData);
       [DllImport("FDLicenceProvider.dll", PreserveSig=true)]
       unsafe public static extern uint Decrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData);

   
       static public unsafe string EncryptData(string value)
       {
           if (value.Length == 0)
               return "";

           HRESULT hr = HRESULT.S_OK;
           byte[] dataIn;
           dataIn = Encoding.UTF8.GetBytes(value);
           int lenIn = dataIn.GetLength(0);
           byte[] dataOut = new byte[lenIn * 100];
           int lenOut = dataOut.GetLength(0);
           int lenEncryptData = lenOut;
           bool bEncrypt = false;

           while (!bEncrypt)
           {
               fixed (byte* pDataIn = dataIn, pDataOut = dataOut)
               {
                   hr = (HRESULT)Encrypt(pDataIn, lenIn, pDataOut, lenOut, &lenEncryptData);

               }
               if ((hr == HRESULT.E_OUTOFMEMORY) && (lenOut < lenEncryptData))
               {
                   if (lenEncryptData < Int32.MaxValue / 2)
                   {
                       dataOut = new byte[lenEncryptData * 2];
                       lenOut = dataOut.GetLength(0);
                   }
                   else
                   {
                       bEncrypt = true;
                   }
               }
               else
               {
                   bEncrypt = true;
               }
           }
           if ((hr == HRESULT.S_OK) && (lenOut >= lenEncryptData))
           {
               return Encoding.UTF8.GetString(dataOut, 0, lenEncryptData);
               //xmlChildDB.InnerXml = Encoding.UTF8.GetString(dataOut, 0, lenDecryptData);
           }
           else
           {   
               hr = (hr == HRESULT.S_OK) ? HRESULT.S_OK : HRESULT.E_OUTOFMEMORY;
               MessageBox.Show("Encypt database data error is " + hr.ToString() + " (0x" + Convert.ToString(((uint)hr), 16) + ").");
               return "";
           }

       }

        static public unsafe string DecryptData(string value)
       {
           if (value.Length == 0)
               return "";

           HRESULT hr;
           byte[] dataIn = Encoding.UTF8.GetBytes(value);
           int lenIn = dataIn.GetLength(0);

           byte[] dataOut = new byte[lenIn];
           int lenOut = lenIn;
           int lenDecryptData;
           fixed (byte* pDataIn = dataIn, pDataOut = dataOut)
           {
               hr = (HRESULT)Decrypt(pDataIn, lenIn, pDataOut, lenOut, &lenDecryptData);
           }
           if (hr == HRESULT.S_OK)
           {
               return Encoding.UTF8.GetString(dataOut, 0, lenDecryptData);
               //xmlChildDB.InnerXml = Encoding.UTF8.GetString(dataOut, 0, lenDecryptData);
           }
           else
           {
               MessageBox.Show("Encypt database data error is " + hr.ToString() + " (0x" + Convert.ToString(((uint)hr), 16) + ").");
               return "";
           }
        }

        static public string ConnectionString()
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Settings file can not be loaded: " + ex.Message, "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }

            XmlElement rootSettings = xmlDocument.DocumentElement;
            if (!rootSettings.HasChildNodes)
            {
                MessageBox.Show("Settings file can not be loaded. ", "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            XmlNodeList xmlElemList = rootSettings.GetElementsByTagName("database");
            if (xmlElemList.Count > 1)
            {
                MessageBox.Show("Settings file has error. ", "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";

            }
            XmlNodeList xmlChildList = xmlElemList.Item(0).ChildNodes;
            int i = 0;
            Boolean bReturn = false;
            string strDBConnect =  "Data Source=%hostname;Initial Catalog=FaceDetector;%integrated";
            while (!bReturn && (i < xmlChildList.Count))
            {
                switch  (xmlChildList.Item(i).Attributes.GetNamedItem("stname").Value) 
                {
                    case "integrated":
                        strDBConnect = strDBConnect.Replace("%integrated", ((Convert.ToBoolean(DecryptData(xmlChildList.Item(i).Attributes.GetNamedItem("stvalue").Value)) == false) ? 
                                                            "Integrated Security=SSPI;Persist Security Info=False" :
                                                            "Persist Security Info=True;Password=%password;User ID=%login"));
                        bReturn = Convert.ToBoolean(DecryptData(xmlChildList.Item(i).Attributes.GetNamedItem("stvalue").Value));
                        break;
                    default :
                        strDBConnect = strDBConnect.Replace("%" + xmlChildList.Item(i).Attributes.GetNamedItem("stname").Value, 
                        DecryptData(xmlChildList.Item(i).Attributes.GetNamedItem("stvalue").Value));
                        break;
                }
                i++;

            }
            return strDBConnect;
        }
 
        public Settings()
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
                        XmlAttribute attrMinval = (XmlAttribute)xmlchild.Attributes.GetNamedItem("minval");
                        XmlAttribute attrMaxval = (XmlAttribute)xmlchild.Attributes.GetNamedItem("maxval");
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
                            string minVal = attrMinval != null ? attrMinval.Value : null;
                            string maxVal = attrMaxval != null ? attrMaxval.Value : null;
                            
                            DataSetSettings.TableSettingsRow row = this.dataSetSettings.TableSettings.NewTableSettingsRow();
                            row.Name = attrName.Value;
                            row.Value = (xmlnode.Name == "database" ? DecryptData(attrValue.Value) : attrValue.Value);
                            row.Type = attrType.Value;
                            row.Selitems = selItems;
                            row.MinValue = minVal;
                            row.MaxValue = maxVal;
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
            this.checkBoxValue.Visible = false;
            this.comboBoxValue.Visible = false;
            this.textBoxValue.Visible = false;
            this.numericValue.Visible = false;
            this.rangeValue.Visible = false;
            this.lblMaxval.Visible = false;
            this.lblMinVal.Visible = false;
            this.lblRangeval.Visible = false;

            this.lblValue.Text = "";
            this.comboBoxValue.Items.Clear();

            GetCurrentGridRow(sender);
            if (this.rowCurrent != null)
            {
                this.lblValue.Text = this.rowCurrent.Name;
                switch (this.rowCurrent.Type)
                {
                    case "text":
                        this.textBoxValue.Visible = true;
                        this.textBoxValue.Text = this.rowCurrent.Value;
                        break;

                    case "bool":
                        this.checkBoxValue.Visible = true;
                        this.checkBoxValue.Checked = this.rowCurrent.Value.Equals("true");
                        break;

                    case "select":
                    case "selectnull":
                        if (this.rowCurrent.Type.Equals("selectnull"))
                            this.comboBoxValue.Items.Add("");
                        string[] selItems = this.rowCurrent.Selitems != null && this.rowCurrent.Selitems.Length > 0 ? this.rowCurrent.Selitems.Split(new char[] { ';' }) : new string[0];
                        this.comboBoxValue.Items.AddRange(selItems);
                        this.comboBoxValue.Visible = true;
                        this.comboBoxValue.SelectedIndex = this.comboBoxValue.FindString(this.rowCurrent.Value);
                        break;

                    case "counter":
                        this.numericValue.Visible = true;
                        this.numericValue.Value = decimal.Parse(this.rowCurrent.Value);
                        this.numericValue.Minimum = 0;
                        break;

                    case "range":
                        this.rangeValue.Visible = true;
                        this.lblMaxval.Visible = true;
                        this.lblMinVal.Visible = true;
                        this.lblRangeval.Visible = true;
                        this.rangeValue.Minimum = int.Parse(this.rowCurrent.MinValue);
                        this.rangeValue.Maximum = int.Parse(this.rowCurrent.MaxValue);
                        this.rangeValue.Value = int.Parse(this.rowCurrent.Value);
                        int diff = this.rangeValue.Maximum - this.rangeValue.Minimum;
                        this.rangeValue.TickFrequency = diff <= 20 ? 1 : (int)((diff + 10) / 20);
                        this.lblMaxval.Text = this.rangeValue.Maximum.ToString();
                        this.lblMinVal.Text = this.rangeValue.Minimum.ToString();
                        this.lblRangeval.Text = this.rangeValue.Value.ToString();
                        break;

                }
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

                case "counter":
                    this.rowCurrent.Value = this.numericValue.Value.ToString();
                    break;

                case "range":
                    this.rowCurrent.Value = this.rangeValue.Value.ToString();
                    break;
            }

            XmlAttribute xmlAttrValue = (XmlAttribute)this.xmlattrMap[this.rowCurrent];
            if (xmlAttrValue != null)
                xmlAttrValue.Value = this.rowCurrent.Value;
        }

        private void rangeValue_Scroll(object sender, EventArgs e)
        {
            this.lblRangeval.Text = this.rangeValue.Value.ToString();
        }
    }
}
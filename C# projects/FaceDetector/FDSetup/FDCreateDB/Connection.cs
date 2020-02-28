using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Xml;
using System.Collections;
namespace FDCreateDB
{
    public partial class Connection : Form
    {
        public DbConnection m_Connection;
        private const string SETTINGS_FILE_NAME = "FDSettings.xml";
        private string strServerName;
        private int countRecords;
        private DbProviderFactory m_Provider;
        private string m_TargetDir;
        private XmlDocument xmlDocument;

        public Connection(DbProviderFactory provider, string targetDir)
        {
            InitializeComponent();
            this.xmlDocument = new XmlDocument();
            m_Provider = provider;
            m_TargetDir = targetDir;
        }

        private void Connection_Load(object sender, EventArgs e)
        {
            List<string> lLocalServers = ServersBrowser.LocalServersList();
            foreach (string item in lLocalServers)
              cbServerName.Items.Add(item);
            cbServerName.Items.Add("<Browser for more...>");
            countRecords = cbServerName.Items.Count;
            if (countRecords == 1)
            {
                //If no local server exists add empty string in the list
                cbServerName.Items.Insert(0, "");
            }
            cbServerName.SelectedIndex = 0;
            cbAuthentication.SelectedIndex = 0;
        }
 
        private void cbServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServerName.Items[cbServerName.SelectedIndex].ToString() == "<Browser for more...>")
            {
                ServersBrowser browser = new ServersBrowser();
                browser.ShowDialog();
                if (browser.DialogResult == DialogResult.OK)
                {
                    if (cbServerName.Items.Count > countRecords)
                    {
                        cbServerName.Items.RemoveAt(0);
                    }
                    strServerName = browser.ServerName;
                    if (cbServerName.FindString(browser.ServerName, 0) == -1)
                    {
                        cbServerName.Items.Insert(0, browser.ServerName);
                        cbServerName.SelectedIndex = 0;
                    }
                }
                else
                {
                    cbServerName.SelectedIndex = 0;
                    strServerName = cbServerName.Items[cbServerName.SelectedIndex].ToString();
                }
            }
            else
            {
                strServerName = cbServerName.Items[cbServerName.SelectedIndex].ToString();
            }
        }

        private void cbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            eUserName.Enabled = (cbAuthentication.SelectedIndex == 1);
            ePassword.Enabled = (cbAuthentication.SelectedIndex == 1);
            chbRemPassword.Enabled = (cbAuthentication.SelectedIndex == 1);
        }

        private bool ServerConnection(string hostname)
        {
            try
            {
                m_Connection = m_Provider.CreateConnection();
                m_Connection.ConnectionString = "Provider=SQLOLEDB.1;" +
                                               ((cbAuthentication.SelectedIndex == 0) ?  "Integrated Security=SSPI;Persist Security Info=False" : "Persist Security Info=True") 
                                               + ";Data Source=" + strServerName 
                                               + ((cbAuthentication.SelectedIndex == 1) ?  
                                               (";Password=" + ePassword.Text + ";User ID=" + eUserName.Text) : "");
                m_Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No connection to Server " + hostname + ". " + ex.Message, "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void Connection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                e.Cancel = !ServerConnection(strServerName);
                if (!e.Cancel)
                {
                    SaveDatabaseConnectionSettings(); 
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void SaveDatabaseConnectionSettings()
        {
            try
            {
                this.xmlDocument.Load(m_TargetDir + SETTINGS_FILE_NAME);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Settings file can not be loaded: " + ex.Message, "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                System.Xml.XmlNode xmlnode = this.xmlDocument.DocumentElement;

                if (xmlnode.NodeType != XmlNodeType.Element || !xmlnode.HasChildNodes)
                    return;
                foreach (XmlNode xmlchild in xmlnode.ChildNodes)
                {
                    if (xmlchild.Name == "database")  
                    {
                        foreach (XmlNode xmlchildDB in xmlchild.ChildNodes)
                        {
                            switch (xmlchildDB.Attributes.GetNamedItem("stname").Value)
                            {
                                case "hostname":
                                    {
                                        SetAttributeValue(xmlchildDB, FDSettings.Settings.EncryptData(cbServerName.Text));
                                        break;
                                    }
                                case "integrated":
                                    {
                                        SetAttributeValue(xmlchildDB, FDSettings.Settings.EncryptData(((cbAuthentication.SelectedIndex == 1) ? "false" : "true")));
                                        break;
                                    }
                                case "login":
                                    {
                                    if (cbAuthentication.SelectedIndex == 1)
                                        {
                                            SetAttributeValue(xmlchildDB, FDSettings.Settings.EncryptData(eUserName.Text));
                                        }
                                        break;
                                    }
                                case "password":
                                    {
                                         if ((cbAuthentication.SelectedIndex == 1) && chbRemPassword.Checked)
                                        {
                                            SetAttributeValue(xmlchildDB, FDSettings.Settings.EncryptData(ePassword.Text));
                                        }
                                        break;
                                    }
                             }
                        }
                        this.xmlDocument.Save(m_TargetDir + SETTINGS_FILE_NAME);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Settings file can not be loaded: " + ex.Message, "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SetAttributeValue(XmlNode xmlChildDBCurrent, string value)
        {
            XmlAttribute attrValue = (XmlAttribute)xmlChildDBCurrent.Attributes.GetNamedItem("stvalue");
            if (attrValue == null)
            {
                attrValue = this.xmlDocument.CreateAttribute("stvalue");
            }
            attrValue.Value = value;
            xmlChildDBCurrent.Attributes.SetNamedItem(attrValue);

        } 
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data.Common;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace FDCreateDB
{
    [RunInstaller(true)]
    public partial class CreateDB : Installer
    {
        private const string PROVIDER_NAME = "System.Data.OleDb";
        private const string SQL_FILE_NAME = "FaceDetectorDB.sql";
        private string m_TargetDir;
        private DbProviderFactory m_Provider;
        private DbConnection m_Connection = null;
        
        public CreateDB()
        {
            InitializeComponent();
        }
        
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            stateSaver.Add("TARGETDIR", Context.Parameters["DP_TargetDir"].ToString());
            m_TargetDir = stateSaver["TARGETDIR"].ToString();
            this.CreateConnection();
        }

        private void CreateConnection()
        {
            try
            {
                m_Provider = System.Data.Common.DbProviderFactories.GetFactory(PROVIDER_NAME);
            }
            catch (Exception ex)
            {
                MessageBox.Show(PROVIDER_NAME + " is not found. " + ex.Message, "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Connection connect = new Connection(m_Provider, m_TargetDir);
            connect.ShowDialog();
            if (connect.DialogResult == DialogResult.OK)
            {
                m_Connection = connect.m_Connection;
                try
                {
                    DbCommand command = m_Provider.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.Connection = m_Connection;
                    List<string> commandsSQL;
                    commandsSQL = GetSQL();

                    if (commandsSQL.Count > 0)
                    {
                        for (int i = 0; i < commandsSQL.Count; i++)
                        {
                            command.CommandText = commandsSQL[i];
                            command.ExecuteNonQuery();
                        }
                    }
                    command.Connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error by executing SQL script on the database: " + ex.Message, "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private List<string> GetSQL()
        {
            List<string> cmds = new List<string>();
            try
            {
                if (File.Exists(m_TargetDir + SQL_FILE_NAME))
                {
                    StreamReader strContents = new StreamReader(m_TargetDir + SQL_FILE_NAME);
                    string line = "";
                    string cmd = "";
                    while ((line = strContents.ReadLine()) != null)
                    {
                        if (line.Trim().ToUpper() == "GO")
                        {
                            cmds.Add(cmd);
                            cmd = "";
                        }
                        else
                        {
                            cmd += line + "\r\n";
                        }
                    }
                    if (cmd.Length > 0)
                    {
                        cmds.Add(cmd);
                        cmd = "";
                    }
                    strContents.Close();
                }
                else
                {
                    MessageBox.Show("Sql file is not found.", "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read sql file: " + ex.Message, "Create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return cmds;
        }
    }
}
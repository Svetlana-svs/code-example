using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using Microsoft.SqlServer.Management;

// It's necessery to add reference to Microsoft.SqlServer.WmiEnum, Microsoft.SqlServer.Smo, Microsoft.SqlServer.ConnectionInfo

namespace FDCreateDB
{
  public partial class ServersBrowser : Form
    {
         public string ServerName;
        public ServersBrowser()
        {
            InitializeComponent();
            tabLocal_SelectedIndexChanged(null, null);
            btnOk.Enabled = false;
        }
        public static List<string> LocalServersList()
        {
            List<string> localServers = new List<string>();
            bool bServerBrowserRun = false;
            bool bServerRun = false;
            try
            {
                ManagedComputer comp = new ManagedComputer(System.Environment.MachineName.ToString());

                foreach (Service service in comp.Services)
                {
                   bServerRun = bServerRun ||((!bServerRun) && service.Name.StartsWith("MSSQL") && (service.ServiceState == ServiceState.Running));
                   bServerBrowserRun = bServerBrowserRun || ((!bServerBrowserRun) && service.Name.StartsWith("SQLBROWSER") && (service.ServiceState == ServiceState.Running));
                }
                if (!bServerRun)
                {
                    localServers.Add("Running SQL Server is not found.");
                    return localServers;
                }
                if (bServerBrowserRun)
                {
                    DataTable table = SmoApplication.EnumAvailableSqlServers(true);
                    foreach (DataRow row in table.Rows)
                    {
                        localServers.Add(row["Name"].ToString());
                    }
                }
                else
                {
                    foreach (ServerInstance instance in comp.ServerInstances)
                    {
                        localServers.Add(comp.Name + ((instance.Name != "MSSQLSERVER") ? (@"\" + instance.Name) : ""));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return localServers;
        }
        private List<string> NetworkServersList()
        {
            Cursor.Current = Cursors.WaitCursor;
            List<string> networkServers = new List<string>();
            try
            {
                System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
                DataTable tableServers = instance.GetDataSources();
                foreach (DataRow row in tableServers.Rows)
                {
                    networkServers.Add(row["ServerName"].ToString() + ((row.IsNull("InstanceName") || (row["InstanceName"].ToString() == "MSSQLSERVER")) ? "" : (@"\" + row["InstanceName"].ToString())));
                }
            }
            catch (Exception ex)
            {

            }
            Cursor.Current = Cursors.Default;
            return networkServers;
        }
 
        private void tabLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            ListBox lstBox = new ListBox();
            List<string> servers = new List<string>();
            switch (tabLocal.SelectedIndex)
            {
                case 0:
                    lstBox = lstLocal;
                    servers = LocalServersList();
                    break;
                case 1:
                    lstBox = lstNetwork;
                    servers = NetworkServersList();
                    break;
            }

            lstBox.BeginUpdate();
            lstBox.Items.Clear();
            foreach(string str in servers)
            {
                lstBox.Items.Add(str);
            }
            lstBox.EndUpdate();
        }

        private void btnOk_Click(object sender, EventArgs e)
        { 
            ServerName = (tabLocal.SelectedIndex == 0) ? lstLocal.Text : lstNetwork.Text;
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = true;
        }

    }
}
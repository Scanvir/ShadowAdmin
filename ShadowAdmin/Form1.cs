using Cassia;
using System;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace ShadowAdmin
{
    public partial class Form1 : Form
    {
        public DataTable table;
        public Form1()
        {
            InitializeComponent();
            GetTerminalInfo();
        }
        public void GetTerminalInfo()
        {
            table = new DataTable("tsTable");
            DataColumn column;
            DataRow row;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "id";
            column.Caption = "ID";
            column.ReadOnly = true;
            column.Unique = true;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "UserName";
            column.Caption = "Имя";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "State";
            column.Caption = "Статус";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ConnectTime";
            column.Caption = "Подключен";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            ITerminalServicesManager manager = new TerminalServicesManager();
            using (ITerminalServer server = manager.GetRemoteServer("LOCALHOST"))
            {
                server.Open();
                foreach (ITerminalServicesSession session in server.GetSessions())
                {
                    NTAccount account = session.UserAccount;
                    if (account != null)
                    {
                        row = table.NewRow();
                        row["id"] = session.SessionId;
                        row["UserName"] = session.UserName;
                        row["State"] = session.ConnectionState;
                        row["ConnectTime"] = session.ConnectTime;
                        table.Rows.Add(row);
                    }
                }
            }

            dataGridView1.DataSource = table;
            dataGridView1.Columns["id"].Width = 50;
            dataGridView1.Columns["UserName"].Width = 150;
            dataGridView1.Columns["State"].Width = 80;
            dataGridView1.Columns["ConnectTime"].Width = 245;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = table.Rows[dataGridView1.CurrentRow.Index][0].ToString();

            string strCmdLine = "/shadow:" + id + " /noConsentPrompt /control";
            Process.Start("mstsc.exe", strCmdLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id = table.Rows[dataGridView1.CurrentRow.Index][0].ToString();

            string strCmdLine = "/shadow:" + id + " /noConsentPrompt";
            Process.Start("mstsc.exe", strCmdLine);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetTerminalInfo();
        }
    }

}

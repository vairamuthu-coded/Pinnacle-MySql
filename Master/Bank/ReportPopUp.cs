using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pinnacle.Master.Bank
{
    public partial class ReportPopUp : Form
    {
        Models.Master mas = new Models.Master();
        public ReportPopUp()
        {
            InitializeComponent();
            this.Font = Class.Users.FontName;
            button1.ForeColor = Class.Users.Color1;
            button1.BackColor = Class.Users.BackColors;          
            button1.Text = Class.Users.Paramid.ToString();           
            //string folderPath = Directory.GetCurrentDirectory();
            //string baseName = $"temp1_{Class.Users.HUserName}_{Class.Users.PreParamid}";
            //string[] files = Directory.GetFiles(folderPath, baseName + ".*");
            //if (files.Length > 0)
            //{
            //    foreach (string file in files)
            //    {
            //        File.Delete(file);
            //    }

            //}
            DataTable dt1 = Utility.SQLQuery("SELECT  a.asptbladvpaydetid,a.asptbladvpaymasid,a.compcode,b.department,c.partyname,a.invoicetype,a.invoice,a.INVBLOB,a.INVPROBLOB,a.QUABLOB,a.powoblob,a.OTHBLOB from asptbladvpaydet a join asptbldeptmas b on b.asptbldeptmasid=a.department join asptblpartymas c on c.asptblpartymasid=a.partyname   where a.asptbladvpaymasid='" + Class.Users.Paramid + "'");
            dataGridView1.Rows.Clear();
            if (dt1.Rows.Count > 0)
            {
              Class.Users.bisconnected = true;
            }
            else
            {
                Class.Users.bisconnected = false;
            }
            int i = 0; 
            foreach (DataRow myRow in dt1.Rows)
            {
               
                
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = myRow["asptbladvpaydetid"].ToString();
                dataGridView1.Rows[i].Cells[1].Value = myRow["invoicetype"].ToString();
                dataGridView1.Rows[i].Cells[2].Value = myRow["invoice"].ToString();
                string[] ext = myRow["invoice"].ToString().Split('.');
                byte[] fileBytes = null;
              string  extension = "";
                extension = Path.GetExtension(myRow["invoice"].ToString());


                switch (i)
                {
                    case 0: fileBytes = myRow["INVBLOB"] as byte[];  break;
                    case 1: fileBytes = myRow["INVPROBLOB"] as byte[]; break;
                    case 2: fileBytes = myRow["QUABLOB"] as byte[];  break;
                    case 3: fileBytes = myRow["powoblob"] as byte[];  break;
                    case 4: fileBytes = myRow["OTHBLOB"] as byte[];  break;
                }

                if (fileBytes != null && fileBytes.Length > 0)
                {
                    string fileName = $"temp1_{Class.Users.HUserName}_{Class.Users.Paramid}{extension}";
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                    FileStream fs = new FileStream(filepath, FileMode.Create);
                    fs.Write(fileBytes, 0, fileBytes.Length);
                    fs.Close();   // Must close manually
                }

            

                i++;

            }
            CommonFunctions.SetRowNumber(dataGridView1);
        }

        private void PopUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            string folderPath = Directory.GetCurrentDirectory(); Class.Users.PreParamid = Class.Users.Paramid;
            string baseName = $"temp1_{Class.Users.HUserName}_{Class.Users.PreParamid}";
            string[] files = Directory.GetFiles(folderPath, baseName + ".*");
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }

            }

            this.Dispose();
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int64 GridID = Class.Users.Paramid; ;

           
                // Ignore clicks on headers or out-of-range rows
                if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count)
                    return;

                DataGridViewRow dv = dataGridView1.Rows[e.RowIndex];

                // Safe parsing
                if (!Int64.TryParse(dv.Cells[2].Value?.ToString(), out GridID))
                {
                    GridID = 0; // default if parsing fails
                }

                if (Class.Users.bisconnected)
                {
                    Master.Bank.PopUp pop = new Master.Bank.PopUp();
                    pop.Show();
                }
                else
                {
                    string responsePerson = dv.Cells[12].Value?.ToString() ?? "N/A";
                    string supplier = dv.Cells[5].Value?.ToString() ?? "N/A";

                    mas.pop(
                        "Data not Found : ",
                        $"Response Person : {responsePerson}  Supplier : {supplier}",
                        $"Response ID : {GridID}"
                    );
                }            


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Class.Users.UserTime = 0;
        }

        private void ReportPopUp_Load(object sender, System.EventArgs e)
        {

        }

        private void ReportPopUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}

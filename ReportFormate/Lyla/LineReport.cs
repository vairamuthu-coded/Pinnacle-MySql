using CrystalDecisions.Shared;
using Pinnacle.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QRCoder.PayloadGenerator;

namespace Pinnacle.ReportFormate.Lyla
{
    public partial class LineReport : Form,ToolStripAccess
    {
        private static LineReport _instance;
        public static LineReport Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LineReport();
                GlobalVariables.CurrentForm = _instance; return _instance;
            }
        }
        public LineReport()
        {
            InitializeComponent();
        }

        private void LineReport_Load(object sender, EventArgs e)
        {


            
        }
        private void empty()
        {
 comboline.SelectedIndex = -1;comboBox1.SelectedIndex = -1;
  pictureBox1.Image = null;
           
            combomachine.SelectedIndex = -1;
           
            this.BackColor = Class.Users.BackColors; crystalReportViewer1.ReportSource = null;
            butheader.BackColor = Class.Users.BackColors;
           
            butlinewise.BackColor = Class.Users.BackColors;
            butlinewise.ForeColor = System.Drawing.Color.White;

            butmachinewise.BackColor = Class.Users.BackColors;
            butmachinewise.ForeColor = System.Drawing.Color.White;
            butdatewise.BackColor = Class.Users.BackColors;
            butdatewise.ForeColor = System.Drawing.Color.White;
            this.Font = Class.Users.FontName;
            //string sel1 = "SELECT distinct B.GTCOMPMASTID,b.compcode,b.compname,a.finyear FROM  asptblpur a join gtcompmast b on a.compcode=b.gtcompmastid where b.compcode='" + Class.Users.HCompcode + "'";
            //DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblpur");
            //DataTable dt = ds.Tables["asptblpur"];
            //combocompcode.DisplayMember= "compcode";
            //combocompcode.ValueMember= "GTCOMPMASTID";
            //combocompcode.DataSource=dt;
            //combofinyear.DisplayMember = "finyear";
            //combofinyear.ValueMember = "finyear";
            //combofinyear.DataSource = dt;
            
        }
        void LoadData()
        {
           


            Class.Users.UserTime = 0;
            string sel3 = " select 'ALL' line FROM DUAL union ALL  select  distinct  d.line   from asptblmacdet a  join asptblmacmas b on a.asptblmacmasid=b.asptblmacmasid  join asptblmanmas c on  c.machine=a.asptblmacdetid  join asptbllinmas d on d.asptbllinmasid=b.line  where b.compcode='" + Class.Users.COMPCODE + "'  ";
            DataSet ds3 = Utility.ExecuteSelectQuery(sel3, "asptblmacmas");
            DataTable dt3 = ds3.Tables["asptblmacmas"];
            comboline.DisplayMember = "line";
            comboline.ValueMember = "line";
            comboline.DataSource = dt3;
        }
        public void GridLoad()
        {

        }
        public void News()
        {
            LoadData();
            empty();
            //Class.Users.Query = "SELECT distinct a.asptblpurid,  C.Asptblsizmasid, B.Compcode, A.PoNo,D.ColorName, C.SizeName,E.OrderQty,e.ExcessQty,'' Inward,'' Rework, '' Delivery,'' CheckingInward, ''CheckingRework,'' StitchingRework,''CheckingDelivery,''Notes,b.CompName,b.Address,b.companylogo,''BalQty,''inward1,''rework1,''delivery1,''checkin1,''checkcheck1,''checkstitch1,''checkdel1,''balqty1,j.modified as FromDate,''Todate,e.orderno,e.styleref FROM asptblpurdet1 A JOIN gtcompmast B ON A.compcode=B.gtcompmastid JOIN asptblsizmas C ON C.sizename=A.sizename JOIN asptblcolmas D ON D.colorname=A.colorname JOIN asptblpur E ON E.compcode=A.compcode  AND E.pono=A.pono   and e.asptblpurid=a.asptblpurid join asptblbuymas h on h.asptblbuymasid=e.buyer  join asptblstymas i on i.asptblstymasid=e.stylename  join asptblprolot j on j.compcode=b.gtcompmastid and j.pono=a.pono WHERE a.asptblpurdet1id<0;";
            //Class.Users.dt = CC.select(Class.Users.Query, "ASPTBLPROLOT");
            //Class.Users.dt1 = Class.Users.dt.Clone();
           
        }

        public void Saves()
        {
           
        }

        public void Prints()
        {
            
        }

        public void Searchs()
        {
            
        }

        public void Searchs(int EditID)
        {
            
        }

        public void Deletes()
        {
            
        }

        public void ReadOnlys()
        {
            
        }

        public void Imports()
        {
            
        }

        public void Pdfs()
        {
            
        }

        public void ChangePasswords()
        {
            
        }

        public void DownLoads()
        {
        
            if (comboBox1.Text != "")
            {
                DialogResult result = MessageBox.Show("Do you want to Formate ??", "PRINT OUT", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.OK))
                {
                    if (comboBox1.Text == "EXCEL")
                    {
                       

                            l1.ExportToDisk(ExportFormatType.ExcelRecord, "d:\\'"+Class.Users.HCompcode+"'-LineWise.xls");
                       
                    }
                    if (comboBox1.Text == "PDF")
                    {
                       

                            l1.ExportToDisk(ExportFormatType.PortableDocFormat, "d:\\'"+Class.Users.HCompcode+ "'-LineWise.pdf");
                       
                    }
                }
            }
            else
            {
                
                MessageBox.Show("Pls Select Combo Box Value");
            }
        }

        public void ChangeSkins()
        {
            
        }

        public void Logins()
        {
            
        }

        public void GlobalSearchs()
        {
            
        }

        public void TreeButtons()
        {
           
        }

        public void Exit()
        {
            GlobalVariables.MdiPanel.Show();
            empty();
            this.Hide();
            GlobalVariables.HeaderName.Text = "";
            GlobalVariables.TabCtrl.TabPages.RemoveAt(GlobalVariables.TabCtrl.SelectedIndex);

        }
        Models.CommonClass CC = new Models.CommonClass();
        private void button1_Click(object sender, EventArgs e)
        {
            
           
        }
        Report.Lyla.LineReport l1 = new Report.Lyla.LineReport();
      
        private void BindGrid(DataGridView grid, DataTable dt1)
        {

            int j = 0, i = 0; Int64 inqty=0, bqty=0, total = 0,inqty2=0, inchdel2=0, inqty1 = 0, inrew1 = 0, indel1 = 0, inchin1 = 0, inchch1 = 0, inchst1 = 0, inchdel1 = 0;
            foreach (DataRow row in dt1.Rows)
            {
                
                grid.Rows.Add();
                grid.Rows[i].Cells[0].Value = Convert.ToInt64("0" + row["ASPTBLSIZMASID"].ToString());
                grid.Rows[i].Cells[1].Value = row["Compcode"].ToString();
                grid.Rows[i].Cells[2].Value = row["PoNo"].ToString();
                grid.Rows[i].Cells[3].Value = row["ColorName"].ToString();
                grid.Rows[i].Cells[4].Value = row["SizeName"].ToString();
                grid.Rows[i].Cells[5].Value = row["OrderQty"].ToString();
                grid.Rows[i].Cells[6].Value = row["ExcessQty"].ToString();
                grid.Rows[i].Cells[7].Value = row["Inward"].ToString();
                grid.Rows[i].Cells[8].Value = row["Rework"].ToString();
                grid.Rows[i].Cells[9].Value = row["Delivery"].ToString();
                grid.Rows[i].Cells[10].Value = row["CheckingInward"].ToString();
                grid.Rows[i].Cells[11].Value = row["CheckingRework"].ToString();
                grid.Rows[i].Cells[12].Value = row["StitchingRework"].ToString();
                grid.Rows[i].Cells[13].Value = row["CheckingDelivery"].ToString();
                grid.Rows[i].Cells[14].Value = row["Notes"].ToString();
                inqty2 += Convert.ToInt64("0" + row["Inward"].ToString());
                inqty1 = Convert.ToInt64("0" + row["Inward"].ToString());
                inrew1 += Convert.ToInt64("0" + row["Rework"].ToString());
                indel1 += Convert.ToInt64("0" + row["Delivery"].ToString());
                inchin1 += Convert.ToInt64("0" + row["CheckingInward"].ToString());
                inchch1 += Convert.ToInt64("0" + row["CheckingRework"].ToString());
                inchst1 += Convert.ToInt64("0" + row["StitchingRework"].ToString());
                inchdel1 = Convert.ToInt64("0" + row["CheckingDelivery"].ToString());
                inchdel2 += Convert.ToInt64("0" + row["CheckingDelivery"].ToString());
                total = inqty1 - inchdel1;
               
                grid.Rows[i].Cells[18].Value = total.ToString();
                grid.Rows[i].Cells[19].Value = row["FromDate"].ToString();
                grid.Rows[i].DefaultCellStyle.BackColor = j % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;
                Class.Users.dt1.Rows[i]["inward1"] = inqty2;
                Class.Users.dt1.Rows[i]["rework1"] = inrew1;
                Class.Users.dt1.Rows[i]["delivery1"] = indel1;
                Class.Users.dt1.Rows[i]["checkin1"] = inchin1;
                Class.Users.dt1.Rows[i]["checkcheck1"] = inchch1;
                Class.Users.dt1.Rows[i]["checkstitch1"] = inchst1;
                Class.Users.dt1.Rows[i]["checkdel1"] = inchdel2;
                Class.Users.dt1.Rows[i]["Balqty"] = total;
                bqty += total;             
                j++; i++;
            }
            Cursor = Cursors.Default;
        }
        byte[] stdbytes; Int64 std;
        private void combopono_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string sel="select a.garmentimage from asptblpur a where a.pono='"+combomachine.Text+"'";
            DataTable dt = CC.select(sel, "asptblpur");
            int c = dt.Rows.Count;
            if (c >= 1)
            {
                if (c >= 1 && dt.Rows[0]["garmentimage"].ToString() !="")
                {
                    pictureBox1.Image = null; stdbytes = null;
                    stdbytes = (byte[])dt.Rows[0]["garmentimage"];
                    Image img = Models.Device.ByteArrayToImage(stdbytes);
                    pictureBox1.Image = img;
                }
                else
                {
                    pictureBox1.Image = null; stdbytes = null;
                }
            }
        }

        private void downLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void butdatewise_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet(); DataTable dt = new DataTable(); DataTable dt1 = new DataTable();
           

                string sel = "select b.compname,b.address,  b.compcode,a.finyear,a.barcode,c.machine,e.processname,f.linename,f.linenumber,g.floor, date(a.date) as date,a.time,a.notes from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname  join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "' and a.date between'" +Convert.ToDateTime(frmdate.Value.ToString()).ToString("yyyy-MM-dd").Substring(0,10) + "'  and '" + Convert.ToDateTime(todate.Value.ToString()).ToString("yyyy-MM-dd").Substring(0, 10) + "'";
                ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                dt = ds.Tables["asptblmanmas"];
        
            string sel1 = "select a.companylogo from gtcompmast a where a.compcode='" + Class.Users.HCompcode + "'";
            ds = Utility.ExecuteSelectQuery(sel1, "gtcompmast");
            dt1 = ds.Tables["gtcompmast"];
            l1.Database.Tables["DataTable1"].SetDataSource(dt);
            l1.Database.Tables["DataTableImage"].SetDataSource(dt1);

            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = l1;
            crystalReportViewer1.Refresh();


            Cursor.Current = Cursors.Default;



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        //void reportwise()
        //{
        //    if (comboline.Text == "PO WISE")
        //    {
        //        frmdate.Enabled = false;
        //        todate.Enabled = false;
        //        butdatewise.Enabled = false;
        //        combomachine.Enabled = true;
        //        button1.Enabled = true;
        //    }
        //    else
        //    {
        //        combomachine.Enabled = false;
        //        button1.Enabled = false;
        //        frmdate.Enabled = true;
        //        todate.Enabled = true;
        //        butdatewise.Enabled = true;
        //    }
        //}
        //void reportwise1()
        //{
        //    if (combotype1.Text == "PO WISE")
        //    {
        //        datepcsrate1.Enabled = false;
        //        datepcsrate2.Enabled = false;
        //        butdatewise.Enabled = false;
        //        combopopcsrate.Enabled = true;
        //        butpcsmonthly.Enabled = true;
        //        button2.Enabled = false;
        //    }
        //    else
        //    {
        //        butpcsmonthly.Enabled = false;
        //        combopopcsrate.Enabled = false;
        //        button2.Enabled = true;
        //        datepcsrate1.Enabled = true;
        //        datepcsrate2.Enabled = true;
        //        butdatewise.Enabled = true;
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
           




        }

    
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void combotype1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = null;
        }

        private void butlinewise_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet(); DataTable dt = new DataTable(); DataTable dt1 = new DataTable();
            if (comboline.Text == "ALL")
            {
                string sel = "select  b.compname,b.address,  b.compcode,a.finyear,a.barcode,c.machine,e.processname,f.linename,f.linenumber,g.floor, a.date,a.time,a.notes from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname  join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "'";
                ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                dt = ds.Tables["asptblmanmas"];
            }
            if (comboline.Text != "ALL")
            {
                string sel = "select b.compname,b.address,  b.compcode,a.finyear,a.barcode,c.machine,e.processname,f.linename,f.linenumber,g.floor, a.date,a.time,a.notes from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname  join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "' and f.line='" + comboline.Text + "' ";
                ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                dt = ds.Tables["asptblmanmas"];
            }
            string sel1 = "select a.companylogo from gtcompmast a where a.compcode='" + Class.Users.HCompcode + "'";
            ds = Utility.ExecuteSelectQuery(sel1, "gtcompmast");
            dt1 = ds.Tables["gtcompmast"];
            l1.Database.Tables["DataTable1"].SetDataSource(dt);
            l1.Database.Tables["DataTableImage"].SetDataSource(dt1);

            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = l1;
            crystalReportViewer1.Refresh();


            Cursor.Current = Cursors.Default;
            
        }

        private void comboline_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboline.Text == "ALL")
            //{
            //    string sel2 = " select 'ALL' machine FROM DUAL union ALL   select distinct c.machine from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname   join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "'  ";
            //    DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "asptblmacmas");
            //    DataTable dt2 = ds2.Tables["asptblmacmas"];
            //    combomachine.DisplayMember = "machine";
            //    combomachine.ValueMember = "machine";
            //    combomachine.DataSource = dt2;
            //}
            if (comboline.Text != "ALL" && comboline.Text !="")
            {
                string sel2 = " select 'ALL' machine FROM DUAL union ALL   select distinct c.machine from asptblmacdet c join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid   join asptbllinmas f on f.asptbllinmasid=d.line  where d.compcode='" + Class.Users.COMPCODE + "' AND F.LINE='" + comboline.Text.Trim() + "'  ";
                DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "asptblmacmas");
                DataTable dt2 = ds2.Tables["asptblmacmas"];
                combomachine.DisplayMember = "machine";
                combomachine.ValueMember = "machine";
                combomachine.DataSource = dt2;
            }
        }

        private void butmachinewise_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet(); DataTable dt = new DataTable(); DataTable dt1 = new DataTable();
            if (combomachine.Text == "ALL" && comboline.Text == "ALL")
            {
                string sel = "select  b.compname,b.address,  b.compcode,a.finyear,a.barcode,c.machine,e.processname,f.linename,f.linenumber,g.floor, a.date,a.time,a.notes from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname  join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "'";

                ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                dt = ds.Tables["asptblmanmas"];
            }
            if (combomachine.Text != "ALL" && comboline.Text == "")
            {

                string sel = "select b.compname,b.address,  b.compcode,a.finyear,a.barcode,c.machine,e.processname,f.linename,f.linenumber,g.floor, a.date,a.time,a.notes from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname  join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "' and c.machine='" + combomachine.Text + "' ";
                ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                dt = ds.Tables["asptblmanmas"];
            }
            if (combomachine.Text != "ALL" && comboline.Text != "")
            {

                string sel = "select b.compname,b.address,  b.compcode,a.finyear,a.barcode,c.machine,e.processname,f.linename,f.linenumber,g.floor, a.date,a.time,a.notes from asptblmanmas a join gtcompmast b on a.compcode=b.gtcompmastid join asptblmacdet c on a.machine=c.asptblmacdetid  join asptblmacmas d on d.asptblmacmasid=c.asptblmacmasid  join asptblbarpromas e on e.asptblbarpromasid=c.processname  join asptbllinmas f on f.asptbllinmasid=d.line  join asptblflomas g on g.asptblflomasid=f.floor  where b.compcode='" + Class.Users.HCompcode + "' and f.line='" + comboline.Text + "'  and c.machine='" + combomachine.Text + "'";
                ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                dt = ds.Tables["asptblmanmas"];
            }
            string sel1 = "select a.companylogo from gtcompmast a where a.compcode='" + Class.Users.HCompcode + "'";
            ds = Utility.ExecuteSelectQuery(sel1, "gtcompmast");
            dt1 = ds.Tables["gtcompmast"];
            l1.Database.Tables["DataTable1"].SetDataSource(dt);
            l1.Database.Tables["DataTableImage"].SetDataSource(dt1);

            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = l1;
            crystalReportViewer1.Refresh();


            Cursor.Current = Cursors.Default;
        }
    }
}

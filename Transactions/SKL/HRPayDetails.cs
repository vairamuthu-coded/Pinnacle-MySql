using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;


namespace Pinnacle.Transactions.SKL
{
    public partial class HRPayDetails : Form,ToolStripAccess
    {
        private static HRPayDetails _instance; Models.Master mas = new Models.Master();
        public static HRPayDetails Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HRPayDetails();
                GlobalVariables.CurrentForm = _instance; return _instance;

            }
        }
        public HRPayDetails()
        {
            InitializeComponent();
            Class.Users.IPADDRESS = GenFun.GetLocalIPAddress();            
            Class.Users.CREATED = Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy") + " " + System.DateTime.Now.ToLongTimeString());
            Class.Users.SysDate = Convert.ToString(System.DateTime.Now.ToString("dd-MM-yyyy"));
            Class.Users.SysTime = Convert.ToString(DateTime.Now.ToShortTimeString().ToString());
            combofinyear.Text = Convert.ToString(System.DateTime.Now.Year.ToString());
            Class.Users.Finyear = System.DateTime.Now.Year.ToString();
        }
        private string readserialvalue;
        private Decimal wt = 0;
        bool valid = false; bool validprint = false;
        Models.Validate va = new Models.Validate();
        ListView listfilter = new ListView();
        private object worksheet;
        private void empty()
        {
            txthrpaydetailsid.Text = ""; combocompcode.Text = ""; combocompname.Text = ""; combocompcode.SelectedIndex = -1;
            combocompcode.Enabled = true; combofinyear.Text = Class.Users.Finyear; txtdate.Value = System.DateTime.Now;
            txtidcard.Text = "";
            txtempname.Text = "";
            txtuanno.Text = "0";
            txtesino.Text = "0";
            txtfathername.Text = "0";
            txtunited.Text = "";
            txtdepartment.Text = "";
            txtdesignation.Text = "";
            txtorjpayabledays.Text = "0";
            txtnhdays.Text = "0";
            txtpayabledays.Text = "0";
            txtgovtdaysal.Text = "0";
            txtotwages.Text = "0";
            txtbasicda.Text = "0";
            txtbasic.Text = "0";
            txtda.Text = "0";
            txthra.Text = "0";
            txtothers.Text = "0";
            txtebasic.Text = "0";
            txteda.Text = "0";
            txtebasicda.Text = "0";
            txtehra.Text = "0";
            txteothers.Text = "0";
            txtpayableothours.Text = "0";
            txtotamount.Text = "0";
            txtincentive.Text = "0";
            txtgovtgross.Text = "0";
            txtpfamount.Text = "0";
            txtesiamount.Text = "0";
            txtmessamount.Text = "0";
            txtdeduction.Text = "0";
            txtnetamount.Text = "0";
            txtbankaccountno.Text = "0";
            txtbankname.Text = "0";
            txtifsccode.Text = "0";
            txtpayperiod.Text = "";
            txtcategory.Text = "0";
            txtotherexp.Text = "0";
            txtadvance.Text = "0";
            comboperiodsearh.SelectedIndex = -1; comboperiodsearh.Text = "";
            txtcreditdate.Text = "0"; dataGridView1.AllowUserToAddRows = true;
         
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();


              combounitsearch.SelectedIndex = -1;combounitsearch.Text = ""; comboperiodsearh.SelectedIndex = -1; comboperiodsearh.Text = "";
           
             dataGridView1.AllowUserToAddRows = false; 
            checkall.Checked = false;
           
            this.Font = Class.Users.FontName;
           
            this.BackColor = Class.Users.BackColors;
            panel2.BackColor = Class.Users.BackColors;
           // panel1.BackColor = Class.Users.BackColors;
            panel4.BackColor = Class.Users.BackColors;
            panel3.BackColor = Class.Users.BackColors;
            panel5.BackColor = Class.Users.BackColors;
          butheader.BackColor= Class.Users.BackColors;
            listView1.Font = Class.Users.FontName;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Class.Users.BackColors;
        }
        public void companyload()
        {
            try
            {
                string sel = "select a.gtcompmastid,a.compcode from  gtcompmast a  where a.ptransaction ='COMPANY' and a.active='T'  and a.compcode='" + Class.Users.HCompcode + "' order by 2 ;";
                DataSet ds = Utility.ExecuteSelectQuery(sel, "gtcompmast");
                DataTable dt = ds.Tables["gtcompmast"];

                combocompcode.DisplayMember = "compcode";
                combocompcode.ValueMember = "gtcompmastid";
                combocompcode.DataSource = dt;

                try
                {

                    string sel1 = "select '-1' as  gtcompmastid,'-------' as compcode from dual union all select distinct a.gtcompmastid,a.compcode from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where a.ptransaction ='COMPANY' and a.active='T' and a.compcode='" + Class.Users.HCompcode + "' order by 2";
                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "hrpaydetails");
                    DataTable dt1 = ds1.Tables["hrpaydetails"];

                    combounitsearch.DisplayMember = "compcode";
                    combounitsearch.ValueMember = "gtcompmastid";
                    combounitsearch.DataSource = dt1;

                    combounitreport.DisplayMember = "compcode";
                    combounitreport.ValueMember = "gtcompmastid";
                    combounitreport.DataSource = dt1;
                    //combounitsearch.Text = ""; combounitsearch.SelectedIndex = -1;
                    //combounitreport.Text = ""; combounitreport.SelectedIndex = -1;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("companyload: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("companyload: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void combocompcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txthrpaydetailsid.Text == "" && Class.Users.HCompcode != "")
                {
                    //string sel = "select a.gtcompmastid,a.compcode, a.compname from  gtcompmast a  where a.ptransaction ='COMPANY' and a.compcode='" + combocompcode.Text + "' ;";
                    //DataSet ds = Utility.ExecuteSelectQuery(sel, "gtcompmast");
                    //DataTable dt = ds.Tables["gtcompmast"];
                    //combocompname.Text = dt.Rows[0]["compname"].ToString();
                    autonumberload();
                }
                //if (txthrpaydetailsid.Text != "")
                //{

                //    txtdocid.Text = ""; txthrpaydetailsid1.Text = "";
                //    string sel = "select max(HRPayDetailsid)+1 as id,b.compname from HRPayDetails a join gtcompmast b on a.compname=b.gtcompmastid where b.ptransaction='COMPANY'  and b.compcode='" + combocompcode.Text + "'; ";
                //    DataSet ds = Utility.ExecuteSelectQuery(sel, "HRPayDetails");
                //    DataTable dt = ds.Tables["HRPayDetails"];
                //    combocompname.Text = dt.Rows[0]["compname"].ToString();
                //    txtdocid.Text = combocompcode.Text + "/" + Class.Users.Finyear + "/" + dt.Rows[0]["id"].ToString();
                //    txthrpaydetailsid1.Text = dt.Rows[0]["id"].ToString();

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void autonumberload()
        {
            try
            {

                string sel = "select max(a.HRPayDetailsid1)+1 as id,b.compname from HRPayDetails a join gtcompmast b on a.compname=b.gtcompmastid  where b.ptransaction='COMPANY' and a.active='T' and a.finyear='" + Class.Users.Finyear + "' and b.compcode='" + Class.Users.HCompcode + "' group by b.compname; ";
                DataSet ds = Utility.ExecuteSelectQuery(sel, "HRPayDetails");
                DataTable dt = ds.Tables["HRPayDetails"];
                int cnt = dt.Rows.Count;
                if (cnt == 0)
                {

                    string sel1 = "select a.gtcompmastid,a.compcode, a.compname from  gtcompmast a where a.compcode='" + Class.Users.HCompcode + "'  order by 2 ;";
                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "gtcompmast");
                    DataTable dt1 = ds1.Tables["gtcompmast"];
                    combocompname.DisplayMember = "compname";
                    combocompname.ValueMember = "gtcompmastid";
                    combocompname.DataSource = dt1;
                    combocompname.Text = dt1.Rows[0]["compname"].ToString();
                    txtdocid.Text = combocompcode.Text + "/" + Class.Users.Finyear + "/" + 1;
                    txthrpaydetailsid1.Text = "1";
                }
                else
                {
                    combocompname.Text = dt.Rows[0]["compname"].ToString();
                    txtdocid.Text = combocompcode.Text + "/" + Class.Users.Finyear + "/" + dt.Rows[0]["id"].ToString();
                    txthrpaydetailsid1.Text = dt.Rows[0]["id"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("autonumberload: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void News()
        {
            tabControl1.SelectTab(tabPageraw2);
            GridLoad(); companyload(); autonumberload();
            empty();


            txtsearch.Focus();
        }

        public void Saves()
        {
            int result = 0; Models.HrPayModel c = new Models.HrPayModel();
            try
            {


                if (dataGridView1.Rows.Count > 0 && Class.Users.Log >= Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    Cursor = Cursors.WaitCursor; lblprogress2.Visible = true; lblprogress2.Refresh(); label48.Refresh();
                   
                    int cc = 0;
                    cc = dataGridView1.Rows.Count;
                    c.DocDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    c.FinYear = Convert.ToString(Class.Users.Finyear);
                    c.CompCode = Convert.ToInt64("0" + Class.Users.COMPCODE);
                    c.CompName = Convert.ToInt64("0" + Class.Users.COMPCODE);
                    c.CompCode1 = Class.Users.COMPCODE;
                    c.UserName = Class.Users.USERID;
                    c.CreatedBy = Convert.ToString(Class.Users.CREATED);
                    c.CreatedOn = Convert.ToString(Class.Users.CREATED);
                    c.ModifiedBy = Convert.ToString(Class.Users.HUserName);
                    c.IpAddress = Class.Users.IPADDRESS;
                    if (checkactive.Checked == true)
                        c.Active = "T";
                    else c.Active = "F";
                    if (cc >= 0)
                    {
                        progressBar3.Minimum = 0; progressBar3.Refresh();
                        progressBar3.Maximum = dataGridView1.Rows.Count;

                        for (int i = 0; i < cc; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value.ToString() != "")
                            {
                                combocompcode.Text = Class.Users.HCompcode;
                                combofinyear.Text = Class.Users.Finyear;
                                c.CompCode = Class.Users.COMPCODE;
                                c.CompName = Class.Users.COMPCODE;
                                c.FinYear = Class.Users.Finyear;
                                if (txthrpaydetailsid.Text == "") { c.HrPayDetailsId = Convert.ToInt64("0" + txthrpaydetailsid.Text); c.HrPayDetailsId1 = Convert.ToInt64("0" + txthrpaydetailsid1.Text); }
                                else { c.HrPayDetailsId = Convert.ToInt64("0" + txthrpaydetailsid.Text); c.HrPayDetailsId1 = Convert.ToInt64("0" + txthrpaydetailsid1.Text); }
                                c.DocId = Convert.ToString(txtdocid.Text);
                                c.IdCardNo = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value);
                                c.MidCard = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);
                                c.EmpName = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value);
                                c.Doj = Convert.ToDateTime(dataGridView1.Rows[i].Cells[3].Value.ToString().Substring(0, 10));

                                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                                {
                                    c.Dol = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString().Substring(0, 10));

                                }
                                else
                                {
                                    c.Dol = "";
                                }
                                c.UanNo = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value);
                                c.EsiNo = Convert.ToString(dataGridView1.Rows[i].Cells[6].Value);
                                c.FatherName = Convert.ToString(dataGridView1.Rows[i].Cells[7].Value);
                                c.United = Convert.ToString(Class.Users.HCompcode);
                                c.Category = Convert.ToString(dataGridView1.Rows[i].Cells[9].Value);
                                c.Department = Convert.ToString(dataGridView1.Rows[i].Cells[10].Value);
                                c.Designation = Convert.ToString(dataGridView1.Rows[i].Cells[11].Value);
                                c.OrjPayableDays = Convert.ToString(dataGridView1.Rows[i].Cells[12].Value);
                                c.NhDays = Convert.ToString(dataGridView1.Rows[i].Cells[13].Value);
                                c.PayableDays = Convert.ToString(dataGridView1.Rows[i].Cells[14].Value);
                                c.GovtDaySalary = Convert.ToString(dataGridView1.Rows[i].Cells[15].Value);
                                c.OtWages = Convert.ToString(dataGridView1.Rows[i].Cells[16].Value);
                                c.BasicDa = Convert.ToString(dataGridView1.Rows[i].Cells[17].Value);
                                c.Basic = Convert.ToString(dataGridView1.Rows[i].Cells[18].Value);
                                c.Da = Convert.ToString(dataGridView1.Rows[i].Cells[19].Value);
                                c.Hra = Convert.ToString(dataGridView1.Rows[i].Cells[20].Value);
                                c.Others = Convert.ToString(dataGridView1.Rows[i].Cells[21].Value);
                                c.EBasic = Convert.ToString(dataGridView1.Rows[i].Cells[22].Value);
                                c.EBasicDa = Convert.ToString(dataGridView1.Rows[i].Cells[23].Value);
                                c.EDa = Convert.ToString(dataGridView1.Rows[i].Cells[24].Value);
                                c.EHra = Convert.ToString(dataGridView1.Rows[i].Cells[25].Value);
                                c.EOthers = Convert.ToString(dataGridView1.Rows[i].Cells[26].Value);
                                c.PayableOtHours = Convert.ToString(dataGridView1.Rows[i].Cells[27].Value);
                                c.OtAmount = Convert.ToString(dataGridView1.Rows[i].Cells[28].Value);
                                c.Incentive = Convert.ToString(dataGridView1.Rows[i].Cells[29].Value);
                                c.GovtGross = Convert.ToString(dataGridView1.Rows[i].Cells[30].Value);
                                c.PfAmount = Convert.ToString(dataGridView1.Rows[i].Cells[31].Value);
                                c.EsiAmount = Convert.ToString(dataGridView1.Rows[i].Cells[32].Value);
                                c.MessAmount = Convert.ToString(dataGridView1.Rows[i].Cells[33].Value);
                                c.OthersExp = Convert.ToDecimal("0" + dataGridView1.Rows[i].Cells[34].Value).ToString();
                                c.Advance = Convert.ToDecimal("0" + dataGridView1.Rows[i].Cells[35].Value).ToString();
                                c.Deduction = Convert.ToString(dataGridView1.Rows[i].Cells[36].Value);
                                c.NetAmount = Convert.ToString(dataGridView1.Rows[i].Cells[37].Value);
                                c.BankAccountNo = Convert.ToString(dataGridView1.Rows[i].Cells[38].Value);
                                c.BankName = Convert.ToString(dataGridView1.Rows[i].Cells[39].Value);
                                c.IfscCode = Convert.ToString(dataGridView1.Rows[i].Cells[40].Value);
                                c.PayPeriod = Convert.ToString(dataGridView1.Rows[i].Cells[41].Value);
                                c.FromDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[42].Value).ToString("dd-MMM-yyyy");
                                c.ToDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[43].Value).ToString("dd-MMM-yyyy");
                                if (dataGridView1.Rows[i].Cells[44].Value.ToString() != "")
                                {
                                    c.CreditDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[44].Value).ToString("dd-MM-yyyy");                         //
                                }                                                                                                      //
                                                                                                                                       //  c.CreditDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[34].Value).ToString("dd-MMM-yyyy");                          and  doj = '" + c.Doj + "'  and uanno = '" + c.UanNo + "'  and esino = '" + c.EsiNo + "'  and fathername = '" + c.FatherName + "' and united = '" + c.United + "'  and  category='" + c.Category + "' and   department='" + c.Department + "'  and   designation='" + c.Designation + "'  and    orjpayabledays='" + c.OrjPayableDays + "'  and  nhdays='" + c.NhDays + "'  and  payabledays='" + c.PayableDays + "'  and  govtdaysalary='" + c.GovtDaySalary + "' and  otwages='" + c.OtWages + "' and  basicda='" + c.Basic + "' and   basic='" + c.Basic + "' and  da='" + c.Da + "' and  hra='" + c.Hra + "' and  others='" + c.Others + "' and  ebasic='" + c.EBasic + "' and   eda='" + c.EDa + "' and    ebasicda='" + c.EBasicDa + "' and  ehra='" + c.EHra + "' and  eothers='" + c.EOthers + "' and  payableothrs='" + c.payableothrs + "' and  otamount='" + c.OtAmount + "' and  incentive='" + c.Incentive + "' and   govtgross='" + c.GovtGross + "' and    pfamount='" + c.PfAmount + "' and  esiamount='" + c.EsiAmount + "' and  messamount='" + c.MessAmount + "' and  deduction='" + c.Deduction + "' and   netamount='" + c.NetAmount + "' and    bankaccountno='" + c.BankAccountNo + "'  and  bankname='" + c.BankName + "'  and  ifsccode='" + c.IfscCode + "' and  payperiod='" + c.PayPeriod + "'  and  fromdate='" + c.FromDate + "' and  todate='" + c.ToDate + "' and active='" + c.Active + "'
                                DataTable dt = new DataTable();dt = null;
                                string sel = "select  hrpaydetailsid    from  HRPayDetails   WHERE   compcode='" + c.CompCode + "' and midcard='" + c.MidCard + "' and  idcardno='" + c.IdCardNo + "'  and  empname='" + c.EmpName + "' and  doj='" + Convert.ToDateTime(c.Doj).ToString("yyyy-MM-dd").Substring(0, 10) + "'  and  united='" + Class.Users.HCompcode + "' and payperiod='" + c.PayPeriod + "' ;";
                                 dt = Utility.SQLQuery(sel);                              
                                if (dt.Rows.Count != 0)
                                {
                                    result= 2;
                                }                                
                                else if (dt.Rows.Count == 0 && Convert.ToInt32("0" + txthrpaydetailsid.Text) == 0 || Convert.ToInt32("0" + txthrpaydetailsid.Text) == 0)
                                {

                                    string query = @"INSERT INTO HRPayDetails    (hrpaydetailsid1, docid, docdate, finyear, compcode, compname,     midcard, idcardno, empname, doj, dol, uanno, esino, fathername,     united, category, department, designation,     orjpayabledays, nhdays, payabledays, govtdaysalary,     otwages, basicda, basic, da, hra, others,     ebasic, ebasicda, eda, ehra, eothers,     payableothrs, otamount, incentive, govtgross,     pfamount, esiamount, messamount, othersexp,     advance, deduction, netamount,     bankaccountno, bankname, ifsccode,     payperiod, fromdate, todate,     active, creditdate, compcode1,     username, createdby, createdon,     modifiedby, ipaddress)  VALUES    (@hrpaydetailsid1, @docid, @docdate, @finyear, @compcode, @compname,     @midcard, @idcardno, @empname, @doj, @dol, @uanno, @esino, @fathername,     @united, @category, @department, @designation,     @orjpayabledays, @nhdays, @payabledays, @govtdaysalary,     @otwages, @basicda, @basic, @da, @hra, @others,     @ebasic, @ebasicda, @eda, @ehra, @eothers,     @payableothrs, @otamount, @incentive, @govtgross,     @pfamount, @esiamount, @messamount, @othersexp,     @advance, @deduction, @netamount,     @bankaccountno, @bankname, @ifsccode,     @payperiod, @fromdate, @todate,     @active, @creditdate, @compcode1,     @username, @createdby, @createdon,     @modifiedby, @ipaddress)";
                                   
                                    using (MySqlCommand cmd = new MySqlCommand(query, Utility.Connect()))
                                    {
                                        cmd.Parameters.AddWithValue("@hrpaydetailsid1", c.HrPayDetailsId1);
                                        cmd.Parameters.AddWithValue("@docid", c.DocId);
                                        cmd.Parameters.AddWithValue("@docdate", c.DocDate);
                                        cmd.Parameters.AddWithValue("@finyear", c.FinYear);
                                        cmd.Parameters.AddWithValue("@compcode", c.CompCode);
                                        cmd.Parameters.AddWithValue("@compname", c.CompName);
                                        cmd.Parameters.AddWithValue("@midcard", c.MidCard);
                                        cmd.Parameters.AddWithValue("@idcardno", c.IdCardNo);
                                        cmd.Parameters.AddWithValue("@empname", c.EmpName);
                                        cmd.Parameters.AddWithValue("@doj", c.Doj);
                                        if (string.IsNullOrEmpty(c.Dol))
                                            cmd.Parameters.AddWithValue("@dol", DBNull.Value);
                                        else
                                            cmd.Parameters.AddWithValue("@dol", Convert.ToDateTime(c.Dol));

                                        cmd.Parameters.AddWithValue("@uanno", c.UanNo);
                                        cmd.Parameters.AddWithValue("@esino", c.EsiNo);
                                        cmd.Parameters.AddWithValue("@fathername", c.FatherName);

                                        cmd.Parameters.AddWithValue("@united", c.United);
                                        cmd.Parameters.AddWithValue("@category", c.Category);
                                        cmd.Parameters.AddWithValue("@department", c.Department);
                                        cmd.Parameters.AddWithValue("@designation", c.Designation);
                                        cmd.Parameters.AddWithValue("@orjpayabledays", c.OrjPayableDays);
                                        cmd.Parameters.AddWithValue("@nhdays", c.NhDays);
                                        cmd.Parameters.AddWithValue("@payabledays", c.PayableDays);
                                        cmd.Parameters.AddWithValue("@govtdaysalary", c.GovtDaySalary);
                                        cmd.Parameters.AddWithValue("@otwages", c.OtWages);
                                        cmd.Parameters.AddWithValue("@basicda", c.BasicDa);

                                        cmd.Parameters.AddWithValue("@basic", c.Basic);
                                        cmd.Parameters.AddWithValue("@da", c.Da);
                                        cmd.Parameters.AddWithValue("@hra", c.Hra);
                                        cmd.Parameters.AddWithValue("@others", c.Others);
                                        cmd.Parameters.AddWithValue("@ebasic", c.EBasic);
                                        cmd.Parameters.AddWithValue("@ebasicda", c.EBasicDa);
                                        cmd.Parameters.AddWithValue("@eda", c.EDa);
                                        cmd.Parameters.AddWithValue("@ehra", c.EHra);
                                        cmd.Parameters.AddWithValue("@eothers", c.EOthers);
                                        cmd.Parameters.AddWithValue("@payableothrs", c.PayableOtHours);

                                        cmd.Parameters.AddWithValue("@otamount", c.OtAmount);
                                        cmd.Parameters.AddWithValue("@incentive", c.Incentive);
                                        cmd.Parameters.AddWithValue("@govtgross", c.GovtGross);
                                        cmd.Parameters.AddWithValue("@pfamount", c.PfAmount);
                                        cmd.Parameters.AddWithValue("@esiamount", c.EsiAmount);
                                        cmd.Parameters.AddWithValue("@messamount", c.MessAmount);
                                        cmd.Parameters.AddWithValue("@othersexp", c.OthersExp);
                                        cmd.Parameters.AddWithValue("@advance", c.Advance);
                                        cmd.Parameters.AddWithValue("@deduction", c.Deduction);
                                        cmd.Parameters.AddWithValue("@netamount", c.NetAmount);

                                        cmd.Parameters.AddWithValue("@bankaccountno", c.BankAccountNo);
                                        cmd.Parameters.AddWithValue("@bankname", c.BankName);
                                        cmd.Parameters.AddWithValue("@ifsccode", c.IfscCode);
                                        cmd.Parameters.AddWithValue("@payperiod", c.PayPeriod);
                                        cmd.Parameters.AddWithValue("@fromdate", c.FromDate);
                                        cmd.Parameters.AddWithValue("@todate", c.ToDate);
                                        cmd.Parameters.AddWithValue("@active", c.Active);
                                        cmd.Parameters.AddWithValue("@creditdate", c.CreditDate);
                                        cmd.Parameters.AddWithValue("@compcode1", c.CompCode1);
                                        cmd.Parameters.AddWithValue("@username", c.UserName);

                                        cmd.Parameters.AddWithValue("@createdby", c.CreatedBy);
                                        cmd.Parameters.AddWithValue("@createdon", c.CreatedOn);
                                        cmd.Parameters.AddWithValue("@modifiedby", c.ModifiedBy);
                                        cmd.Parameters.AddWithValue("@ipaddress", c.IpAddress);
                                        // Just map property → parameter

                                        result = cmd.ExecuteNonQuery();
                                    }

                                    //string ins = "";
                                    //if (c.Dol == "")
                                    //{
                                    //    ins = "insert into HRPayDetails(hrpaydetailsid1,  docid,  docdate ,  finyear,  compcode,  compname,midcard,  idcardno ,  empname,  doj,uanno,  esino,  fathername ,  united,  category,  department, designation, orjpayabledays ,  nhdays ,  payabledays ,  govtdaysalary,  otwages,  basicda ,  basic,  da ,  hra,  others, ebasic, ebasicda,  eda,  ehra,  eothers,  payableothrs ,  otamount,  incentive,  govtgross,  pfamount,  esiamount,  messamount,  othersexp,  advance, deduction,  netamount ,  bankaccountno,  bankname , ifsccode,payperiod, fromdate ,  todate,  active,creditdate,  compcode1,  username,  createdby,  createdon,  modifiedby,  ipaddress)values('" + c.HrPayDetailsId1 + "', '" + c.DocId + "',  '" + c.DocDate + "' ,  '" + c.FinYear + "',  '" + c.CompCode + "',  '" + c.CompName + "', '" + c.MidCard + "' ,  '" + c.IdCardNo + "' ,  '" + c.EmpName + "',  '" + Convert.ToDateTime(c.Doj).ToString("yyyy-MM-dd").Substring(0, 10) + "','" + c.UanNo + "',  '" + c.EsiNo + "',  '" + c.FatherName + "' ,  '" + c.United + "',  '" + c.Category + "',  '" + c.Department + "', '" + c.Designation + "', '" + c.OrjPayableDays + "' ,  '" + c.NhDays + "' ,  '" + c.PayableDays + "' ,  '" + c.GovtDaySalary + "',  '" + c.OtWages + "',  '" + c.BasicDa + "' ,  '" + c.Basic + "',  '" + c.Da + "' ,  '" + c.Hra + "',  '" + c.Others + "', '" + c.EBasic + "', '" + c.EBasicDa + "',  '" + c.EDa + "',  '" + c.EHra + "', '" + c.EOthers + "',  '" + c.PayableOtHours + "' ,  '" + c.OtAmount + "',  '" + c.Incentive + "',  '" + c.GovtGross + "',  '" + c.PfAmount + "',  '" + c.EsiAmount + "',  '" + c.MessAmount + "',  '" + c.OthersExp + "',  '" + c.Advance + "', '" + c.Deduction + "',  '" + c.NetAmount + "' ,  '" + c.BankAccountNo + "',  '" + c.BankName + "' ,  '" + c.IfscCode + "',  '" + c.PayPeriod + "', '" + c.FromDate + "',  '" + c.ToDate + "',  '" + c.Active + "','" + c.CreditDate + "',  '" + c.CompCode1 + "',  '" + c.UserName + "' , '" + c.CreatedBy + "',  '" + c.CreatedOn + "',  '" + c.ModifiedBy + "',  '" + c.IpAddress + "' );";
                                    //   Utility.ExecuteNonQuery(ins);
                                    //}
                                    //else
                                    //{
                                    //    ins = "insert into HRPayDetails(hrpaydetailsid1,  docid,  docdate ,  finyear,  compcode,  compname,midcard,  idcardno ,  empname,  doj,dol,  uanno,  esino,  fathername ,  united,  category,  department, designation, orjpayabledays ,  nhdays ,  payabledays ,  govtdaysalary,  otwages,  basicda ,  basic,  da ,  hra,  others, ebasic, ebasicda,  eda,  ehra,  eothers,  payableothrs ,  otamount,  incentive,  govtgross,  pfamount,  esiamount,  messamount,  othersexp,  advance, deduction,  netamount ,  bankaccountno,  bankname , ifsccode,payperiod, fromdate ,  todate,  active,creditdate,  compcode1,  username,  createdby,  createdon,  modifiedby,  ipaddress)values('" + c.HrPayDetailsId1 + "', '" + c.DocId + "',  '" + c.DocDate + "' ,  '" + c.FinYear + "',  '" + c.CompCode + "',  '" + c.CompName + "', '" + c.MidCard + "' ,  '" + c.IdCardNo + "' ,  '" + c.EmpName + "',  '" + Convert.ToDateTime(c.Doj).ToString("yyyy-MM-dd").Substring(0, 10) + "','" + Convert.ToDateTime(c.CreditDate).ToString("yyyy-MM-dd").Substring(0, 10) + "',  '" + c.UanNo + "',  '" + c.EsiNo + "',  '" + c.FatherName + "' ,  '" + c.United + "',  '" + c.Category + "',  '" + c.Department + "', '" + c.Designation + "', '" + c.OrjPayableDays + "' ,  '" + c.NhDays + "' ,  '" + c.PayableDays + "' ,  '" + c.GovtDaySalary + "',  '" + c.OtWages + "',  '" + c.BasicDa + "' ,  '" + c.Basic + "',  '" + c.Da + "' ,  '" + c.Hra + "',  '" + c.Others + "', '" + c.EBasic + "', '" + c.EBasicDa + "',  '" + c.EDa + "',  '" + c.EHra + "', '" + c.EOthers + "',  '" + c.PayableOtHours + "' ,  '" + c.OtAmount + "',  '" + c.Incentive + "',  '" + c.GovtGross + "',  '" + c.PfAmount + "',  '" + c.EsiAmount + "',  '" + c.MessAmount + "',  '" + c.OthersExp + "',  '" + c.Advance + "', '" + c.Deduction + "',  '" + c.NetAmount + "' ,  '" + c.BankAccountNo + "',  '" + c.BankName + "' ,  '" + c.IfscCode + "',  '" + c.PayPeriod + "', '" + c.FromDate + "',  '" + c.ToDate + "',  '" + c.Active + "','" + c.CreditDate + "',  '" + c.CompCode1 + "',  '" + c.UserName + "' , '" + c.CreatedBy + "',  '" + c.CreatedOn + "',  '" + c.ModifiedBy + "',  '" + c.IpAddress + "' );";
                                    //   Utility.ExecuteNonQuery(ins);
                                    //}
                                    decimal per = Convert.ToDecimal(100 / GenFun.ToDecimal(dataGridView1.Rows.Count)) * (i + 1);
                                    lblprogress3.Text = " Data Transfer to Table : " + (per).ToString("N0") + " %";
                                    label48.Text = " Total Rows : " + i.ToString() + "  " + (per).ToString("N0") + " %" + "Emp Id" + c.MidCard + " Name : " + c.EmpName;
                                    lblprogress3.Refresh(); label48.Refresh();
                                    progressBar3.Value = i + 1;
                                }
                                else
                                {                            

                                    string up = "update  HRPayDetails  set hrpaydetailsid1='" + c.HrPayDetailsId1 + "' ,docid='" + c.DocId + "' , docdate='" + c.DocDate + "',finyear='" + c.FinYear + "',compcode='" + c.CompCode + "', compname='" + c.CompName + "', midcard='" + c.MidCard + "',idcardno='" + c.IdCardNo + "' , empname='" + c.EmpName + "', doj='" + c.Doj + "',dol='" + c.Dol + "', uanno='" + c.UanNo + "' , esino='" + c.EsiNo + "' , fathername='" + c.FatherName + "', united='" + c.United + "' , category='" + c.Category + "',  department='" + c.Department + "', designation='" + c.Designation + "' , orjpayabledays='" + c.OrjPayableDays + "' , nhdays='" + c.NhDays + "' , payabledays='" + c.PayableDays + "' , govtdaysalary='" + c.GovtDaySalary + "', otwages='" + c.OtWages + "', basicda='" + c.BasicDa + "',  basic='" + c.Basic + "', da='" + c.Da + "', hra='" + c.Hra + "', others='" + c.Others + "',ebasicda='" + c.EBasicDa + "', ebasic='" + c.EBasic + "',  eda='" + c.EDa + "', ehra='" + c.EHra + "', eothers='" + c.EOthers + "', payableothrs='" + c.PayableOtHours + "', otamount='" + c.OtAmount + "', incentive='" + c.Incentive + "',  govtgross='" + c.GovtGross + "',   pfamount='" + c.PfAmount + "', esiamount='" + c.EsiAmount + "', messamount='" + c.MessAmount + "', othersexp='" + c.OthersExp + "', advance='" + c.Advance + "', deduction='" + c.Deduction + "',  netamount='" + c.NetAmount + "',   bankaccountno='" + c.BankAccountNo + "' , bankname='" + c.BankName + "' , ifsccode='" + c.IfscCode + "', payperiod='" + c.PayPeriod + "' , fromdate='" + c.FromDate + "', todate='" + c.ToDate + "',active='" + c.Active + "',creditdate='" + c.CreditDate + "',compcode1='" + Class.Users.COMPCODE + "', username='" + Class.Users.USERID + "',createdby='" + Class.Users.CREATED + "', modifiedby='" + Class.Users.HUserName + "',ipaddress='" + Class.Users.IPADDRESS + "' where hrpaydetailsid='" + txthrpaydetailsid.Text + "';";
                                   result=  Utility.ExecuteNonQuery(up);
                                    decimal per = Convert.ToDecimal(100 / GenFun.ToDecimal(dataGridView1.Rows.Count)) * (i + 1);
                                    lblprogress3.Text = " Data Transfer to Table : " + (per).ToString("N0") + " %";
                                    label48.Text = " Total Rows : " + i.ToString() + "  " + (per).ToString("N0") + " %" + "Emp Id" + c.MidCard + " Name : " + c.EmpName;
                                    lblprogress3.Refresh(); label48.Refresh();

                                    progressBar3.Value = i + 1;
                                }

                            }
                        }

                        if (result==1)
                        {
                            Cursor = Cursors.Default;
                           // MessageBox.Show("Record Saved Successfully.Toal Record are:," + cc.ToString(), " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            mas.pop("Record Saved Successfully.Toal Record are: ", " " + cc.ToString(), "");
                            GridLoad(); companyload(); progressBar3.Value = 0; lblprogress3.Text = "";
                            tabControl1.SelectTab(tabPageraw2);empty();
                        }
                        if (result == 2)
                        {
                            Cursor = Cursors.Default;
                            //MessageBox.Show("Record Updated  Successfully.Toal Record are:," + cc.ToString(), " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            mas.pop("Record Saved Successfully ", "Vehicle No: " + cc.ToString(), "");
                            GridLoad(); companyload(); progressBar3.Value = 0; lblprogress3.Text = "";
                            tabControl1.SelectTab(tabPageraw2); empty();
                        }

                        Cursor = Cursors.Default;
                    }
                }
                else
                {
                    if (dataGridView1.Rows.Count == 0)
                    {

                        if (combocompcode.Text == "")
                        {
                            MessageBox.Show("CompCode is Empty." + combocompcode.Text, " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            combocompcode.Select();
                            return;

                        }

                        if (txtempname.Text == "")
                        {
                            MessageBox.Show("Pls Enter EmpName", "Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtidcard.Text == "")
                        {
                            MessageBox.Show("Pls Enter IDCardNo", "Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtunited.Text == "")
                        {
                            MessageBox.Show("Pls Enter UnitName", "Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtempname.Text != "" && txtidcard.Text != "" && txtunited.Text != "")
                        {
                           

                            if (txthrpaydetailsid.Text == "") { c.HrPayDetailsId = Convert.ToInt64("0" + txthrpaydetailsid.Text); autonumberload(); c.HrPayDetailsId1 = Convert.ToInt64("0" + txthrpaydetailsid1.Text); }
                            else { c.HrPayDetailsId = Convert.ToInt64("0" + txthrpaydetailsid.Text); c.HrPayDetailsId1 = Convert.ToInt64("0" + txthrpaydetailsid1.Text); }
                            c.HrPayDetailsId = Convert.ToInt64("0" + txthrpaydetailsid.Text);
                            c.HrPayDetailsId1 = Convert.ToInt64("0" + txthrpaydetailsid1.Text);
                            c.DocId = Convert.ToString(txtdocid.Text);
                            c.DocDate = txtdate.Value.ToString("yyyy-MM-dd");
                            c.FinYear = Convert.ToString(combofinyear.Text);
                            c.CompCode = Convert.ToInt64("0" + combocompcode.SelectedValue);
                            c.CompName = Convert.ToInt64("0" + combocompname.SelectedValue);
                            c.IdCardNo = Convert.ToString(txtsequenceid.Text);
                            c.MidCard = Convert.ToString(txtidcard.Text);
                            c.EmpName = Convert.ToString(txtempname.Text);
                            c.Doj = Convert.ToDateTime(txtdoj.Text);
                            if (txtdol.Value.ToString() != "")
                            {
                                c.Dol = Convert.ToString(txtdol.Text.ToString().Substring(0, 10));

                            }
                            else
                            {
                                c.Dol = "";
                            }                            
                            c.UanNo = Convert.ToString(txtuanno.Text);
                            c.EsiNo = Convert.ToString(txtesino.Text);
                            c.FatherName = Convert.ToString(txtfathername.Text);
                            c.United = Convert.ToString(txtunited.Text);
                            c.Category = Convert.ToString(txtcategory.Text);
                            c.Department = Convert.ToString(txtdepartment.Text);
                            c.Designation = Convert.ToString(txtdesignation.Text);
                            c.OrjPayableDays = Convert.ToString(txtorjpayabledays.Text);
                            c.NhDays = Convert.ToString(txtnhdays.Text);
                            c.PayableDays = Convert.ToString(txtpayabledays.Text);
                            c.GovtDaySalary = Convert.ToString(txtgovtdaysal.Text);
                            c.OtWages = Convert.ToString(txtotwages.Text);
                            c.BasicDa = Convert.ToString(txtbasicda.Text);
                            c.Basic = Convert.ToString(txtbasic.Text);
                            c.Da = Convert.ToString(txtda.Text);
                            c.Hra = Convert.ToString(txthra.Text);
                            c.Others = Convert.ToString(txtothers.Text);
                            c.EBasic = Convert.ToString(txtebasic.Text);
                            c.EDa = Convert.ToString(txteda.Text);
                            c.EBasicDa = Convert.ToString(txtebasicda.Text);
                            c.EHra = Convert.ToString(txtehra.Text);
                            c.EOthers = Convert.ToString(txteothers.Text);
                            c.PayableOtHours = Convert.ToString(txtpayableothours.Text);
                            c.OtAmount = Convert.ToString(txtotamount.Text);
                            c.Incentive = Convert.ToString(txtincentive.Text);
                            c.GovtGross = Convert.ToString(txtgovtgross.Text);
                            c.PfAmount = Convert.ToString(txtpfamount.Text);
                            c.EsiAmount = Convert.ToString(txtesiamount.Text);
                            c.MessAmount = Convert.ToString(txtmessamount.Text);
                            c.OthersExp = Convert.ToString(txtotherexp.Text);
                            c.Advance = Convert.ToString(txtadvance.Text);
                            c.Deduction = Convert.ToString(txtdeduction.Text);
                            c.NetAmount = Convert.ToString(txtnetamount.Text);
                            c.BankAccountNo = Convert.ToString(txtbankaccountno.Text);
                            c.BankName = Convert.ToString(txtbankname.Text);
                            c.IfscCode = Convert.ToString(txtifsccode.Text);
                            c.PayPeriod = Convert.ToString(txtpayperiod.Text);
                            c.FromDate = txtfromdate.Value.ToString("dd-MMM-yyyy");
                            c.ToDate = txttodate.Value.ToString("dd-MMM-yyyy");
                            c.CreditDate = Convert.ToString(txtcreditdate.Text);
                            string sel = "select hrpaydetailsid  from  HRPayDetails   WHERE  compcode='" + c.CompCode + "' and  compname='" + c.CompName + "' and midcard='" + c.MidCard + "' and  idcardno='" + c.IdCardNo + "'  and  empname='" + c.EmpName + "' and  doj='" + Convert.ToDateTime(c.Doj).ToString("yyyy-MM-dd").Substring(0, 10) + "' and  uanno='" + c.UanNo + "'  and  esino='" + c.EsiNo + "'  and  fathername='" + c.FatherName + "' and  united='" + c.United + "'  and  category='" + c.Category + "' and   department='" + c.Department + "'  and   designation='" + c.Designation + "'  and    orjpayabledays='" + c.OrjPayableDays + "'  and  nhdays='" + c.NhDays + "'  and  payabledays='" + c.PayableDays + "'  and  govtdaysalary='" + c.GovtDaySalary + "' and  otwages='" + c.OtWages + "' and  basicda='" + c.BasicDa + "' and   basic='" + c.Basic + "' and  da='" + c.Da + "' and  hra='" + c.Hra + "' and  others='" + c.Others + "' and    ebasicda='" + c.EBasicDa + "' and  ebasic='" + c.EBasic + "' and   eda='" + c.EDa + "'  and  ehra='" + c.EHra + "' and  eothers='" + c.EOthers + "' and  payableothrs='" + c.PayableOtHours + "' and  otamount='" + c.OtAmount + "' and  incentive='" + c.Incentive + "' and   govtgross='" + c.GovtGross + "' and    pfamount='" + c.PfAmount + "' and  esiamount='" + c.EsiAmount + "' and  messamount='" + c.MessAmount + "' and  othersexp='" + c.OthersExp + "' and  advance='" + c.Advance + "'   and  deduction='" + c.Deduction + "' and   netamount='" + c.NetAmount + "' and    bankaccountno='" + c.BankAccountNo + "'  and  bankname='" + c.BankName + "'  and  ifsccode='" + c.IfscCode + "' and  payperiod='" + c.PayPeriod + "'  and  fromdate='" + c.FromDate + "' and  todate='" + c.ToDate + "' and active='" + c.Active + "' and  creditdate='" + c.CreditDate + "' ;";
                            DataTable dt = Utility.SQLQuery(sel);
                            if (dt.Rows.Count != 0)
                            {
                                empty(); tabControl1.SelectTab(tabPageraw2);
                            }
                            else if (dt.Rows.Count != 0 && Convert.ToInt32("0" + txthrpaydetailsid.Text) == 0 || Convert.ToInt32("0" + txthrpaydetailsid.Text) == 0)
                            {                            


                                string query = @"INSERT INTO HRPayDetails    (hrpaydetailsid1, docid, docdate, finyear, compcode, compname,     midcard, idcardno, empname, doj, dol, uanno, esino, fathername,     united, category, department, designation,     orjpayabledays, nhdays, payabledays, govtdaysalary,     otwages, basicda, basic, da, hra, others,     ebasic, ebasicda, eda, ehra, eothers,     payableothrs, otamount, incentive, govtgross,     pfamount, esiamount, messamount, othersexp,     advance, deduction, netamount,     bankaccountno, bankname, ifsccode,     payperiod, fromdate, todate,     active, creditdate, compcode1,     username, createdby, createdon,     modifiedby, ipaddress)  VALUES    (@hrpaydetailsid1, @docid, @docdate, @finyear, @compcode, @compname,     @midcard, @idcardno, @empname, @doj, @dol, @uanno, @esino, @fathername,     @united, @category, @department, @designation,     @orjpayabledays, @nhdays, @payabledays, @govtdaysalary,     @otwages, @basicda, @basic, @da, @hra, @others,     @ebasic, @ebasicda, @eda, @ehra, @eothers,     @payableothrs, @otamount, @incentive, @govtgross,     @pfamount, @esiamount, @messamount, @othersexp,     @advance, @deduction, @netamount,     @bankaccountno, @bankname, @ifsccode,     @payperiod, @fromdate, @todate,     @active, @creditdate, @compcode1,     @username, @createdby, @createdon,     @modifiedby, @ipaddress)";

                                var parameters = new Dictionary<string, object>
                                {{"@hrpaydetailsid1", c.HrPayDetailsId1},    {"@docid", c.DocId},    {"@docdate", c.DocDate},    {"@finyear", c.FinYear},    {"@compcode", c.CompCode},    {"@compname", c.CompName},    {"@midcard", c.MidCard},    {"@idcardno", c.IdCardNo},    {"@empname", c.EmpName},    {"@doj", c.Doj},    {"@dol", c.Dol},    {"@uanno", c.UanNo},    {"@esino", c.EsiNo},    {"@fathername", c.FatherName},    {"@united", c.United},    {"@category", c.Category},    {"@department", c.Department},    {"@designation", c.Designation},    {"@orjpayabledays", c.OrjPayableDays},    {"@nhdays", c.NhDays},    {"@payabledays", c.PayableDays},    {"@govtdaysalary", c.GovtDaySalary},    {"@otwages", c.OtWages},    {"@basicda", c.BasicDa},    {"@basic", c.Basic},    {"@da", c.Da},    {"@hra", c.Hra},    {"@others", c.Others},    {"@ebasic", c.EBasic},    {"@ebasicda", c.EBasicDa},    {"@eda", c.EDa},    {"@ehra", c.EHra},    {"@eothers", c.EOthers},    {"@payableothrs", c.PayableOtHours},    {"@otamount", c.OtAmount},    {"@incentive", c.Incentive},    {"@govtgross", c.GovtGross},    {"@pfamount", c.PfAmount},    {"@esiamount", c.EsiAmount},    {"@messamount", c.MessAmount},    {"@othersexp", c.OthersExp},    {"@advance", c.Advance},    {"@deduction", c.Deduction},    {"@netamount", c.NetAmount},    {"@bankaccountno", c.BankAccountNo},    {"@bankname", c.BankName},    {"@ifsccode", c.IfscCode},    {"@payperiod", c.PayPeriod},    {"@fromdate", c.FromDate},    {"@todate", c.ToDate},    {"@active", c.Active},    {"@creditdate", c.CreditDate},    {"@compcode1", c.CompCode1},    {"@username", c.UserName},    {"@createdby", c.CreatedBy},    {"@createdon", c.CreatedOn},    {"@modifiedby", c.ModifiedBy},    {"@ipaddress", c.IpAddress}};
                                result = Utility.ExecuteNonQuery(query, parameters);
                         

                               
                                //    ins = "insert into HRPayDetails(hrpaydetailsid1, docid,  docdate ,finyear,compcode,compname, midcard,idcardno , empname, doj,dol,uanno,esino,fathername,united,category,department, designation, orjpayabledays , nhdays , payabledays , govtdaysalary, otwages, basicda , basic, da ,hra, others, ebasic, ebasicda,  eda,  ehra,  eothers, payableothrs ,otamount,incentive,  govtgross,  pfamount, esiamount,messamount,othersexp,advance, deduction,  netamount ,  bankaccountno,  bankname , ifsccode,payperiod, fromdate ,todate,active,creditdate,compcode1,username, createdby, createdon,  modifiedby,  ipaddress)values('" + c.HrPayDetailsId1 + "', '" + c.DocId + "',  '" + c.DocDate + "' ,  '" + c.FinYear + "',  '" + c.CompCode + "',  '" + c.CompName + "', '" + c.MidCard + "', '" + c.IdCardNo + "' ,  '" + c.EmpName + "',  '" + c.Doj.ToString("yyyy-MM-dd") + "','" + c.Dol + "','" + c.UanNo + "','" + c.EsiNo + "','" + c.FatherName + "' ,'" + c.United + "','" + c.Category + "','" + c.Department + "','" + c.Designation + "','" + c.OrjPayableDays + "' ,'" + c.NhDays + "' , '" + c.PayableDays + "' ,  '" + c.GovtDaySalary + "',  '" + c.OtWages + "',  '" + c.BasicDa + "' ,  '" + c.Basic + "',  '" + c.Da + "' ,  '" + c.Hra + "',  '" + c.Others + "', '" + c.EBasic + "', '" + c.EBasicDa + "',  '" + c.EDa + "',  '" + c.EHra + "', '" + c.EOthers + "',  '" + c.PayableOtHours + "' ,  '" + c.OtAmount + "',  '" + c.Incentive + "',  '" + c.GovtGross + "',  '" + c.PfAmount + "',  '" + c.EsiAmount + "',  '" + c.MessAmount + "',  '" + c.OthersExp + "',  '" + c.Advance + "', '" + c.Deduction + "',  '" + c.NetAmount + "' ,  '" + c.BankAccountNo + "',  '" + c.BankName + "' ,  '" + c.IfscCode + "',  '" + c.PayPeriod + "', '" + c.FromDate + "',  '" + c.ToDate + "',  '" + c.Active + "','" + c.CreditDate + "',  '" + c.CompCode1 + "',  '" + c.UserName + "' , '" + c.CreatedBy + "',  '" + c.CreatedOn + "',  '" + c.ModifiedBy + "',  '" + c.IpAddress + "' );";
                                //    Utility.ExecuteNonQuery(ins);
                               
                                if (result > 0)
                                {
                                    MessageBox.Show("Record Saved Successfully.Toal Record are:," + c.HrPayDetailsId1.ToString(), " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    empty(); GridLoad();
                                }
                                
                                else
                                {                                   
                                        MessageBox.Show("Insert Query Error.:," + query.ToString(), " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                string query = @"UPDATE HRPayDetails SET    hrpaydetailsid1 = @hrpaydetailsid1,    docid = @docid,    docdate = @docdate,    finyear = @finyear,    compcode = @compcode,    compname = @compname,    midcard = @midcard,    idcardno = @idcardno,    empname = @empname,    doj = @doj,    dol = @dol,    uanno = @uanno,    esino = @esino,    fathername = @fathername,    united = @united,    category = @category,    department = @department,    designation = @designation,    orjpayabledays = @orjpayabledays,    nhdays = @nhdays,    payabledays = @payabledays,    govtdaysalary = @govtdaysalary,    otwages = @otwages,    basicda = @basicda,    basic = @basic,    da = @da,    hra = @hra,    others = @others,    ebasicda = @ebasicda,    ebasic = @ebasic,    eda = @eda,    ehra = @ehra,    eothers = @eothers,    payableothrs = @payableothrs,    otamount = @otamount,    incentive = @incentive,    govtgross = @govtgross,    pfamount = @pfamount,    esiamount = @esiamount,    messamount = @messamount,    othersexp = @othersexp,    advance = @advance,    deduction = @deduction,    netamount = @netamount,    bankaccountno = @bankaccountno,    bankname = @bankname,    ifsccode = @ifsccode,    payperiod = @payperiod,    fromdate = @fromdate,    todate = @todate,    active = @active,    creditdate = @creditdate,    compcode1 = @compcode1,    username = @username,    createdby = @createdby,    modifiedby = @modifiedby,    ipaddress = @ipaddress WHERE hrpaydetailsid = @id";
                                using (MySqlCommand cmd = new MySqlCommand(query, Utility.Connect()))
                                {
                                    
                                    var parameters = new Dictionary<string, object>
                                    { {"@hrpaydetailsid1", c.HrPayDetailsId1}, {"@docid", c.DocId}, {"@docdate", c.DocDate},{"@finyear", c.FinYear},{"@compcode", c.CompCode}, {"@compname", c.CompName},{"@midcard", c.MidCard},        {"@idcardno", c.IdCardNo},        {"@empname", c.EmpName},        {"@doj", c.Doj},        {"@dol", c.Dol},        {"@uanno", c.UanNo},        {"@esino", c.EsiNo},        {"@fathername", c.FatherName},        {"@united", c.United},        {"@category", c.Category},        {"@department", c.Department},        {"@designation", c.Designation},        {"@orjpayabledays", c.OrjPayableDays},        {"@nhdays", c.NhDays},        {"@payabledays", c.PayableDays},        {"@govtdaysalary", c.GovtDaySalary},        {"@otwages", c.OtWages},        {"@basicda", c.BasicDa},        {"@basic", c.Basic},        {"@da", c.Da},        {"@hra", c.Hra},        {"@others", c.Others},        {"@ebasicda", c.EBasicDa},        {"@ebasic", c.EBasic},        {"@eda", c.EDa},        {"@ehra", c.EHra},        {"@eothers", c.EOthers},        {"@payableothrs", c.PayableOtHours},        {"@otamount", c.OtAmount},        {"@incentive", c.Incentive},        {"@govtgross", c.GovtGross},        {"@pfamount", c.PfAmount},        {"@esiamount", c.EsiAmount},        {"@messamount", c.MessAmount},        {"@othersexp", c.OthersExp},        {"@advance", c.Advance},        {"@deduction", c.Deduction},        {"@netamount", c.NetAmount},        {"@bankaccountno", c.BankAccountNo},        {"@bankname", c.BankName},        {"@ifsccode", c.IfscCode},        {"@payperiod", c.PayPeriod},        {"@fromdate", c.FromDate},        {"@todate", c.ToDate},        {"@active", c.Active},        {"@creditdate", c.CreditDate},        {"@compcode1", Class.Users.COMPCODE},        {"@username", Class.Users.USERID},        {"@createdby", Class.Users.HUserName},        {"@modifiedby", Class.Users.HUserName},        {"@ipaddress", Class.Users.IPADDRESS},        {"@id", txthrpaydetailsid.Text}    };
                                    foreach (var p in parameters)
                                        cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);

                                    result = cmd.ExecuteNonQuery();
                                }

                                // string up = "update  HRPayDetails  set hrpaydetailsid1='" + c.HrPayDetailsId1 + "' ,docid='" + c.DocId + "' , docdate='" + c.DocDate + "',finyear='" + c.FinYear + "',compcode='" + c.CompCode + "', compname='" + c.CompName + "',  midcard='" + c.MidCard + "',idcardno='" + c.IdCardNo + "' , empname='" + c.EmpName + "', doj='" + c.Doj.ToString("yyyy-MM-dd") + "',dol='" + Convert.ToDateTime(c.Dol).ToString("yyyy-MM-dd").Substring(0, 10) + "', uanno='" + c.UanNo + "' , esino='" + c.EsiNo + "' , fathername='" + c.FatherName + "', united='" + c.United + "' , category='" + c.Category + "',  department='" + c.Department + "', designation='" + c.Designation + "' , orjpayabledays='" + c.OrjPayableDays + "' , nhdays='" + c.NhDays + "' , payabledays='" + c.PayableDays + "' , govtdaysalary='" + c.GovtDaySalary + "', otwages='" + c.OtWages + "', basicda='" + c.BasicDa + "',  basic='" + c.Basic + "', da='" + c.Da + "', hra='" + c.Hra + "', others='" + c.Others + "',ebasicda='" + c.EBasicDa + "', ebasic='" + c.EBasic + "',  eda='" + c.EDa + "',    ehra='" + c.EHra + "', eothers='" + c.EOthers + "', payableothrs='" + c.PayableOtHours + "', otamount='" + c.OtAmount + "', incentive='" + c.Incentive + "',  govtgross='" + c.GovtGross + "',   pfamount='" + c.PfAmount + "', esiamount='" + c.EsiAmount + "', messamount='" + c.MessAmount + "', othersexp='" + c.OthersExp + "', advance='" + c.Advance + "', deduction='" + c.Deduction + "',  netamount='" + c.NetAmount + "',   bankaccountno='" + c.BankAccountNo + "' , bankname='" + c.BankName + "' , ifsccode='" + c.IfscCode + "', payperiod='" + c.PayPeriod + "' , fromdate='" + c.FromDate + "', todate='" + c.ToDate + "',active='" + c.Active + "',creditdate='" + c.CreditDate + "',compcode1='" + Class.Users.COMPCODE + "', username='" + Class.Users.USERID + "',createdby='" + Class.Users.HUserName + "', modifiedby='" + Class.Users.HUserName + "',ipaddress='" + Class.Users.IPADDRESS + "' where hrpaydetailsid='" + txthrpaydetailsid.Text + "';";
                                //result= Utility.ExecuteNonQuery(up);
                                if (result > 0 && txthrpaydetailsid.Text=="")
                                {
                                   // MessageBox.Show("Record Saved Successfully." + c.HrPayDetailsId1.ToString(), " Insert Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    mas.pop("Record Saved Successfully." + c.HrPayDetailsId1.ToString(), "Insert Message ","");
                                    empty(); GridLoad();
                                }
                                else if (result > 0 && txthrpaydetailsid.Text != "")
                                {
                                    //MessageBox.Show("Record Updated Successfully." + c.HrPayDetailsId1.ToString(), " Update Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    mas.pop("Record Updated Successfully." + c.HrPayDetailsId1.ToString(), "Update Message ", "");
                                    empty(); GridLoad();
                                }
                                else
                                {
                                    MessageBox.Show("Update Query Error.:," + query.ToString(), " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {

                        MessageBox.Show("InvalidDate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); //custom messageBox to show error  
                        this.Dispose();
                    }
                }
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Saves_Click " + "        " + ex.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Prints()
        {
            Saves();
            if (validprint == true)
            {
               
                empty();
            }
        }

        private void buttsearch_Click_1(object sender, EventArgs e)
        {
            Txtsearch_TextChanged(sender, e);
        }

        private void txtcertifiedby_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtlotno_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtnoofbags_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }
        private void txtsampledby_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtvechileno_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtdelayreason_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txttripwagonno_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtthirdpartywt_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtgrossweight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Back) //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txttareweight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Back) //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txtthirdpartywt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Back) //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }
        //public DataTable ReadExcel(string fileName, string fileExt)
        //{
        //    string conn = string.Empty;
        //    DataTable dtexcel = new DataTable();
        //    if (fileExt.CompareTo(".xls") == 0)
        //        conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
        //    else
        //        conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
        //    using (System.Data.OleDb.OleDbConnection con = new OleDbConnection(conn))
        //    {
        //        try
        //        {
        //            System.Data.OleDb.OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
        //            oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
        //        }
        //        catch { }
        //    }
        //    return dtexcel;
        //}

        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {

       

            if (dataGridView2.Rows.Count == 0)
                return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files|*.xlsx";
                sfd.FileName = "Report.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                   
                   // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("Sheet1");

                        // Header
                        for (int col = 0; col < dataGridView2.Columns.Count; col++)
                        {
                            ws.Cells[1, col + 1].Value =
                                dataGridView2.Columns[col].HeaderText;

                            ws.Cells[1, col + 1].Style.Font.Bold = true;
                        }

                        // Rows
                        int rowIndex = 2;

                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int col = 0; col < dataGridView2.Columns.Count; col++)
                                {
                                    ws.Cells[rowIndex, col + 1].Value =
                                        row.Cells[col].Value?.ToString();
                                }
                                rowIndex++;
                            }
                        }

                        ws.Cells[ws.Dimension.Address].AutoFitColumns();

                        File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());
                    }

                    MessageBox.Show("Export Successful!", "Success");
                }

                //if (dataGridView2.Rows.Count > 0)
                //{
                //    var xcelApp = new Microsoft.Office.Interop.Excel.Application();
                //    var workbook = xcelApp.Workbooks.Add(Type.Missing);
                //    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;

                //    // Header
                //    for (int i = 0; i < dataGridView2.Columns.Count; i++)
                //    {
                //        worksheet.Cells[1, i + 1] = dataGridView2.Columns[i].HeaderText;
                //    }

                //    // Rows
                //    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                //    {
                //        if (!dataGridView2.Rows[i].IsNewRow)
                //        {
                //            for (int j = 0; j < dataGridView2.Columns.Count; j++)
                //            {
                //                worksheet.Cells[i + 2, j + 1] =
                //                    dataGridView2.Rows[i].Cells[j].Value?.ToString();
                //            }
                //        }
                //    }

                //    worksheet.Columns.AutoFit();
                //    xcelApp.Visible = true;
                //}

            }
        }




        private void button2_Click(object sender, EventArgs e)
        {
            string sel2 = "select a.idcardno as  Sequence, a.midcard ,  a.empname,  a.doj, a.dol, a.uanno,  a.esino,  a.fathername ,  a.united,  a.category,  a.department, a.designation, a.orjpayabledays ,  a.nhdays ,  a.payabledays ,  a.govtdaysalary,  a.otwages,  a.basicda ,  a.basic,  a.da ,  a.hra,  a.others, a.ebasic, a.ebasicda,  a.eda,  a.ehra,  a.eothers,  a.payableothrs ,  a.otamount,  a.incentive,  a.govtgross,  a.pfamount,  a.esiamount,  a.messamount,  a.othersexp,  a.advance, a.deduction,  a.netamount ,  a.bankaccountno,  a.bankname , a.ifsccode,a.payperiod, a.fromdate ,  a.todate, a.creditdate from  hrpaydetails a join gtcompmast b on a.compcode=b.gtcompmastid where b.compcode='" + combounitreport.Text + "' and a.payperiod='" + comboperiodreport.Text + "' order by 1;";
            DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "HRPayDetails");
            DataTable dt2 = ds2.Tables["HRPayDetails"];
            dataGridView2.DataSource = dt2;
            int cnt = dataGridView2.Rows.Count - 1;
            lbltotalcount.Text = "Total Count  :" + cnt.ToString();
        }

       

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //ListViewItem item = e.Item as ListViewItem;

            //if (item.Checked==true)
            //{
            //    try
            //    {

            //        if (item.SubItems[2].Text != "")
            //        {
            //            var confirmation = MessageBox.Show("Do You want Delete this Record ?. IDCARD:"+ item.SubItems[2].Text+"=="+ item.SubItems[6].Text, "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //            if (confirmation == DialogResult.Yes)
            //            {
            //                string del = "delete from HRPayDetails  where HRPayDetailsid='" + item.SubItems[2].Text + "';";
            //                Utility.ExecuteNonQuery(del);
            //                period = item.SubItems[6].Text;
            //                gridload();
            //                empty();
            //                MessageBox.Show("Record Deleted Successfully " + item.SubItems[2].Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);


            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }


            //}
        }

        private void HRPayDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                Saves();
            }

        }

        private void refreshToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            GridLoad();
            companyload();
        }



        private void combounitsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt64(combounitsearch.SelectedValue) > 0)
                {
                    checkall.Checked = true;
                    string sel1 = "select  distinct '------' as  payperiod from dual union all  select distinct b.payperiod from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where a.ACTIVE ='T' and  a.compcode='" + combounitsearch.Text + "'";

                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "hrpaydetails");
                    DataTable dt1 = ds1.Tables["hrpaydetails"];
                    comboperiodsearh.DisplayMember = "payperiod";
                    comboperiodsearh.ValueMember = "payperiod";
                    comboperiodsearh.DataSource = dt1;
                }
            }
            catch (Exception ex) { }

        }

        private void comboperiodsearh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt64(combounitsearch.SelectedValue) > 0 && comboperiodsearh.Text != "------" && comboperiodsearh.Text != "")
                {
                    Cursor = Cursors.WaitCursor; lbltotal.Text = "";
                    listView1.Items.Clear(); listfilter.Items.Clear();
                    string sel1 = "select  a.hrpaydetailsid,a.docid ,b.compcode, a.idcardno ,a.midcard, a.payperiod,a.empname, a.doj,a.fathername,a.department,a.united from  hrpaydetails a join gtcompmast b on a.compcode=b.gtcompmastid where b.compcode='" + combounitsearch.Text + "' and a.payperiod='" + comboperiodsearh.Text + "' order by 1;";
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "HRPayDetails");
                    DataTable dt = ds.Tables["HRPayDetails"];
                    if (dt.Rows.Count >= 0)
                    {
                        int i = 1;
                        foreach (DataRow myRow in dt.Rows)
                        {
                            ListViewItem list = new ListViewItem();
                            list.SubItems.Add(i.ToString());
                            list.SubItems.Add(myRow["hrpaydetailsid"].ToString());
                            list.SubItems.Add(myRow["docid"].ToString());
                            list.SubItems.Add(myRow["compcode"].ToString());
                            list.SubItems.Add(myRow["idcardno"].ToString());
                            list.SubItems.Add(myRow["midcard"].ToString());
                            list.SubItems.Add(myRow["payperiod"].ToString());
                            list.SubItems.Add(myRow["empname"].ToString());
                            list.SubItems.Add(myRow["doj"].ToString());
                            list.SubItems.Add(myRow["fathername"].ToString());
                            list.SubItems.Add(myRow["department"].ToString());
                            list.SubItems.Add(myRow["united"].ToString());
                            this.listfilter.Items.Add((ListViewItem)list.Clone());
                            if (i % 2 == 0)
                            {
                                list.BackColor = Color.WhiteSmoke;
                            }
                            else
                            {
                                list.BackColor = Color.White;
                            }
                            listView1.Items.Add(list);
                            i++;
                        }
                        lbltotal.Text = "Total Count: " + listView1.Items.Count;
                    }
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void combounitreport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt64(combounitreport.SelectedValue) > 0)
                {
                    string sel1 = "select '------' as  payperiod from dual union all select distinct b.payperiod from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where a.ptransaction ='COMPANY' and  a.compcode='" + combounitreport.Text + "'";
                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "hrpaydetails");
                    DataTable dt1 = ds1.Tables["hrpaydetails"];
                    comboperiodreport.DisplayMember = "payperiod";
                    comboperiodreport.ValueMember = "payperiod";
                    comboperiodreport.DataSource = dt1;
                }
            }
            catch (Exception ex) { }
        }

      

        private void checkall_CheckedChanged(object sender, EventArgs e)
        {
            if (txthrpaydetailsid.Text != "")
            {
                //var confirmation = MessageBox.Show("Do You want Delete this Record ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //if (confirmation == DialogResult.Yes)
                //{

                //    string sel1 = "select b.hrpaydetailsid from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where a.ptransaction ='COMPANY' and  a.compcode='" + Class.Users.HCompcode + "' AND B.payperiod='"+ comboperiodsearh.Text + "';";

                //    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "hrpaydetails");
                //    DataTable dt1 = ds1.Tables["hrpaydetails"];
                //    if (dt1.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt1.Rows.Count; i++)
                //        {

                //            string del = "delete from HRPayDetails  where compcode='" + combocompcode.SelectedValue + "' and payperiod='" + comboperiodsearh.Text + "' and  HRPayDetailsid='" + dt1.Rows[i]["hrpaydetailsid"].ToString() + "';";
                //            Utility.ExecuteNonQuery(del);
                //        }
                //        MessageBox.Show("Record Deleted Successfully " + txthrpaydetailsid.Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //        gridload(); empty();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Invalid  Delete","Invalid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //    }
                //}
            }
        }

        private void tabPageraw3_Click(object sender, EventArgs e)
        {

        }
        public void Searchs()
        {
            throw new NotImplementedException();
        }

        public void Searchs(int EditID)
        {
            throw new NotImplementedException();
        }

        public void Deletes()
        {
            try
            {

                if (txthrpaydetailsid.Text != "" && checkall.Checked == false)
                {
                    string sel1 = "select b.empid from  gtcompmast a join pldatta b on a.compcode=b.compcode  where   a.compcode='" + combounitsearch.Text + "' AND B.payperiod='" + comboperiodsearh.Text + "';";
                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "hrpaydetails");
                    DataTable dt1 = ds1.Tables["hrpaydetails"];
                    if (dt1.Rows.Count <= 0)
                    {
                        var confirmation = MessageBox.Show("Do You want Delete this Record ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmation == DialogResult.Yes)
                        {
                            Cursor = Cursors.WaitCursor;
                            string del = "delete from HRPayDetails where compcode='" + combocompcode.SelectedValue + "' and  HRPayDetailsid='" + txthrpaydetailsid.Text + "'";
                            Utility.ExecuteNonQuery(del);
                            if (Convert.ToInt64(combounitsearch.SelectedValue) > 0)
                            {
                                string sel2 = "select  distinct '------' as  payperiod from dual union all  select distinct b.payperiod from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where a.ACTIVE ='T' and  a.compcode='" + combounitsearch.Text + "'";
                                DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "hrpaydetails");
                                DataTable dt2 = ds2.Tables["hrpaydetails"];
                                comboperiodsearh.DisplayMember = "payperiod";
                                comboperiodsearh.ValueMember = "payperiod";
                                comboperiodsearh.DataSource = dt2;
                                comboperiodsearh.Refresh();
                            }
                            MessageBox.Show("Record Deleted Successfully " + txthrpaydetailsid.Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            empty(); Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Child Record Found.Can Not Delete.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                if (txthrpaydetailsid.Text == "" && checkall.Checked == true)
                {
                    string sel1 = "select b.empid from  gtcompmast a join pldatta b on a.compcode=b.compcode  where   a.compcode='" + combounitsearch.Text + "' AND B.payperiod='" + comboperiodsearh.Text + "';";

                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "hrpaydetails");
                    DataTable dt1 = ds1.Tables["hrpaydetails"];
                    if (dt1.Rows.Count <= 0)
                    {
                        var confirmation = MessageBox.Show("Do You want Delete this Record ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmation == DialogResult.Yes)
                        {
                            Cursor = Cursors.WaitCursor;
                            string sel0 = "select b.hrpaydetailsid from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where   a.compcode='" + combounitsearch.Text + "' AND B.payperiod='" + comboperiodsearh.Text + "';";

                            DataSet ds0 = Utility.ExecuteSelectQuery(sel0, "hrpaydetails");
                            DataTable dt0 = ds0.Tables["hrpaydetails"]; int tot = 0;
                            if (dt0.Rows.Count > 0)
                            {
                                progressBar2.Minimum = 0;
                                progressBar2.Maximum = dt0.Rows.Count;
                                for (int i = 0; i < dt0.Rows.Count; i++)
                                {

                                    string del = "delete from HRPayDetails  where compcode='" + combounitsearch.SelectedValue + "' and payperiod='" + comboperiodsearh.Text + "' and  HRPayDetailsid='" + dt0.Rows[i]["hrpaydetailsid"].ToString() + "';";
                                    Utility.ExecuteNonQuery(del); tot++;
                                    decimal per = Convert.ToDecimal(100 / GenFun.ToDecimal(dt0.Rows.Count)) * (i + 1);
                                    lblprogress2.Text = " Data Transfer to Table : " + (per).ToString("N0") + " %" + dt0.Rows[i]["hrpaydetailsid"].ToString();
                                    lbltotal.Text = "Data Remove from Table" + (per).ToString("N0") + " % " + "EmpID " + dt0.Rows[i]["hrpaydetailsid"].ToString();
                                    lblprogress2.Refresh(); lbltotal.Refresh();
                                    progressBar2.Value = i + 1; tot++;

                                }

                                MessageBox.Show("Record Deleted Successfully. Total:- " + tot.ToString(), "Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblprogress2.Text = ""; progressBar2.Value = 0; lbltotal.Text = "";
                                empty();
                                if (Convert.ToInt64(combounitsearch.SelectedValue) > 0)
                                {
                                    string sel2 = "select  distinct '------' as  payperiod from dual union all  select distinct b.payperiod from  gtcompmast a join hrpaydetails b on a.gtcompmastid=b.compcode  where a.ACTIVE ='T' and  a.compcode='" + combounitsearch.Text + "'";
                                    DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "hrpaydetails");
                                    DataTable dt2 = ds2.Tables["hrpaydetails"];
                                    comboperiodsearh.DisplayMember = "payperiod";
                                    comboperiodsearh.ValueMember = "payperiod";
                                    comboperiodsearh.DataSource = dt2;
                                    comboperiodsearh.Refresh();
                                }
                                Cursor = Cursors.Default;
                            }
                            else
                            {
                                MessageBox.Show("No Data Found.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Information); checkall.Checked = false;
                            }
                            Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Child Record Found.Can Not Delete.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageraw1"])//your specific tabname
            {
                txtidcard.Select();
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageraw2"])//your specific tabname
            {

            }

            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageraw3"])//your specific tabname
            {

            }


            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageraw4"])//your specific tabname
            {
                combounitreport.Select();
                comboperiodreport.Select();
            }

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
        private void txtvechileno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtdoj.Focus();
            }
        }

        private void comboreceivedfrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtpayabledays.Select();

            }
        }

        private void txtgrossweight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtgovtdaysal.Select();

            }
        }

        private void combocompcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtidcard.Focus();

            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void combovarietyitem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {



                txtgovtdaysal.Select();
            }
        }

        private void txttripwagonno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtbasicda.Focus();

            }
        }

        private void txtthirdpartywt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtunited.Focus();

            }
        }

        private void combogodown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                txtbasic.Focus();
            }
        }

        private void txtlotno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtcategory.Focus();

            }
        }

        private void txtsampledby_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtda.Focus();

            }
        }

        private void txtcertifiedby_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtdepartment.Focus();

            }
        }

        private void combovisualstatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txthra.Focus();

            }
        }


        public void ReadOnlys()
        {
            throw new NotImplementedException();
        }

        public void Imports()
        {
            throw new NotImplementedException();
        }

        public void Pdfs()
        {
            throw new NotImplementedException();
        }

        public void ChangePasswords()
        {
            throw new NotImplementedException();
        }

        //private DataTable ImportExcelToDataTable(string filePath)
        //{
        //    DataTable dt = new DataTable();

        //    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        using (ExcelPackage package = new ExcelPackage(fs))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

        //            bool hasHeader = true;
        //            int startRow = hasHeader ? 2 : 1;

        //            // Create Columns
        //            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //            {
        //                dt.Columns.Add(hasHeader
        //                    ? worksheet.Cells[1, col].Text
        //                    : "Column" + col);
        //            }

        //            // Add Rows
        //            for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
        //            {
        //                DataRow newRow = dt.NewRow();

        //                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                {
        //                    newRow[col - 1] = worksheet.Cells[row, col].Text;
        //                }

        //                dt.Rows.Add(newRow);
        //            }
        //        }
        //    }

        //    return dt;
        //}

        public void DownLoads()
        {
            if (Class.Users.Log >= Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd")))
            {
                string filePath = string.Empty; dataGridView1.AllowUserToAddRows = false;
                string fileExt = string.Empty; combocompcode.Text = ""; combocompcode.SelectedIndex = -1;
                OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
                {
                    
                    filePath = file.FileName; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    filePath = file.FileName; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (fileExt.CompareTo(".xls") == 0)
                    {
                        try
                        {
                            DataTable dtExcel = new DataTable();

                            dtExcel = Class.Master.ReadExcel(filePath, fileExt); //read excel file  
                            dataGridView1.Visible = true;
                            dataGridView1.DataSource = dtExcel;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else if (fileExt.CompareTo(".xlsx") == 0)
                    {
                        DataTable dt = new DataTable();
                         dt = Class.Master.ImportExcelToDataTable(file.FileName);
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("invalid Excel Formate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                combounitsearch.SelectedIndex = -1;
                tabControl1.SelectTab(tabPageraw3);
                int cnt = dataGridView1.Rows.Count - 1;
                label48.Text = "Total Count  :" + cnt.ToString();
            }
            else
            {
                MessageBox.Show("InvalidDate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); //custom messageBox to show error  

                this.Dispose();
            }


        }

        public void ChangeSkins()
        {
            throw new NotImplementedException();
        }

        public void Logins()
        {
            throw new NotImplementedException();
        }

        public void GlobalSearchs()
        {
            throw new NotImplementedException();
        }

        public void TreeButtons()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            GlobalVariables.MdiPanel.Show();
            empty();
            this.Hide();
            GlobalVariables.HeaderName.Text = "";
            GlobalVariables.TabCtrl.TabPages.RemoveAt(GlobalVariables.TabCtrl.SelectedIndex);

        }

        public void GridLoad()
        {
            try
            {

                //if (period == "")
                //{
                if (Convert.ToInt64(combounitsearch.SelectedValue) > 0)
                {
                    string sel1 = "select  a.hrpaydetailsid,a.docid ,b.compcode, a.idcardno ,a.midcard, a.payperiod,a.empname, a.doj,a.fathername,a.department,a.united from  hrpaydetails a join gtcompmast b on a.compcode=b.gtcompmastid WHERE b.compcode='" + Class.Users.HCompcode + "'   order by a.hrpaydetailsid desc;";


                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "HRPayDetails");
                    DataTable dt = ds.Tables["HRPayDetails"];
                    if (dt.Rows.Count >= 0)
                    {
                        int i = 1; listView1.Items.Clear(); listfilter.Items.Clear();
                        foreach (DataRow myRow in dt.Rows)
                        {
                            ListViewItem list = new ListViewItem();
                            list.SubItems.Add(i.ToString());
                            list.SubItems.Add(myRow["hrpaydetailsid"].ToString());
                            list.SubItems.Add(myRow["docid"].ToString());
                            list.SubItems.Add(myRow["compcode"].ToString());
                            list.SubItems.Add(myRow["idcardno"].ToString());
                            list.SubItems.Add(myRow["midcard"].ToString());
                            list.SubItems.Add(myRow["payperiod"].ToString());
                            list.SubItems.Add(myRow["empname"].ToString());
                            list.SubItems.Add(myRow["doj"].ToString());
                            list.SubItems.Add(myRow["fathername"].ToString());
                            list.SubItems.Add(myRow["department"].ToString());
                            list.SubItems.Add(myRow["united"].ToString());
                            this.listfilter.Items.Add((ListViewItem)list.Clone());
                            if (i % 2 == 0)
                            {
                                list.BackColor = Color.WhiteSmoke;
                            }
                            else
                            {
                                list.BackColor = Color.White;
                            }
                            listView1.Items.Add(list);
                            i++;
                        }
                        lbltotal.Text = "Total Count: " + listView1.Items.Count;
                    }
                }
       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ListView1_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                //empty();

                if (listView1.Items.Count >= 0)
                {

                    txthrpaydetailsid.Text = listView1.SelectedItems[0].SubItems[2].Text;
                    string sel1 = "select a.HRPayDetailsid, a.hrpaydetailsid1 ,a.docid , a.docdate,a.finyear,b.compcode, b.compname, a.idcardno , a.midcard,a.empname, a.doj,a.dol, a.uanno , a.esino , a.fathername, a.united , a.category,  a.department, a.designation,  a.orjpayabledays , a.nhdays , a.payabledays , a.govtdaysalary, a.otwages, a.basicda,  a.basic, a.da, a.hra, a.others, a.ebasic,  a.eda,   a.ebasicda, a.ehra, a.eothers, a.payableothrs, a.otamount, a.incentive,  a.govtgross,   a.pfamount, a.esiamount, a.messamount,a.othersexp,a.advance,a.creditdate, a.deduction,  a.netamount,   a.bankaccountno , a.bankname , a.ifsccode, a.payperiod ,   a.fromdate, a.todate,a.active from HRPayDetails a  join gtcompmast b on a.compcode = b.gtcompmastid  where a.HRPayDetailsid='" + txthrpaydetailsid.Text + "'; ";
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "HRPayDetails");
                    DataTable dt = ds.Tables["HRPayDetails"];
                    if (dt.Rows.Count > 0)
                    {
                        if (txthrpaydetailsid.Text != "")
                        {
                            txthrpaydetailsid.Text = dt.Rows[0]["HRPayDetailsid"].ToString();
                            txthrpaydetailsid1.Text = dt.Rows[0]["HRPayDetailsid1"].ToString();
                            txtdocid.Text = dt.Rows[0]["docid"].ToString();
                            txtdate.Text = dt.Rows[0]["docdate"].ToString();
                            combofinyear.Text = dt.Rows[0]["finyear"].ToString();
                            combocompcode.Text = dt.Rows[0]["compcode"].ToString();
                            combocompname.Text = dt.Rows[0]["compname"].ToString();
                            txtsequenceid.Text = dt.Rows[0]["idcardno"].ToString();
                            txtidcard.Text = dt.Rows[0]["midcard"].ToString();
                            txtempname.Text = dt.Rows[0]["empname"].ToString();
                            txtdoj.Text = dt.Rows[0]["doj"].ToString(); string s = "";
                            if (dt.Rows[0]["dol"].ToString() != "")
                            {
                                s = dt.Rows[0]["dol"].ToString().Substring(0,10);                               
                                if (s.Substring(6,4) == "0001") { txtdol.CustomFormat = ""; }
                                else
                                {
                                    txtdol.Text = Convert.ToDateTime(s).ToString();
                                }
                            }
                            else
                            {
                                txtdol.CustomFormat = "";
                            }
                            txtuanno.Text = dt.Rows[0]["uanno"].ToString();
                            txtesino.Text = dt.Rows[0]["esino"].ToString();
                            txtfathername.Text = dt.Rows[0]["fathername"].ToString();
                            txtunited.Text = dt.Rows[0]["united"].ToString();
                            txtcategory.Text = dt.Rows[0]["category"].ToString();
                            txtdepartment.Text = dt.Rows[0]["department"].ToString();
                            txtdesignation.Text = dt.Rows[0]["designation"].ToString();
                            txtorjpayabledays.Text = dt.Rows[0]["orjpayabledays"].ToString();
                            txtnhdays.Text = dt.Rows[0]["nhdays"].ToString();
                            txtpayabledays.Text = dt.Rows[0]["payabledays"].ToString();
                            txtgovtdaysal.Text = dt.Rows[0]["govtdaysalary"].ToString();
                            txtotwages.Text = dt.Rows[0]["otwages"].ToString();
                            txtbasicda.Text = dt.Rows[0]["basicda"].ToString();
                            txtbasic.Text = dt.Rows[0]["basic"].ToString();
                            txtda.Text = dt.Rows[0]["da"].ToString();
                            txthra.Text = dt.Rows[0]["hra"].ToString();
                            txtothers.Text = dt.Rows[0]["others"].ToString();
                            txtebasicda.Text = dt.Rows[0]["ebasicda"].ToString();
                            txtebasic.Text = dt.Rows[0]["ebasic"].ToString();
                            txteda.Text = dt.Rows[0]["eda"].ToString();
                            txtehra.Text = dt.Rows[0]["ehra"].ToString();
                            txteothers.Text = dt.Rows[0]["eothers"].ToString();
                            txtpayableothours.Text = dt.Rows[0]["payableothrs"].ToString();
                            txtotamount.Text = dt.Rows[0]["otamount"].ToString();
                            txtincentive.Text = dt.Rows[0]["incentive"].ToString();
                            txtgovtgross.Text = dt.Rows[0]["govtgross"].ToString();
                            txtpfamount.Text = dt.Rows[0]["pfamount"].ToString();
                            txtesiamount.Text = dt.Rows[0]["esiamount"].ToString();
                            txtmessamount.Text = dt.Rows[0]["messamount"].ToString();
                            txtotherexp.Text = dt.Rows[0]["othersexp"].ToString();
                            txtadvance.Text = dt.Rows[0]["advance"].ToString();
                            txtdeduction.Text = dt.Rows[0]["deduction"].ToString();
                            txtnetamount.Text = dt.Rows[0]["netamount"].ToString();
                            txtbankaccountno.Text = dt.Rows[0]["bankaccountno"].ToString();
                            txtbankname.Text = dt.Rows[0]["bankname"].ToString();
                            txtifsccode.Text = dt.Rows[0]["ifsccode"].ToString();
                            txtpayperiod.Text = dt.Rows[0]["payperiod"].ToString();
                            txtfromdate.Value = Convert.ToDateTime(dt.Rows[0]["fromdate"].ToString());
                            txttodate.Value = Convert.ToDateTime(dt.Rows[0]["todate"].ToString());
                            txtcreditdate.Text = dt.Rows[0]["creditdate"].ToString();
                            if (dt.Rows[0]["active"].ToString() == "T")
                                checkactive.Checked = true;
                            else checkactive.Checked = false;
                            combocompcode.Enabled = false;
                            tabControl1.SelectTab(tabPageraw1);
                        }
                        else
                        {
                            return;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                //   MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            tabControl1.SelectTab(tabPageraw1);
        }

        private void Txtsearch_TextChanged(object sender, EventArgs e)
        {


            try
            {
                if (Convert.ToInt64(combounitsearch.SelectedValue) > 0 && comboperiodsearh.Text != "")
                {
                    int item0 = 0; listView1.Items.Clear();
                    if (txtsearch.Text.Length > 1)
                    {

                        foreach (ListViewItem item in listfilter.Items)
                        {
                            ListViewItem list = new ListViewItem();
                            if (listfilter.Items[item0].SubItems[6].ToString().Contains(txtsearch.Text.ToUpper()) || listfilter.Items[item0].SubItems[8].ToString().Contains(txtsearch.Text.ToUpper()))
                            {


                                list.Text = listfilter.Items[item0].SubItems[0].Text;
                                list.SubItems.Add(listfilter.Items[item0].SubItems[1].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[2].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[3].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[4].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[5].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[6].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[7].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[8].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[9].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[10].Text);
                                list.SubItems.Add(listfilter.Items[item0].SubItems[11].Text);
                                if (item0 % 2 == 0)
                                {
                                    list.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    list.BackColor = Color.White;
                                }
                                item0++;
                                listView1.Items.Add(list);


                            }
                            item0++;
                        }
                        lbltotal.Text = "Total Count: " + listView1.Items.Count;

                    }
                    else
                    {

                        ListView ll = new ListView();

                        listView1.Items.Clear();
                        foreach (ListViewItem item in listfilter.Items)
                        {

                            this.listView1.Items.Add((ListViewItem)item.Clone());
                            if (item0 % 2 == 0)
                            {
                                item.BackColor = Color.WhiteSmoke;
                            }
                            else
                            {
                                item.BackColor = Color.White;
                            }


                            item0++;
                        }
                        lbltotal.Text = "Total Count: " + listView1.Items.Count;
                    }

                }
                else
                {
                    MessageBox.Show("pls select CompCode and PayPeriod");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("---" + ex.ToString());
            }

        }



        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void HRPayDetails_Load(object sender, EventArgs e)
        {
           
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

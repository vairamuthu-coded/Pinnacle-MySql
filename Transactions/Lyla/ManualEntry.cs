using Pinnacle.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pinnacle.Transactions.Lyla
{
    public partial class ManualEntry : Form,ToolStripAccess
    {
        private static ManualEntry _instance;
        Models.Master mas = new Models.Master();
        Models.UserRights sm = new Models.UserRights();
        ListView listfilter = new ListView();
        Pinnacle.UserControls.UCCListView ucclist = new UCCListView();
        public static ManualEntry Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ManualEntry();
                GlobalVariables.CurrentForm = _instance;
                return _instance;
            }
        }
        public ManualEntry()
        {
            InitializeComponent();
            Class.Users.IPADDRESS = GenFun.GetLocalIPAddress();
            Class.Users.CREATED = Convert.ToDateTime(System.DateTime.Now.ToString("dd-MM-yyyy") + " " + System.DateTime.Now.ToLongTimeString());

        }
 
        private void ManualEntry_Load(object sender, EventArgs e)
        {
            combomachine.Select();
        }
        private bool ValidateContact(TextBox s)
        {
            if (!Regex.Match(s.Text, "^[A-Z][a-zA-Z]*$").Success)
            {

                s.Text.Remove(s.Text.Length - 1);

            }
            return true;
        }

        public void Saves()
        {
            try
            {
                string dat, tim, datetim, chk ,notes, msg = "";
                if (txtbarcode.Text != "" && dataGridView1.Rows.Count == 0 && Convert.ToInt64("0" + combocompcode.SelectedValue) > 0)
                {
                    try
                    {
                        if (txtbarcode.Text.ToString().Length == 9)
                        {
                            dat = Convert.ToDateTime(System.DateTime.Now.Date.ToString().Substring(0, 10)).ToString("yyyy-MM-dd");
                            tim = Convert.ToDateTime(System.DateTime.Now.ToString().Substring(10, 8)).ToString("HH:mm:ss");
                            datetim = dat + " " + tim;
                            txtnotes.Text = "Manual Entry";
                            if (checkactive.Checked == true) { chk = "T"; } else { chk = "F"; checkactive.Checked = false; }
                            string sel = "select asptblmanmasid    from  asptblmanmas    WHERE machine='" + combomachine.SelectedValue + "' and  barcode='" + txtbarcode.Text + "'  and compcode='" + combocompcode.SelectedValue + "'";
                            DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                            DataTable dt = ds.Tables["asptblmanmas"];
                            if (dt.Rows.Count != 0)
                            {
                                msg = "Child Record Found ";
                            }
                            else if (dt.Rows.Count != 0 && Convert.ToInt32("0" + txtmanualid.Text) == 0 || Convert.ToInt32("0" + txtmanualid.Text) == 0)
                            {
                                string ins = "insert into asptblmanmas(machine,barcode,date,time,datetime, active,notes,compcode,username,createdby,modifiedby,ipaddress,finyear)  VALUES('" + combomachine.SelectedValue + "','" + txtbarcode.Text.ToUpper() + "',date_format('" + dat + "','%Y-%m-%d'),time_format('" + tim + "','%H:%i:%s'),date_format('" + datetim + "','%Y-%m-%d %H:%i:%s'),'" + chk + "','" + txtnotes.Text + "','" + combocompcode.SelectedValue + "','" + Class.Users.USERID + "','" + Class.Users.HUserName + "','" + Class.Users.HUserName + "','" + Class.Users.IPADDRESS + "','" + Class.Users.Finyear + "' )";
                                Utility.ExecuteNonQuery(ins);
                                msg = "Record Saved Successfully ";
                            }
                            else
                            {
                                string up = "update  asptblmanmas  set machine='" + combomachine.SelectedValue + "' ,  barcode='" + txtbarcode.Text.ToUpper() + "' ,date=date=date_format('" + dat + "','%Y-%m-%d') , time=time_format('" + tim + "','%H:%i:%s') , datetime=date_format('" + datetim + "','%Y-%m-%d %H:%i:%s'), active='" + chk + "',notes='" + txtnotes.Text + "' ,compcode='" + combocompcode.SelectedValue + "',modifiedby='" + Class.Users.HUserName + "',ipaddress='" + Class.Users.IPADDRESS + "' where asptblmanmasid='" + txtmanualid.Text + "'";
                                Utility.ExecuteNonQuery(up);
                                msg = "Record Updated Successfully ";
                            }
                            MessageBox.Show(msg + txtbarcode.Text, " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Barcode Length : " + txtbarcode.Text, " Barcode Length is 9 . Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
                if (dataGridView1.Rows.Count > 1 && Convert.ToInt64("0"+combocompcode.SelectedValue)>0)
                {
                    try
                    {
                        progressBar1.Minimum = 0;
                       
                        int rowcount = dataGridView1.Rows.Count;
                        progressBar1.Maximum = rowcount;
                        for (int i = 0; i < rowcount - 1; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[1].Value.ToString().Length==9 && dataGridView1.Rows[i].Cells[0].Value.ToString() != "" && dataGridView1.Rows[i].Cells[1].Value.ToString() != "" && dataGridView1.Rows[i].Cells[2].Value.ToString() != "" && dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                            {


                                string sel1 = "select a.asptblmacdetid from asptblmacdet a  join asptblmacmas b on a.asptblmacmasid=b.asptblmacmasid WHERE  a.machine='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "'  and  b.compcode='" + combocompcode.SelectedValue + "' ";
                                DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "asptblmacmas");
                                DataTable dt1 = ds1.Tables["asptblmacmas"];
                                if (dt1.Rows.Count > 0 && dataGridView1.Rows[i].Cells[0].Value.ToString() != null)
                                {
                                    dat = Convert.ToDateTime(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(0, 10)).ToString("yyyy-MM-dd");
                                    tim = Convert.ToDateTime(dataGridView1.Rows[i].Cells[3].Value.ToString().Substring(10, 8)).ToString("HH:mm:ss");
                                    datetim = dat + " " + tim;
                                    chk = "T";
                                 
                                    txtbarcode.Text = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                    combomachine.SelectedValue = Convert.ToString(dt1.Rows[0]["asptblmacdetid"].ToString());
                                    txtdate.Text = dat;
                                    txttime.Text = tim;
                                    txtdatetime.Text = datetim;
                                    txtnotes.Text = "Excel Entry";
                                    //string sel2 = "select asptblmanmasid    from  asptblmanmas    WHERE machine='" + combomachine.SelectedValue + "' and  barcode='" + txtbarcode.Text + "' and date=date_format('" + dat + "','%Y-%m-%d') and time=time_format('" + tim + "','%H:%i:%s') and datetime=date_format('" + datetim + "','%Y-%m-%d %H:%i:%s') and active='" + chk + "' and compcode='" + combocompcode.SelectedValue + "'";


                                    string sel = "select asptblmanmasid    from  asptblmanmas    WHERE machine='" + combomachine.SelectedValue + "' and  barcode='" + txtbarcode.Text + "'  and compcode='" + combocompcode.SelectedValue + "'";
                                    DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblmanmas");
                                    DataTable dt = ds.Tables["asptblmanmas"];
                                    if (dt.Rows.Count != 0)
                                    {
                                        msg = "";
                                        msg = "Record Saved Successfully   .";
                                    }
                                    else if (dt.Rows.Count <= 0)
                                    {
                                        string ins = "insert into asptblmanmas(machine,barcode,date,time,datetime, active,notes,compcode,username,createdby,modifiedby,ipaddress,finyear)  VALUES('" + combomachine.SelectedValue + "','" + txtbarcode.Text.ToUpper() + "',date_format('" + dat + "','%Y-%m-%d'),time_format('" + tim + "','%H:%i:%s'),date_format('" + datetim + "','%Y-%m-%d %H:%i:%s'),'" + chk + "','"+ txtnotes .Text + "','" + combocompcode.SelectedValue + "','" + Class.Users.USERID + "','" + Class.Users.HUserName + "','" + Class.Users.HUserName + "','" + Class.Users.IPADDRESS + "','" + Class.Users.Finyear + "' )";
                                        Utility.ExecuteNonQuery(ins);
                                        msg = "";
                                        msg = "Record Saved Successfully   .";


                                    }



                                }
                                else
                                {
                                    MessageBox.Show("pls check Excel Data ( Row Index '" + i.ToString() + "'  Machine  name :  '" + dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString() + "' and Barcode No : " + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + ")", " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                                decimal per = Convert.ToDecimal(100 / GenFun.ToDecimal(rowcount)) * (i + 1);
                                lblprogress1.Text = per.ToString("N0") + " % '" + i.ToString() + "' barcode : -" + txtbarcode.Text;

                                lblprogress1.Refresh();

                                progressBar1.Value = i + 1;


                            }
                            else
                            {
                                MessageBox.Show("pls check Null value invalid. ( Row Index '" + i.ToString() + "'  Machine  name :  '" + dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString() + "' and Barcode No : " + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "and Date Time:"+ dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + " = "+ dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + ")", " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }

                        }
                        MessageBox.Show(msg + dataGridView1.Rows.Count.ToString(), " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {

                    }

                }
                if (dataGridView1.Rows.Count == 0 && txtbarcode.Text == "")
                {
                    MessageBox.Show("pls Upload Load . " + dataGridView1.Rows.Count.ToString(), " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                empty();
            }
            catch (Exception ex)
            {

                MessageBox.Show("machine " + "        " + ex.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void ManualEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
        public void News()
        {
            GridLoad();
            string sel = "select b.asptblmacdetid, b.machine   from  ASPTBLMACMAS a join asptblmacdet b on b.asptblmacmasid=a.asptblmacmasid join asptbllinmas  c on c.asptbllinmasid=a.line where a.active='T' AND A.COMPCODE='" + Class.Users.COMPCODE + "'";
            DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblmacmas");
            DataTable dt1 = ds.Tables["asptblmacmas"];
            if (dt1.Rows.Count > 0)
            {
                combomachine.DisplayMember = "machine";
                combomachine.ValueMember = "asptblmacdetid";
                combomachine.DataSource = dt1;

            }

            DataTable dt = mas.aspcomcode(Class.Users.HCompcode);
            if (dt.Rows.Count > 0)
            {
                combocompcode.DisplayMember = "compcode";
                combocompcode.ValueMember = "gtcompmastid";
                combocompcode.DataSource = dt;

            }
            empty();
        }

        
        private void empty()
        {
            dtgridview.Rows.Clear(); dtgridview.Columns.Clear(); progressBar1.Minimum = 0; progressBar1.Maximum = 0; lblprogress1.Text = "";
            this.BackColor = Class.Users.BackColors;
            butheader.BackColor = Class.Users.BackColors;
            panel2.BackColor = Class.Users.BackColors;
            panel3.BackColor = Class.Users.BackColors;
            this.Font = Class.Users.FontName;
            listView1.Font = Class.Users.FontName;
            txtmanualid.Text = "";   combomachine.Text = "";         
            txtsearch.Text = "";
            txtbarcode.Text = "";
            txtdate.Text = "";
            txttime.Text = "";
            txtdatetime.Text = "";
            combomachine.Select();
        }
        public void GridLoad()
        {
            try
            {
                listView1.Items.Clear(); listfilter.Items.Clear();
                string sel1 = "select b.asptblmanmasid, b.machine,b.barcode, b.date,b.time,b.datetime,b.active from asptblmacmas a  join asptblmanmas b on b.machine=a.asptblmacmasid  order by 1";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblmanmas");
                DataTable dt = ds.Tables["asptblmanmas"];
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.Text = i.ToString();
                        list.SubItems.Add(myRow["asptblmanmasid"].ToString());
                        list.SubItems.Add(myRow["machine"].ToString());
                        list.SubItems.Add(myRow["barcode"].ToString());
                        list.SubItems.Add(myRow["date"].ToString());
                        list.SubItems.Add(myRow["time"].ToString());
                        list.SubItems.Add(myRow["datetime"].ToString());
                        list.SubItems.Add(myRow["active"].ToString());
                        listView1.Items.Add(list);
                        listfilter.Items.Add((ListViewItem)list.Clone());
                        list.BackColor = i % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                        i++;
                    }
                    lbltotal.Text = "Total Count    :" + listView1.Items.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void ListView1_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (listView1.Items.Count > 0)
                {

                    txtmanualid.Text = listView1.SelectedItems[0].SubItems[1].Text;
                    string sel1 = " select a.asptblmanmasid, b.machine ,a.barcode,a.date,a.time,a.datetime,a.active    from  asptblmanmas a  join asptblmacmas b on a.machine=b.asptblmacmasid     where a.asptblmanmasid=" + txtmanualid.Text;
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblmanmas");
                    DataTable dt = ds.Tables["asptblmanmas"];

                    if (dt.Rows.Count > 0)
                    {
                        txtmanualid.Text = Convert.ToString(dt.Rows[0]["asptblmanmasid"].ToString());
                        combomachine.Text = Convert.ToString(dt.Rows[0]["machine"].ToString());
                        txtbarcode.Text = Convert.ToString(dt.Rows[0]["barcode"].ToString());
                        txtdate.Text = Convert.ToString(dt.Rows[0]["date"].ToString());
                        txttime.Text = Convert.ToString(dt.Rows[0]["time"].ToString());
                        txtdatetime.Text = Convert.ToString(dt.Rows[0]["datetime"].ToString());
                        if (dt.Rows[0]["active"].ToString() == "T") { checkactive.Checked = true; } else { checkactive.Checked = true; checkactive.Checked = false; }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Search()
        {
            try
            {
                if (txtsearch.Text.ToUpper() != "")
                {
                    listView1.Items.Clear(); int iGLCount = 1;
                    string sel1 = "  select a.asptblmanmasid, b.machine ,a.barcode,a.date,a.time,a.datetime, a.active    from  asptblmanmas a  join asptblmacmas b on a.machine=b.asptblmacmasid  where a.machine LIKE'%" + txtsearch.Text.ToUpper() + "%' ";
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblmanmas");
                    DataTable dt = ds.Tables["asptblmanmas"];
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow myRow in dt.Rows)
                        {
                            ListViewItem list = new ListViewItem();
                            list.Text = iGLCount.ToString();
                            list.SubItems.Add(myRow["asptblmanmasid"].ToString());
                            list.SubItems.Add(myRow["machine"].ToString());
                            list.SubItems.Add(myRow["barcode"].ToString());
                            list.SubItems.Add(myRow["date"].ToString());
                            list.SubItems.Add(myRow["time"].ToString());
                            list.SubItems.Add(myRow["datetime"].ToString());
                            list.SubItems.Add(myRow["active"].ToString());
                            listView1.Items.Add(list);
                            iGLCount++;
                        }
                        lbltotal.Text = "Total Count    :" + listView1.Items.Count;
                    }
                    else
                    {
                        listView1.Items.Clear();
                    }
                }
                else
                {

                    listView1.Items.Clear();
                    GridLoad();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Deletes()
        {
            if (txtmanualid.Text != "")
            {
                string sel1 = "select a.asptblmanmasid from asptblmanmas a join gtstatemast b on a.asptblmanmasid=b.machine where a.asptblmanmasid='" + txtmanualid.Text + "'";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblmanmas");
                DataTable dt = ds.Tables["asptblmanmas"];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Child Record Found.Can Not Delete." + txtbarcode.Text, " Alert Message ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                else
                {

                    string del = "delete from asptblmanmas where asptblmanmasid='" + Convert.ToInt64("0" + txtmanualid.Text) + "'";
                    Utility.ExecuteNonQuery(del);
                    MessageBox.Show("Record Deleted Successfully " + txtbarcode.Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GridLoad(); empty();
                }
            }
        }

        public void Prints()
        {
            throw new NotImplementedException();
        }

        public void Searchs()
        {
            throw new NotImplementedException();
        }



        public void ReadOnlys()
        {
            throw new NotImplementedException();
        }

        public void Imports()
        {
            
        }

        public void Pdfs()
        {
            throw new NotImplementedException();
        }

        public void ChangePasswords()
        {
            throw new NotImplementedException();
        }
        DataTable dtgridview = new DataTable();
        public void DownLoads()
        {
            try
            {

                dtgridview.Rows.Clear(); dtgridview.Columns.Clear();
                int i = 0;
                System.Data.OleDb.OleDbConnection OledbConn;
                System.Data.OleDb.OleDbCommand OledbCmd;
                System.Data.OleDb.OleDbDataAdapter OledbAdapter;
                string filePath = string.Empty;
                string fileExt = string.Empty;
                OpenFileDialog file = new OpenFileDialog();
                string path = "";
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
                {
                    filePath = file.FileName; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (fileExt.CompareTo(".xls") == 0)

                        path = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HRD=Yes;'"; //for below excel 2007  

                    //path = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties="Excel 8.0;HRD=Yes;";"; //for below excel 2007  
                    else
                        path = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;'"; //for above excel 2007  

                    OledbConn = new System.Data.OleDb.OleDbConnection(path);
                    OledbConn.Open();
                    string qry1 = "Select * from [Sheet1$]";
                    OledbAdapter = new OleDbDataAdapter(qry1, OledbConn);
                    OledbAdapter.Fill(dtgridview);
                    if (dtgridview.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dtgridview;

                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            News();
            GlobalVariables.HeaderName.Text = "";
            GlobalVariables.TabCtrl.TabPages.RemoveAt(GlobalVariables.TabCtrl.SelectedIndex);
            this.Hide();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int item0 = 0;
                if (txtsearch.Text.Length > 0)
                {
                    listView1.Items.Clear();
                    foreach (ListViewItem item in listfilter.Items)
                    {
                        ListViewItem list = new ListViewItem();
                        if (item.SubItems[2].ToString().Contains(txtsearch.Text))
                        {
                            list.Text = item.SubItems[0].Text;
                            list.SubItems.Add(item.SubItems[1].Text);
                            list.SubItems.Add(item.SubItems[2].Text);
                            list.SubItems.Add(item.SubItems[3].Text);
                            list.SubItems.Add(item.SubItems[4].Text);
                            list.SubItems.Add(item.SubItems[5].Text);
                            list.SubItems.Add(item.SubItems[6].Text);
                            list.SubItems.Add(item.SubItems[7].Text);
                            list.BackColor = item0 % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                            listView1.Items.Add(list);
                        }
                        item0++;
                    }
                }
                else
                {
                    GridLoad();
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("---" + ex.ToString());
            }
        }

        public void Searchs(int EditID)
        {
            throw new NotImplementedException();
        }

        private void txtbarcode_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}

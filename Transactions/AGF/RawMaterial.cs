using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using CrystalDecisions.Shared;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using SuperSimpleTcp;
using System.Text;
using System.Linq;

namespace Pinnacle.Transactions.AGF
{
    public partial class RawMaterial : Form,ToolStripAccess
    {
        ListView listfilter = new ListView();
        private static RawMaterial _instance; string s = "";
        bool tabcheck = false; string readValue = "";
        public static RawMaterial Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RawMaterial();
                _instance.Font = Class.Users.FontName; 
                GlobalVariables.CurrentForm = _instance; return _instance;
               
            }
           
        }
        Models.Master mas = new Models.Master();
        Models.UserRights sm = new Models.UserRights();
       

        public RawMaterial()
        {
            InitializeComponent(); 
            Class.Users.IPADDRESS = GenFun.GetLocalIPAddress();
            Class.Users.CREATED = Convert.ToDateTime(System.DateTime.Now.ToString());
            Class.Users.SysDate = Convert.ToString(System.DateTime.Now.ToString("dd-MM-yyyy"));
            Class.Users.SysTime = Convert.ToString(DateTime.Now.ToShortTimeString().ToString());
            usercheck(Class.Users.HCompcode, Class.Users.HUserName, Class.Users.ScreenName);
            tabcheck = false; Class.Users.PayPeriod = ""; Class.Users.Paramid = 0;
        }
        public void usercheck(string s, string ss, string sss)
        {
            try
            {
                DataTable dt1 = sm.headerdropdowns(s, ss, sss);
                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["Menuname"].ToString() == Class.Users.ScreenName)
                    {
                        for (int r = 0; r < dt1.Rows.Count; r++)
                        {

                           if (dt1.Rows[r]["TREEBUTTON"].ToString() == "T") 
                            { 
                                GlobalVariables.TreeButtons.Visible = false; txtgrossweight.Enabled = true; txttareweight.Enabled = true;
                            } 
                            else {
                                GlobalVariables.TreeButtons.Visible = false;  txttareweight.Enabled = false; txtgrossweight.Enabled = false; 
                            }
                      

                        }
                    }


                }
                else
                {
                    MessageBox.Show("Invalid");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("usercheck: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private string readserialvalue;
        private Decimal wt = 0;
        bool valid = false; bool validprint = false;
        Models.Validate va = new Models.Validate();

        // ThulliamMdi mdi = new ThulliamMdi();

              SimpleTcpClient client; string[] words1; SimpleTcpServer server;
        private void RawMaterial_Load(object sender, EventArgs e)
        {



            tabControl1.SelectTab(tabPageraw1);
            Models.Serial.PortIP = Class.Users.PortIP + ":" + Class.Users.PortNo;
            Models.Serial.PortNo = Class.Users.PortNo;
            //server = new SimpleTcpServer(Models.Serial.PortIP);
            //server.Events.ClientConnected += Events_ClientConnected;
            //server.Events.ClientDisconnected += Events_ClientDisconnected;
            //server.Events.DataReceived += Events_DataReceived;
            //client = new SimpleTcpClient(Models.Serial.PortIP);
            //client.Events.DataReceived += Events_DataReceived;
            butstart_Click(sender, e);
            //serialPort2.Close();
            txtsearch.Select();
           
            //txtgrossweight.Text = "0.00";
            //txttareweight.Text = "0.00";
            //if (server.IsListening)
            //{
            //    server.Start();
            //    //  server.Send(Models.Serial.PortIP, "0");

            //}

        }
        //private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    s = "";
        //    this.Invoke((MethodInvoker)delegate
        //   {

        //       s += Encoding.UTF8.GetString(e.Data.Array).Trim();
        //   });

        //}

   
        public void News()
        {
            empty();
            Models.Serial.PortIP = Class.Users.PortIP + ":" + Class.Users.PortNo;
            Models.Serial.PortNo = Class.Users.PortNo;
            
            GridLoad(); companyload();
            Varityitemload(); ReceivedFormload();
            comboitemload();

            this.BackColor = Class.Users.BackColors;
            buttdelete.BackColor = Class.Users.BackColors;
            buttsave.BackColor = Class.Users.BackColors;
            buttprint.BackColor = Class.Users.BackColors;
            buttnew.BackColor = Class.Users.BackColors;
            this.Font = Class.Users.FontName;
            panel3.BackColor = Class.Users.BackColors;
            panel2.BackColor = Class.Users.BackColors;
            listView1.Font = Class.Users.FontName;
            btnsecondweight.BackColor = Class.Users.BackColors;
            btnfirstgetweight.BackColor = Class.Users.BackColors;

            buttsearch.Font = Class.Users.FontName;
            butfirstweight.Font = Class.Users.FontName;
            butgetall.Font = Class.Users.FontName;
            butnetweight.Font = Class.Users.FontName;
           
            try
            {
                tabControl1.SelectTab(tabPageraw1);  txtrawmetid.Text = "";
                txtvechileno.Select(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //if (tabcheck == false)
            //{
            //    tabControl1.SelectTab(tabPageraw1);
            //    tabcheck = true;
            //}
            //else
            //{
            //    tabControl1.SelectTab(tabPageraw2);
            //    tabcheck = true;
            //}
            txtvechileno.Text = "";
        }


        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.N | Keys.Control:
                    // ... Process Shift+Ctrl+Alt+B ...
                    News(); tabcheck = true; txtvechileno.Text = ""; tabControl1.SelectTab(tabPageraw2);
                    return true; // signal that we've processed this key
                case Keys.S | Keys.Control:
                    // ... Process Shift+Ctrl+Alt+B ...
                    Saves();
                    return true; // signal that we've processed this key
                case Keys.P | Keys.Control:
                    // ... Process Shift+Ctrl+Alt+B ...
                    Saves();
                    Prints();

                    return true; // signal that we've processed this key
                //case Keys.E | Keys.Control:
                //    // ... Process Shift+Ctrl+Alt+B ...
                //    updating = true;
                //    adding = false;
                //    EnableText();
                //    return true; // signal that we've processed this key
                case Keys.D | Keys.Control:
                    // ... Process Shift+Ctrl+Alt+B ...
                    Deletes();
                    tabControl1.SelectTab(tabPageraw2);
                    return true; // signal that we've processed this key
            }
            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }


        public void ReceivedFormload()
        {
            try
            {
                string sel = "select '' as asptblpartymasid, '' as partyname from dual union all select a.asptblpartymasid,a.partyname from asptblpartymas a ";
                DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblpartymas");
                DataTable dt = ds.Tables["asptblpartymas"];
                comboreceivedfrom.DataSource = dt;
                comboreceivedfrom.DisplayMember = "partyname";
                comboreceivedfrom.ValueMember = "asptblpartymasid";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReceivedFormload: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
      
        public void companyload()
        {
            try
            {
                string sel = "select a.gtcompmastid,a.compcode from  gtcompmast a  where a.ptransaction ='COMPANY' order by 2 ;";
                DataSet ds = Utility.ExecuteSelectQuery(sel, "gtcompmast");
                DataTable dt = ds.Tables["gtcompmast"];

                combocompcode.DisplayMember = "compcode";
                combocompcode.ValueMember = "gtcompmastid"; combocompcode.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Finyearload: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void combocompcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                    autonumberload();
                

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

                string sel = "select max(a.asptblrawmaterialid1)+1 as id,b.compname from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  where b.ptransaction='COMPANY' and a.finyear='" + combofinyear.Text + "'  and b.compcode='" + combocompcode.Text + "' GROUP BY B.compname;  ";
                DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblrawmaterial");
                DataTable dt = ds.Tables["asptblrawmaterial"];
                int cnt = dt.Rows.Count;
                if (cnt == 0)
                {
                    string sel1 = "select b.gtcompmastid, b.compname from  gtcompmast b  where b.ptransaction='COMPANY'  and b.compcode='" + combocompcode.Text + "'; ";
                    DataSet ds1 = Utility.ExecuteSelectQuery(sel1, "gtcompmast");
                    DataTable dt1 = ds1.Tables["gtcompmast"];
                    combocompname.DisplayMember = "compname";
                    combocompname.ValueMember = "gtcompmastid";
                    combocompname.DataSource = dt1;

                    txtcertificateno.Text = combocompcode.Text + "/" + Class.Users.Finyear + "/" + 1;
                    txtrawmetid1.Text = "1";
                }
                else
                {
                    combocompname.Text = dt.Rows[0]["compname"].ToString();
                    txtcertificateno.Text = combocompcode.Text + "/" + Class.Users.Finyear + "/" + dt.Rows[0]["id"].ToString();
                    txtrawmetid1.Text = dt.Rows[0]["id"].ToString();
                    // }
                }
                
            }
            catch (Exception ex)
            {
               // MessageBox.Show("autonumberload: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        public void Varityitemload()
        {
            //string sel = "select a.asptblrawmaterialid from asptblrawmaterial a where  a.compcode='" + Convert.ToInt64("0"+combocompcode.SelectedValue.ToString()) + "' and a.finyear='"+ System.DateTime.Now.Year+ "' and SUBSTR(a.datetime1, 1, 10)= '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "'  ";
            //DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblitemmast");
            //DataTable dt = ds.Tables["asptblitemmast"];

            //comboticketno.DisplayMember = "asptblrawmaterialid";
            //comboticketno.ValueMember = "asptblrawmaterialid";
            //comboticketno.DataSource = dt;

        }
        public void comboitemload()
        {
            string sel = "select '' as asptblitemmastid, '' as itemname from dual union all select a.asptblitemmastid,a.itemname from asptblitemmast a where a.active='T' ";
            DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblitemmast");
            DataTable dt = ds.Tables["asptblitemmast"];

            comboitem.DisplayMember = "itemname";
            comboitem.ValueMember = "asptblitemmastid";
            comboitem.DataSource = dt;

        }
        private void combovarietyitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (comboticketno.SelectedValue.ToString() != "")
            //    {
            //        string sel = "select distinct a.asptblrawmaterialid, a.vechileNo from asptblrawmaterial a where a.compcode='" + combocompcode.SelectedValue.ToString() + "' and a.finyear='" + System.DateTime.Now.Year + "' and  a.asptblrawmaterialid='" + comboticketno.Text + "' ";
            //        DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblitemmast");
            //        DataTable dt = ds.Tables["asptblitemmast"];
            //        txtvechileno.Text = "";
            //        txtvechileno.Text = dt.Rows[0]["vechileNo"].ToString();
            //    }
            //}
            //catch(Exception ex) { }
          
        }
        public void Saves()
        {
            try
            {
                Models.Validate va = new Models.Validate(); Int64 maxid = 0;
                Class.Users.PayPeriod = "";Class.Users.Paramid = 0;
                string sw = Convert.ToString(txttareweight.Text);
               
                string status1 = sw == "0.00" ? "P" : "C";
                if (va.IsStringNumberic(txtvechileno.Text) == false)
                {
                    MessageBox.Show("'VechileNo  is Empty'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtvechileno.Select();
                    return;
                }
                if (comboloadstatus.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("'Load Status  is Empty'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.comboloadstatus.Focus();
                    return;
                }
                if (comboitem.SelectedValue.ToString()== "")
                {
                    MessageBox.Show("'Material  is Empty'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboitem.Select();
                    return;
                }
                if (comboreceivedfrom.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("'Supplier Name Field is Empty'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboreceivedfrom.Select(); 
                    return;
                }                
               

                

                if (va.IsDecimal(txtgrossweight.Text) == false)
                {
                    MessageBox.Show("'First Weight Field  is Empty'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtgrossweight.Focus();
                   
                }
                else
                {
                    validprint = false;
                    string words = txtvechileno.Text;
                    words = Regex.Replace(words, @" ", "");
                    txtvechileno.Text = words;
                    string dat = "";
                    dat = dateTimePicker1.Value.ToString("yyyy-MM-dd").Substring(0,10);
                    string dat1 = ""; int loadstatus = comboloadstatus.Text == "Empty" ? 0 : 1;
                    dat1 = Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    string times = Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("hh-mm-ss");
                    string sel = "select asptblrawmaterialid    from  asptblrawmaterial   WHERE  finyear='" + combofinyear.Text + "' and compcode='" + combocompcode.SelectedValue.ToString() + "' and compname='" + combocompcode.SelectedValue.ToString() + "' and certificateno='" + txtcertificateno.Text + "' and  vechileno='" + txtvechileno.Text.ToUpper() + "' and datetime1='" + dat + "' and receivedFrom='" + comboreceivedfrom.SelectedValue.ToString() + "' and itemname='" + comboitem.SelectedValue + "' and thirdpartyweight='"+txtthirdpartyweight.Text+ "' and itemnamevarity='" + loadstatus + "' and noofbag='" + txtbags.Text + "'  and grossweight='" + txtgrossweight.Text + "'  and tareweight='" + txttareweight.Text + "' and netweight='" + txtnetweight.Text + "'   and remarks='" + txtremarks.Text + "';";
                    DataSet ds = Utility.ExecuteSelectQuery(sel, "asptblrawmaterial");
                    DataTable dt = ds.Tables["asptblrawmaterial"];
                    Class.Users.Paramid = 0;
                    Class.Users.PayPeriod = "";
                   
                    if (dt.Rows.Count != 0)
                    {
                         validprint = true;
                       
                         tabControl1.SelectTab(tabPageraw2);
                        Class.Users.Paramid = Convert.ToInt64("0" + txtrawmetid.Text);
                        Class.Users.PayPeriod = txtvechileno.Text;
                    }
                    else if (dt.Rows.Count != 0 && Convert.ToInt32("0" + txtrawmetid.Text) == 0 || Convert.ToInt32("0" + txtrawmetid.Text) == 0)
                    {
                        string ins = "insert into asptblrawmaterial(asptblrawmaterialid1,finyear,compcode,compname,certificateno, vechileno,datetime1,receivedFrom,itemname,thirdpartyweight,itemnamevarity,grossweight,tareweight,netweight,remarks,compcode1, username,createdby, modifiedby,ipaddress,createdon,noofbag)values('" + txtrawmetid1.Text + "','" + combofinyear.Text + "','" + combocompcode.SelectedValue.ToString() + "','" + combocompcode.SelectedValue.ToString() + "','" + txtcertificateno.Text.ToUpper() + "', '" + txtvechileno.Text.ToUpper() + "','" + dat + "','" + comboreceivedfrom.SelectedValue.ToString() + "','" + comboitem.SelectedValue + "','" + Convert.ToInt32("0"+txtthirdpartyweight.Text) + "','" + loadstatus + "','" + Convert.ToString(txtgrossweight.Text) + "','" + Convert.ToDecimal("0" + txttareweight.Text).ToString() + "','" + Convert.ToString(txtnetweight.Text) + "','"+txtremarks.Text.ToUpper() + "','" + Class.Users.COMPCODE + "', '" + Class.Users.USERID + "','" + Class.Users.HUserName + "', '','" + Class.Users.IPADDRESS + "' ,'" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','"+Convert.ToInt32("0"+txtbags.Text)+"' );";
                        Utility.ExecuteNonQuery(ins);  
                        validprint = true; Class.Users.Paramid = 0;
                        string sel2 = "select max(asptblrawmaterialid) as asptblrawmaterialid   from  asptblrawmaterial   WHERE  finyear='" + combofinyear.Text + "' and compname='" + combocompcode.SelectedValue.ToString() + "' ;";
                        DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "asptblrawmaterial");
                        DataTable dt2 = ds2.Tables["asptblrawmaterial"]; maxid = 0;
                        maxid = Convert.ToInt64(dt2.Rows[0]["asptblrawmaterialid"].ToString());
                        string ins1 = "insert into asptblrawmaterial2(asptblrawmaterialid,asptblrawmaterialid1,finyear,compcode,compname,certificateno, vechileno,datetime1,datetime2,receivedFrom,itemname,thirdpartyweight,itemnamevarity,grossweight,tareweight,netweight,remarks,compcode1, username,createdby, modifiedby,ipaddress,createdon,transaction, operation,noofbag)values('" + maxid + "','" + txtrawmetid1.Text + "','" + combofinyear.Text + "','" + combocompcode.SelectedValue.ToString() + "','" + combocompcode.SelectedValue.ToString() + "','" + txtcertificateno.Text.ToUpper() + "', '" + txtvechileno.Text.ToUpper() + "','" + Convert.ToDateTime(dat).ToString("dd-MM-yyyy") + " " + times + "','" + dat1 + "','" + comboreceivedfrom.SelectedValue.ToString() + "','" + comboitem.SelectedValue + "','"+ Convert.ToInt32("0" + txtthirdpartyweight.Text) + "','" + loadstatus + "','" + Convert.ToString(txtgrossweight.Text) + "','" + Convert.ToDecimal("0" + txttareweight.Text).ToString() + "','" + Convert.ToString(txtnetweight.Text) + "','" + txtremarks.Text.ToUpper() + "','" + Class.Users.COMPCODE + "', '" + Class.Users.USERID + "','" + Class.Users.HUserName + " : " + Class.Users.IPADDRESS + "', '','" + Class.Users.IPADDRESS + "' ,'" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + Class.Users.ScreenName + "','INSERTED','" + Convert.ToInt32("0" + txtbags.Text) + "')";
                        Utility.ExecuteNonQuery(ins1);                       

                        Class.Users.Paramid = Convert.ToInt64("0" + maxid);
                        Class.Users.PayPeriod = txtvechileno.Text;
                        mas.pop("Record Saved Successfully", "Ticket No :" + txtcertificateno.Text, "NetWight :" + txtnetweight.Text);
                        Class.Users.Paramid = Convert.ToInt64("0" + txtrawmetid.Text);
                        GridLoad();
                        empty();txtvechileno.Text = "";
                        autonumberload(); tabControl1.SelectTab(tabPageraw2);
                    }
                    else
                    {
                        string up = "update  asptblrawmaterial  set asptblrawmaterialid1='" + txtrawmetid1.Text + "',finyear='" + combofinyear.Text + "',compcode='" + combocompcode.SelectedValue.ToString() + "',compname='" + combocompcode.SelectedValue.ToString() + "',certificateno='" + txtcertificateno.Text.ToUpper() + "', vechileno='" + txtvechileno.Text.ToUpper() + "',datetime1='" + dat + "',receivedFrom='" + comboreceivedfrom.SelectedValue.ToString() + "',itemname='" + comboitem.SelectedValue + "', thirdpartyweight='" + Convert.ToInt32("0" + txtthirdpartyweight.Text) + "' ,noofbag='"+ Convert.ToInt32("0" + txtbags.Text) + "', itemnamevarity='" + loadstatus+"', grossweight='" + txtgrossweight.Text + "',tareweight='" + Convert.ToDecimal("0" + txttareweight.Text).ToString() + "',netweight='" + txtnetweight.Text + "',compcode1='" + Class.Users.COMPCODE + "', username='" + Class.Users.USERID + "', modifiedby='" + Class.Users.HUserName + "',ipaddress='" + Class.Users.IPADDRESS + "' , modifiedon='" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',remarks='" + txtremarks.Text + "'  where asptblrawmaterialid='" + txtrawmetid.Text + "';";
                        Utility.ExecuteNonQuery(up);

                        string ins = "insert into asptblrawmaterial2(asptblrawmaterialid,asptblrawmaterialid1,finyear,compcode,compname,certificateno, vechileno,datetime1,datetime2,receivedFrom,itemname,thirdpartyweight,itemnamevarity,noofbag,grossweight,tareweight,netweight,compcode1, username,createdby, modifiedby,ipaddress,createdon,transaction, operation,remarks)values('" + txtrawmetid.Text + "','" + txtrawmetid1.Text + "','" + combofinyear.Text + "','" + combocompcode.SelectedValue.ToString() + "','" + combocompcode.SelectedValue.ToString() + "','" + txtcertificateno.Text.ToUpper() + "', '" + txtvechileno.Text.ToUpper() + "','" + Convert.ToDateTime(dat).ToString("dd-MM-yyyy") + " " + times + "','" + dat1 + "','" + comboreceivedfrom.SelectedValue.ToString() + "','" + comboitem.SelectedValue + "','" + Convert.ToInt32("0" + txtthirdpartyweight.Text) + "','" + loadstatus + "','"+ Convert.ToInt32("0" + txtbags.Text) + "','" + Convert.ToString(txtgrossweight.Text) + "','" + Convert.ToDecimal("0" + txttareweight.Text).ToString() + "','" + Convert.ToString(txtnetweight.Text) + "','" + Class.Users.COMPCODE + "', '" + Class.Users.USERID + "','" + Class.Users.HUserName + " : " + Class.Users.IPADDRESS + "', '','" + Class.Users.IPADDRESS + "' ,'" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + Class.Users.ScreenName + "','UPDATED','" + txtremarks.Text + "' );";
                        Utility.ExecuteNonQuery(ins);
                      

                        validprint = true;
                        GridLoad();
                        mas.pop("Record Updated Successfully", "Ticket No :" + txtcertificateno.Text, "NetWight :" + txtnetweight.Text);
                        Class.Users.Paramid = Convert.ToInt64("0"+txtrawmetid.Text);
                        Class.Users.PayPeriod = txtvechileno.Text;
                        empty();txtvechileno.Text = "";
                        autonumberload(); tabControl1.SelectTab(tabPageraw2);

                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Saves_Click " + "        " + ex.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RawMaterial_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        public void Exit()
        {
            GlobalVariables.MdiPanel.Show();
            this.Hide();
            empty(); tabcheck = false;
            GlobalVariables.HeaderName.Text = "";
            GlobalVariables.TabCtrl.TabPages.RemoveAt(GlobalVariables.TabCtrl.SelectedIndex);

        }


        private void empty()
        {

            
            txtrawmetid.Text = "";s = "";words1 = null;txtthirdpartyweight.Text = "";txtcertificateno.Text = "";
            txtrawmetid1.Text = ""; comboloadstatus.Text = "";
           comboreceivedfrom.Text = "";comboitem.Text = "";
            combofinyear.Text = Class.Users.Finyear;txtbags.Text = "";
            txtremarks.Text = "";
            dateTimePicker1.Value = System.DateTime.Now;

            txtgrossweight.Text = "";
            txttareweight.Text = "";
            txtnetweight.Text = "";
   
           
            
        }
        public void GridLoad()
        {
            try
            {
                listView1.Items.Clear(); listfilter.Items.Clear();
                string sel1 = "select a.asptblrawmaterialid,a.vechileno,a.certificateno,c.partyname, date_format(a.datetime1,'%d-%m-%Y') as datetime1,a.thirdpartyweight,a.remarks,a.itemnamevarity,a.grossweight,a.tareweight," +
                    "a.netweight from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid   where  a.datetime1='" +  dateTimePicker3.Value.ToString("yyyy-MM-dd") + "'  order by a.asptblrawmaterialid desc;";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                DataTable dt = ds.Tables["asptblrawmaterial"];
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.SubItems.Add(i.ToString());
                        list.SubItems.Add(myRow["asptblrawmaterialid"].ToString());
                        list.SubItems.Add(myRow["vechileno"].ToString());                     
                        list.SubItems.Add(myRow["certificateno"].ToString());
                        list.SubItems.Add(myRow["partyname"].ToString());
                        list.SubItems.Add(myRow["datetime1"].ToString());
                        list.SubItems.Add(myRow["grossweight"].ToString());
                        list.SubItems.Add(myRow["tareweight"].ToString());
                        list.SubItems.Add(myRow["netweight"].ToString());
                        list.SubItems.Add(myRow["thirdpartyweight"].ToString());
                        list.SubItems.Add(myRow["itemnamevarity"].ToString());                        
                        list.SubItems.Add(myRow["remarks"].ToString());

                        this.listfilter.Items.Add((ListViewItem)list.Clone());
                        list.BackColor = i % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                        listView1.Items.Add(list);
                        i++;
                    }
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
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
                empty(); Class.Users.PayPeriod = ""; Class.Users.Paramid = 0;

                if (listView1.Items.Count >0)
                {
                   Class.Users.Paramid = 0;
                    txtrawmetid.Text = listView1.SelectedItems[0].SubItems[2].Text;
                    txtvechileno.Text = listView1.SelectedItems[0].SubItems[3].Text;                   
                    string sel1 = "select a.asptblrawmaterialid,a.asptblrawmaterialid1,a.finyear,b.compcode,b.compname,a.certificateno, a.vechileno,a.datetime1,substr(a.createdon,11,19) as times,c.partyname as  receivedFrom,a.itemname,a.itemnamevarity,a.grossweight,a.tareweight,a.netweight, a.noofbag,a.thirdpartyweight,a.tripwagonno,a.lotno,a.sampledby,a.certifiedby,a.visualstatus,a.remarks as delayreason from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid  where a.asptblrawmaterialid='" + txtrawmetid.Text + "' and a.vechileNo='"+ txtvechileno.Text + "';";
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                    DataTable dt = ds.Tables["asptblrawmaterial"];

                    if (dt.Rows.Count > 0)
                    {
                       
                        txtrawmetid.Text = Convert.ToString(dt.Rows[0]["asptblrawmaterialid"].ToString());                      
                       txtrawmetid1.Text = Convert.ToString(dt.Rows[0]["asptblrawmaterialid1"].ToString());
                        txtcertificateno.Text= Convert.ToString(dt.Rows[0]["certificateno"].ToString());
                        combofinyear.Text= Convert.ToString(dt.Rows[0]["finyear"].ToString());
                        combocompcode.Text= Convert.ToString(dt.Rows[0]["compcode"].ToString());
                        comboitem.SelectedValue = Convert.ToString(dt.Rows[0]["itemname"].ToString());
                        txtvechileno.Text = Convert.ToString(dt.Rows[0]["vechileno"].ToString());
                        dateTimePicker1.Text = Convert.ToString(dt.Rows[0]["datetime1"].ToString());
                        dateTimePicker2.Text = Convert.ToString(dt.Rows[0]["times"].ToString());
                        comboreceivedfrom.Text = Convert.ToString(dt.Rows[0]["receivedFrom"].ToString());
                        txtthirdpartyweight.Text = Convert.ToString(dt.Rows[0]["thirdpartyweight"].ToString());
                        int loadstatus =Convert.ToInt64(dt.Rows[0]["itemnamevarity"].ToString()) == 0 ? comboloadstatus.SelectedIndex=1 : comboloadstatus.SelectedIndex = 2;
                        comboloadstatus.SelectedIndex = loadstatus;
                        txtgrossweight.Text = Convert.ToString(dt.Rows[0]["grossweight"].ToString());
                        txttareweight.Text = Convert.ToString(dt.Rows[0]["tareweight"].ToString());
                        txtnetweight.Text = Convert.ToString(dt.Rows[0]["netweight"].ToString());                       
                        txtremarks.Text = Convert.ToString(dt.Rows[0]["delayreason"].ToString());
                        txtbags.Text = Convert.ToString(dt.Rows[0]["noofbag"].ToString());
                        combocompcode.Enabled = false; Class.Users.PayPeriod = txtvechileno.Text;
                        tabControl1.SelectTab(tabPageraw1);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Txtsearch_TextChanged(object sender, EventArgs e)
        {
           

            try
            {
                int item0 = 0; listView1.Items.Clear();
                if (txtsearch.Text.Length >= 1)
                {

                    foreach (ListViewItem item in listfilter.Items)
                    {
                        ListViewItem list = new ListViewItem();
                        if (listfilter.Items[item0].SubItems[3].ToString().Contains(txtsearch.Text.ToUpper()) || listfilter.Items[item0].SubItems[4].ToString().Contains(txtsearch.Text.ToUpper()) || listfilter.Items[item0].SubItems[5].ToString().Contains(txtsearch.Text.ToUpper()) || listfilter.Items[item0].SubItems[11].ToString().Contains(txtsearch.Text.ToUpper())  || listfilter.Items[item0].SubItems[12].ToString().Contains(txtsearch.Text.ToUpper()))
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
                            list.BackColor = item0 % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                            listView1.Items.Add(list);


                        }
                        item0++;
                    }
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
                }
                else
                {
                    GridLoad();
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show("---" + ex.ToString());
            }

        }



        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceivedFormload(); usercheck(Class.Users.HCompcode, Class.Users.HUserName, Class.Users.ScreenName);
        }

        private void ContextMenuStrip2_Click(object sender, EventArgs e)
        {
             Varityitemload(); 
        }

        private void butserialportsetting_Click(object sender, EventArgs e)
        {
        

        }
        public void portconnection(string porttype, SerialPort _serialPort2)
        {
            try
            {
                porttype = Models.Serial.PortType;

              
                _serialPort2.PortName = Models.Serial.PortName;
                _serialPort2.BaudRate = Convert.ToInt32("0" + Models.Serial.BaudRate);
                _serialPort2.DataBits = Convert.ToInt32("0" + Models.Serial.DataBits);
                _serialPort2.StopBits = (StopBits)Enum.Parse(typeof(StopBits), Models.Serial.StopBits);
                _serialPort2.Parity = (Parity)Enum.Parse(typeof(Parity), Models.Serial.Parity);
                _serialPort2.Handshake = Handshake.None;
                _serialPort2.Open();
                _serialPort2.DiscardInBuffer();
                _serialPort2.DiscardOutBuffer();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Digitalizer not Connected." + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mas.pop("Digitalizer not Connected", "Digitalizer :" + ex.Message, "-");

            }

        }
        private void butstart_Click(object sender, EventArgs e)
        {

            try
            {



                serialPort2.Close();
                serialPort2.PortName = Models.Serial.PortName;
                serialPort2.BaudRate = Convert.ToInt32("0" + Models.Serial.BaudRate);
                serialPort2.DataBits = Convert.ToInt32("0" + Models.Serial.DataBits);
                serialPort2.StopBits = (StopBits)Enum.Parse(typeof(StopBits), Models.Serial.StopBits);
                serialPort2.Parity = (Parity)Enum.Parse(typeof(Parity), Models.Serial.Parity);
                serialPort2.Handshake = Handshake.None;
                serialPort2.Open();
                serialPort2.DiscardInBuffer();
                serialPort2.DiscardOutBuffer();
            }
            catch (Exception ex)
            {
                mas.pop("Digitalizer not Connected", "Digitalizer :" + ex.Message, "-");
            }
        }
       
        private void btnsecondweight_Click(object sender, EventArgs e)
        {
            try
            {
                txttareweight.Text = "0";

                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Vehicle Number is Empty");
                    return;
                }

                string[] words = null;

                butstart_Click(sender, e);
                //portconnection(Models.Serial.PortType, this.serialPort2);
                readValue = "";
                     readValue = serialPort2.ReadLine().Trim();
                if (readValue.Contains(":") && txtrawmetid.Text != "")
                {

                    words1 = readValue.Split(':');

                    if (words1[1].Length == 5)
                    {
                        txttareweight.Text = words1[1].Substring(0, words1[1].Length - 4);
                    }
                    if (words1[1].Length == 7)
                    {
                        txttareweight.Text = words1[1].Substring(0, words1[1].Length - 5);
                    }
                    if (words1[1].Length == 9)
                    {
                        txttareweight.Text = words1[1].Substring(0, words1[1].Length - 6);
                    }
                    if (words1[1].Length == 11)
                    {
                        txttareweight.Text = words1[1].Substring(0, words1[1].Length - 7);
                    }
                    if (words1[1].Length == 13)
                    {
                        txttareweight.Text = words1[1].Substring(0, words1[1].Length - 8);
                    }
                }
                    //words = readValue.Split(' ');
                    //MessageBox.Show("words[0] : "+words[0].ToString() + "words[1] :" + words[1].ToString() + "readValue : "+ readValue);
                    //string ss = words[1].ToString().Substring(0, words[1].Length - 2);
                    //MessageBox.Show("txttareweight" + ss.Length.ToString());
                    //if (words[1].Length == 7)
                    //{
                    //    txttareweight.Text = words[1].Substring(0, words[1].Length - 5);
                    //}

                    //if (words[1].Length == 9)
                    //{
                    //    txttareweight.Text = words[1].Substring(0, words[1].Length - 6);
                    //}
                    //if (words[1].Length == 11)
                    //{
                    //    txttareweight.Text = words[1].Substring(0, words[1].Length - 7);
                    //}
                    //if (words[1].Length == 13)
                    //{
                    //    txttareweight.Text = words[1].Substring(0, words[1].Length - 8);
                    //}


              
         

                //if (txtrawmetid.Text != "" && TryGetTareWeight(words, out long tare))
                //{
                //    txttareweight.Text = tare.ToString();
                //}
            }
            catch (Exception ex)
            {
              
            }
            finally
            {
                if (serialPort2.IsOpen)
                    serialPort2.Close();
            }

             s = "";  words1 = null; 
        }
       
       
     

    

     

        private void btnfirstgetweight_Click(object sender, EventArgs e)
        {
            try
            {
                txtgrossweight.Text = "0";

                if (string.IsNullOrWhiteSpace(txtvechileno.Text))
                {
                    MessageBox.Show("Vehicle Number is Empty");
                    txtvechileno.Select();
                    return;
                }

                string[] words = null;

                butstart_Click(sender, e);
                //portconnection(Models.Serial.PortType, this.serialPort2);
                readValue = "";

                    readValue = serialPort2.ReadLine().Trim();

                if (readValue.Contains(":") && txtrawmetid.Text == "")
                {
                    words1 = readValue.Split(':');

                    if (words1[1].Length == 5)
                    {
                        txtgrossweight.Text = words1[1].Substring(0, words1[1].Length - 4);
                    }
                    if (words1[1].Length == 7)
                    {
                        txtgrossweight.Text = words1[1].Substring(0, words1[1].Length - 5);
                    }
                    if (words1[1].Length == 9)
                    {
                        txtgrossweight.Text = words1[1].Substring(0, words1[1].Length - 6);
                    }
                    if (words1[1].Length == 11)
                    {
                        txtgrossweight.Text = words1[1].Substring(0, words1[1].Length - 7);
                    }
                    if (words1[1].Length == 13)
                    {
                        txtgrossweight.Text = words1[1].Substring(0, words1[1].Length - 8);
                    }
                }
                    //words =readValue.Split(' ');
                

                //if (txtrawmetid.Text == "" && TryGetWeight(words, out long grossWeight))
                //{
                //    txtgrossweight.Text = grossWeight.ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Weight Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (serialPort2.IsOpen)
                    serialPort2.Close();
            }
           
           words1 = null;s = "";
        }
        private bool TryGetTareWeight(string[] words, out long tare)
        {
            tare = 0;
            if (words == null) return false;

            foreach (string word in words)
            {
                if (long.TryParse(word, out long value))
                {
                    tare = value;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetWeight(string[] words, out long weight)
        {
            weight = 0;

            if (words == null) return false;

            foreach (string word in words)
            {
                if (long.TryParse(word, out long value) && value >= 0)
                {
                    weight = value;
                    return true;
                }
            }
            return false;
        }


        public void Deletes()
        {
            try
            {

                if (txtrawmetid.Text != "")
                {
                    var confirmation = MessageBox.Show("Do You want Delete this Record ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmation == DialogResult.Yes)
                    {
                        int loadstatus = comboloadstatus.Text == "Empty" ? 0 : 1;
                        string times = Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("HH-mm-ss");
                        string dat = "";
                        dat = dateTimePicker1.Value.ToString("dd-MM-yyyy").Substring(0, 10);
                        string dat1 = "";
                        dat1 = Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                        string del = "delete from asptblrawmaterial  where asptblrawmaterialid='" + txtrawmetid.Text + "'";
                        Utility.ExecuteNonQuery(del);


                                              string ins = "insert into asptblrawmaterial2(asptblrawmaterialid,asptblrawmaterialid1,finyear,compcode,compname,certificateno, vechileno,datetime1,datetime2,receivedFrom,itemname,itemnamevarity,grossweight,tareweight,netweight, noofbag,thirdpartyweight,delayreason,compcode1, username,createdby, modifiedby,ipaddress,createdon,transaction, operation,remarks,modifiedon)values('" + txtrawmetid.Text + "','" + txtrawmetid1.Text + "','" + combofinyear.Text + "','" + combocompcode.SelectedValue.ToString() + "','" + combocompcode.SelectedValue.ToString() + "','" + txtcertificateno.Text.ToUpper() + "', '" + txtvechileno.Text.ToUpper() + "','" + Convert.ToDateTime(dat).ToString("dd-MM-yyyy") + " " + times + "','" + dat1 + "','" + comboreceivedfrom.SelectedValue.ToString() + "','" + comboitem.SelectedValue + "','" + loadstatus + "','" + Convert.ToString(txtgrossweight.Text) + "','" + Convert.ToDecimal("0" + txttareweight.Text).ToString() + "','" + Convert.ToString(txtnetweight.Text) + "', '" + txtbags.Text + "','" + Convert.ToDecimal("0" + txtthirdpartyweight.Text) + "','" + txtremarks.Text.ToUpper() + "','" + Class.Users.COMPCODE + "', '" + Class.Users.USERID + "','" + Class.Users.HUserName + "', '','" + Class.Users.IPADDRESS + "' ,'" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + Class.Users.ScreenName + "','DELETED','" + txtremarks.Text + "','" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' );";
                        Utility.ExecuteNonQuery(ins);


                        MessageBox.Show("Record Deleted Successfully " + txtrawmetid.Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mas.pop("Record Deleted Successfully", "ID :" + txtrawmetid.Text, "");

                        GridLoad(); empty();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtgrossweight_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (txtgrossweight.Text != "")
                {
                   
                   // valid = va.IsIntegerdot(txtgrossweight.Text);                   
                    //if (valid == true)
                    //{
                        decimal wt = Convert.ToDecimal("0" + txtgrossweight.Text) - Convert.ToDecimal("0" + txttareweight.Text);
                        txtnetweight.Text = wt.ToString("0.00");
                    // }
                }
                else
                {
                    txtnetweight.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //try
            //{
            //    decimal wt = Convert.ToDecimal("0" + txtgrossweight.Text) - Convert.ToDecimal("0" + txttareweight.Text);
            //    txtnetweight.Text = wt.ToString("0.000");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
        private void txttareweight_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (txttareweight.Text != "" && txtgrossweight.Text != "")
                {
                    //valid = va.IsIntegerdot(txttareweight.Text);
                    //if (valid == false)
                    //{
                    //    MessageBox.Show("Numeric Only Allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    txttareweight.Focus(); txttareweight.Text = ""; return;
                    //}
                    //if (valid == true)
                    // {
                    //if (Convert.ToDecimal("0" + txtgrossweight.Text) <= Convert.ToDecimal("0" + txttareweight.Text))
                    //{
                    //    MessageBox.Show("Invalid Weight in TareWeight Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    //if (Convert.ToDecimal("0" + txtgrossweight.Text) > Convert.ToDecimal("0" + txttareweight.Text))
                    //{

                        decimal wt = Convert.ToDecimal("0" + txtgrossweight.Text) - Convert.ToDecimal("0" + txttareweight.Text);
                        txtnetweight.Text = wt.ToString("0.00");
                    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //try
            //{
            //    decimal wt = Convert.ToDecimal("0" + txtgrossweight.Text) - Convert.ToDecimal("0" + txttareweight.Text);
            //    txtnetweight.Text = wt.ToString("0.000");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }


        public void Searchs()
        {
            tabControl1.SelectTab(tabPageraw2);
        }
        public void Prints()
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument report = new ReportDocument();
            DataTable dt3 = new DataTable();
            try
            {


                Cursor = Cursors.WaitCursor;
                ReportFormate.CFM.AGFRawMaterial rd1 = new ReportFormate.CFM.AGFRawMaterial();

                string sel3 = "select a.asptblrawmaterialid,a.asptblrawmaterialid,a.asptblrawmaterialid1,a.finyear,b.compcode,b.compname,b.address, a.certificateno, a.vechileno, a.createdon as datetime1,c.partyname as  receivedFrom,d.itemname,a.itemnamevarity,a.grossweight,a.tareweight,a.netweight, a.noofbag,a.thirdpartyweight,a.tripwagonno,a.lotno,a.sampledby,a.certifiedby,a.visualstatus,a.delayreason,a.createdon,a.modifiedon,a.remarks,a.createdby as test1,b.companylogo from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid where b.compcode='" + combocompcode.Text + "' and  a.vechileno='" + txtvechileno.Text + "'  and  a.asptblrawmaterialid='" + txtrawmetid.Text + "' order by 1;";
                DataSet ds3 = Utility.ExecuteSelectQuery(sel3, "asptblrawmaterial");
                dt3 = ds3.Tables["asptblrawmaterial"];
                if (dt3.Rows.Count > 0)
                {
                    rd1.SetDataSource(dt3);
                    crystalReportViewer1.ReportSource = null;
                    crystalReportViewer1.ReportSource = rd1;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    crystalReportViewer1.ReportSource = null; Cursor = Cursors.Default;
                }

            }
            catch (Exception ex) { Cursor = Cursors.Default; }
            finally
            {
                Cursor = Cursors.Default;
                if (dt3.Rows.Count > 0)
                {
                    printDialog1 = new PrintDialog();
                    printDialog1.AllowSelection = true;
                    printDialog1.AllowSomePages = true;
                    if (Class.Users.HUserName == "ADMIN" || Class.Users.HUserName == "admin")
                    {
                        if (printDialog1.ShowDialog() == DialogResult.OK)
                        {

                            report.Load(Application.StartupPath + "\\ReportFormate\\CFM\\AGFRawMaterial.rpt");
                            report.SetDataSource(dt3);
                            report.PrintOptions.PrinterName = printDialog1.PrinterSettings.PrinterName;
                            report.PrintToPrinter(printDialog1.PrinterSettings.Copies, printDialog1.PrinterSettings.Collate, printDialog1.PrinterSettings.FromPage, printDialog1.PrinterSettings.ToPage);
                        }
                    }
                    else
                    {
                        report.Load(Application.StartupPath + "\\ReportFormate\\CFM\\AGFRawMaterial.rpt");
                        report.SetDataSource(dt3);
                        report.PrintOptions.PrinterName = printDialog1.PrinterSettings.PrinterName;
                        report.PrintToPrinter(printDialog1.PrinterSettings.Copies, printDialog1.PrinterSettings.Collate, printDialog1.PrinterSettings.FromPage, printDialog1.PrinterSettings.ToPage);

                    }
                }
                else
                {
                    mas.pop("No Data Found in DataTable", "Certificate No :" + txtcertificateno.Text, " DataTable:" + dt3.Rows.Count);
                }
            }

        }
        void printDetails()
        {
          
        }

        private void combocompname_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

 

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageraw1"])//your specific tabname
            {
                if (txtrawmetid.Text == "")
                {
                    empty();
                    autonumberload();
                }
                txtvechileno.Select();
                Class.Users.UserTime = 0;
             

            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageraw2"])//your specific tabname
            {
               txtsearch.Select();
                Class.Users.UserTime = 0;
            }


            
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

      

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

   

        private void txtvechileno_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtvechileno_KeyDown(object sender, KeyEventArgs e)
        {
           if(e.KeyCode==Keys.Enter)
            {
                comboreceivedfrom.Focus();
         
            }
        }

        private void comboreceivedfrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butfirstweight.Select();
      
            }
        }

        private void txtgrossweight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                txttareweight.Select();
            }
        }

        private void combocompcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtvechileno.Focus();
        
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void combovarietyitem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {


                txtvechileno.Focus(); 


            }
        }

    
        private void btngetweight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txttareweight.Select();
            }
        }

        private void txtvechileno_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboloadstatus.Select(); 
            }
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtgrossweight_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txttareweight.Focus(); 
            }
        }

        private void txttareweight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtremarks.Select(); 
            }
        }
 

 

       
        

        private void txtremarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                buttsave.Focus();
            }
        }
        private void buttsave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                buttsave.Focus();
            }
        }
        private void buttsave_Click(object sender, EventArgs e)
        {
            Saves();
           
        }
        private void buttprint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {            
                buttprint.Focus();
            }
        }
        private void buttsearch_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear(); listfilter.Items.Clear();
                string sel1 = "select a.asptblrawmaterialid,a.vechileno,a.certificateno,c.partyname, date_format(a.datetime1,'%d-%m-%Y') as datetime1,a.grossweight,a.tareweight," +
                    "a.netweight,a.thirdpartyweight,a.tripwagonno,a.delayreason from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid  where a.datetime1=date_format('" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "', '%Y-%m-%d')  order by a.asptblrawmaterialid desc;";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                DataTable dt = ds.Tables["asptblrawmaterial"];
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.SubItems.Add(i.ToString());
                        list.SubItems.Add(myRow["asptblrawmaterialid"].ToString());
                        list.SubItems.Add(myRow["vechileno"].ToString());
                        list.SubItems.Add(myRow["certificateno"].ToString());
                        list.SubItems.Add(myRow["partyname"].ToString());
                        list.SubItems.Add(myRow["datetime1"].ToString());
                        list.SubItems.Add(myRow["grossweight"].ToString());
                        list.SubItems.Add(myRow["tareweight"].ToString());
                        list.SubItems.Add(myRow["netweight"].ToString());                      
                        list.SubItems.Add(myRow["thirdpartyweight"].ToString());
                        list.SubItems.Add(myRow["tripwagonno"].ToString());
                        list.SubItems.Add(myRow["delayreason"].ToString());
                        this.listfilter.Items.Add((ListViewItem)list.Clone());

                        list.BackColor = i % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                        listView1.Items.Add(list);
                        i++;
                    }
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


          
        }

        private void buttprint_Click(object sender, EventArgs e)
        {
           
            tabcheck = true;
            Saves();
            if (validprint == true)
            {
                Prints();
                Class.Users.Paramid = 0; Class.Users.PayPeriod = "";
                //empty(); txtvechileno.Text = "";
            }
        }

        void firstweightdetails()
        {
            try
            {

                listView1.Items.Clear(); listfilter.Items.Clear();
                int iGLCount = 1;

                string sel1 = "select a.asptblrawmaterialid,a.vechileno,a.certificateno,c.partyname, date_format(a.datetime1,'%d-%m-%Y') as datetime1,a.grossweight,a.tareweight,a.netweight,a.thirdpartyweight from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid  where  a.tareweight = 0.00 and a.datetime1=date_format('" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "', '%Y-%m-%d') order by a.asptblrawmaterialid desc;";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                DataTable dt = ds.Tables["asptblrawmaterial"];
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.SubItems.Add(iGLCount.ToString());
                        list.SubItems.Add(myRow["asptblrawmaterialid"].ToString());
                        list.SubItems.Add(myRow["vechileno"].ToString());
                        list.SubItems.Add(myRow["certificateno"].ToString());
                        list.SubItems.Add(myRow["partyname"].ToString());
                        list.SubItems.Add(myRow["datetime1"].ToString());
                        list.SubItems.Add(myRow["grossweight"].ToString());
                        list.SubItems.Add(myRow["tareweight"].ToString());
                        list.SubItems.Add(myRow["netweight"].ToString());
                        list.SubItems.Add(myRow["thirdpartyweight"].ToString());
                        //list.SubItems.Add(myRow["tripwagonno"].ToString());
                        //list.SubItems.Add(myRow["delayreason"].ToString());


                        list.BackColor = iGLCount % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                        this.listfilter.Items.Add((ListViewItem)list.Clone());
                        listView1.Items.Add(list);
                        iGLCount++;
                    }
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
                }
                //else
                //{
                //    listView1.Items.Clear();

                //    GridLoad();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void butfirstweight_Click(object sender, EventArgs e)
        {
            firstweightdetails();
        }

        private void butnetweight_Click(object sender, EventArgs e)
        {
            try
            {              
                    listView1.Items.Clear(); listfilter.Items.Clear();
                int iGLCount = 1;
                    string sel1 = "select a.asptblrawmaterialid,a.vechileno,a.certificateno, c.partyname,date_format(a.datetime1,'%d-%m-%Y') as datetime1,a.grossweight,a.tareweight,a.netweight, a.tripwagonno,a.thirdpartyweight,a.delayreason from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid where a.tareweight >=1  and a.datetime1=date_format('" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "', '%Y-%m-%d') order by a.asptblrawmaterialid desc ;";
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                    DataTable dt = ds.Tables["asptblrawmaterial"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.SubItems.Add(iGLCount.ToString());
                        list.SubItems.Add(myRow["asptblrawmaterialid"].ToString());
                        list.SubItems.Add(myRow["vechileno"].ToString());
                        list.SubItems.Add(myRow["certificateno"].ToString());
                        list.SubItems.Add(myRow["partyname"].ToString());
                        list.SubItems.Add(myRow["datetime1"].ToString());
                        list.SubItems.Add(myRow["grossweight"].ToString());
                        list.SubItems.Add(myRow["tareweight"].ToString());
                        list.SubItems.Add(myRow["netweight"].ToString());
                        list.SubItems.Add(myRow["thirdpartyweight"].ToString());
                        list.SubItems.Add(myRow["tripwagonno"].ToString());
                        list.SubItems.Add(myRow["delayreason"].ToString());
                        listView1.Items.Add(list);
                        list.BackColor = iGLCount % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;
                        this.listfilter.Items.Add((ListViewItem)list.Clone());
                        iGLCount++;
                    }
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butgetall_Click(object sender, EventArgs e)
        {
            txtsearch.Text = "";

            try
            {
                listView1.Items.Clear(); listfilter.Items.Clear();

                string sel1 = "select a.asptblrawmaterialid,a.vechileno,a.certificateno,c.partyname, date_format(a.datetime1,'%d-%m-%Y') as datetime1,a.grossweight,a.tareweight," +
                   "a.netweight,a.thirdpartyweight,a.tripwagonno,a.delayreason from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemname=d.asptblitemmastid   where a.datetime1=date_format('" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "', '%Y-%m-%d') order by a.asptblrawmaterialid desc;";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                DataTable dt = ds.Tables["asptblrawmaterial"];


                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.SubItems.Add(i.ToString());
                        list.SubItems.Add(myRow["asptblrawmaterialid"].ToString());
                        list.SubItems.Add(myRow["vechileno"].ToString());
                        list.SubItems.Add(myRow["certificateno"].ToString());
                        list.SubItems.Add(myRow["partyname"].ToString());
                        list.SubItems.Add(myRow["datetime1"].ToString());
                        list.SubItems.Add(myRow["grossweight"].ToString());
                        list.SubItems.Add(myRow["tareweight"].ToString());
                        list.SubItems.Add(myRow["netweight"].ToString());                       
                        list.SubItems.Add(myRow["thirdpartyweight"].ToString());
                        list.SubItems.Add(myRow["tripwagonno"].ToString());
                        list.SubItems.Add(myRow["delayreason"].ToString());
                        
                        this.listfilter.Items.Add((ListViewItem)list.Clone());

                        list.BackColor = i % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                        listView1.Items.Add(list);
                        i++;
                    }
                    lbltotal.Text = "Total Count: " + listView1.Items.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttsearch_Click_1(object sender, EventArgs e)
        {
            Txtsearch_TextChanged(sender,e);
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

        private void txtremarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txttripwagonno_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '/' || e.KeyChar == (char)Keys.Back);
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

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            combocompcode.Enabled = true;
        }

        private void previousCertificateNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtrawmetid.Text != "")
            {
                string sel1 = "select a.asptblrawmaterialid,a.asptblrawmaterialid1,a.finyear,b.compcode,b.compname,a.certificateno from asptblrawmaterial a join gtcompmast b on a.compname=b.gtcompmastid  join asptblpartymas c on a.receivedFrom =c.asptblpartymasid join asptblitemmast d on a.itemnamevarity=d.asptblitemmastid join asptblgodwonmas e on a.godownname=e.asptblgodwonmasid   where a.asptblrawmaterialid='" + txtrawmetid.Text + "';";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblrawmaterial");
                DataTable dt = ds.Tables["asptblrawmaterial"];

                if (dt.Rows.Count > 0)
                {
                   
                    txtrawmetid1.Text = Convert.ToString(dt.Rows[0]["asptblrawmaterialid1"].ToString());
                    txtcertificateno.Text = Convert.ToString(dt.Rows[0]["certificateno"].ToString());
                    combofinyear.Text = Convert.ToString(dt.Rows[0]["finyear"].ToString());
                }
            }
        }

        private void tabPageraw1_Click(object sender, EventArgs e)
        {

        }

       

       
        public void Searchs(int EditID)
        {
            throw new NotImplementedException();
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

        public void DownLoads()
        {
            throw new NotImplementedException();
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

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Models.Serial.PortIP = Class.Users.PortIP + ":" + Class.Users.PortNo;
            Models.Serial.PortNo = Class.Users.PortNo;
            server = new SimpleTcpServer(Models.Serial.PortIP);
            //client = new SimpleTcpClient(Models.Serial.PortIP);
            //client.Events.DataReceived += Events_DataReceived;
            serialPort2.Close();
            txtsearch.Select();
            //if (server.IsListening)
            //{
            //    server.Start();
            //}
        }

        private void buttdelete_Click(object sender, EventArgs e)
        {
            Deletes();  tabControl1.SelectTab(tabPageraw2); txtvechileno.Text = "";
        }

        private void buttnew_Click(object sender, EventArgs e)
        {
            News(); tabControl1.SelectTab(tabPageraw1); txtvechileno.Text = ""; txtvechileno.Select();
        }

        private void comboloadstatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboitem.Select(); 
            }
        }

        private void comboitem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboreceivedfrom.Select(); 
            }
        }

        private void btnsecondweight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtremarks.Select();
            }
        }

        private void butPrintDialog_Click(object sender, EventArgs e)
        {
          
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
        }
    }
}

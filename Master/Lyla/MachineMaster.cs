using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pinnacle.Master.Lyla
{
    public partial class MachineMaster : Form,ToolStripAccess
    {
        private static MachineMaster _instance;
        Models.Master mas = new Models.Master();
        Models.UserRights sm = new Models.UserRights();
        ListView listfilter = new ListView();
        Pinnacle.Models.LYLA.MachineModeldet c1 = new Pinnacle.Models.LYLA.MachineModeldet();
        public static MachineMaster Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MachineMaster();
                GlobalVariables.CurrentForm = _instance;
                return _instance;
            }
        }
        public MachineMaster()
        {
            InitializeComponent();
           // usercheck(Class.Users.HCompcode, Class.Users.HUserName, Class.Users.ScreenName);
                listView1.Items.Clear(); Class.Users.UserTime = 0;
            Class.Users.IPADDRESS = GenFun.GetLocalIPAddress();
            Class.Users.CREATED = Convert.ToDateTime(System.DateTime.Now.ToString("dd-MM-yyyy") + " " + System.DateTime.Now.ToLongTimeString());
            butheader.BackColor = Class.Users.BackColors;
            panel4.BackColor = Class.Users.BackColors;
            panel5.BackColor = Class.Users.BackColors;
            this.BackColor = Class.Users.BackColors;
        }
        //public void usercheck(string s, string ss, string sss)
        //{

        //    DataTable dt1 = sm.headerdropdowns(s, ss, sss);
        //    if (dt1.Rows.Count > 0)
        //    {
        //        if (dt1.Rows[0]["Menuname"].ToString() == Class.Users.ScreenName)
        //        {
        //            //for (int r = 0; r < dt1.Rows.Count; r++)
        //            //{

        //            //    if (dt1.Rows[r]["NEWS"].ToString() == "T") { this.News.Visible = true; } else { this.News.Visible = false; }
        //            //    if (dt1.Rows[r]["SAVES"].ToString() == "T") { this.Saves.Visible = true; } else { this.Saves.Visible = false; }
        //            //    if (dt1.Rows[r]["PRINTS"].ToString() == "T") { this.Prints.Visible = true; } else { this.Prints.Visible = false; }
        //            //    if (dt1.Rows[r]["READONLY"].ToString() == "T") { this.Enabled = true; } else { this.Enabled = false; }
        //            //    if (dt1.Rows[r]["SEARCH"].ToString() == "T") { this.Searchs.Visible = true; } else { this.Searchs.Visible = false; }
        //            //    if (dt1.Rows[r]["DELETES"].ToString() == "T") { this.Deletes.Visible = true; } else { this.Deletes.Visible = false; }
        //            //    if (dt1.Rows[r]["TREEBUTTON"].ToString() == "T") { this.TreeButtons.Visible = true; } else { this.TreeButtons.Visible = false; }
        //            //    if (dt1.Rows[r]["GLOBALSEARCH"].ToString() == "T") { this.GlobalSearchs.Visible = true; } else { this.GlobalSearchs.Visible = false; }
        //            //    if (dt1.Rows[r]["LOGIN"].ToString() == "T") { this.Logins.Visible = true; } else { this.Logins.Visible = false; }
        //            //    if (dt1.Rows[r]["CHANGEPASSWORD"].ToString() == "T") { this.ChangePasswords.Visible = true; } else { this.ChangePasswords.Visible = false; }
        //            //    if (dt1.Rows[r]["CHANGESKIN"].ToString() == "T") { ChangeSkins.Visible = true; } else { ChangeSkins.Visible = false; }
        //            //    if (dt1.Rows[r]["DOWNLOAD"].ToString() == "T") { this.DownLoads.Visible = true; } else { this.DownLoads.Visible = false; }
        //            //    if (dt1.Rows[r]["Pdf"].ToString() == "T") { this.Pdfs.Visible = true; } else { this.Pdfs.Visible = false; }
        //            //    if (dt1.Rows[r]["Imports"].ToString() == "T") { this.Imports.Visible = true; } else { this.Imports.Visible = false; }
        //            //}
        //        }


        //    }
        //    else
        //    {

        //    }

        //}


        private void MachineMaster_Load(object sender, EventArgs e)
        {
            News();
        }

        
        public void Saves()
        {
            try
            {
                if (comboline.Text == "")
                {
                    MessageBox.Show("'MACHINE Name  is Empty'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); this.comboline.Focus();
                    return;
                }
               
                else
                {
                  
                    if (combocompcode.Text != null && comboline.Text != "")
                    {
                        txtfinyear.Text = System.DateTime.Now.Year.ToString();
                        string chk = "";
                        if (checkactive.Checked == true) { chk = "T"; } else { chk = "F"; checkactive.Checked = false; }
                        string sel = "select a.ASPTBLMACMASID    from  ASPTBLMACMAS a   WHERE finyear='" + txtfinyear.Text + "' and  compcode='" + combocompcode.SelectedValue + "' and  line='" + comboline.SelectedValue + "' and a.active='" + chk + "' and a.ASPTBLMACMASID='" + txtmachineid.Text + "'";
                        DataSet ds = Utility.ExecuteSelectQuery(sel, "ASPTBLMACMAS");
                        DataTable dt = ds.Tables["ASPTBLMACMAS"];
                         if (dt.Rows.Count != 0 && Convert.ToInt32("0" + txtmachineid.Text) == 0 || Convert.ToInt32("0" + txtmachineid.Text) == 0)
                        {
                            string ins = "insert into ASPTBLMACMAS(finyear,compcode,line,active,createdby,modifiedby,ipaddress,createdon)  VALUES('" + txtfinyear.Text + "','" + combocompcode.SelectedValue + "','" + comboline.SelectedValue + "','" + chk + "','" + Class.Users.HUserName + "','" + Class.Users.HUserName + "','" + Class.Users.IPADDRESS + "',date_format('" + Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd").Substring(0,10) + "','%Y-%m-%d') )";
                            Utility.ExecuteNonQuery(ins);
                            GridSave();
                            MessageBox.Show("Record Saved Successfully " + comboline.Text, " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GridLoad(); empty();
                        }
                        else
                        {
                            string up = "update  ASPTBLMACMAS  set finyear='" + txtfinyear.Text + "', compcode='" + combocompcode.SelectedValue + "',line='" + comboline.SelectedValue + "',active='" + chk + "' , modifiedby='" + Class.Users.HUserName + "',ipaddress='" + Class.Users.IPADDRESS + "' where ASPTBLMACMASID='" + txtmachineid.Text + "'";
                            Utility.ExecuteNonQuery(up);
                            GridSave();
                            MessageBox.Show("Record Updated Successfully " + comboline.Text, " Update Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GridLoad();
                            empty();
                        }
                       
                    }
                    else
                    {
                        MessageBox.Show("Invalid Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("compcode " + "        " + ex.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        void GridSave()
        {

            string sel1 = "SELECT MAX(A.asptblmacmasid) ID FROM  asptblmacmas A JOIN GTCOMPMAST B ON A.COMPCODE=B.GTCOMPMASTID WHERE A.COMPCODE=" + Class.Users.COMPCODE;
            DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptblmacmas");
          DataTable  dt = ds.Tables["asptblmacmas"];
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (txtmachineid.Text == "") { c1.Asptblmacmasid = Convert.ToInt64("0" + dt.Rows[0]["ID"].ToString()); c1.Asptblmacmas1id = Convert.ToInt64("0" + dt.Rows[0]["ID"].ToString()); }
                else { c1.Asptblmacmasid = Convert.ToInt64("0" + txtmachineid.Text); c1.Asptblmacmas1id = Convert.ToInt64("0" + txtmachineid.Text); }

                c1.Asptblmacdetid = Convert.ToInt64("0" + row.Cells["asptblmacdetid"].Value);
                c1.Machine = Convert.ToString(row.Cells["machine"].EditedFormattedValue.ToString().ToUpper().Trim());
                c1.Processname = Convert.ToInt64("0"+row.Cells["processname"].Value);               
                c1.Notes = Convert.ToString(row.Cells["notes"].Value);
                c1.Compcode =Convert.ToInt64(combocompcode.SelectedValue.ToString());
                if (c1.Machine != "")
                {

                     string sel2 = " SELECT b.asptblmacdetid  FROM  asptblmacmas  A JOIN asptblmacdet B ON A.asptblmacmasid=B.asptblmacmasid WHERE  B.asptblmacmas1id='" + c1.Asptblmacmas1id + "'  and B.machine='" + c1.Machine + "' AND B.processname=" + c1.Processname + "  and B.notes='" + c1.Notes + "' AND B.COMPCODE='" + c1.Compcode + "' AND B.asptblmacmasid=" + c1.Asptblmacmasid + " ";
                    DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "asptblmacdet");
                    DataTable dt2 = ds2.Tables["asptblmacdet"];
                    if (dt2.Rows.Count != 0) { }
                    else if (dt2.Rows.Count != 0 && c1.Asptblmacdetid == 0 || c1.Asptblmacdetid == 0)
                    {
                        c1 = new  Pinnacle.Models.LYLA.MachineModeldet(c1.Asptblmacmasid, c1.Machine, c1.Processname, c1.Notes, c1.Compcode, c1.Asptblmacmas1id);
                    }
                    else
                    {
                        c1 = new Pinnacle.Models.LYLA.MachineModeldet(c1.Asptblmacmasid, c1.Machine, c1.Processname, c1.Notes, c1.Compcode, c1.Asptblmacmas1id,c1.Asptblmacdetid);
                    }
                }
            }
        }

        private void MachineMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        public void Exit()
        {
            GlobalVariables.MdiPanel.Show();
            News();
            GlobalVariables.HeaderName.Text = "";
            GlobalVariables.TabCtrl.TabPages.RemoveAt(GlobalVariables.TabCtrl.SelectedIndex);
            this.Hide();
        }

        public void News()
        {

            GridLoad(); combocode(); empty();
        }
        private void empty()
        {
              Class.Users.UserTime = 0;
            txtmachineid.Text = "";
           combocompcode.Text = "";
            butheader.BackColor = Class.Users.BackColors;comboline.Text = "";
            panel4.BackColor = Class.Users.BackColors;
            panel5.BackColor = Class.Users.BackColors;
            this.BackColor = Class.Users.BackColors;
            this.Font = Class.Users.FontName; txtfinyear.Text = System.DateTime.Now.Year.ToString();
            listView1.Font = Class.Users.FontName;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Class.Users.BackColors;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;
            do
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++) { try { dataGridView1.Rows.RemoveAt(i); } catch (Exception) { } }
            }
            while (dataGridView1.Rows.Count > 1);
            txtsearch.Text = ""; combocompcode.Select();
        }
        private void combocode()
        {
            DataTable dt1 = mas.findcomcode(Class.Users.HCompcode);
            if (dt1.Rows.Count > 0)
            {
                combocompcode.DisplayMember = "COMPCODE";
                combocompcode.ValueMember = "gtcompmastid";
                combocompcode.DataSource = dt1;

            }

     

            string sel3 = "SELECT a.asptblbarpromasid,a.processname FROM asptblbarpromas a  where a.active='T' ORDER BY A.asptblbarpromasid desc";
            DataSet ds3 = Utility.ExecuteSelectQuery(sel3, "asptblbarpromas");
            DataTable dt3 = ds3.Tables[0];  
            if (dt3.Rows.Count > 0)
            {

              
                processname.DisplayMember = "processname";
                processname.ValueMember = "asptblbarpromasid";
                processname.DataSource = dt3;
            }

            string sel4 = "select a.asptbllinmasid,a.line from asptbllinmas a  where a.active='T' order by a.asptbllinmasid desc";
            DataSet ds4 = Utility.ExecuteSelectQuery(sel4, "asptbllinmas");
            DataTable dt4 = ds4.Tables[0];
            if (dt4.Rows.Count > 0)
            {

                comboline.DisplayMember = "line";
                comboline.ValueMember = "asptbllinmasid";
                comboline.DataSource = dt4;
            }
            //combocompcode.SelectedIndex = -1;
            //combocompcode1.SelectedIndex = -1;
        }
        public void GridLoad()
        {
            try
            {
                listView1.Items.Clear();listfilter.Items.Clear();
                string sel1 = "select a.ASPTBLMACMASID,a.finyear,c.compcode,e.line, a.active    from  ASPTBLMACMAS a    join gtcompmast c on c.gtcompmastid=a.compcode    join asptbllinmas e on e.asptbllinmasid=a.line and e.compcode=a.compcode order by 1";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "ASPTBLMACMAS");
                DataTable dt = ds.Tables["ASPTBLMACMAS"];
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.Text = i.ToString();
                        list.SubItems.Add(myRow["ASPTBLMACMASID"].ToString());
                        list.SubItems.Add(myRow["Finyear"].ToString());
                        list.SubItems.Add(myRow["CompCode"].ToString());                        
                        list.SubItems.Add(myRow["line"].ToString());                               
                        list.SubItems.Add(myRow["active"].ToString());
                      
                        list.BackColor = i % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                        listfilter.Items.Add((ListViewItem)list.Clone());
                        i++;
                        listView1.Items.Add(list);
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
                Class.Users.UserTime = 0;

                if (listView1.Items.Count > 0)
                {

                    txtmachineid.Text = listView1.SelectedItems[0].SubItems[1].Text;
                    string sel1 = " select a.ASPTBLMACMASID,a.finyear,c.compcode,e.line,  a.active    from  ASPTBLMACMAS a join gtcompmast c on c.gtcompmastid=a.compcode      join asptbllinmas e on e.asptbllinmasid=a.line and a.compcode=e.compcode   where a.ASPTBLMACMASID=" + txtmachineid.Text;
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "ASPTBLMACMAS");
                    DataTable dt = ds.Tables["ASPTBLMACMAS"];

                    if (dt.Rows.Count > 0)
                    {
                        txtmachineid.Text = Convert.ToString(dt.Rows[0]["ASPTBLMACMASID"].ToString());
                        txtfinyear.Text = Convert.ToString(dt.Rows[0]["finyear"].ToString());
                        combocompcode.Text = Convert.ToString(dt.Rows[0]["compcode"].ToString());
                      
                        comboline.Text = Convert.ToString(dt.Rows[0]["line"].ToString());
                        
                       
                        if (dt.Rows[0]["active"].ToString() == "T") { checkactive.Checked = true; } else { checkactive.Checked = true; checkactive.Checked = false; }

                        string sel2 = "select b.asptblmacdetid, a.asptblmacmasid, b.asptblmacmas1id,a.compcode,b.machine,c.asptblbarpromasid,b.notes    from  ASPTBLMACMAS a join asptblmacdet b on b.asptblmacmasid=a.asptblmacmasid join asptblbarpromas  c on c.asptblbarpromasid=b.processname WHERE A.asptblmacmasid='" + txtmachineid.Text + "' AND A.COMPCODE='" + Class.Users.COMPCODE + "'";
                        DataSet ds2 = Utility.ExecuteSelectQuery(sel2, "asptblmacdet");
                        DataTable dt2 = ds2.Tables["asptblmacdet"];
                        if (dt2.Rows.Count > 0)
                        {
                            
                            //dataGridView1.Rows.Clear();
                            dataGridView1.DataSource = dt2; int j = 1;
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                dataGridView1.Rows[i].Cells[0].Value = j;
                                j++;
                                // dataGridView1.Rows[i].Cells[0].Value = Convert.ToInt64("0" + dt2.Rows[i]["asptblmacdetid"].ToString());
                                //dataGridView1.Rows[i].Cells[2].Value = Convert.ToInt64("0" + dt2.Rows[i]["asptblmacmasid"].ToString());
                                //dataGridView1.Rows[i].Cells[3].Value = Convert.ToInt64("0" + dt2.Rows[i]["asptblmacmasid"].ToString());
                                //dataGridView1.Rows[i].Cells[4].Value = Convert.ToInt64("0" + dt2.Rows[i]["CompCode"].ToString());
                                //dataGridView1.Rows[i].Cells[5].Value = Convert.ToString(dt2.Rows[i]["machine"].ToString());
                                //dataGridView1.Rows[i].Cells[6].Value = Convert.ToInt64("0" + dt2.Rows[i]["processname"].ToString());
                                //dataGridView1.Rows[i].Cells[7].Value = Convert.ToString(dt2.Rows[i]["notes"].ToString());






                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear(); Class.Users.UserTime = 0;
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

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        public void Deletes()
        {
            if (txtmachineid.Text != "")
            {
                string sel1 = "select a.ASPTBLMACMASID from ASPTBLMACMAS a join ASPTBLBUYSAM b on a.ASPTBLMACMASID=b.MACHINE where a.ASPTBLMACMASID='" + txtmachineid.Text + "'";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "ASPTBLMACMAS");
                DataTable dt = ds.Tables["ASPTBLMACMAS"];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Child Record Found.Can Not Delete." + comboline.Text, " Alert Message ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                else
                {

                    string del = "delete from ASPTBLMACMAS where ASPTBLMACMASID='" + Convert.ToInt64("0" + txtmachineid.Text) + "'";
                    Utility.ExecuteNonQuery(del);
                    MessageBox.Show("Record Deleted Successfully " + comboline.Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void Searchs(int id)
        {
            throw new NotImplementedException();
        }

        public void Deletes(int id)
        {
            throw new NotImplementedException();
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name == "machine")
            {
                e.Control.KeyPress += Control_KeyPress;
            }
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar)  || char.IsWhiteSpace(e.KeyChar) ||  e.KeyChar == '-' || e.KeyChar == '(' | e.KeyChar == ')' || e.KeyChar == (char)Keys.Back);
        }
    }
}

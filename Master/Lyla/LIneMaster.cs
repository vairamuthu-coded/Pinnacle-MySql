using Pinnacle.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pinnacle.Master.Lyla
{
    public partial class LineMaster : Form,ToolStripAccess
    {
        private static LineMaster _instance;
        Models.Master mas = new Models.Master();
        Models.UserRights sm = new Models.UserRights();
        ListView listfilter = new ListView();
        Pinnacle.UserControls.UCCListView ucclist = new UCCListView();
        public static LineMaster Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LineMaster();
                GlobalVariables.CurrentForm = _instance;
                return _instance;
            }
        }
        public LineMaster()
        {
            InitializeComponent();
            Class.Users.IPADDRESS = GenFun.GetLocalIPAddress();
            Class.Users.CREATED = Convert.ToDateTime(System.DateTime.Now.ToString("dd-MM-yyyy") + " " + System.DateTime.Now.ToLongTimeString());

        }
 
        private void LineMaster_Load(object sender, EventArgs e)
        {
            txtteam.Select();
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
                if (txtline.Text != "" && Convert.ToInt64(combocompcode.SelectedValue) > 0 && Convert.ToInt64(combofloor.SelectedValue) > 0)
                {
                    //Models.Validate va = new Models.Validate();
                    //if (va.IsString(txtlinenumber.Text) == true)
                    //{

                        string chk = "";
                        if (checkactive.Checked == true) { chk = "T"; } else { chk = "F"; checkactive.Checked = false; }
                        string sel = "select asptbllinmasID    from  asptbllinmas    WHERE compcode='" + combocompcode.SelectedValue + "' and floor='" + combofloor.SelectedValue + "' and  team='" + txtteam.Text + "' and  linename='" + txtlinename.Text.ToUpper().Trim() + "' and  linenumber='" + txtlinenumber.Text.ToUpper().Trim() + "' and line='"+txtline.Text+"' and active='" + chk + "' and compcode='" + Class.Users.COMPCODE + "'";
                        DataSet ds = Utility.ExecuteSelectQuery(sel, "asptbllinmas");
                        DataTable dt = ds.Tables["asptbllinmas"];
                        if (dt.Rows.Count != 0)
                        {
                            MessageBox.Show("Child Record Found " + " Alert " + txtteam.Text, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); empty();
                        }
                        else if (dt.Rows.Count != 0 && Convert.ToInt32("0" + txtlinemasterid.Text) == 0 || Convert.ToInt32("0" + txtlinemasterid.Text) == 0)
                        {
                            string ins = "insert into asptbllinmas(compcode,floor, team,linename,linenumber,line, active,compcode,createdby,modifiedby,ipaddress,createdon)  VALUES('" + combocompcode.SelectedValue + "','" + combofloor.SelectedValue + "''" + txtteam.Text.ToUpper() + "','" + txtlinename.Text.ToUpper() + "','" + txtlinenumber.Text.ToUpper() + "','" + txtline.Text.ToUpper() + "','" + chk + "','" + Class.Users.COMPCODE + "','" + Class.Users.HUserName + "','" + Class.Users.HUserName + "','" + Class.Users.IPADDRESS + "',date_format('" + Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd")) + "','%Y-%m-%d') )";
                            Utility.ExecuteNonQuery(ins);
                            MessageBox.Show("Record Saved Successfully " + txtteam.Text, " Success Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GridLoad(); empty();
                        }
                        else
                        {
                            string up = "update  asptbllinmas  set  compcode='" + combocompcode.SelectedValue + "' , floor='" + combofloor.SelectedValue + "', team='" + txtteam.Text.ToUpper() + "' ,  linename='" + txtlinename.Text + "' ,  linenumber='" + txtlinenumber.Text + "' , line='" + txtline.Text + "', active='" + chk + "' ,compcode='" + Class.Users.COMPCODE + "',modifiedby='" + Class.Users.HUserName + "',ipaddress='" + Class.Users.IPADDRESS + "' where asptbllinmasID='" + txtlinemasterid.Text + "'";
                            Utility.ExecuteNonQuery(up);
                            MessageBox.Show("Record Updated Successfully " + txtteam.Text, " Update Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GridLoad();
                            empty();
                        }

                    //}
                    //else
                    //{
                    //    MessageBox.Show("'linenumber  is Wrong'  Empty not Allowed  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    //}
                }
                else
                {
                    MessageBox.Show("Invalid " + "        " + txtline.Text, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("linenumber " + "        " + ex.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void LineMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
        public void News()
        {
            GridLoad(); dataload();
            empty();
        }

        void dataload()
        {
            DataTable dt1 = mas.findcomcode(Class.Users.HCompcode);
            if (dt1.Rows.Count > 0)
            {
                combocompcode.DisplayMember = "COMPCODE";
                combocompcode.ValueMember = "gtcompmastid";
                combocompcode.DataSource = dt1;

            }

            string sel4 = "select a.asptblflomasid,a.floor from asptblflomas a  where a.active='T' order by a.asptblflomasid desc";
            DataSet ds4 = Utility.ExecuteSelectQuery(sel4, "asptblflomas");
            DataTable dt4 = ds4.Tables[0];
            if (dt4.Rows.Count > 0)
            {

                combofloor.DisplayMember = "floor";
                combofloor.ValueMember = "asptblflomasid";
                combofloor.DataSource = dt4;
            }
        }
        private void empty()
        {
            this.BackColor = Class.Users.BackColors;
            butheader.BackColor = Class.Users.BackColors;
            panel2.BackColor = Class.Users.BackColors;
            panel3.BackColor = Class.Users.BackColors;
            this.Font = Class.Users.FontName;
            listView1.Font = Class.Users.FontName;
            txtlinemasterid.Text = "";combocompcode.Text = ""; combofloor.Text = "";
            txtteam.Text = "";txtlinenumber.Text = "";txtlinename.Text="";txtline.Text = "";
            combocompcode.Select();
        }
        public void GridLoad()
        {
            try
            {
                listView1.Items.Clear(); listfilter.Items.Clear();
                string sel1 = "select a.asptbllinmasID, c.compcode,b.floor, a.team, a.linenumber ,a.linename,a.line, a.active  from  asptbllinmas a join asptblflomas  b on b.asptblflomasid=a.floor  join gtcompmast c on c.gtcompmastid=a.compcode   order by 1";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptbllinmas");
                DataTable dt = ds.Tables["asptbllinmas"];
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow myRow in dt.Rows)
                    {
                        ListViewItem list = new ListViewItem();
                        list.SubItems.Add(i.ToString());
                        list.SubItems.Add(myRow["asptbllinmasID"].ToString());
                        list.SubItems.Add(myRow["compcode"].ToString());
                        list.SubItems.Add(myRow["floor"].ToString());
                        list.SubItems.Add(myRow["team"].ToString());
                        list.SubItems.Add(myRow["linename"].ToString());
                        list.SubItems.Add(myRow["linenumber"].ToString());
                        list.SubItems.Add(myRow["line"].ToString());
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

                    txtlinemasterid.Text = listView1.SelectedItems[0].SubItems[2].Text;

                    string sel1 = "select a.asptbllinmasID, c.compcode,b.floor, a.team, a.linenumber ,a.linename,a.line, a.active  from  asptbllinmas a join asptblflomas  b on b.asptblflomasid=a.floor  join gtcompmast c on c.gtcompmastid=a.compcode where a.asptbllinmasID=" + txtlinemasterid.Text;
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptbllinmas");
                    DataTable dt = ds.Tables["asptbllinmas"];

                    if (dt.Rows.Count > 0)
                    {
                        txtlinemasterid.Text = Convert.ToString(dt.Rows[0]["asptbllinmasID"].ToString());
                        combocompcode.Text = Convert.ToString(dt.Rows[0]["compcode"].ToString());
                        combofloor.Text = Convert.ToString(dt.Rows[0]["floor"].ToString());
                        txtteam.Text = Convert.ToString(dt.Rows[0]["team"].ToString());
                        txtlinename.Text = Convert.ToString(dt.Rows[0]["linename"].ToString());
                        txtlinenumber.Text = Convert.ToString(dt.Rows[0]["linenumber"].ToString());
                        txtline.Text = Convert.ToString(dt.Rows[0]["line"].ToString());
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
                    string sel1 = "select a.asptbllinmasID, c.compcode,b.floor, a.team, a.linenumber ,a.linename,a.line, a.active  from  asptbllinmas a join asptblflomas  b on b.asptblflomasid=a.floor  join gtcompmast c on c.gtcompmastid=a.compcode  where a.linenumber LIKE'%" + txtsearch.Text.ToUpper() + "%' ";
                    DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptbllinmas");
                    DataTable dt = ds.Tables["asptbllinmas"];
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow myRow in dt.Rows)
                        {
                            ListViewItem list = new ListViewItem();
                            list.Text = iGLCount.ToString();
                            list.SubItems.Add(myRow["asptbllinmasID"].ToString());
                            list.SubItems.Add(myRow["compcode"].ToString());
                            list.SubItems.Add(myRow["floor"].ToString());
                            list.SubItems.Add(myRow["team"].ToString());
                            list.SubItems.Add(myRow["linename"].ToString());
                            list.SubItems.Add(myRow["linenumber"].ToString());
                            list.SubItems.Add(myRow["line"].ToString());
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
            if (txtlinemasterid.Text != "")
            {
                string sel1 = "select a.asptbllinmasID from asptbllinmas a join gtstatemast b on a.asptbllinmasID=b.linenumber where a.asptbllinmasID='" + txtlinemasterid.Text + "'";
                DataSet ds = Utility.ExecuteSelectQuery(sel1, "asptbllinmas");
                DataTable dt = ds.Tables["asptbllinmas"];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Child Record Found.Can Not Delete." + txtteam.Text, " Alert Message ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                else
                {

                    string del = "delete from asptbllinmas where asptbllinmasID='" + Convert.ToInt64("0" + txtlinemasterid.Text) + "'";
                    Utility.ExecuteNonQuery(del);
                    MessageBox.Show("Record Deleted Successfully " + txtteam.Text, " Delete Message ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            list.BackColor = item0 % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                            listView1.Items.Add(list);
                        }
                        item0++;
                    }
                }
                else
                {
                    try
                    {
                        listView1.Items.Clear(); item0 = 0;
                        foreach (ListViewItem item in listfilter.Items)
                        {
                            ListViewItem list = new ListViewItem();

                            list.Text = item.SubItems[0].Text;
                            list.SubItems.Add(item.SubItems[1].Text);
                            list.SubItems.Add(item.SubItems[2].Text);
                            list.SubItems.Add(item.SubItems[3].Text);
                            list.BackColor = item0 % 2 == 0 ? Class.Users.Color1 : Class.Users.Color2;

                            listView1.Items.Add(list);

                            item0++;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
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

        private void txtlinenumber_TextChanged(object sender, EventArgs e)
        {
            txtline.Text = combofloor.Text.ToUpper().Trim() + "-" + txtlinename.Text.ToUpper().Trim() + "-" + txtlinenumber.Text.ToUpper().Trim();
        }

        private void txtlinename_TextChanged(object sender, EventArgs e)
        {
            txtline.Text = combofloor.Text.ToUpper().Trim() + "-" + txtlinename.Text.ToUpper().Trim() + "-" + txtlinenumber.Text.ToUpper().Trim();
        }

        private void txtteam_TextChanged(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void txtlinename_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '-' || e.KeyChar == '(' || e.KeyChar == ')');

        }

        private void txtlinenumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '-' || e.KeyChar == '(' || e.KeyChar == ')');

        }

        private void combofloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtline.Text = combofloor.Text.ToUpper().Trim() + "-" + txtlinename.Text.ToUpper().Trim() + "-" + txtlinenumber.Text.ToUpper().Trim();

        }
    }
}

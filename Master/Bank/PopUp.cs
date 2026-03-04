
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
namespace Pinnacle.Master.Bank
{
    public partial class PopUp : Form
    {
        Models.Master mas = new Models.Master();
        public PopUp()
        {
            InitializeComponent();
            this.Font = Class.Users.FontName; 
            button1.ForeColor = Class.Users.Color2;
            button1.BackColor = Class.Users.BackColors;
            button1.Text = Class.Users.TableName;
            tabControl1_SelectedIndexChanged(tabControl1, EventArgs.Empty);
   
        }
        private void PopUp_FormClosing(object sender, FormClosingEventArgs e)
        {

            // axAcroPDF1.src = "D:\\temp2.pdf";

            string folderPath = Directory.GetCurrentDirectory();
            string baseName = $"temp1_{Class.Users.HUserName}_{Class.Users.PreParamid}";
            string[] files = Directory.GetFiles(folderPath, baseName + ".*");
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }

            }
            this.Hide();

        }

        private void PopUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            string folderPath = Directory.GetCurrentDirectory();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string folderPath = Directory.GetCurrentDirectory();
            string baseName = $"temp1_{Class.Users.HUserName}_{Class.Users.Paramid}";
            string[] files = Directory.GetFiles(folderPath, baseName + ".*");
            Class.Users.PreParamid=Class.Users.Paramid;
            string pdfPath = null;
            string imagePath = null;
            string excelPath = null;
            string wordPath = null;
            button1.Text = ""; button1.Refresh();
            foreach (string file in files)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".pdf")
                {
                    pdfPath = file;
                    
                }
                else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                {
                    imagePath = file;
                   
                }
                else if (ext == ".xls" || ext == ".xlsx")
                {
                    excelPath = file;
                    
                }

                else if (ext == ".doc" || ext == ".docx")
                {
                    wordPath = file;
                   
                }
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])
            {
                button1.Text = "Pdf -" + Class.Users.Paramid.ToString();
                if (!string.IsNullOrEmpty(pdfPath))
                {
                    axAcroPDF1.src = null;
                    axAcroPDF1.src = pdfPath;
                    
                }
                else
                {

                    mas.pop("Pdf file doesn't contain", Class.Users.Paramid.ToString() , "");
                }
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])
            {
                button1.Text = ""; button1.Refresh();
                button1.Text = "Image -" + Class.Users.Paramid.ToString();
                if (!string.IsNullOrEmpty(imagePath))
                {
                    pictureBox1.ImageLocation = imagePath;
                    
                }
                else
                {
                    mas.pop("Image file doesn't contain", Class.Users.Paramid.ToString(), "");
                }
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage3"])
            {
                button1.Text = "Excel -" + Class.Users.Paramid.ToString();
                if (!string.IsNullOrEmpty(excelPath))
                {
                    DataTable dt0;
                    string ext = Path.GetExtension(excelPath).ToLower();
                    if (ext==".xls")

                        dt0 = Class.Master.ReadExcel(excelPath,".xls");
                    else
                        dt0 = Class.Master.ImportExcelToDataTable(excelPath);

                    dataGridView1.DataSource = dt0;
                }
                else
                {
                    mas.pop("Excel file doesn't contain", Class.Users.Paramid.ToString(), "");
                }
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage4"])
            {
                button1.Text = "Word Document -" + Class.Users.Paramid.ToString();
                if (!string.IsNullOrEmpty(wordPath))
                {
                    richTextBox1.Text = Class.Master.LoadWordFile(wordPath);
                }
                else
                {
                    mas.pop("Word Document doesn't contain", Class.Users.Paramid.ToString(), "");
                }
            }

           


        }
    }
}

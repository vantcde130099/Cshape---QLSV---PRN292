using QLSV_Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRN292___LAB
{
    public partial class MainForm : Form
    {
        const string stringcnn = @"Data Source=DESKTOP-HKGIH5H;Initial Catalog=QLSV;Persist Security Info=True;User ID=sa;Password=123456";
        const string show_query = "select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop";
        
        public MainForm()
        {
            InitializeComponent();
            List<string> list_class = get_list_class();
            //box_class_search
            boxClass_show.Items.Add("All");
            foreach (string i in list_class)
            {
                boxClass_show.Items.Add(i);
                boxClass_info.Items.Add(i);
            }
            boxClass_info.SelectedItem = list_class[0];
            boxClass_show.SelectedItem = "All";
            //box_sort
            box_sort.Items.Add("Name");
            box_sort.Items.Add("Birthday");
            box_sort.Items.Add("Điểm trung bình");
            box_sort.SelectedItem="Name";
            
            
        }
               
        public List<string> get_list_class()
        {
            List<string> list_class = new List<string>();
            DBHelper db = new DBHelper(stringcnn);
            string query = "select IdLop ,NameLop from Lop";
            DataTable dt = db.getRecord(query);
            int count_rows = dt.Rows.Count;
            for(int i = 0; i < count_rows; i++)
            {
                list_class.Add(dt.Rows[i]["NameLop"].ToString());
            }
            return list_class;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string classs = boxClass_show.SelectedItem.ToString();
            string name = txtSearch.Text;

            DBHelper db = new DBHelper(stringcnn);
            string query ="";
            if (classs == "All" && name == "")
            {

                query += "select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop";

            }
            else if (classs == "All" && name != "")
            {
                query += "select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop where SV.SVName like '%" + name + "%'";
            }
            else if (classs != "All" && name == "")
            {
                query += "select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop where Lop.NameLop like '%" + classs + "%'";
            }
            else if (classs != "All" && name != "")
            {
                query += "select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop where Lop.NameLop like '%" + classs + "%' and SV.SVName like '%" + name + "%'";
            }
            View_Show.DataSource = db.getRecord(query);
        }   

        private void btAdd_Click(object sender, EventArgs e)
        {
            //get info
            string mssv = txtId.Text;
            if (mssv == "")
            {
                MessageBox.Show("Vui lòng nhập MSSV");
                return;
            }
            string name = txtName.Text;
            if (name == "")
            {
                MessageBox.Show("Vui lòng nhập tên");
                return;
            }
            string CheckClass = boxClass_info.Items[boxClass_info.SelectedIndex].ToString();
            int IDLop = 0;            
            DBHelper db = new DBHelper(stringcnn);
            string query = "select IdLop ,NameLop from Lop where NameLop = '"+ CheckClass + "'";
            DataTable dt = db.getRecord(query);
            int count_rows = dt.Rows.Count;
            for (int i = 0; i < count_rows; i++)
            {
                IDLop = Convert.ToInt32(dt.Rows[i]["IdLop"]);
            }            
            string txtdtb = txtTB.Text;
            if (txtdtb.ToString() == "")
            {
                MessageBox.Show("Vui lòng nhập điểm trung bình");
                return;
            }
            double dtb = Convert.ToDouble(txtTB.Text);
            DateTime date = datePick.Value.Date;
            int cmnd = 0;
            if (cbCMND.Checked)
                cmnd = 1;
            int hocba = 0;
            if (cbHB.Checked)
                hocba = 1;
            int thpt = 0;
            if (cbTHPT.Checked)
                thpt = 1;
            int gender = 0;
            if (radioMale.Checked)
                gender = 1;
            string tel = txtTel.Text;
            if (tel == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại");
                return;
            }
                        
            
            
            //check MSSV
            List<string> list_mssv = getMSSVs(db);
            foreach(string i in list_mssv)
            {
                if(i == mssv)
                {
                    MessageBox.Show("Trùng MSSV!, Vui lòng nhập mã khác");
                    return;
                }
            }
            //add db
            string querry = "insert into SV values('"+mssv+ "', '" + name + "', " + gender + ",'" + date + "', " + dtb + ", " + IDLop + ", '" + tel + "', " + cmnd + ", " + hocba + ", " + thpt + ")";
            db.InitDB(querry);
            MessageBox.Show("Add Succes");
            View_Show.DataSource = db.getRecord(show_query);
        }


        //Click row show info
        private void View_Show_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewSelectedRowCollection r = View_Show.SelectedRows;
            if(r.Count == 1)
            {
                string mssv = r[0].Cells["MSSV"].Value.ToString();
                string query = "select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop where MSSV = '" + mssv + "'";
                DBHelper db = new DBHelper(stringcnn);
                DataTable dt = db.getRecord(query);

                //MessageBox.Show(dt.Rows[0]["NameLop"].ToString);

                if (dt.Rows.Count == 1)
                {
                    txtId.Enabled = false;
                    txtId.Text = dt.Rows[0]["MSSV"].ToString();
                    txtName.Text = dt.Rows[0]["SVName"].ToString();
                    boxClass_info.SelectedItem = dt.Rows[0]["NameLop"].ToString();
                    txtTB.Text = dt.Rows[0]["DTB"].ToString();
                    string date = dt.Rows[0]["Birthday"].ToString();
                    datePick.Value = Convert.ToDateTime(date);
                    txtTel.Text = dt.Rows[0]["Tel"].ToString();
                    if (Convert.ToBoolean(dt.Rows[0]["HocBa"]) == true)
                        cbHB.Checked = true;
                    else
                        cbHB.Checked = false;
                    if (Convert.ToBoolean(dt.Rows[0]["CMND"]) == true)
                        cbCMND.Checked = true;
                    else
                        cbCMND.Checked = false;
                    if (Convert.ToBoolean(dt.Rows[0]["THPT"]) == true)
                        cbTHPT.Checked = true;
                    else
                        cbTHPT.Checked = false;
                    if (Convert.ToBoolean(dt.Rows[0]["Gender"]) == true)
                        radioMale.Checked = true;
                    else
                        radioFemale.Checked = true;
                }
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            //getinfo
            string mssv = txtId.Text;
            if (mssv == "")
            {
                MessageBox.Show("Vui lòng nhập MSSV");
                return;
            }
            string name = txtName.Text;
            if (name == "")
            {
                MessageBox.Show("Vui lòng nhập tên");
                return;
            }
            string CheckClass = boxClass_info.Items[boxClass_info.SelectedIndex].ToString();
            int IDLop = 1;
            switch (CheckClass)
            {
                case "SE1305":
                    IDLop = 1;
                    break;
                case "SE1302":
                    IDLop = 2;
                    break;
            }
            string txtdtb = txtTB.Text;
            if (txtdtb.ToString() == "")
            {
                MessageBox.Show("Vui lòng nhập điểm trung bình");
                return;
            }
            double dtb = Convert.ToDouble(txtTB.Text);
            DateTime date = datePick.Value.Date;
            int cmnd = 0;
            if (cbCMND.Checked)
                cmnd = 1;
            int hocba = 0;
            if (cbHB.Checked)
                hocba = 1;
            int thpt = 0;
            if (cbTHPT.Checked)
                thpt = 1;
            int gender = 0;
            if (radioMale.Checked)
                gender = 1;
            string tel = txtTel.Text;
            if (tel == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại");
                return;
            }
            //update db
            string query = "update SV set SVName ='" + name + "', Gender = " + gender + ",Birthday = '" + date + "',DTB = " + dtb + ", IDLop = " + IDLop + ", Tel= '" + tel + "', CMND = " + cmnd + ",HocBa = " + hocba + ", THPT = " + thpt + " where MSSV = '"+mssv+"'";
            
            DBHelper db = new DBHelper(stringcnn);
            db.InitDB(query);
            View_Show.DataSource = db.getRecord(show_query);
            MessageBox.Show("Update Success");
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            DBHelper db = new DBHelper(stringcnn);
            
            List<string> list_mssv = getMSSVs(db);            
            
            foreach(string i in list_mssv)
            {
                string query = "delete SV where MSSV = '" + i + "'";
                db.InitDB(query);
            }
            View_Show.DataSource = db.getRecord(show_query);
        }

        
        private void btSort_Click(object sender, EventArgs e)
        {
            string sortAction = box_sort.SelectedItem.ToString();
            DBHelper db = new DBHelper(stringcnn);            
            if (sortAction == "Name")
            {
                View_Show.DataSource = db.getRecord("select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop order by SV.SVName");
            }
            else if (sortAction == "Điểm trung bình")
            {
                View_Show.DataSource = db.getRecord("select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop order by SV.DTB");
            }
            else if (sortAction == "Birthday")
            {
                View_Show.DataSource = db.getRecord("select SV.MSSV, SV.SVName, Lop.NameLop, SV.Gender, SV.Birthday, SV.DTB, SV.Tel, SV.CMND, SV.HocBa, SV.THPT from SV inner join Lop on SV.IDLop = Lop.IDLop order by SV.Birthday");
            }
        }
        
        public List<string> getMSSVs(DBHelper db)
        {
            DataGridViewSelectedRowCollection r = View_Show.SelectedRows;
            List<string> list_mssv = new List<string>();
            if (r.Count >= 1)
            {
                for(int i = 0; i < r.Count; i++)
                {
                    list_mssv.Add(r[i].Cells["MSSV"].Value.ToString());
                }
            }

            return list_mssv;
        }
        
    }
    
}

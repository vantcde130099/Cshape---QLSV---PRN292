using QLSV___SQLServer;
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
        DBQLSVDataContext db = new DBQLSVDataContext();
        public MainForm()
        {
            InitializeComponent();

            var list_class = db.Lops.Select(l => l);
            

            //box_class_search
            boxClass_show.Items.Add("All");
            foreach (var i in list_class)
            {
                boxClass_show.Items.Add(i.NameLop);
                boxClass_info.Items.Add(i.NameLop);
            }
            boxClass_show.SelectedItem = "All";
            //box_sort
            box_sort.Items.Add("Name");
            box_sort.Items.Add("Birthday");
            box_sort.Items.Add("Điểm trung bình");
            box_sort.SelectedItem = "Name";
        }
               
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBQLSVDataContext db = new DBQLSVDataContext();
            //query
            var list = from p in db.SVs select new { p.MSSV, p.SVName, p.Lop.NameLop};

            string classs = boxClass_show.SelectedItem.ToString();
            string name = txtSearch.Text;

            if (classs == "All" && name == "")
            {
                View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
            }
            else if (classs == "All" && name != "")
            {
                View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND }).Where(p => p.SVName.Contains(name));
            }
            else if (classs != "All" && name == "")
            {
                View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND }).Where(p => p.NameLop == classs);
            }else if(classs != "All" && name != "")
            {
                View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND }).Where(p => p.NameLop == classs && p.SVName.Contains(name));
            }
        }
        private void btAdd_Click(object sender, EventArgs e)
        {
            DBQLSVDataContext db = new DBQLSVDataContext();
            bool gender = false;
            if (radioMale.Checked)
                gender = true;
            var list_class = db.Lops.Select(l => l).Where(p => p.NameLop == boxClass_info.SelectedItem.ToString());
            SV sv_add = new SV
            {
                MSSV = txtId.Text,
                Gender = gender,
                SVName = txtName.Text,
                IDLop = list_class.First().IDLop,
                Birthday = datePick.Value.Date,
                Tel = txtTel.Text,
                DTB = Convert.ToDouble(txtTB.Text),
                THPT = cbTHPT.Checked ? true:false,
                CMND = cbCMND.Checked ? true : false,
                HocBa = cbHB.Checked ? true : false
            };
            db.SVs.InsertOnSubmit(sv_add);
            db.SubmitChanges();
            View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
        }


        //Click row show info
        private void View_Show_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DBQLSVDataContext db = new DBQLSVDataContext();
            DataGridViewSelectedRowCollection r = View_Show.SelectedRows;
            if(r.Count == 1)
            {
                string mssv = r[0].Cells["MSSV"].Value.ToString();
                var sv = db.SVs.Where(p => p.MSSV == mssv).FirstOrDefault();

                txtId.Enabled = false;
                txtId.Text = sv.MSSV;
                txtName.Text = sv.SVName;
                boxClass_info.SelectedItem = sv.Lop.NameLop;
                txtTB.Text = sv.DTB.ToString();
                string date = sv.Birthday.ToString();
                datePick.Value = Convert.ToDateTime(date);
                txtTel.Text = sv.Tel;
                cbHB.Checked = sv.HocBa == true? true :false ;
                cbCMND.Checked = sv.CMND == true ? true : false;
                cbTHPT.Checked = sv.THPT == true ? true : false;
                radioMale.Checked = sv.Gender == true ? true : false;
                radioFemale.Checked = sv.Gender == false ? true :false;
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            DBQLSVDataContext db = new DBQLSVDataContext();
            string MSSV = txtId.Text;
            var sv_up = db.SVs.Where(p => p.MSSV == MSSV).FirstOrDefault();
            var list_class = db.Lops.Select(l => l).Where(p => p.NameLop == boxClass_info.SelectedItem.ToString());
            if (sv_up != null)
            {
                sv_up.Gender = radioMale.Checked ? true : false;
                sv_up.SVName = txtName.Text;
                sv_up.IDLop = list_class.First().IDLop;
                sv_up.Birthday = datePick.Value.Date;
                sv_up.Tel = txtTel.Text;
                sv_up.DTB = Convert.ToDouble(txtTB.Text);
                sv_up.THPT = cbTHPT.Checked ? true : false;
                sv_up.CMND = cbCMND.Checked ? true : false;
                sv_up.HocBa = cbHB.Checked ? true : false;
            }
            db.SubmitChanges();
            View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            DBQLSVDataContext db = new DBQLSVDataContext();
            DataGridViewSelectedRowCollection r = View_Show.SelectedRows;
            if (r.Count > 0)
            {
                foreach(DataGridViewRow i in r)
                {
                    string MSSV = null;
                    MSSV = i.Cells["MSSV"].Value.ToString();
                    if(MSSV == null)
                    {
                        MessageBox.Show("Not found");
                        return;
                    }
                    var sv_del = db.SVs.Where(p => p.MSSV == MSSV).FirstOrDefault();
                    db.SVs.DeleteOnSubmit(sv_del);
                    //Đồng bộ từ LINQ lên SQLServer
                    db.SubmitChanges();                    
                }
                View_Show.DataSource = db.SVs.Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
            }
        }

        
        private void btSort_Click(object sender, EventArgs e)
        {
            DBQLSVDataContext db = new DBQLSVDataContext();
            string sortAction = box_sort.SelectedItem.ToString();
            if(sortAction == "Name")
            {
                //this.View_Show.Sort(this.View_Show.Columns["SVName"], ListSortDirection.Ascending);
                View_Show.DataSource = db.SVs.OrderBy(p => p.SVName).Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
            }
            else if (sortAction == "Điểm trung bình")
            {
                //this.View_Show.Sort(this.View_Show.Columns["DTB"], ListSortDirection.Ascending);
                View_Show.DataSource = db.SVs.OrderBy(p => p.DTB).Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
            }
            else if (sortAction == "Birthday")
            {
                //this.View_Show.Sort(this.View_Show.Columns["Birthday"], ListSortDirection.Ascending);
                View_Show.DataSource = db.SVs.OrderBy(p => p.Birthday).Select(p => new { p.MSSV, p.SVName, p.Lop.NameLop, p.Birthday, p.Gender, p.DTB, p.Tel, p.THPT, p.HocBa, p.CMND });
            }
        }
    }
    
}

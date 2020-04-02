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
        List<SV> listSV { get; set; }
        DataTable dt { get; set; }
        public delegate bool MyCompare(object o1, object o2);
        public MainForm()
        {
            InitializeComponent();
            CreateDB();
            Loadccb();
            boxClass_show.SelectedItem = "All";
            boxClass_info.SelectedItem = "SE1305";
            box_sort.SelectedItem = "Name";
            printOut();
        }

        //Tạo dữ liêuj ban đầu
        public void CreateDB()
        {
            SV sv1 = new SV { MSSV = "DE130099", Class = "SE1305", Name = "Trần Công Văn", Birthday = new DateTime(1999, 06, 17), Gender = true, Tel = "079 885 0400", DTB = 9.9, CMND = true, HocBa = true, THPT = true };
            SV sv2 = new SV { MSSV = "DE130015", Class = "SE1305", Name = "Phạm Trung Nam", Birthday = new DateTime(1998, 06, 04), Gender = false, Tel = "079 885 0400", DTB = 1.9, CMND = false, HocBa = false, THPT = true };
            SV sv3 = new SV { MSSV = "DE130111", Class = "SE1305", Name = "Phạm Văn Phú Thịnh", Birthday = new DateTime(2002, 12, 11), Gender = true, Tel = "079 885 0400", DTB = 2.3, CMND = true, HocBa = true, THPT = false };
            SV sv4 = new SV { MSSV = "DE130992", Class = "SE1302", Name = "Nguyễn Hoàng Minh", Birthday = new DateTime(1990, 06, 13), Gender = false, Tel = "079 885 0400", DTB = 6.0, CMND = true, HocBa = true, THPT = false };
            SV sv5 = new SV { MSSV = "DE131232", Class = "SE1303", Name = "Trần Việt Long", Birthday = new DateTime(2000, 11, 01), Gender = true, Tel = "079 885 0400", DTB = 2.4, CMND = false, HocBa = false, THPT = true };

            listSV = new List<SV>();

            listSV.Add(sv1);
            listSV.Add(sv2);
            listSV.Add(sv3);
            listSV.Add(sv4);
            listSV.Add(sv5);
        }

        public void Loadccb()
        {
            boxClass_show.Items.Add("All");
            foreach (SV i in listSV)
            {
                if (boxClass_show.Items.IndexOf(i.Class) < 0)//Kiểm tra tồn tại
                {
                    boxClass_show.Items.Add(i.Class);
                }
            }

            foreach (SV i in listSV)
            {
                if (boxClass_info.Items.IndexOf(i.Class) < 0)//Kiểm tra tồn tại
                {
                    boxClass_info.Items.Add(i.Class);
                }
            }
            box_sort.Items.Add("Name");
            box_sort.Items.Add("Birthday");
            box_sort.Items.Add("Điểm trung bình");
        }

        public void sortAction(string sort)
        {
            if (sort == "Name")
            {
                //List<SV> SortedList = listSV.OrderBy(o => o.Name).ToList();
                //listSV.Clear();
                //listSV = SortedList;                
                SortUsingDelegates(listSV, SV.NameCompareTo);
            }
            else if (sort == "Điểm trung bình")
            {
                //List<SV> SortedList = listSV.OrderBy(o => o.DTB).ToList();
                //listSV.Clear();
                //listSV = SortedList;
                SortUsingDelegates(listSV, SV.DTBCompareTo);
            }
            else if (sort == "Birthday")
            {
                //List<SV> SortedList = listSV.OrderBy(o => o.Birthday).ToList();
                //listSV.Clear();
                //listSV = SortedList;
                SortUsingDelegates(listSV, SV.BithdayCompareTo);
            }
        }

        public DataTable GetListSV(string Name, string Class)
        {


            dt = new DataTable();

            dt.Columns.Add("MSSV", typeof(string));
            dt.Columns.Add("Class", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Birthday", typeof(DateTime));
            dt.Columns.Add("Gender", typeof(bool));
            dt.Columns.Add("Tel", typeof(string));
            dt.Columns.Add("DTB", typeof(double));
            dt.Columns.Add("CMND", typeof(bool));
            dt.Columns.Add("HocBa", typeof(bool));
            dt.Columns.Add("THPT", typeof(bool));

            List<SV> searchList = new List<SV>();
            if(Class == "All" && Name == "")
            {
                searchList = listSV;
            }
            else
            {
                foreach(SV s in listSV)
                {
                    if(Class == "All" && s.Name.IndexOf(Name) >= 0)
                    {
                        searchList.Add(s);
                    }else if(Class.IndexOf(s.Class) >= 0 && s.Name.IndexOf(Name) >= 0)
                    {
                        searchList.Add(s);
                    }
                }
            }
            AddDT(dt, searchList);
            return dt;
        }

        public void AddDT(DataTable dt, List<SV> searchList)
        {
            foreach (SV i in searchList)
            {
                DataRow row = dt.NewRow();
                row["MSSV"] = i.MSSV;
                row["Class"] = i.Class;
                row["Name"] = i.Name;
                row["Birthday"] = i.Birthday;
                row["Gender"] = i.Gender;
                row["Tel"] = i.Tel;
                row["DTB"] = i.DTB;
                row["CMND"] = i.CMND;
                row["HocBa"] = i.HocBa;
                row["THPT"] = i.THPT;
                dt.Rows.Add(row);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txtName = txtSearch.Text;
            string txtClass = boxClass_show.Items[boxClass_show.SelectedIndex].ToString();
            View_Show.DataSource = GetListSV(txtName, txtClass);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
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
            string Class = boxClass_info.Items[boxClass_info.SelectedIndex].ToString();
            string txtdtb = txtTB.Text;
            if (txtdtb.ToString() == "")
            {
                MessageBox.Show("Vui lòng nhập điểm trung bình");
                return;
            }
            double dtb = Convert.ToDouble(txtTB.Text);
            DateTime date = datePick.Value.Date;
            bool cmnd = cbCMND.Checked;
            bool hocba = cbHB.Checked;
            bool thpt = cbTHPT.Checked;
            bool gender = false;
            if (radioMale.Checked)
                gender = true;
            string tel = txtTel.Text;
            if (tel == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại");
                return;
            }
            SV newSv = new SV { MSSV = mssv, Name = name, Class = Class, DTB = dtb, Birthday = date, CMND = cmnd, HocBa = hocba, THPT = thpt, Tel = tel, Gender = gender };
            //
            //kiểm tra trùng
            bool check = false;
            foreach (SV i in listSV)
            {
                if (mssv == i.MSSV)
                    check = true;
            }

            if (check == true)
            {
                MessageBox.Show("Trùng rồi!");

            }
            else
            {
                listSV.Add(newSv);
                MessageBox.Show("Đã thêm Sinh viên " + name);
            }

            //print
            printOut();
        }


        //Click row show info
        private void View_Show_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (View_Show.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                txtId.Enabled = false;
                txtId.Text = View_Show.Rows[e.RowIndex].Cells["MSSV"].FormattedValue.ToString();
                txtName.Text = View_Show.Rows[e.RowIndex].Cells["Name"].FormattedValue.ToString();
                boxClass_info.SelectedItem = View_Show.Rows[e.RowIndex].Cells["Class"].FormattedValue.ToString();
                txtTB.Text = View_Show.Rows[e.RowIndex].Cells["DTB"].FormattedValue.ToString();
                string date = View_Show.Rows[e.RowIndex].Cells["Birthday"].FormattedValue.ToString();
                datePick.Value = Convert.ToDateTime(date);
                txtTel.Text = View_Show.Rows[e.RowIndex].Cells["Tel"].FormattedValue.ToString();
                //checkbox
                if (Convert.ToBoolean(View_Show.Rows[e.RowIndex].Cells["HocBa"].Value) == true)
                    cbHB.Checked = true;
                else
                    cbHB.Checked = false;
                if (Convert.ToBoolean(View_Show.Rows[e.RowIndex].Cells["THPT"].Value) == true)
                    cbTHPT.Checked = true;
                else
                    cbTHPT.Checked = false;
                if (Convert.ToBoolean(View_Show.Rows[e.RowIndex].Cells["CMND"].Value) == true)
                    cbCMND.Checked = true;
                else
                    cbCMND.Checked = false;
                //radio button
                if (Convert.ToBoolean(View_Show.Rows[e.RowIndex].Cells["Gender"].Value) == true)
                    radioMale.Checked = true;
                else
                    radioFemale.Checked = true;
            }

        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
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
            string Class = boxClass_info.Items[boxClass_info.SelectedIndex].ToString();
            string txtdtb = txtTB.Text;
            if (txtdtb.ToString() == "")
            {
                MessageBox.Show("Vui lòng nhập điểm trung bình");
                return;
            }
            double dtb = Convert.ToDouble(txtTB.Text);
            DateTime date = datePick.Value.Date;
            bool cmnd = cbCMND.Checked;
            bool hocba = cbHB.Checked;
            bool thpt = cbTHPT.Checked;
            bool gender = false;
            if (radioMale.Checked)
                gender = true;
            string tel = txtTel.Text;
            if (tel == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại");
                return;
            }
            for (int i = 0; i < listSV.Count; i++)
            {
                if (listSV[i].MSSV.Equals(mssv))
                {
                    listSV[i].Name = name;
                    listSV[i].Class = Class;
                    listSV[i].Birthday = date;
                    listSV[i].DTB = dtb;
                    listSV[i].HocBa = hocba;
                    listSV[i].THPT = thpt;
                    listSV[i].Tel = tel;
                    listSV[i].Gender = gender;
                    listSV[i].CMND = cmnd;
                    MessageBox.Show("Đã cập nhập thông tin cho sinh viên có MSSV: " + listSV[i].MSSV);

                    printOut();
                    return;
                }
            }

        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            //SINGLE DELETE
            /*string mssv = View_Show.Rows[View_Show.CurrentCell.RowIndex].Cells["MSSV"].FormattedValue.ToString();
           
            foreach (SV s in listSV)
            {
                if (s.MSSV.Equals(mssv))
                {
                    listSV.Remove(s);
                    MessageBox.Show("Đã xóa sinh viên có MSSV: " + s.MSSV);
                    printOut();
                    return;
                }
            }*/

            //MULTI DELETE
            DataGridViewSelectedRowCollection rowArr = View_Show.SelectedRows;
            if(rowArr.Count >= 1)
            {
                List<string> MSSV_del = new List<string>();
                foreach(DataGridViewRow r in rowArr)
                {
                    MSSV_del.Add(r.Cells["MSSV"].Value.ToString());
                }
                Del(MSSV_del);
                //print out
                printOut();
            }

        }

        public void Del(List<string> MSSV_del)
        {
            foreach(string r in MSSV_del)
            {
                for(int i = 0; i < listSV.Count; i++)
                {
                    if(r == listSV[i].MSSV)
                    {
                        listSV.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void btSort_Click(object sender, EventArgs e)
        {

            string ccbSort = box_sort.Items[box_sort.SelectedIndex].ToString();
            if (ccbSort.Equals(null))
            {
                MessageBox.Show("Vui lòng chọn cách sắp xếp");
                return;
            }
            sortAction(ccbSort);

            //reload
            printOut();
        }
        public void printOut()
        {
            string txtNamePrint = txtSearch.Text;
            string txtClass = boxClass_show.Items[boxClass_show.SelectedIndex].ToString();
            View_Show.DataSource = GetListSV(txtNamePrint, txtClass);
        }

        public static void SortUsingDelegates(List<SV> listSV, MyCompare cmp)
        {
            int n = listSV.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n - i - 2; j++)
                {
                    if (cmp(listSV[j], listSV[j + 1]))
                    {
                        SV temp = listSV[j];
                        listSV[j] = listSV[j + 1];
                        listSV[j + 1] = temp;
                    }
                }
            }
        }
    }
    
}

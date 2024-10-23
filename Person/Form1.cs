using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2013.Word;
using Person.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Person
{
    public partial class Form1 : Form
    {
        private DbContext1 dbContext;
        public BindingList<Student> students;
        private int selectedStudentId = -1;
        private int currentIndex = -1; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbContext = new DbContext1(); 
            LoadStudents();  
        }
        private void LoadStudents()
        {
            var studentList = dbContext.Students.ToList();
            students = new BindingList<Student>(studentList);
            dgvSinhVien.DataSource = students;

            if (students.Count > 0)
            {
                currentIndex = 0;
                DisplayCurrentStudent();
            }
        }
        private void DisplayCurrentStudent()
        {
            if (currentIndex >= 0 && currentIndex < students.Count)
            {
                var currentStudent = students[currentIndex];
                txtHoTen.Text = currentStudent.FullNAME;
                txtTuoi.Text = currentStudent.Age.ToString();
                cmbKhoa.SelectedItem = currentStudent.Major;
            }
        }

        private bool CheckIdSinhVien(string idNewStudent)
        {
            int length = dgvSinhVien.Rows.Count;
            for (int i = 0; i < length; i++)
            {
                if (dgvSinhVien.Rows[i].Cells[0].Value != null)
                {
                    if (dgvSinhVien.Rows[i].Cells[0].Value.ToString() == idNewStudent)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private void AddStudent(string fullName, int age, string major)
        {
            var newStudent = new Student
            {
                FullNAME= fullName,
                Age = age,
                Major = major
            };

            dbContext.Students.Add(newStudent);
            dbContext.SaveChanges();
            students.Add(newStudent); 
        }

      
        private void btnThem_Click(object sender, EventArgs e)
        {
            string fullName = txtHoTen.Text;  
            int age = int.Parse(txtTuoi.Text);     
            string major = cmbKhoa.SelectedItem.ToString(); 
            AddStudent(fullName, age, major);
            txtHoTen.Clear();
            txtTuoi.Clear();
            cmbKhoa.SelectedIndex = 0;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out string fullName, out int age, out string major))
            {
                return;
            }

            if (selectedStudentId == -1)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var studentToEdit = dbContext.Students.FirstOrDefault(s => s.Studentid == selectedStudentId);

            if (studentToEdit != null)
            {
                studentToEdit.FullNAME = fullName;
                studentToEdit.Age = age;
                studentToEdit.Major = major;

                dbContext.SaveChanges();
                LoadStudents(); 

                MessageBox.Show("Thông tin sinh viên đã được cập nhật!");
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ValidateInput(out string fullName, out int age, out string major)
        {
            fullName = txtHoTen.Text.Trim();
            major = cmbKhoa.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                age = 0;
                return false;
            }

            if (!int.TryParse(txtTuoi.Text, out age) || age <= 0)
            {
                MessageBox.Show("Vui lòng nhập tuổi hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(major))
            {
                MessageBox.Show("Vui lòng chọn khoa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }



        private void btnXoa_Click(object sender, EventArgs e)
        {
            string fullName = txtHoTen.Text.Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var studentToDelete = dbContext.Students.FirstOrDefault(s => s.FullNAME == fullName);

            if (studentToDelete != null)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    dbContext.Students.Remove(studentToDelete);
                    dbContext.SaveChanges();
                    LoadStudents(); 
                    MessageBox.Show("Sinh viên đã được xóa thành công!");
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên với tên đã nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvsv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgvSinhVien.Rows[e.RowIndex].DataBoundItem as Student;

                if (selectedRow != null)
                {
                    selectedStudentId = selectedRow.Studentid;
                    txtHoTen.Text = selectedRow.FullNAME;
                    txtTuoi.Text = selectedRow.Age.ToString();
                    cmbKhoa.SelectedItem = selectedRow.Major;
                }
            }
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            if (currentIndex < students.Count - 1)
            {
                currentIndex++;
                DisplayCurrentStudent();
            }
            else
            {
                MessageBox.Show("Đã đến sinh viên cuối cùng.", "Thông báo");
            }
        }

        private void btnp_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayCurrentStudent();
            }
            else
            {
                MessageBox.Show("Đã đến sinh viên đầu tiên.", "Thông báo");
            }
        }
    }
}

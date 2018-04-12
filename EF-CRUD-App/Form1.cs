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

namespace EF_CRUD_App
{
    public partial class Form1 : Form
    {
        Employee model = new Employee();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();

        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtGender.Text = txtAge.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.EmployeeID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();



            using (DBEntities db = new DBEntities())
            {
                if (model.EmployeeID == 0) //Insert
                    db.Employees.Add(model);
                else //Update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }

            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted Succesfully");
        }

        void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvCustomer.DataSource = db.Employees.ToList<Employee>();
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.EmployeeID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["EmployeeID"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.Employees.Where(x => x.EmployeeID == model.EmployeeID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;


                }



                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to DELETE this record?", "EF_CRUD_App CRUD operation",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Employees.Attach(model);
                    db.Employees.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }

        private void dgvCustomer_ReadOnlyChanged(object sender, EventArgs e)
        {

        }
    }
}


using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DairyFarm
{
    public partial class Login : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-Q05C0DKC\SQLEXPRESS01;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        public Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnameTb.Text = "";
            PasswordTb.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (RoleCb.SelectedIndex == -1)
            {
                MessageBox.Show("Select Role");
                return; // added return to exit the method if Role is not selected
            }

            if (PasswordTb.Text == "" || UnameTb.Text == "")
            {
                MessageBox.Show("Enter Admin Name and Password");
                return; // added return to exit the method if username or password is empty
            }

            if (RoleCb.SelectedItem.ToString() == "Admin")
            {
                if(UnameTb.Text == "Admin" && PasswordTb.Text == "Admin")
                {
                    Employees emp = new Employees();
                    emp.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("If You are the Admin, Enter the Correct Username and Password");

                }

            }
            else if (RoleCb.SelectedItem.ToString() == "Employee") // fixed the condition
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from EmployeeTbl where EmpName= '" + UnameTb.Text + "' and EmpPass = '" + PasswordTb.Text + "' ", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Con.Close(); // moved Con.Close() outside of if-else block

                if (dt.Rows[0][0].ToString() == "1")
                {
                    Cows cow = new Cows();
                    cow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Wrong Username or Password");
                }
            }
            else
            {
                MessageBox.Show("Invalid Role");
            }
        }
    }
}

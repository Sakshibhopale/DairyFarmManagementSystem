using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DairyFarm
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
            Finance();
            Logistics();
            GetMax();
        }

        

        private void label12_Click(object sender, EventArgs e)
        {
            Cows Ob = new Cows();
            Ob.Show();
            this.Hide();
        }
        private void label13_Click(object sender, EventArgs e)
        {
            MilkProduction Ob = new MilkProduction();
            Ob.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CowHealth Ob = new CowHealth();
            Ob.Show();
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            Breed Ob = new Breed();
            Ob.Show();
            this.Hide();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            MilkSales Ob = new MilkSales();
            Ob.Show();
            this.Hide();
        }

        private void label21_Click(object sender, EventArgs e)
        {
            Finance Ob = new Finance();
            Ob.Show();
            this.Hide();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-Q05C0DKC\SQLEXPRESS01;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        private void Finance()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(IncAmt) from IncomeTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            int inc, exp;
            double bal;
            inc = Convert.ToInt32(dt.Rows[0][0].ToString());
            
            IncLbl.Text = "Rs " + dt.Rows[0][0].ToString();

            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(ExpAmount) from ExpenditureTbl", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            exp = Convert.ToInt32(dt1.Rows[0][0].ToString());
            bal = inc - exp;
            ExpLbl.Text = "Rs " + dt1.Rows[0][0].ToString();
            BalLbl.Text = "Rs " + bal;
            Con.Close();   
        }
        private void Logistics()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from CowTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            CownumLbl.Text = dt.Rows[0][0].ToString();

            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(TotalMilk) from MilkTbl", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            MilkLbl.Text =  dt1.Rows[0][0].ToString() + " Litters";

            SqlDataAdapter sda2 = new SqlDataAdapter("select count(*) from EmployeeTbl", Con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            EmpNumLbl.Text = dt2.Rows[0][0].ToString();
            Con.Close();
        }
        
        private void GetMax()
        {
            Con.Open();

            // Get the maximum income amount and its date
            SqlDataAdapter sda = new SqlDataAdapter("select Top 1 IncAmt, IncDate from IncomeTbl order by IncAmt desc", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            // Display the maximum income amount and its date
            HighAmtLbl.Text = "Rs " + dt.Rows[0]["IncAmt"].ToString();
            HighDateLbl.Text = dt.Rows[0]["IncDate"].ToString();
             
            //----------------------------------------

            SqlDataAdapter sda1 = new SqlDataAdapter("select Top 1 ExpAmount, ExpDate from ExpenditureTbl order by ExpAmount desc", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);

            // Display the maximum expenditure amount and its date
            HighExpLbl.Text = "Rs " + dt1.Rows[0]["ExpAmount"].ToString();
            HighExpDate.Text = dt1.Rows[0]["ExpDate"].ToString();

            Con.Close();
        }



        private void DashBoard_Load(object sender, EventArgs e)
        {

        }
    }
}

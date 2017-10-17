using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.Migrations;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public MainWindow()
        {
            InitializeComponent();
            comboBox.Items.Add("Заключен");
            comboBox.Items.Add("Расторгнут");
            comboBox.Items.Add("Еще не заключен");

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != null && textBox2.Text != null && textBox3.Text != null && textBox4.Text != null)
            {
                using (CompAndUsersEntities context = new CompAndUsersEntities())
                {
                    Users user = new Users();
                    user.Name = textBox.Text;
                    user.Login = textBox2.Text;
                    user.Password = textBox3.Text;
                    user.CompanyID = Convert.ToInt32(textBox4.Text);
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != null)
            {
                using (CompAndUsersEntities context = new CompAndUsersEntities())
                {
                    int deleteId = Convert.ToInt32(textBox1.Text);
                    Users removeUser = context.Users.FirstOrDefault(r => r.Id == deleteId);
                    if (removeUser != null)
                    {
                        context.Users.Remove(removeUser);
                        context.SaveChanges();
                    }
                }
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (textBox6.Text != null && comboBox.SelectedItem != null)
            {
                using (CompAndUsersEntities context = new CompAndUsersEntities())
                {
                    Companies company = new Companies();
                    company.Name = textBox6.Text;
                    company.ContractStatus = comboBox.SelectedIndex;
                    context.Companies.Add(company);
                    context.SaveChanges();
                }
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (textBox5.Text != null)
            {
                using (CompAndUsersEntities context = new CompAndUsersEntities())
                {
                    int deleteId = Convert.ToInt32(textBox5.Text);
                    Companies removeCompany = context.Companies.FirstOrDefault(r => r.Id == deleteId);
                    if (removeCompany != null)
                    {
                        context.Companies.Remove(removeCompany);
                        var removeUsers = context.Users.Where(u => u.CompanyID == deleteId);
                        foreach (Users removeUser in removeUsers)
                        {
                            context.Users.Remove(removeUser);
                        }
                        context.SaveChanges();
                    }
                }
                showInfo();
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            showInfo();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            string index = listBox.SelectedItem.ToString();
            listBox1.Items.Clear();
            using (CompAndUsersEntities context = new CompAndUsersEntities())
            {
                Companies company = context.Companies.FirstOrDefault(c => c.Name == index);
                label.Content = "Company ID " + company.Id;
                ContractStatus cs = (ContractStatus)company.ContractStatus;
                label2.Content = "Contract " + cs.ToString();
                var users = context.Users.Where(u => u.CompanyID == company.Id);
                foreach(Users user in users)
                {
                    listBox1.Items.Add(user.Id + " " + user.Name + " " + user.Login + " " + user.Password);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            using (CompAndUsersEntities context = new CompAndUsersEntities())
            {
                int id = Convert.ToInt32(textBox4.Text);
                if (textBox.Text != null && textBox2.Text != null && textBox3.Text != null && textBox4.Text != null
                && textBox4.Text != null && context.Companies.FirstOrDefault(c => c.Id == id) != null)
                {
                    Users user = new Users();
                    user.Name = textBox.Text;
                    user.Login = textBox2.Text;
                    user.Password = textBox3.Text;
                    user.CompanyID = id;
                    user.Id = Convert.ToInt32(textBox1.Text);
                    context.Users.AddOrUpdate(user);
                    context.SaveChanges();
                }
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (textBox6.Text != null && textBox5.Text != null && comboBox.SelectedItem != null)
            {
                using (CompAndUsersEntities context = new CompAndUsersEntities())
                {
                    Companies company = new Companies();
                    company.Name = textBox6.Text;
                    company.ContractStatus = (int)comboBox.SelectedIndex;
                    company.Id = Convert.ToInt32(textBox5.Text);
                    context.Companies.AddOrUpdate(company);
                    context.SaveChanges();
                }
            }
            showInfo();
        }


        private void showInfo()
        {
            listBox.Items.Clear();
            using (CompAndUsersEntities context = new CompAndUsersEntities())
            {
                foreach (Companies company in context.Companies)
                {
                    listBox.Items.Add(company.Name);
                }
            }
        }
    }
}

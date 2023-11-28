using firsttime.Data;
using firsttime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace firsttime
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private AppDbContext _db;
        public RegisterWindow()
        {
            InitializeComponent();
            _db = new AppDbContext(); // создание базы данных
        }

        private void UserRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLoginField.Text.Trim();
            string email = UserEmailField.Text.Trim();
            string password = UserPasswordField.Password.Trim();
            if (login.Equals("") || !email.Contains("@") || password.Length < 3)
            {
                MessageBox.Show("Вы что то ввели неверно");
                return;
            }

            User authUser = _db.Users.Where(el => el.Login == login).FirstOrDefault(); // значит пользователь уже есть в бд
            if (authUser != null)
            {
                MessageBox.Show("Такой пользователь уже есть");
                return;
            }

            User user = new User(login, email, Hash(password));
            _db.Users.Add(user);
            _db.SaveChanges();

            UserLoginField.Text = "";
            UserEmailField.Text = "";
            UserPasswordField.Password = "";
            UserRegisterButton.Content = "Готово";

        }

        private string Hash(string input)
        {
            byte[] temp = Encoding.UTF8.GetBytes(input);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(temp);
                return Convert.ToBase64String(hash);
            }
        }

        private void GoToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AuthWindow window = new AuthWindow();
            window.Show();
            Close();
        }
    }
}

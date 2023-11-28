using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using firsttime.Data;
using firsttime.Models;

namespace firsttime
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            if (File.Exists("usersave.xml"))
                ShowMainWindow();
        }

        private void UserAuthButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLoginField.Text.Trim();
            string password = UserPasswordField.Password.Trim();

            if (login.Equals(""))
            {
                MessageBox.Show("Неверный логин");
                return;
            } else if (password.Length < 3)
            {
                MessageBox.Show("Неверный пароль");
            }

            User authUser = null;
            using (AppDbContext db = new AppDbContext()) {
                authUser = db.Users.Where(user => user.Login == login && user.Password == Hash(password)).FirstOrDefault();
            }

            if (authUser == null)
                MessageBox.Show("Пользователь не найден");
            else
            {
                AuthUser auth = new AuthUser(authUser.Login, authUser.Email);
                XmlSerializer xml = new XmlSerializer(typeof(AuthUser));
                using (FileStream file = new FileStream("usersave.xml", FileMode.CreateNew))
                {
                    xml.Serialize(file, auth);
                }

                ShowMainWindow();
            }
        }

        private void ShowMainWindow()
        {
            Hide();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
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

        private void GoToRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            RegisterWindow window = new RegisterWindow();
            window.Show();
            Close();
        }
    }
}

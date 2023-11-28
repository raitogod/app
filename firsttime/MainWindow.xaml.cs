using firsttime.Data;
using firsttime.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;
using CloudIpspSDK;
using CloudIpspSDK.Checkout;
using CloudIpspSDK.Response;
using CloudIpspSDK.Models;

namespace firsttime
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _db;

        public ObservableCollection<Product> shopCart { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _db = new AppDbContext();
            shopCart = new ObservableCollection<Product>();
            BuyAll.Visibility = Visibility.Hidden;
            LoadProducts("All");
            MainScreen.Visibility = Visibility.Hidden;


            if (!File.Exists("usersave.xml"))

                ShowAuthWindow();

            XmlSerializer xml = new XmlSerializer(typeof(AuthUser));
            using (FileStream file = new FileStream("usersave.xml", FileMode.Open))
            {
                AuthUser auth = (AuthUser)xml.Deserialize(file);
                UserNameLabel.Content = auth.Login;
                UserEmailInfo.Content = "Ваш email : " +  auth.Email;
                UserNameInfo.Content = "Ваш логин : " + auth.Login;
            }
        }


        private void ShowAuthWindow()
        {
            Hide();
            AuthWindow window = new AuthWindow();
            window.Show();
            Close();
        }

        private void LoadProducts(string catergoryname)
        {
            switch (catergoryname)
            {
                case "All":
                    using (var db = new AppDbContext())
                    {
                        var products = db.Products.ToList();
                        productListView.ItemsSource = products;
                    }
                    break;
                case "Phones":
                    using (var db = new AppDbContext())
                    {
                        var products = db.Products.Where(el => el.CategoryID == 2).ToList();
                        productListView.ItemsSource = products;
                    }
                    break;
                case "Tablets":
                    using (var db = new AppDbContext())
                    {
                        var products = db.Products.Where(el => el.CategoryID == 1).ToList();
                        productListView.ItemsSource = products;
                    }
                    break;
                case "HeadSets":
                    using (var db = new AppDbContext())
                    {
                        var products = db.Products.Where(el => el.CategoryID == 3).ToList();
                        productListView.ItemsSource = products;
                    }
                    break;

            }

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            File.Delete("usersave.xml");
            ShowAuthWindow();
        }



        private void RefreshCartView()
        {
            CartListBox.ItemsSource = shopCart;


            if (shopCart.Any())
            {
                int totalPrice = shopCart.Sum(item => item.Price);
                CartInfo.Text = $"Общая стоимость: {totalPrice:C}";
                BuyAll.Visibility = Visibility.Visible;
            }
            else
            {
                CartInfo.Text = "Корзина пуста";
                BuyAll.Visibility = Visibility.Hidden;
            }
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Visibility = Visibility.Hidden;
            CartScreen.Visibility = Visibility.Visible;
            AboutUsScreen.Visibility = Visibility.Hidden;
            CabinetScreen.Visibility = Visibility.Hidden;
            BackProductScreen.Visibility = Visibility.Hidden;
            HelpScreen.Visibility = Visibility.Hidden;
            StartScreen.Visibility = Visibility.Hidden;
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button addButton && addButton.Tag is Product product)
            {
                shopCart.Add(product);
            }
            RefreshCartView();
        }

        private void DeleteFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is Product product)
            {
                shopCart.Remove(product);
                RefreshCartView();

                if (!shopCart.Any())
                {
                    CartInfo.Text = "Корзина пуста";
                }
            }
        }


        private void MenuChekedButton(object sender, RoutedEventArgs e)
        {
            string objName = ((RadioButton)sender).Name;

            StackPanel[] panels = { MainScreen, CartScreen, AboutUsScreen, CabinetScreen, BackProductScreen, HelpScreen };
            foreach (var pan in panels)
                pan.Visibility = Visibility.Hidden;

            switch (objName)
            {
                case "MainScreenView":
                    MainScreen.Visibility = Visibility.Visible;
                    CartScreen.Visibility = Visibility.Hidden;
                    StartScreen.Visibility = Visibility.Hidden;
                    break;
                case "AboutUsScreenView":
                    AboutUsScreen.Visibility = Visibility.Visible;
                    CartScreen.Visibility = Visibility.Hidden;
                    StartScreen.Visibility = Visibility.Hidden;
                    break;
                case "CabinetScreenView":
                    CabinetScreen.Visibility = Visibility.Visible;                   
                    CartScreen.Visibility = Visibility.Hidden;
                    StartScreen.Visibility = Visibility.Hidden;
                    break;
                case "BackProductScreenView":
                    BackProductScreen.Visibility = Visibility.Visible;
                    CartScreen.Visibility = Visibility.Hidden;
                    StartScreen.Visibility = Visibility.Hidden;
                    break;
                case "HelpScreenView":
                    HelpScreen.Visibility = Visibility.Visible;
                    CartScreen.Visibility = Visibility.Hidden;
                    StartScreen.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void AllProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts("All");
        }

        private void Phones_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts("Phones");
        }

        private void Tablets_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts("Tablets");
        }

        private void HeadSets_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts("HeadSets");
        }

        private decimal CalculateTotalPrice()
        {
            return shopCart.Sum(item => item.Price);
        }

        private void BuyAll_Click(object sender, RoutedEventArgs e)
        {
            Config.MerchantId = 1396424;
            Config.SecretKey = "test";

            var req = new CheckoutRequest
            {
                order_id = Guid.NewGuid().ToString("N"),
                amount = (int)CalculateTotalPrice() * 100,
                order_desc = "checkout json demo",
                currency = "UAH"
            };
            var resp = new Url().Post(req);
            if (resp.Error == null)
            {
                string url = resp.checkout_url;

                System.Diagnostics.Process.Start(url);
            }
            else
            {
                MessageBox.Show("Error: " + resp.Error.Message);
            }
        }
    }
}

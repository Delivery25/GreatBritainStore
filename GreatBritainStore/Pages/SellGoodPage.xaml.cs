using GreatBritainStore.Models;
using System;
using System.Linq;
using System.Windows.Controls;

namespace GreatBritainStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для SellGoodsPage.xaml
    /// </summary>
    public partial class SellGoodPage
    {
        public SellGoodPage(Product good)
        {
            InitializeComponent();
            LoadData(good);
        }

        // загрузка данных в DataGrid и ComboBox
        void LoadData(Product good)
        {
            using (var context = new EnglishStoreEntities())
            {
                DataGridSells.ItemsSource = context.Sale.Where(p => p.ID == good.ID).OrderBy(p => p.SaleTime).ToList();
                ComboGoods.ItemsSource = context.Product.OrderBy(p => p.Name).ToList(); ;
                ComboGoods.SelectedIndex = 0;
                ComboGoods.SelectedValue = good.ID;
                GridGood.DataContext = good;
            }
        }

        // фильтрация продаж по товару
        private void ComboGoodsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new EnglishStoreEntities())
            {
                if (ComboGoods.SelectedIndex > 0)
                {
                    var goodId = Convert.ToInt32(ComboGoods.SelectedValue);
                    var x = context.Sale.Where(p => p.ID == goodId).OrderBy(p => p.SaleTime).ToList();
                    DataGridSells.ItemsSource = x;
                    GridGood.DataContext = ComboGoods.SelectedItem;
                }
            }
        }
    }
}

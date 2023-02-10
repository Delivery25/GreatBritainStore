using GreatBritainStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace GreatBritainStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для GoodsPage.xaml
    /// </summary>
    public partial class GoodsPage
    {
        public GoodsPage()
        {
            InitializeComponent();
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // открытие редактирования товара
            // передача выбранного товара в AddGoodPage
            Manager.MainFrame.Navigate(new AddGoodPage((sender as Button).DataContext as Product));
        }
        private void PageIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (var context = new EnglishStoreEntities())
            {
                //событие отображения данного Page
                // обновляем данные каждый раз когда активируется этот Page
                if (Visibility == Visibility.Visible)
                {
                    DataGridGood.ItemsSource = null;
                    //загрузка обновленных данных
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    var goods = context.Product.OrderBy(p => p.Name).ToList();
                    var manufacturers = context.Manufacturer.ToList();
                    foreach (var good in goods)
                    {
                        good.Manufacturer = manufacturers.Find(x => x.ID == good.ManufacturerID);
                    }
                    DataGridGood.ItemsSource = goods;
                }
            }
        }
        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            // открытие AddGoodPage для добавления новой записи
            Manager.MainFrame.Navigate(new AddGoodPage(null));
        }
        private void BtnDeleteClick(object sender, RoutedEventArgs e)
        {
            // удаление выбранного товара из таблицы
            //получаем все выделенные товары
            var selectedGoods = DataGridGood.SelectedItems.Cast<Product>().ToList();
            // вывод сообщения с вопросом Удалить запись?
            var messageBoxResult = MessageBox.Show($"Удалить {selectedGoods.Count()} записей?",
            "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            //если пользователь нажал ОК пытаемся удалить запись
            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    using (var context = new EnglishStoreEntities())
                    {
                        // берем из списка удаляемых товаров один элемент
                        var product = selectedGoods[0];
                        // проверка, есть ли у товара в таблице о продажах связанные записи
                        // если да, то выбрасывается исключение и удаление прерывается
                        if (product.Sale.Count > 0)
                            throw new Exception("Есть записи в продажах");

                        //ищем записи в таблице Complect, с которой связан этот товар
                        //var complects = GreatBritainEntities.GetContext().Complects.Where(p => p.MainGoodId == x.GoodId || p.SecondGoodId == x.GoodId).ToList();
                        //// удаляем эти записи
                        //GreatBritainEntities.GetContext().Complects.RemoveRange(complects);

                        // удаляем товара
                        context.Product.Remove(product);
                        //сохраняем изменения
                        context.SaveChanges();

                        MessageBox.Show("Записи удалены");
                        var goods = context.Product.OrderBy(p => p.Name).ToList();
                        DataGridGood.ItemsSource = null;
                        DataGridGood.ItemsSource = goods;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BtnSellClick(object sender, RoutedEventArgs e)
        {
            // открытие страницы о продажах SellGoodsPage
            // передача в него выбранного товара
            Manager.MainFrame.Navigate(new SellGoodPage((sender as Button).DataContext as Product)); 
        }
        // отображение номеров строк в DataGrid
        private void DataGridGoodLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}

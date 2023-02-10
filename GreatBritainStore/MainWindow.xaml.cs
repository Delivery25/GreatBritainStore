using GreatBritainStore.Models;
using GreatBritainStore.Pages;
using System;
using System.Windows;
namespace GreatBritainStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new CatalogOfGoodsPage());
            Manager.MainFrame = MainFrame;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            // показать владельца формы
            Owner.Show();
            // или если надо, то можно закрыть приложение командой
            // App.Current.Shutdown();
        }

        //событие попытки закрытия окна,
        // если пользователь выберет Cancel, то форму не закроем
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Вы действительно хотите выйти?",
            "Выйти", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        // Кнопка назад
        private void BtnBackClick(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        // Кнопка навигации
        private void BtnEditGoodsClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new GoodsPage());
        }

        // Событие отрисовки страницы
        // Скрываем или показываем кнопку Назад 
        // Скрываем или показываем кнопки Для перехода к остальным страницам
        private void MainFrameContentRendered(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                BtnBack.Visibility = Visibility.Visible;
                BtnEditGoods.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnBack.Visibility = Visibility.Collapsed;
                BtnEditGoods.Visibility = Visibility.Visible;
            }
        }
    }
}
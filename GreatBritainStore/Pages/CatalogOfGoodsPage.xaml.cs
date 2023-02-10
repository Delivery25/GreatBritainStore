using GreatBritainStore.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace GreatBritainStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogOfGoodsPage.xaml
    /// </summary>
    public partial class CatalogOfGoodsPage
    {
        private int _itemCount;
        public CatalogOfGoodsPage()
        {
            InitializeComponent();
            // загрузка данных в combobox + добавление дополнительной строки
            using (var context = new EnglishStoreEntities())
            {
                var developers = context.Manufacturer.OrderBy(p => p.Name).ToList();
                developers.Insert(0, new Manufacturer
                {
                    Name = "Все типы"
                }
                );
                ComboDeveloper.ItemsSource = developers;
                ComboDeveloper.SelectedIndex = 0;
                // загрузка данных в listview сортируем по названию
                LViewGoods.ItemsSource = context.Product.OrderBy(p => p.Name).ToList();
                _itemCount = LViewGoods.Items.Count;
                // отображение количества записей
                TextBlockCount.Text = $" Результат запроса: {_itemCount} записей из {_itemCount}";

            }
        }
        private void PageIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (var context = new EnglishStoreEntities())
            {
                //обновление данных после каждой активации окна
                if (Visibility == Visibility.Visible)
                {
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    LViewGoods.ItemsSource = context.Product.OrderBy(p => p.Name).ToList();
                }
            }
        }
        // Поиск товаров, которые содержат данную поисковую строку
        private void TBoxSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }
        // Поиск товаров конкретного производителя
        private void ComboTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
        /// <summary>
        /// Метод для фильтрации и сортировки данных
        /// </summary>
        private void UpdateData()
        {
            using (var context = new EnglishStoreEntities())
            {
                // получаем текущие данные из бд
                var currentGoods = context.Product.OrderBy(p => p.Name).ToList();
                // выбор только тех товаров, которые принадлежат данному производителю
                if (ComboDeveloper.SelectedIndex > 0)
                    currentGoods = currentGoods.Where(p => p.ManufacturerID == (ComboDeveloper.SelectedItem as Manufacturer).ID).ToList();
                // выбор тех товаров, в названии которых есть поисковая строка
                currentGoods = currentGoods.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
                // сортировка
                if (ComboSort.SelectedIndex >= 0)
                {
                    // сортировка по возрастанию цены
                    if (ComboSort.SelectedIndex == 0)
                        currentGoods = currentGoods.OrderBy(p => p.Price).ToList();
                    // сортировка по убыванию цены
                    if (ComboSort.SelectedIndex == 1)
                        currentGoods = currentGoods.OrderByDescending(p => p.Price).ToList();
                }
                // В качестве источника данных присваиваем список данных
                LViewGoods.ItemsSource = currentGoods;
                // отображение количества записей
                TextBlockCount.Text = $" Результат запроса: {currentGoods.Count} записей из {_itemCount}";
            }
        }
        // сортировка товаров
        private void ComboSortSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
    }
}
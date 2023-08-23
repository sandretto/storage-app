using StorageApp.DataLibrary.Models;
using StorageApp.DataLibrary.ORM;

namespace StorageApp.ConsoleApp
{
    public class Storage : IAsyncInitialization
    {
        private const string boxesPath = @"Boxes.json";
        private const string palletsPath = @"Pallets.json";

        private List<Box> _boxes = new();
        private List<Pallet> _pallets = new();

        public Task Initialization { get; private set; }

        /// <summary>
        /// Инициализация коллекций в конструкторе
        /// </summary>
        public Storage()
        {
            using var db = new DataContext();
            Initialization = InitializeAsync(db);
        }

        /// <summary>
        /// Инициализация коллекций в конструкторе
        /// </summary>
        private async Task InitializeAsync(DataContext dataContext)
        {
            _pallets = await DataInitializer<Pallet>.GetDataFromJson(dataContext, palletsPath);
            _boxes = await DataInitializer<Box>.GetDataFromJson(dataContext, boxesPath);
            _pallets = _pallets
                .Select(pallet =>
                new Pallet
                {
                    Id = pallet.Id,
                    Width = pallet.Width,
                    Height = pallet.Height,
                    Depth = pallet.Depth,
                    Boxes = _boxes
                    .Where(b => b.PalletId == pallet.Id)
                })
                .ToList();
        }

        /// <summary>
        /// Сгруппировать все паллеты по сроку годности, 
        /// отсортировать по возрастанию срока годности, 
        /// в каждой группе отсортировать паллеты по весу
        /// </summary>
        /// <returns></returns>
        public List<Pallet> GetPalletsGroupedByExpiryDateSortedByWeight()
        {
            var result = _pallets
                .OrderBy(pallet => pallet.GetExpiryDate())
                .ThenBy(pallet => pallet.Weight)
                .GroupBy(pallet => pallet.GetExpiryDate())
                .SelectMany(group => group)
                .ToList();

            return result;
        }


        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, 
        /// отсортированные по возрастанию объема
        /// </summary>
        public List<Pallet> Get3PalletsWithMaxExpiryDateSortedByVolume()
        {
            var result = _pallets
                .Take(3)
                .OrderByDescending(pallet => pallet.GetExpiryDate())
                .OrderBy(pallet => pallet.Volume)
                .ToList();
            return result;
        }

        /// <summary>
        /// Вывод результатов на экран
        /// </summary>
        /// <param name="pallets"></param>
        public void DisplayResults(IList<Pallet> pallets, string task)
        {
            Console.WriteLine(task);
            DisplayPallets(pallets);
            Console.WriteLine();
        }

        /// <summary>
        /// Вывод списка паллет на экран
        /// </summary>
        /// <param name="pallets"></param>
        private void DisplayPallets(IList<Pallet> pallets)
        {
            foreach (Pallet pallet in pallets)
            {
                if (pallet == null)
                    continue;

                var palletExpiryDate = pallet.GetExpiryDate() != null ? pallet.GetExpiryDate().Value.ToShortDateString() : "не указан";

                Console.WriteLine($"Паллета №{pallet.Id}: ширина - {pallet.Width}, высота - {pallet.Height}, " +
                    $"глубина - {pallet.Depth}, вес - {pallet.Weight}, " +
                    $"срок годности - {palletExpiryDate}, объем - {pallet.Volume} ");

                if (pallet.Boxes == null)
                    continue;
                DisplayBoxes(pallet);
            }
        }

        /// <summary>
        /// Вывод списка коробок на экран
        /// </summary>
        /// <param name="pallet"></param>
        private void DisplayBoxes(Pallet pallet)
        {
            foreach (Box box in pallet.Boxes)
            {
                if (box == null)
                    continue;

                var boxExpiryDate = box.GetExpiryDate() != null ? box.GetExpiryDate().Value.ToShortDateString() : "не указан";

                Console.WriteLine($"\tКоробка №{box.Id}: № паллеты - {box.PalletId}, ширина - {box.Width}, " +
                    $"высота - {box.Height}, глубина - {box.Depth}, " +
                    $"срок годности - {boxExpiryDate}, вес - {box.Weight}");
            }
        }
    }
}

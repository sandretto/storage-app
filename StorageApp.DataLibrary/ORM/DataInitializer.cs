using Newtonsoft.Json;
using StorageApp.DataLibrary.Models;
using System.Diagnostics;

namespace StorageApp.DataLibrary.ORM
{
    /// <summary>
    /// Класс для заполнения таблиц БД из JSON-файла
    /// </summary>
    public class DataInitializer<T> where T : BaseModel
    {
        public static async Task<List<T>> GetDataFromJson(DataContext dataContext, string path)
        {
            try
            {
                var json = File.ReadAllText(@"Seed" + Path.DirectorySeparatorChar + path);
                var list = JsonConvert.DeserializeObject<List<T>>(json) ??
                    throw new NullReferenceException("Список пуст");
                await dataContext.AddRangeAsync(list);
                await dataContext.SaveChangesAsync();
                return list;
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<T>();
            }
        }
    }
}

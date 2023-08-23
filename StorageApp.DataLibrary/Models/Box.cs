using System.Diagnostics;

namespace StorageApp.DataLibrary.Models
{
    public class Box : BaseModel
    {
        public int PalletId { get; set; }

        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// Срок годности, если не указан, 
        /// вычисляется в зависимости от даты изготовления
        /// </summary>
        public DateTime? GetExpiryDate()
        {
            try
            {
                if (ExpiryDate != null)
                    return (DateTime)ExpiryDate;
                else if (ProductionDate != null)
                {
                    return ((DateTime)ProductionDate).AddDays(100);
                }
                else
                {
                    throw new NullReferenceException("Укажите дату изготовления либо срок годности");
                }
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return DateTime.MinValue;
            }
        }


        public override int Volume
        {
            get
            {
                return GetVolume();
            }
        }

        private int GetVolume()
        {
            try
            {
                var volume = Width * Height * Depth;
                if (volume >= 0) return volume;
                else
                {
                    throw new InvalidDataException("Один из параметров меньше нуля");
                }
            }
            catch (InvalidDataException ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}

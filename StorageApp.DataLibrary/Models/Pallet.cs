using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.DataLibrary.Models
{
    public class Pallet : BaseModel
    {
        /// <summary>
        /// Вес пустой паллеты
        /// </summary>
        public const int TareWeight = 30;

        /// <summary>
        /// Коллекция коробок в паллете
        /// </summary>
        public IEnumerable<Box> Boxes { get; set; } = new List<Box>();

        /// <summary>
        /// Срок годности вычисляется в зависимости 
        /// от коробки с минимальным сроком годности
        /// </summary>
        public DateTime? GetExpiryDate()
        {
            return Boxes == null || !Boxes.Any() ?
                null :
                Boxes.Select(b => b.GetExpiryDate()).Min();
        }

        /// <summary>
        /// Объем паллеты с коробками
        /// </summary>
        /// <returns></returns>
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
                if (BoxesVolume > TareVolume)
                {
                    throw new InvalidDataException("Суммарный объем коробок в паллете не должен превышать объем паллеты!");
                }
                else
                    return TareVolume + BoxesVolume;
            }
            catch (InvalidDataException ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Объем паллеты без коробок
        /// </summary>
        /// <returns></returns>
        private int TareVolume => Width * Height * Depth;

        /// <summary>
        /// Суммарный объем всех коробок
        /// </summary>
        /// <returns></returns>
        private int BoxesVolume
        {
            get
            {
                return GetBoxesVolume();
            }
        }

        private int GetBoxesVolume()
        {
            if (Boxes == null || !Boxes.Any()) return 0;
            else
            {
                var volume = 0;

                foreach (var box in Boxes)
                {
                    volume += box.Volume;
                }

                return volume;
            }
        }

        /// <summary>
        /// Вес тары плюс вес коробок
        /// </summary>
        /// <returns></returns>
        public override int Weight => TareWeight + NetWeight;

        /// <summary>
        /// Вес коробок
        /// </summary>
        /// <returns></returns>
        private int NetWeight
        {
            get
            {
                return GetNetWeight();
            }
        }

        private int GetNetWeight()
        {
            if (Boxes == null || !Boxes.Any()) return 0;
            else
            {
                int weight = 0;

                foreach (var box in Boxes)
                {
                    weight += box.Weight;
                }

                return weight;
            }
        }
    }
}

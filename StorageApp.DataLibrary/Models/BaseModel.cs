using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.DataLibrary.Models
{
    /// <summary>
    /// Базовый класс объекта на складе
    /// </summary>
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public virtual int Weight { get; set; }

        public virtual DateTime? ExpiryDate { get; set; }

        public virtual int Volume { get; set; }
    }
}

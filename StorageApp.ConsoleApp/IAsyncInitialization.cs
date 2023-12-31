﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.ConsoleApp
{
    /// <summary>
    /// Интерфейс, определяющий асинхронную инициализацию
    /// реализуемого объекта
    /// </summary>
    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }
}

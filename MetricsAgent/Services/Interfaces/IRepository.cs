using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<T> GetAll();
        void Create(T item);
    }

}
                                                                                                                                                                                                                                                                                        
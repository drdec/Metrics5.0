using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<T> GetAll();
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }

}
                                                                                                                                                                                                                                                                                        
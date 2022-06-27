using PIMBackend.Database;
using PIMBackend.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PIMBackend.Repositories.Imp
{
    /// <summary>
    ///     The implementation of sample repository
    /// </summary>
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PIMContext context) : base(context)
        {
        }


        public Employee GetByVisa(string visa)
        {
            return Set.SingleOrDefault(x => x.Visa == visa);
        }

        public string[] GetExistOrNotExistEmployeeArray(string memString, bool isExist = true)
        {
            List<string> ret = new List<string>();
            List<string> visas = new List<string>();

            if (memString != null && memString.Length > 0)
            {
                visas = memString.Split(",").ToList<string>();

                for (int i = 0; i < visas.Count; ++i)
                {
                    visas[i] = visas[i].Trim();
                }

            }

            visas = visas.Where(x => string.IsNullOrEmpty(x) == false).ToList();

            for (int i = 0; i < visas.Count; ++i)
            {
                if (Set.Any<Employee>(e => e.Visa == visas[i]) == isExist)
                {
                    ret.Add(visas[i].ToUpper());
                }
            }

            return ret.Distinct().ToArray();
        }

    }
}
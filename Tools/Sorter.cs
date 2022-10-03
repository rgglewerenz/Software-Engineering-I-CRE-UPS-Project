using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Sorter
    {
        #region Sorting Methods
        public static List<Package> Sort(List<Package> packages, Func<Package, Package> doEach)
        {
            List<Task<Package>> tasks = new List<Task<Package>>();

            foreach (Package package in packages)
            {
                tasks.Add(Task.Run(() => doEach(package)));
            }

            var items = Task.WhenAll(tasks).Result;

            packages = items.OrderBy(x => x.DistanceFromPoint).ToList();
            return packages;
        }
        public static async Task<List<Package>> SortAsync(List<Package> packages, Func<Package, Task<Package>> doEach)
        {
            List<Task<Package>> tasks = new List<Task<Package>>();
            foreach(Package package in packages)
            {
                tasks.Add(doEach(package));
            }

            var items = await Task.WhenAll(tasks);

            packages = items.OrderBy(x => x.DistanceFromPoint).ToList();
            return packages;
        }
        #endregion Sorting
    }
}

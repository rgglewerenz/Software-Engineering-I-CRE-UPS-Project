using DTO;
using System.Collections.Generic;

namespace TestingWithBlazor.Interface
{
    public interface IPackageHandler
    {
        public List<Package> Packages { get; set; }
        void AddPackage(Package new_package);
        List<Package> GetPackages();
        void SetPackages(List<Package> new_packages);
    }
}

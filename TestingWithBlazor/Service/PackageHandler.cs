using DTO;
using System.Collections.Generic;
using TestingWithBlazor.Interface;

namespace TestingWithBlazor.Service
{
    public class PackageHandler : IPackageHandler
	{
		private List<Package> packages { get; set; } = new List<Package>();
        public List<Package> Packages { get { return packages; } set { packages = value; } }
		public List<Package> GetPackages() { return packages; }
        public void SetPackages(List<Package> new_packages)
        {
            packages = new_packages;
        }
        public void AddPackage(Package new_package)
        {
            packages.Add(new_package);
        }
    }
}

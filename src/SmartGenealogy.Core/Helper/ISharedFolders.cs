using SmartGenealogy.Core.Helper.Packages;
using SmartGenealogy.Core.Models.FileInterfaces;

namespace SmartGenealogy.Core.Helper;

public interface ISharedFolders
{
    void SetupLinksForPackage(BasePackage basePackage, DirectoryPath installDirectory);
    void RemoveLinksForAllPackages();
}
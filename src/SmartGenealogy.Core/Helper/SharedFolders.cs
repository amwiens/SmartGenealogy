using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Helper.Packages;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Services;

namespace SmartGenealogy.Core.Helper;

[Singleton(typeof(ISharedFolders))]
public class SharedFolders : ISharedFolders
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly ISettingsManager settingsManager;


    public SharedFolders(ISettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;
    }

    public void RemoveLinksForAllPackages()
    {
        throw new NotImplementedException();
    }

    public void SetupLinksForPackage(BasePackage basePackage, DirectoryPath installDirectory)
    {
        throw new NotImplementedException();
    }

    // TODO: update class
}
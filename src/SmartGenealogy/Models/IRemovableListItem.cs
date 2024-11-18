using System;

namespace SmartGenealogy.Models;

public interface IRemovableListItem
{
    public event EventHandler ParentListRemoveRequested;
}
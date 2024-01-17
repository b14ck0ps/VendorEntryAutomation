using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace VE.DataAccessLayer.Interface
{
    public interface ISharePointRepository
    {
        List<ListItem> GetAllItemsFromList(string listName);
        List<ListItem> GetItemsByQuery(string listName, string camlQuery);
    }
}

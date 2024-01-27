using System.Collections.Generic;
using Microsoft.SharePoint.Client;

namespace VE.DataAccessLayer.Interface
{
    public interface ISharePointRepository
    {
        List<ListItem> GetAllItemsFromList(string listName);
        List<ListItem> GetItemsByQuery(string listName, string camlQuery);
        void InsertItem(string listName, Dictionary<string, object> fieldValues);
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using VE.DataAccessLayer.Interface;

namespace VE.DataAccessLayer.Repository
{
    internal class SharePointRepository : ISharePointRepository
    {
        public List<ListItem> GetAllItemsFromList(string listName)
        {
            using (var ctx = SpConnection.GetContext())
            {
                var web = ctx.Web;
                var list = web.Lists.GetByTitle(listName);

                var query = CamlQuery.CreateAllItemsQuery();
                var items = list.GetItems(query);

                ctx.Load(items);
                ctx.ExecuteQuery();

                var itemList = items.ToList();
                return itemList;
            }
        }

        public List<ListItem> GetItemsByQuery(string listName, string camlQuery)
        {
            using (var ctx = SpConnection.GetContext())
            {
                var web = ctx.Web;
                var caml = new CamlQuery();
                caml.ViewXml = camlQuery;

                var listItem = web.Lists.GetByTitle(listName).GetItems(caml);

                ctx.Load(listItem);
                ctx.ExecuteQuery();

                var itemList = listItem.ToList();
                return itemList;
            }
        }
    }
}
﻿using System.Collections.Generic;
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

        public void InsertItem(string listName, Dictionary<string, object> fieldValues)
        {
            using (var ctx = SpConnection.GetContext())
            {
                var web = ctx.Web;
                var list = web.Lists.GetByTitle(listName);

                var itemCreateInfo = new ListItemCreationInformation();
                var newItem = list.AddItem(itemCreateInfo);

                foreach (var fieldValue in fieldValues)
                {
                    newItem[fieldValue.Key] = fieldValue.Value;
                }

                newItem.Update();

                ctx.ExecuteQuery();
            }
        }
        public void UpdateItemsByQuery(string listName, string camlQuery, Dictionary<string, object> fieldValues)
        {
            using (var ctx = SpConnection.GetContext())
            {
                var web = ctx.Web;
                var list = web.Lists.GetByTitle(listName);

                var caml = new CamlQuery();
                caml.ViewXml = camlQuery;

                var items = list.GetItems(caml);

                ctx.Load(items);
                ctx.ExecuteQuery();

                foreach (var item in items)
                {
                    foreach (var fieldValue in fieldValues)
                    {
                        item[fieldValue.Key] = fieldValue.Value;
                    }

                    item.Update();
                }

                ctx.ExecuteQuery();
            }
        }

    }
}
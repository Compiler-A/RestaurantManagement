using ContractsLayerRestaurant.DTORequest.MenuItems;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataLayerRestaurant.Classes.EF
{
    public class MenuItemsRepositoryReaderEF : IMenuItemsRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public MenuItemsRepositoryReaderEF(IOptions<clsMySettings> settings, AppDBContext dBContext)
        {
            _Settings = settings.Value;
            _DbContext = dBContext;
        }
        public async Task<MenuItem?> GetDataAsync(int ID)
        {
            var query = _DbContext.MenuItems
                .AsNoTracking()
                .Where(x => x.ItemID == ID);

            var data = query
                .Select(x => new MenuItem
                {
                   ItemName = x.ItemName,
                   ItemID = x.ItemID,
                   Price = x.Price,
                   Image = x.Image,
                   TypeItemID = x.TypeItemID,
                   StatusMenuID = x.StatusMenuID,
                   TypeItem = new TypeItem
                   {
                       TypeItemID = x.TypeItem.TypeItemID,
                       TypeName = x.TypeItem.TypeName
                   },
                   StatusMenu = new StatusMenu
                   {
                       StatusMenuID = x.StatusMenu.StatusMenuID,
                       StatusMenuName = x.StatusMenu.StatusMenuName
                   }
                });

            return await data.FirstOrDefaultAsync();
        }
        public async Task<List<MenuItem>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.MenuItems.AsNoTracking();

            var data = query.Where(x => Ids.Contains(x.ItemID));

            var list = data.Select(x => new MenuItem
            {
                ItemName = x.ItemName,
                ItemID = x.ItemID,
                Price = x.Price,
                Image = x.Image,
                TypeItemID = x.TypeItemID,
                StatusMenuID = x.StatusMenuID,
                TypeItem = new TypeItem
                {
                    TypeItemID = x.TypeItem.TypeItemID,
                    TypeName = x.TypeItem.TypeName
                },
                StatusMenu = new StatusMenu
                {
                    StatusMenuID = x.StatusMenu.StatusMenuID,
                    StatusMenuName = x.StatusMenu.StatusMenuName
                }
            });

            return await data.ToListAsync();
        }

        public async Task<List<MenuItem>> GetAllDataAsync(int Page)
        {
            var query = _DbContext.MenuItems
                .AsNoTracking();

            var data = query
                .Select(x => new MenuItem
                {
                    ItemName = x.ItemName,
                    ItemID = x.ItemID,
                    Price = x.Price,
                    Image = x.Image,
                    TypeItemID = x.TypeItemID,
                    StatusMenuID = x.StatusMenuID,
                    TypeItem = new TypeItem
                    {
                        TypeItemID = x.TypeItem.TypeItemID,
                        TypeName = x.TypeItem.TypeName
                    },
                    StatusMenu = new StatusMenu
                    {
                        StatusMenuID = x.StatusMenu.StatusMenuID,
                        StatusMenuName = x.StatusMenu.StatusMenuName
                    }
                });

            var list = data.Skip((Page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await list.ToListAsync();
        }

        public async Task<List<MenuItem>> GetAllDataAvailablesAsync()
        {
            var query = _DbContext.MenuItems
            .AsNoTracking()
            .Where(x => x.StatusMenuID == 1);

            var data = query
                .Select(x => new MenuItem
                {
                    ItemName = x.ItemName,
                    ItemID = x.ItemID,
                    Price = x.Price,
                    Image = x.Image,
                    TypeItemID = x.TypeItemID,
                    StatusMenuID = x.StatusMenuID,
                    TypeItem = new TypeItem
                    {
                        TypeItemID = x.TypeItem.TypeItemID,
                        TypeName = x.TypeItem.TypeName
                    },
                    StatusMenu = new StatusMenu
                    {
                        StatusMenuID = x.StatusMenu.StatusMenuID,
                        StatusMenuName = x.StatusMenu.StatusMenuName
                    }
                });

            return await data.ToListAsync();
        }

        public async Task<List<MenuItem>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            var query = _DbContext.MenuItems
                .AsNoTracking()
                .Where(x => (x.StatusMenuID == Request.StatusMenuID || Request.StatusMenuID == 0) 
                && (x.TypeItemID == Request.TypeItemID || Request.TypeItemID == 0));

            var data = query
                .Select(x => new MenuItem
                {
                    ItemName = x.ItemName,
                    ItemID = x.ItemID,
                    Price = x.Price,
                    Image = x.Image,
                    TypeItemID = x.TypeItemID,
                    StatusMenuID = x.StatusMenuID,
                    TypeItem = new TypeItem
                    {
                        TypeItemID = x.TypeItem.TypeItemID,
                        TypeName = x.TypeItem.TypeName
                    },
                    StatusMenu = new StatusMenu
                    {
                        StatusMenuID = x.StatusMenu.StatusMenuID,
                        StatusMenuName = x.StatusMenu.StatusMenuName
                    }
                });

            var list = data.Skip((Request.Page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await list.ToListAsync();
        }

    }

    public class MenuItemsRepositoryWriterEF : IMenuItemsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;
        public MenuItemsRepositoryWriterEF(IOptions<clsMySettings> settings, AppDBContext DBcontext)
        {
            _Settings = settings.Value;
            _DbContext = DBcontext;
        }

        public async Task<MenuItem?> CreateDataAsync(DTOMenuItemsCRequest menuItem)
        {
            var StatusMenu = await _DbContext.StatusMenus.FindAsync(menuItem.StatusMenuID);
            if (StatusMenu == null)
            {
                return null;
            }
            var TypItem = await _DbContext.TypeItems.FindAsync(menuItem.TypeItemID);
            if (TypItem == null)
            {
                return null;
            }

            var Entity = new MenuItem
            {
                ItemName = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                TypeItemID = menuItem.TypeItemID,
                StatusMenuID = menuItem.StatusMenuID,
                Image = menuItem.Image
            };

            await _DbContext.MenuItems.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();

            Entity.StatusMenu = StatusMenu;
            Entity.TypeItem = TypItem;

            return Entity;
        }

        public async Task<MenuItem?> UpdateDataAsync(DTOMenuItemsURequest menuItem)
        {
            var StatusMenu = await _DbContext.StatusMenus.FindAsync(menuItem.StatusMenuID);
            if (StatusMenu == null)
            {
                return null;
            }
            var TypItem = await _DbContext.TypeItems.FindAsync(menuItem.TypeItemID);
            if (TypItem == null)
            {
                return null;
            }

            var Entity = await _DbContext.MenuItems.FindAsync(menuItem.ID);
            if (Entity == null)
            {
                return null;
            }

            Entity.ItemName = menuItem.Name;
            Entity.Description = menuItem.Description;
            Entity.Price = menuItem.Price;
            Entity.TypeItemID = menuItem.TypeItemID;
            Entity.StatusMenuID = menuItem.StatusMenuID;
            Entity.Image = menuItem.Image;

            await _DbContext.SaveChangesAsync();
            Entity.TypeItem = TypItem;
            Entity.StatusMenu = StatusMenu;
            return Entity;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            var RemoveMenuItem = await _DbContext.MenuItems.FindAsync(id);
            if (RemoveMenuItem == null)
            {
                return false;
            }

            _DbContext.MenuItems.Remove(RemoveMenuItem);

            int AffectRows = await _DbContext.SaveChangesAsync();
            return AffectRows > 0;
        }
    }
}

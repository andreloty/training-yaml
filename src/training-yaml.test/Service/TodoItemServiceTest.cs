using System;
using AutoFixture;
using Xunit;
using System.Threading.Tasks;
using TodoApi.Services;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace training_yaml.test
{
    public class TodoItemServiceTest
    {
        private ITodoItemService InitDb(bool resetDb = true, bool runSeed = true)
        {
            var builder = new DbContextOptionsBuilder<TodoContext>();
            builder.UseInMemoryDatabase("TodoList");
            var options = builder.Options;

            var context = new TodoContext(options);

            if (resetDb)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            if (runSeed)
            {
                for (int i = 1; i <= 5; i++)
                {
                    var item = new TodoItem { Name = $"Item No {i}" };
                    context.TodoItems.AddAsync(item).ConfigureAwait(false);
                }
                context.SaveChangesAsync().ConfigureAwait(false);
            }

            var service = new TodoItemService(context);

            return service;
        }

        [Fact]
        public void List_All_TodoItems()
        {
            var service = InitDb();

            var list = service.GetAll();

            Assert.Equal(5, list.Count());

            service.Dispose();
        }

        [Fact]
        public async Task Get_Item_Id_3()
        {
            var service = InitDb();

            var item = await service.GetItemAsync(3);

            Assert.Equal("Item No 3", item.Name);

            service.Dispose();
        }

        [Fact]
        public async Task Insert_New_TodoItem()
        {
            var service = InitDb(runSeed: false);

            var todoItem = new TodoItem
            {
                Name = "Nova Tarefa"
            };

            await service.AddItemAsync(todoItem);

            Assert.Equal(1, todoItem.Id);

            service.Dispose();
        }

        [Fact]
        public async Task Update_Item_Id_3()
        {
            using (var service = InitDb())
            {
                var item = await service.GetItemAsync(3);
                item.Name = "Item updated No 3";
                await service.UpdateItemAsync(item);
            }

            using (var service = InitDb(false, false))
            {
                var item = await service.GetItemAsync(3);
                Assert.Equal("Item updated No 3", item.Name);
            }
        }

        [Fact]
        public async Task Delete_Item_Id_3()
        {
            using (var service = InitDb())
            {
                var item = await service.GetItemAsync(3);
                item.Name = "Item updated No 3";
                await service.DeleteItemAsync(item);
            }

            using (var service = InitDb(false, false))
            {
                var list = service.GetAll();

                var item = await service.GetItemAsync(3);

                Assert.Null(item);
                Assert.Equal(4, list.Count());
            }
        }
    }
}

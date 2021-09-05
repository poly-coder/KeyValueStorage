using DotNetX.Azure.Storage.Blobs.DependencyInjection;
using DotNetX.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KeyValueStorage.SampleApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KeyValueStorage.SampleApp", Version = "v1" });
            });

            services.Configure<BlobServiceClientSettings>("Files", options => options.ConnectionString = "UseDevelopmentStorage=true");
            services.AddNamedBlobServiceClient<BlobServiceClientSettings>("Files");

            services.Configure<BlobContainerClientSettings>("Files", options =>
            {
                options.ConnectionString = "UseDevelopmentStorage=true";
                options.Container = "files";
            });
            services.AddNamedBlobContainerClient<BlobContainerClientSettings>("Files");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeyValueStorage.SampleApp v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public abstract record TodoListCommand
    {
        public record AddItem(string Title, TaskCompletionSource<int> Reply) : TodoListCommand;
        public record CheckItem(int ItemId) : TodoListCommand;
        public record UncheckItem(int ItemId) : TodoListCommand;
        public record DeleteItem(int ItemId) : TodoListCommand;
        public record GetCount(TaskCompletionSource<int> Reply) : TodoListCommand;
    }

    public class SampleMailboxActor
    {
        public async Task RunSample()
        {

            var todoList = MailboxActor
                .StartUnbounded<TodoListCommand>(TodoListActor);

            var id1 = await todoList.Mailbox
                .PostAndReplyAsync<TodoListCommand, int>(reply =>
                new TodoListCommand.AddItem("hello", reply));

            var id2 = await todoList.Mailbox
                .PostAndReplyAsync<TodoListCommand, int>(reply =>
                new TodoListCommand.AddItem("world", reply));

            await todoList.Mailbox
                .PostAsync(new TodoListCommand.CheckItem(id1));

            var count = await todoList.Mailbox
                .PostAndReplyAsync<TodoListCommand, int>(reply =>
                new TodoListCommand.GetCount(reply));

            Debug.Assert(count == 2);
        }

        private async Task TodoListActor(ChannelReader<TodoListCommand> reader)
        {
            var state = new Dictionary<int, (string title, bool done)>();

            await foreach (var command in reader.ReadAllAsync())
            {
                switch (command)
                {
                    case TodoListCommand.AddItem(var title, var reply):
                        {
                            var itemId = state.Count;
                            state.Add(itemId, (title, false));
                            reply.TrySetResult(itemId);
                            Console.WriteLine($"AddItem {new { title }} => {new { itemId }}");
                            break;
                        }

                    case TodoListCommand.CheckItem(var itemId):
                        {
                            if (state.TryGetValue(itemId, out var data) && !data.done)
                            {
                                state[itemId] = (data.title, true);
                                Console.WriteLine($"CheckItem {new { itemId }}");
                            }

                            break;
                        }

                    case TodoListCommand.UncheckItem(var itemId):
                        {
                            if (state.TryGetValue(itemId, out var data) && data.done)
                            {
                                state[itemId] = (data.title, false);
                                Console.WriteLine($"UncheckItem {new { itemId }}");
                            }

                            break;
                        }

                    case TodoListCommand.DeleteItem(var itemId):
                        {
                            state.Remove(itemId);
                            Console.WriteLine($"DeleteItem {new { itemId }}");
                            break;
                        }

                    case TodoListCommand.GetCount(var reply):
                        {
                            var count = state.Count;
                            reply.TrySetResult(state.Count);
                            Console.WriteLine($"GetCount => {new { count }}");
                            break;
                        }
                }
            }
        }
    }
}

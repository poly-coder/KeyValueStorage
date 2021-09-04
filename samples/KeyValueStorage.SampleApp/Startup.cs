using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DotNetX.Azure.Storage.Blobs.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            async Task<Dictionary<int, (string title, bool done)>> TodoListActor(ChannelReader<TodoListCommand> reader)
            {
                var state = new Dictionary<int, (string title, bool done)>();

                await foreach (var command in reader.ReadAllAsync())
                {
                    switch (command)
                    {
                        case TodoListCommand.AddItem(var title, var reply):
                            var id = state.Count;
                            state.Add(id, (title, false));
                            reply.TrySetResult(id);
                            break;

                        case TodoListCommand.CheckItem(var itemId):
                        {
                            if (state.TryGetValue(itemId, out var data) && !data.done)
                            {
                                state[itemId] = (data.title, true);
                            }

                            break;
                        }

                        case TodoListCommand.UncheckItem(var itemId):
                        {
                            if (state.TryGetValue(itemId, out var data) && data.done)
                            {
                                state[itemId] = (data.title, false);
                            }

                            break;
                        }

                        case TodoListCommand.DeleteItem(var itemId):
                            state.Remove(itemId);
                            break;

                        case TodoListCommand.GetCount(var reply):
                            reply.TrySetResult(state.Count);
                            break;
                    }
                }

                return state;
            }

            var (todoList, _) = MailboxActor.StartUnbounded<TodoListCommand, Dictionary<int, (string title, bool done)>>(TodoListActor);

            async Task RunLoop()
            {
                var id1 = await todoList.PostAndReplyAsync<TodoListCommand, int>(reply =>
                    new TodoListCommand.AddItem("hello", reply));

                var id2 = await todoList.PostAndReplyAsync<TodoListCommand, int>(reply =>
                    new TodoListCommand.AddItem("world", reply));

                await todoList.PostAsync(new TodoListCommand.CheckItem(id1));

                var count = await todoList.PostAndReplyAsync<TodoListCommand, int>(reply =>
                    new TodoListCommand.GetCount(reply));

                Debug.Assert(count == 2);
            }
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

    public static class MailboxActor
    {
        #region [ StartOnChannel ]

        public static (ChannelWriter<T> mailbox, Task actor) StartOnChannel<T>(
            this Channel<T> channel,
            Func<ChannelReader<T>, Task> actorLoop)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            var actor = actorLoop(channel.Reader);

            return (channel.Writer, actor);
        }

        public static (ChannelWriter<T> mailbox, Task<TResult> actor) StartOnChannel<T, TResult>(
            this Channel<T> channel,
            Func<ChannelReader<T>, Task<TResult>> actorLoop)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            var actor = actorLoop(channel.Reader);

            return (channel.Writer, actor);
        }

        #endregion

        #region [ StartBounded ]

        public static (ChannelWriter<T> mailbox, Task actor) StartBounded<T>(
            int capacity,
            Func<ChannelReader<T>, Task> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateBounded<T>(capacity)
                .StartOnChannel(actorLoop);
        }

        public static (ChannelWriter<T> mailbox, Task actor) StartBounded<T>(
            BoundedChannelOptions options,
            Func<ChannelReader<T>, Task> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateBounded<T>(options)
                .StartOnChannel(actorLoop);
        }

        public static (ChannelWriter<T> mailbox, Task<TResult> actor) StartBounded<T, TResult>(
            int capacity,
            Func<ChannelReader<T>, Task<TResult>> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateBounded<T>(capacity)
                .StartOnChannel(actorLoop);
        }

        public static (ChannelWriter<T> mailbox, Task<TResult> actor) StartBounded<T, TResult>(
            BoundedChannelOptions options,
            Func<ChannelReader<T>, Task<TResult>> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateBounded<T>(options)
                .StartOnChannel(actorLoop);
        }

        #endregion

        #region [ StartUnbounded ]

        public static (ChannelWriter<T> mailbox, Task actor) StartUnbounded<T>(
            Func<ChannelReader<T>, Task> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateUnbounded<T>()
                .StartOnChannel(actorLoop);
        }

        public static (ChannelWriter<T> mailbox, Task actor) StartUnbounded<T>(
            UnboundedChannelOptions options,
            Func<ChannelReader<T>, Task> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateUnbounded<T>(options)
                .StartOnChannel(actorLoop);
        }

        public static (ChannelWriter<T> mailbox, Task<TResult> actor) StartUnbounded<T, TResult>(
            Func<ChannelReader<T>, Task<TResult>> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateUnbounded<T>()
                .StartOnChannel(actorLoop);
        }

        public static (ChannelWriter<T> mailbox, Task<TResult> actor) StartUnbounded<T, TResult>(
            UnboundedChannelOptions options,
            Func<ChannelReader<T>, Task<TResult>> actorLoop)
        {
            if (actorLoop == null) throw new ArgumentNullException(nameof(actorLoop));

            return Channel
                .CreateUnbounded<T>(options)
                .StartOnChannel(actorLoop);
        }

        #endregion

        #region [ PostAsync | TryPost | PostAndReplyAsync ]

        public static ValueTask PostAsync<T>(
            this ChannelWriter<T> writer,
            T message, 
            CancellationToken cancellationToken = default) =>
            writer.WriteAsync(message, cancellationToken);

        public static bool TryPost<T>(
            this ChannelWriter<T> writer,
            T message) =>
            writer.TryWrite(message);

        public static async Task<TResult> PostAndReplyAsync<T, TResult>(
            this ChannelWriter<T> writer,
            Func<TaskCompletionSource<TResult>, T> getMessage, 
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<TResult>();

            var message = getMessage(tcs);

            await writer.WriteAsync(message, cancellationToken);

            return await tcs.Task;
        }

        public static async Task PostAndReplyAsync<T>(
            this ChannelWriter<T> writer,
            Func<TaskCompletionSource, T> getMessage, 
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource();

            var message = getMessage(tcs);

            await writer.WriteAsync(message, cancellationToken);

            await tcs.Task;
        }

        #endregion
    }
}

using TodoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TodoApi.Services;

public class TodoService
{
    private readonly IMongoCollection<TodoItem> _todoCollection;
    public TodoService(IOptions<todoStoreDatabaseSettings> todoStoreDBSettings)
    {
        var mongoClient = new MongoClient(todoStoreDBSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDBSettings.Value.DatabaseName);
        _todoCollection = mongoDatabase.GetCollection<TodoItem>(todoStoreDBSettings.Value.TodoCollectionName);
    }
    public async Task<IEnumerable<TodoItem>> GetAsync() => await _todoCollection.Find(_ => true).ToListAsync();
    public async Task<TodoItem?> GetAsync(string id) => await _todoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task CreateAsync(TodoItem todoItem) => await _todoCollection.InsertOneAsync(todoItem);
    public async Task UpdateAsync(string id, TodoItem todoItem) => await _todoCollection.ReplaceOneAsync(x => x.Id == id, todoItem);
    public async Task RemoveAsync(string id) => await _todoCollection.DeleteOneAsync(x => x.Id == id);
}
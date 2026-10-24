using SQLite;
using System.IO;
using praktika_6.Models;

namespace praktika_6.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    private async Task Init()
    {
        if (_database != null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "nutrition.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<MealEntry>();
    }

    public async Task<List<MealEntry>> GetMealsAsync()
    {
        await Init();
        return await _database.Table<MealEntry>().ToListAsync();
    }

    public async Task<int> SaveMealAsync(MealEntry meal)
    {
        await Init();
        return await _database.InsertAsync(meal);
    }

    public async Task<int> DeleteMealAsync(MealEntry meal)
    {
        await Init();
        return await _database.DeleteAsync(meal);
    }
}
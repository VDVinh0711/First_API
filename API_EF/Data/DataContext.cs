using API_EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_EF.Data
{
    public class DataContext:IdentityDbContext
    {

        //ConectData
        public DataContext(DbContextOptions<DataContext> option) : base(option)
        {
            
        }
        public DbSet<GameEnties> GameStoreEntites { get; set; }
    }
}

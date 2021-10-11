using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace cd_c_bankAccounts.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Transaction> Transactions {get;set;}
    }
}
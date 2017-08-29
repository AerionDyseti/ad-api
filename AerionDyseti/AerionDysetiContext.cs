using AerionDyseti.Auth.Models;
using AerionDyseti.GroceryList;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDyseti
{
    public class AerionDysetiContext : IdentityDbContext<AerionDysetiUser>
    {

        public AerionDysetiContext(DbContextOptions<AerionDysetiContext> options) : base(options)
        {

        }


        public DbSet<GroceryItem> GroceryItems { get; set; }

    }
}

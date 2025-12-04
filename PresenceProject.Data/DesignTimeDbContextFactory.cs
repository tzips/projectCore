using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Configuration.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // ודאי שאת מריצה את הפקודה מתוך פרויקט ה-API או שיש בו appsettings.json
                .AddJsonFile("appsettings.json")
                .Build();

            return new DataContext(configuration);
        }
    }
}

using CafeApp.Data;
using Microsoft.EntityFrameworkCore;
using CafeApp.Models;

namespace CafeApp.Data
{
    public class DatabaseInitializer 
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext(serviceProvider.GetRequiredService
            <DbContextOptions<DataContext>>()))
            {
                if (context.Baristas.Any())
                {
                    return; // BD a fost creata anterior
                }

                context.Baristas.Add(new Barista
                {
                    Email = "admin@gmail.com",
                    Password = "admin"
                });


                context.SaveChanges();
            }
        }

       
    }
}

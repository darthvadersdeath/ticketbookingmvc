using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketMasterMVC.Models;

namespace TicketMasterMVC.Data
{
    public class TicketContext : IdentityDbContext<User>
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) { }
        public DbSet<Ticket> Tickets { get; set; }
    }

}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Repository
{
    public class ApplicationDbContext : IdentityDbContext<MoviesUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketInUserTickets> TicketsInUserTickets { get; set; }
        public virtual DbSet<UserTickets> UserTickets { get; set; }
        public virtual DbSet<MoviesUser> MoviesUsers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketInOrder> TicketsInOrders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TicketInUserTickets>()
                .HasOne<Ticket>(tiut => tiut.Ticket)
                .WithMany(t => t.TicketsInUserTickets)
                .HasForeignKey(tiut => tiut.TicketId);


            builder.Entity<TicketInUserTickets>()
                .HasOne<UserTickets>(tiut => tiut.UserTickets)
                .WithMany(ut => ut.TicketsInUserTickets)
                .HasForeignKey(tiut => tiut.UserTicketsId);

            builder.Entity<TicketInOrder>()
               .HasOne<Ticket>(tio => tio.Ticket)
               .WithMany(t => t.TicketsInOrders)
               .HasForeignKey(tio => tio.TicketId);


            builder.Entity<TicketInOrder>()
                .HasOne<Order>(tio => tio.Order)
                .WithMany(o => o.TicketsInOrders)
                .HasForeignKey(tio => tio.OrderId);
        }


    }
}

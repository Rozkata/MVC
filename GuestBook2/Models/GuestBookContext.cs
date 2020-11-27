using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GuestBook2.Models
{
    public class GuestbookContext
        : DbContext
    { 
        public GuestbookContext(): base("Guestbook")
        {

        }
        public DbSet<GuestBookEntry> Entries { get; set; }
    }
}
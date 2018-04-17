using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECore.Domain.Entities
{
    /// <summary>
    /// Application user
    /// </summary>
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.CardsCollections = new HashSet<CardsCollection>();
        }

        public virtual ICollection<CardsCollection> CardsCollections { get; set; }
    }
}

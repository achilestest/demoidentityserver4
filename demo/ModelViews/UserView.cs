using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace demo.ModelViews
{
    public class UserView:ModelView
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Test { get; set; }

       
    }
}

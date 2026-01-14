using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectADONetMVC.Models
{
    public class Empleado
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("Nombres")]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }
    }
}
namespace Person.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Studentid { get; set; }

        [StringLength(100)]
        public string FullNAME { get; set; }
        [Range(1, 100)]
        public int? Age { get; set; }

        [StringLength(50)]
        public string Major { get; set; }
    }
}

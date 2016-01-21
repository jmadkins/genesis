using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eSIS.Database.Entities
{
    /// <summary>
    /// A type of school. Ex: Elementary, Middle School, High School
    /// </summary>
    [Table("SchoolType", Schema = "sis")]
    public class SchoolType : BaseEntity
    {
        [MaxLength(45)]
        public string Name { get; set; }

        public bool AllowCustomGradingScales { get; set; }
    }
}
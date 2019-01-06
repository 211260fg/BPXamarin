using SQLite;

namespace App.Droid
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }


        [MaxLength(50)]
        public string Email { get; set; }
    }
}
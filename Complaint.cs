namespace WebApplication1.Models
//"Make a table in the database for this Complaint class, with columns for Id, Description, and DateSubmitted."
{
    public class Complaint // This defines a class named Complaint
    {
        public int Id { get; set; } // Represents the unique identifier for the complaint
        public string? Description { get; set; } // Represents the complaint description
        public DateTime DateSubmitted { get; set; } // Represents when the complaint was submitted
    }
}

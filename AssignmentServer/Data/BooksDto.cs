namespace AssignmentServer.Data
{
    public class BooksDto
    {      
        public string title { get; set; } = null!;    
        public string author { get; set; } = null!;  
        public string? pages { get; set; }

        public string? rating { get; set; }
        public string? description { get; set; }
        public string publisher { get; set; } = null!;
    }

}

namespace AxiApi.DTOs
{
    public class UserFavouritesDTO
    {
        public Guid FavouritesId { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string CommandText { get; set; } = string.Empty;
        public string OriginalCommandText { get; set; } = string.Empty; 
        public int? FavOrder { get; set; }
        public string TargetURL { get; set; } = string.Empty;
        //public bool HasAccess { get; set; } = true; 
        public DateTime? CreatedOn { get; set; }

        
    }
}

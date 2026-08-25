namespace AxiApi.DTOs
{
    public class UserFavouritesRequestDTO
    {
       
        public string Username { get; set; } = string.Empty;
        public string CommandText { get; set; } = string.Empty;
        public string OriginalCommandText { get; set; } = string.Empty; 
        public int? FavOrder { get; set; }
        public string Action { get; set; } = string.Empty;
        public string TargetURL { get; set; } = string.Empty;
    }
}

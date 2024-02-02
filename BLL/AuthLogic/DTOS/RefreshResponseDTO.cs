using System.ComponentModel.DataAnnotations;

namespace BLL.AuthLogic.DTOS
{
    public class RefreshResponseDTO
    {
        [Required]
        public required string AccessToken { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.DTOs.Games
{
    public class LibraryDto : BaseDto
    {
        public DateTime PurchasedAt { get; set; }
        public GameDto Game { get; set; } = default!;
    }
}

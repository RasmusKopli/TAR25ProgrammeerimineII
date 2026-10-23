using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    public class Loan
    {
        public Guid Id { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateOnly LoanDate { get; set; }
        public DateOnly ReturnDate { get; set; }
    }
}

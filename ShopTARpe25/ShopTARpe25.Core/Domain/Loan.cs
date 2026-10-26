using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    public class Loan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid ReaderId { get; set; }
        public DateOnly LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cd_c_bankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get;set;}

        [Required(ErrorMessage = "You must enter an amount.")]
        [Display(Name = "Amount: ")]
        public decimal Amount {get;set;}

        public int UserId {get;set;}
        public User Creator {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}
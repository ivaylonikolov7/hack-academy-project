﻿namespace HackChain.Core.Model
{
    public class Account
    {
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public long Nonce { get; set; }
    }
}
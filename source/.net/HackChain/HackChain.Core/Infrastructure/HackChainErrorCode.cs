namespace HackChain.Core.Infrastructure
{
    public enum HackChainErrorCode
    {
        GenericError = 100,


        Transaction_Invalid_Sender = 1000,
        Transaction_Invalid_Recipient,
        Transaction_Invalid_Value,
        Transaction_Invalid_Fee,
        Transaction_Invalid_Hash,
        Transaction_Invalid_Signature,
        Transaction_Invalid_Nonce,
        Transaction_Insufficient_Balance,
        Transaction_Duplicate,

        Block_Invalid_Operation = 2000,
    }
}

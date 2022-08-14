namespace HackChain.Node.DTO
{
    public enum HackChainErrorCode
    {
        GenericError = 100,


        Invalid_Address = 120,

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
        Block_Invalid_Difficulty,
        Block_Invalid_Hash,
        Block_Invalid_PreviousHash,


        Wallet_PrivateKey_Missing = 3000,
        Wallet_Invalid_Recipient
    }
}

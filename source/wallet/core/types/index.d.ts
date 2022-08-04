export type Transaction = {
    Sender: String,
    Recipient: String,
    Nonce: Number,
    Value?: Number,
    Fee?: Number,
    Hash?: String,
    Signature?: String,
};
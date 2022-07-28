import { Transaction } from "./types";

export default class {
    public build(Sender: String, Recipient: String, Nonce: Number, Value?: Number, Fee?: Number): Transaction {
        const transaction: Transaction = {
            Sender,
            Recipient,
            Nonce,
            Value,
            Fee,
            Hash: '',
            Signature: '',
        };

        return transaction;
    }
};

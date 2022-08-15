import { Transaction } from "./types";
import { sha256 } from 'js-sha256';
import WalletError from "./wallet-error";
import { validPublicKey } from "./utils";

export default class {
    sender: string;
    recipient: string;
    nonce: Number;
    value: Number;
    fee: Number;
    hash: String;
    signature: String;
    rawTx: Transaction;

    constructor(sender: string, recipient: string, nonce = 0, value?: Number, fee?: Number) {
        this.sender = sender;
        this.recipient = recipient;
        this.nonce = nonce;
        this.value = value;
        this.fee  = fee;
    }

    public build(): Transaction {
        this.validate();
        const transaction: Transaction = {
            Sender: this.sender,
            Recipient: this.recipient,
            Nonce: this.nonce,
            Value: this.value,
            Fee: this.fee
        };

        this.hash = sha256(JSON.stringify(transaction));

        // transaction.Hash = this.hash;

        this.rawTx = transaction;

        return transaction;
    }

    public validate() {
        if (!this.sender || (this.sender && !validPublicKey(this.sender))) {
            throw new WalletError({ error: 'Invalid transaction sender', errorCode: 'Transaction_Invalid_Sender' });
        }

        if (!this.recipient || (this.recipient && !validPublicKey(this.recipient))) {
            throw new WalletError({ error: 'Invalid transaction recipient', errorCode: 'Transaction_Invalid_Recipient' });
        }

        if (!this.nonce && this.nonce !== 0) {
            throw new WalletError({ error: 'Invalid transaction nonce', errorCode: 'Transaction_Invalid_Nonce' });
        }

        if (!this.value || !Number(this.value)) {
            throw new WalletError({ error: 'Invalid transaction value', errorCode: 'Transaction_Invalid_Value' });
        }

        if (!this.fee && this.fee !== 0 || (this.fee && !Number(this.fee))) {
            throw new WalletError({ error: 'Invalid transaction fee', errorCode: 'Transaction_Invalid_Fee' });
        }
    }

    public toString() {
        return JSON.stringify(this.rawTx);
    }

    public setSignature(signature: string) {
        this.signature = signature;
        this.rawTx.Signature = signature;
    }

    public setHash() {
        this.rawTx.Hash = this.hash;
    }
};

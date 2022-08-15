import { Transaction } from "./types";
import sha256 from 'js-sha256';

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
        if (!this.sender) {
            throw new Error('Tx: valid sender is required');
        }

        if (!this.recipient) {
            throw new Error('Tx: valid recipient is required');
        }

        if (!this.nonce && this.nonce !== 0) {
            throw new Error('Tx: valid nonce is required');
        }

        if (!this.value) {
            throw new Error('Tx: valid value is required');
        }

        if (!this.fee && this.fee !== 0) {
            throw new Error('Tx: valid fee is required');
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

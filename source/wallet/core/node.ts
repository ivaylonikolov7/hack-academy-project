import WalletError from "./wallet-error";

class Node {
    public id: string;
    public url: string;

    constructor(id: string, url: string) {
        this.id = id;
        this.url = url;
    }

    async broadcastTransaction(tx: any) {
        const response = await fetch(`${this.url}/api/transactions/add`, {
            method: 'POST',
            headers: new Headers({ 'content-type': 'application/json' }),
            body: tx,
        });

        const result = await response.json();

        if (result.ok) {
            return result;
        }


        throw new WalletError(result.errors[0]);
    }

    async getAccountInfo(address: string) {
        const response = await fetch(`${this.url}/api/accounts/${address}`, { method: 'GET' });

        const { data } = await response.json();

        return data;
    }

    async getAccountTxs(address: string) {
        const response = await fetch(`${this.url}/api/accounts/${address}/transactions`, { method: 'GET' });

        const { data } = await response.json();

        return data;
    }
}

export default Node;
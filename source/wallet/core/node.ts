import Transaction from './transaction';

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


        return response.json();
    }

    async getAccountInfo(address: string) {
        const response = await fetch(`${this.url}/api/accounts/${address}`, { method: 'GET' });


        const { data } = await response.json();

        return data;
    }
}

export default Node;
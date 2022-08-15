import axios from "axios";
import WalletError from "./wallet-error";

class Node {
    public id: string;
    public url: string;

    constructor(id: string, url: string) {
        this.id = id;
        this.url = url;
    }

    async broadcastTransaction(tx: any) {
        const response = await axios.post(`${this.url}/api/transactions/add`, tx, { headers: {
            'Content-Type': 'application/json',
        }}).catch(e => e);

        if (response?.status === 200) {
            return response?.data.data;
        }


        throw new WalletError(response?.response.data.errors[0]);
    }

    async getAccountInfo(address: string) {
        const response = await axios.get(`${this.url}/api/accounts/${address}`, { method: 'GET' }).catch(e => e);

        return response?.data.data;
    }

    async getAccountTxs(address: string) {
        const response = await axios.get(`${this.url}/api/accounts/${address}/transactions`, { method: 'GET' }).catch(e => e);

        return response?.data.data;
    }
}

export default Node;
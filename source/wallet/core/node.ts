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

        if (response?.response?.status === 200) {
            return response.response;
        }


        throw new WalletError(response.response.data.errors[0]);
    }

    async getAccountInfo(address: string) {
        const response = await axios.get(`${this.url}/api/accounts/${address}`, { method: 'GET' }).catch(e => e);

        return response?.response?.data;
    }

    async getAccountTxs(address: string) {
        const response = await axios.get(`${this.url}/api/accounts/${address}/transactions`, { method: 'GET' }).catch(e => e);

        return response?.response?.data;
    }
}

export default Node;
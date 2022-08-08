import Account from "./account";
import Node from './node';
import Transaction from './transaction';

class Wallet {
    public accounts: Map<string, Account> = new Map();
    public nodes: Map<string, Node> = new Map();

    public selectedAccount: string;
    public selectedNode: string;

    constructor({ nodes }: { nodes: Array<Node> }) {
        nodes.forEach(node => this.addNode(node))
    }

    addAccount(account: Account, { selected }: { selected?: Boolean }) {
        const address = account.address();

        this.accounts.set(address, account);
    
        if (selected) {
            this.selectedAccount = address;
        }
    }

    selectAccount(account: string) {
        this.selectedAccount = account;
    }

    addNode(node: Node) {
        this.nodes.set(node.id, node);
    }

    selectNode(nodeId: string) {
        this.selectedNode = nodeId;
    }

    async sendTransaction(rawTx: any) {
        const node = this.nodes.get(this.selectedNode);
        const account = this.accounts.get(this.selectedAccount);

        const accountInfo = await node.getAccountInfo(this.selectedAccount);

        if (!accountInfo) {
            throw new Error('Cannot fetch account info');
        }

        if (accountInfo.balance < rawTx.value + rawTx.fee) {
            throw new Error('Insufficient funds');
        }

        const tx = new Transaction(this.selectedAccount,
            rawTx.recipient,
            (accountInfo?.nonce + 1) ?? 1,
            rawTx.value,
            rawTx.fee);

        tx.build();

        const signature = account.sign(tx.toString());

        tx.setSignature(signature.base64);
        tx.setHash();

        return node.broadcastTransaction(tx.toString());
    }

    async getActiveAccountInfo() {
        const node = this.nodes.get(this.selectedNode);
        const accountInfo = await node.getAccountInfo(this.selectedAccount);

        return accountInfo;
    }

    async getAccountTxs() {
        const node = this.nodes.get(this.selectedNode);
        const txs = await node.getAccountTxs(this.selectedAccount);

        return txs;
    }
}

export default Wallet;

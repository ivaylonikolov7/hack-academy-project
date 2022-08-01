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
            console.log('address', address);
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

        console.log(this.accounts);

        // TODO: get account nonce
        // TODO: check if account has enough funds

        console.log(this.selectedAccount);

        const tx = new Transaction(this.selectedAccount,
            rawTx.recipient,
            rawTx.nonce,
            rawTx.value,
            rawTx.fee);

        tx.build();

        const signature = account.sign(tx.toString());

        tx.setSignature(signature);

        return node.broadcastTransaction(tx.toString());
    }
}

export default Wallet;

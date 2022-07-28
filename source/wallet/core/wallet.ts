import Account from "./account";
import Node from './node';

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

        this.accounts[address] = account;
    
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

    buildTx() {

    }

    sendTransaction(tx: string) {
        console.log(this.nodes);
        console.log(this.selectedNode);
        const node = this.nodes.get(this.selectedNode);

        // console.log('send transaction', node);
    }
}

export default Wallet;

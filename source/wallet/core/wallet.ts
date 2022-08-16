import Account from "./account";
import Node from './node';
import Transaction from './transaction';
import WalletError from './wallet-error';

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

    async sendTransaction(rawTx: any, mine?: boolean) {
        const node = this.nodes.get(this.selectedNode);
        const account = this.accounts.get(this.selectedAccount);

        if (!node) {
            throw new WalletError({ error: 'Node is not connected', errorCode: 'Node_Not_Connected' });
        }

        if (!account) {
            throw new WalletError({ error: 'No account selected', errorCode: 'No_Active_Account' });
        }

        const accountInfo = await node.getAccountInfo(this.selectedAccount);

        if (!accountInfo) {
            throw new WalletError({ error: 'Cannot fetch account info', errorCode: 'Missing_Account_Info' });
        }

        if (accountInfo.balance < rawTx.value + rawTx.fee) {
            throw new WalletError({ error: 'This account has insufficient funds', errorCode: 'Insufficient funds' });
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

        return node.broadcastTransaction(tx.toString(), mine);
    }

    async getActiveAccountInfo() {
        const node = this.nodes.get(this.selectedNode);

        if (node) {
            const accountInfo = await node.getAccountInfo(this.selectedAccount);

            return accountInfo;
        }
        
        return {};
    }

    async getAccountTxs() {
        const node = this.nodes.get(this.selectedNode);

        if (node) {
            const txs = await node.getAccountTxs(this.selectedAccount);

            return txs;
        }
        
        return [];
    }
}

export default Wallet;

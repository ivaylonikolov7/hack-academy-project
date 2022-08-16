import Wallet from 'hackchain-wallet-core/wallet';
import Account from 'hackchain-wallet-core/account';
import Node from "hackchain-wallet-core/node";
import open from "open";
import Cache from "./cache";

const restoreAccounts = async (): Promise<any[]> => {
    const accounts = await Cache.getKey("accounts");

    if (accounts) {
        return JSON.parse(accounts);
    }

    return [];
};

const createAccount = async (alias: string) => {
    const accounts = await restoreAccounts();
    const account = new Account();

    await Cache.setKey("accounts", [...accounts,  { alias, publicKey: account.publicKey, privateKey: account.privateKey } ], true);

    console.log(`Account created succesfully! Public Key: ${account.publicKey} | Private Key: ${account.privateKey}`);
}

const importAccount = async (alias: string, privateKey: string) => {
    const accounts = await restoreAccounts();
    if (!accounts.find(a => a.privateKey === privateKey)) {
        const account = new Account(privateKey);

        await Cache.setKey("accounts", [...accounts,  { alias, publicKey: account.publicKey, privateKey: account.privateKey } ], true);
    }

    console.log(`Account ${alias} succesfully imported!`);
};

const createTransaction = async (alias: string, amount: Number, recipient: string, broadcast?: boolean) => {
    const accounts = await restoreAccounts();

    const account = accounts.find(a => a.alias === alias);
    if (!account) {
        console.error("Account not found!");
    }
    const wallet = new Wallet({ nodes: [new Node("mainnet", "http://hackchain.pirin.pro")] });
    wallet.selectNode("mainnet");
    wallet.addAccount(new Account(account.privateKey), { selected: true });
    const tx = await wallet.sendTransaction({
        recipient: recipient,
        value: Number(amount),
        fee: 0,
    });

    if (tx?.hash) {
        await open(`http://hackchain.pirin.pro/api/transactions/${tx.hash}`);
    }

    console.log("Tx", tx);
};

export default {
    importAccount,
    createAccount,
    createTransaction,
};
<template>
  <div>
    <input v-model="privateKey" />
    <button @click="submit">Submit</button>
    <button @click="generateWallet">Generate Wallet</button>
  </div>
</template>

<script>
import Node from '../../../core/node';
import Account from '../../../core/account';
import Wallet from "../../../core/wallet";

export default {
  data: () => ({ privateKey: null }),
  created() {
    console.log(Wallet);

    const node = new Node("testnet", "https://testnet.hackchaing.com");

    console.log(node);

    const account = new Account('83f919649688da47e81ea3802d49b902d0367b027ca708dbf7a078b844f196b5');


    console.log('address', account.address());

    console.log('sign', account.sign('test'));

    console.log(account);

    const wallet = new Wallet({
      nodes: [node],
    });


    wallet.selectNode("testnet");

    wallet.addAccount(account, { selected: true });

    // console.log(wallet.nodes);


    wallet.sendTransaction();
  },
  methods: {
    submit() {
      // const wallet = new Wallet({ alias: "test", privateKey: this.privateKey });
      // console.log(wallet);
    },
  },
};
</script>

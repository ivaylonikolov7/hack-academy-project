<template>
  <div>
    {{ $store.getters['accounts/active'].publicKey }}
    <Send @send="sendTx" />
  </div>
</template>

<script>
import Send from '../components/Send.vue';

function hexToBase64(hexStr) {
  return btoa([...hexStr].reduce((acc, _, i) =>
    acc += !(i - 1 & 1) ? String.fromCharCode(parseInt(hexStr.substring(i - 1, i + 1), 16)) : "" 
  ,""));
}

export default {
  components: { Send },
  data: () => ({ privateKey: null }),
  methods: {
    sendTx(tx) {
      this.$store.state.wallet.instance.sendTransaction(tx);
    },
  },
};
</script>

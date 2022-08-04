<template>
  <div>
    Your public key is: {{ $store.getters["accounts/active"].publicKey }}
    <Send @send="sendTx" />
  </div>
</template>

<script>
import Send from "../components/Send.vue";

export default {
  components: { Send },
  methods: {
    sendTx(tx) {
      this.$store.state.wallet.instance.sendTransaction(tx);
    },
  },
};
</script>

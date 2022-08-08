<template>
  <div class="transactions-container">
    <div v-for="(tx, k) in transactions" :key="k" class="transaction">
      <div class="recipient">{{ tx.recipient }}</div>
      <div class="type">{{ tx.type }}</div>
      <div class="amount">{{ tx.value }} HCK</div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    transactions() {
      return this.$store.state.accounts.transactions
        .map((tx) => ({
          ...tx,
          type:
            tx.sender === this.$store.getters["accounts/active"].publicKey
              ? "sent"
              : "received",
        }))
        .sort((a, b) => b.blockIndex - a.blockIndex);
    },
  },
};
</script>

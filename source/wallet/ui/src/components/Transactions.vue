<template>
  <div class="transactions-container">
    <div
      v-for="(tx, k) in transactions"
      :key="k"
      class="transaction"
      :class="tx.type"
    >
      <div class="sender"><label>From:</label> <Address :address="tx.sender" /></div>
      <div class="recipient"><label>To:</label> <Address :address="tx.recipient" /></div>
      <div class="amount">
        <span class="sign">{{ tx.type === "sent" ? "-" : "+" }}</span> {{ tx.value }} HCT
      </div>
    </div>
  </div>
</template>

<script>
import Address from "./Address.vue";

export default {
  components: { Address },
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

<style lang="scss">
.transactions-container {
  .transaction {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    padding: 8px 0;

    &:not(:last-child) {
      border-bottom: 1px solid #000;
    }

    .amount {
      flex: 0 0 auto;
      margin-left: auto;
      font-weight: 600;
      font-size: 14px;
    }

    label {
      font-size: 12px;
      font-weight: 600;
    }

    .address {
      font-size: 14px;
    }

    &.sent {
      .amount {
        color: red;
      }
    }

    &.received {
      .amount {
        color: green;
      }
    }

    .sender,
    .recipient {
      max-width: 100px;
      flex: 0 0 auto;
    }
  }
}
</style>

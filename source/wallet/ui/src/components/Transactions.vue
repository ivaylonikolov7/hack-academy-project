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
        <span class="sign">{{ tx.type === "sent" ? "-" : "+" }}</span>{{ tx.value }} HCT
      </div>
    </div>
    <div v-if="!transactions.length" class="no-transactions">
      No transactions for this account!
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
      border-bottom: 1px solid #efefef;
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
      background-color: var(--primary-light);
    }

    &.sent {
      .amount {
        color: var(--primary-red);
      }
    }

    &.received {
      .amount {
        color: var(--primary-green);
      }
    }

    .sender,
    .recipient {
      max-width: 90px;
      flex: 0 0 auto;
    }
  }

  .no-transactions {
    font-size: 14px;
    color: #8f8f8f;
  }
}
</style>

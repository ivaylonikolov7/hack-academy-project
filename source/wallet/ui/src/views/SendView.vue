<template>
  <div class="send">
    <div v-if="error" class="errors">
      {{ error }}
    </div>
    <div v-else-if="success" class="success">
      Your transaction is succesfully send. You can explore it
      <a
        target="_blank"
        :href="`http://hackchain.pirin.pro/api/transactions/${tx.hash}`"
      >
        here
      </a>
    </div>
    <label>
      Recipient
      <input v-model="recipient" placeholder="Enter recipient address" />
    </label>
    <label>
      Amount
      <input
        v-model="amount"
        type="number"
        placeholder="Enter amount to send"
      />
    </label>
    <label>
      Fee
      <input v-model="fee" type="number" placeholder="Enter fee amount" />
    </label>
    <div class="btn-container">
      <router-link to="/" class="btn btn-secondary">Back</router-link>
      <button
        @click="send"
        class="btn btn-primary"
        :disabled="!recipient || !amount"
      >
        Send
      </button>
    </div>
  </div>
</template>

<script>
import WalletError from "hackchain-wallet-core/wallet-error";

export default {
  data() {
    return {
      recipient: null,
      amount: null,
      fee: null,
      error: false,
      success: false,
      tx: null,
    };
  },
  methods: {
    async send() {
      try {
        this.error = false;
        const tx = await this.$store.state.wallet.instance.sendTransaction(
          {
            recipient: this.recipient,
            value: Number(this.amount),
            fee: Number(this.fee) || 0,
          },
          true
        );
        this.success = true;
        this.recipient = null;
        this.amount = null;
        this.fee = null;
        this.tx = tx;
      } catch (e) {
        if (e instanceof WalletError) {
          this.error = e.getErrorMessage();
        }
      }
    },
  },
};
</script>

<style lang="scss">
.send {
  label {
    margin-bottom: 12px;
    display: block;
    font-size: 14px;
  }

  .btn-container {
    display: flex;
    justify-content: space-between;

    .btn {
      width: 50%;

      &:not(:last-child) {
        margin-right: 8px;
      }
    }
  }

  .errors,
  .success {
    position: fixed;
    top: 20px;
    left: 50%;
    transform: translateX(-50%);
    padding: 8px 24px;
    color: var(--primary-light);
    border-radius: 4px;
  }

  .errors {
    background-color: var(--primary-red);
  }

  .success {
    background-color: var(--primary-green);

    a {
      color: var(--primary-color);
    }
  }
}
</style>

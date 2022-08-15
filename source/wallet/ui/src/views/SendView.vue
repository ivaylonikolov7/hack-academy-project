<template>
  <div class="send">
    <div v-if="error" class="errors">
      {{ error }}
    </div>
    <div v-if="success" class="success">
      Your transaction is succesfully send. You can explore your transaction at <a href="">explorer url</a>
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
import WalletError from "../../../core/wallet-error";

export default {
  data() {
    return {
      recipient: null,
      amount: null,
      fee: null,
      error: false,
      success: false,
    };
  },
  methods: {
    async send() {
      try {
        this.error = false;
        await this.$store.state.wallet.instance.sendTransaction({
          recipient: this.recipient,
          value: Number(this.amount),
          fee: Number(this.fee),
        });
        this.success = true;
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

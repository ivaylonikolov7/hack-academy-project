<template>
  <div class="send">
    <label>
      Recipient
      <input v-model="recipient" placeholder="Enter recipient address" />
    </label>
    <label>
      Amount
      <input v-model="amount" type="number" placeholder="Enter amount to send" />
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
export default {
  data() {
    return {
      recipient: null,
      amount: null,
      fee: null,
    };
  },
  methods: {
    send() {
      this.$store.state.wallet.instance.sendTransaction({
        recipient: this.recipient,
        value: Number(this.amount),
        fee: Number(this.fee),
      });
    },
  },
};
</script>

<style lang="scss">
.send {
  label {
    margin-bottom: 8px;
    display: block;
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
}
</style>

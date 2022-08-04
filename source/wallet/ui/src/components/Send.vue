<template>
  <div>
    <h5>Send</h5>
    <label for="">
      Recipient
      <input v-model="recipient" placeholder="Enter recipient address" />
    </label>
    <br />
    <label for="">
      Amount
      <input v-model="amount" placeholder="Enter amount to send" />
    </label>
    <br />
    <label for="">
      Fee
      <input v-model="fee" placeholder="Enter fee amount" />
    </label>
    <br />

    <button @click="send" class="btn btn-primary">Send</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      recipient: null,
      amount: 0,
      fee: 0,
    };
  },
  methods: {
    send() {
      this.$emit("send", {
        recipient: this.recipient,
        value: Number(this.amount),
        fee: Number(this.fee),
      });
    },
  },
};
</script>

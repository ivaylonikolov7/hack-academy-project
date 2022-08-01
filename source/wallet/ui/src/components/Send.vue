<template>
  <div>
    Send
    <input v-model="recipient" placeholder="Enter recipient address"/>
    <input v-model="amount" placeholder="Enter amount to send" />
    <input v-model="fee" placeholder="Enter fee amount" />

    <button @click="send">Send</button>
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
      this.$emit('send', {
        recipient: this.recipient,
        value: this.amount,
        fee: this.fee,
      });
    },
  },
};
</script>

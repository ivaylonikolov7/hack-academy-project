<template>
  <div class="login-container">
    <img src="../assets/images/logo.svg" alt="Logo" />
    <h2>HackChain Wallet</h2>
    <p>Welcome to HackChain Wallet, to start interacting with HackChain blockchain</p>
    <div class="btn-actions">
      <button class="btn btn-primary generate" @click="generate">Generate Wallet</button>
      <button class="btn btn-secondary">Import Private Key</button>
    </div>
  </div>
</template>

<script>

export default {
  methods: {
    generate() {
      this.$store.dispatch('wallet/create');
    }
  }
};
</script>

<style lang="scss">
.login-container {
  text-align: center;

  img {
    max-width: 300px;
  }

  .generate {
    margin-bottom: 8px;
  }

  .btn-actions {
    margin-top: 24px;
    display: flex;
    flex-wrap: wrap;

    button {
      width: 100%;
    }
  }
}
</style>

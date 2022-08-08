<template>
  <div class="login-container">
    <h2>HackChain Wallet</h2>
    <p>
      Welcome to HackChain Wallet, to start interacting with HackChain
      blockchain
    </p>
    <div class="btn-actions">
      <button
        v-if="!importPrivateKey"
        class="btn btn-primary generate"
        @click="generate"
      >
        Generate Wallet
      </button>
      <input v-else placeholder="Enter private key" v-model="privateKey" />
      <button class="btn btn-secondary" @click="importPrivKey">
        Import Private Key
      </button>
      <button
        v-if="importPrivateKey"
        class="btn btn-primary"
        @click="importPrivateKey = false"
      >
        Back
      </button>
    </div>
  </div>
</template>

<script>
export default {
  data: () => ({ importPrivateKey: false, privateKey: "" }),
  methods: {
    generate() {
      this.$store.dispatch("wallet/create");

      this.$router.push({ name: "home" });
    },
    importPrivKey() {
      if (!this.importPrivateKey) {
        this.importPrivateKey = true;
        return;
      }

      this.$store.dispatch("wallet/import", this.privateKey);
      this.$router.push({ name: "home" });
    },
  },
};
</script>

<style lang="scss">
.login-container {
  text-align: center;

  img {
    max-width: 300px;
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

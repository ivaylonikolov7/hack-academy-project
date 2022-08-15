<template>
  <div>
    <div class="account">
      <div class="address">
        <Address :address="address" />
      </div>
      <div class="balance">{{ $store.state.accounts.balance }} HCT</div>
    </div>
    <router-link to="send" class="send btn btn-primary">Send</router-link>
    <h3>Transactions History</h3>
    <Transactions />
  </div>
</template>

<script>
import Address from "../components/Address.vue";
import Transactions from "../components/Transactions.vue";

export default {
  components: { Address, Transactions },
  computed: {
    address() {
      return this.$store.getters["accounts/active"].publicKey;
    },
  },
};
</script>

<style lang="scss">
.account {
  background-color: var(--secondary-color);
  color: var(--primary-light);
  padding: 12px 8px 8px;
  text-align: center;
  border-radius: 4px;

  .balance {
    font-size: 36px;
    margin-top: 12px;
  }
}

a.send {
  display: inline-block;
  width: 100%;
  margin-top: 16px;
  text-decoration: none;
}
</style>

<template>
  <div>
    <div class="account">
      <div class="address">
        {{ $store.getters["accounts/active"].publicKey }}
      </div>
      <div class="balance">{{ $store.state.accounts.balance }} HCK</div>
    </div>
    <Transactions />
    <Send @send="sendTx" />
  </div>
</template>

<script>
import Send from "../components/Send.vue";
import Transactions from "../components/Transactions.vue";

export default {
  components: { Send, Transactions },
};
</script>

<style lang="scss">
.account {
  background-color: var(--secondary-color);
  color: var(--primary-light);
  padding: 8px;
  text-align: center;
  border-radius: 4px;

  .address {
    
  }

  .balance {
    font-size: 36px;
  }
}
</style>

<template>
  <header>
    <template v-if="$route.name === 'login'">
      <img src="../assets/images/logo.svg" class="logo-lg" />
    </template>
    <template v-else>
      <img src="../assets/images/favicon.ico" class="logo-sm" />
      <div class="node">
        {{ $store.getters["nodes/active"].name }}
      </div>
      <div v-if="$store.getters['accounts/active']" class="dropdown">
        <Avatar
          :key="$store.getters['accounts/active'].publicKey"
          :address="$store.getters['accounts/active'].publicKey"
          @click="dropdownOpened = !dropdownOpened"
        />
        <ul
          class="list"
          :class="{ show: dropdownOpened }"
          @click="dropdownOpened = !dropdownOpened"
        >
          <li>
            <small>Accounts</small>
            <ul>
              <li
                v-for="(account, k) in $store.getters['accounts/list']"
                :key="k"
                :class="{
                  selected: account === $store.getters['accounts/active'].publicKey,
                }"
                @click="
                  $store.dispatch('wallet/selectAccount', {
                    address: account,
                    idx: k,
                  })
                "
              >
                <Address :address="account" />
              </li>
            </ul>
          </li>
          <li>
            <small>Other</small>
            <ul>
              <li @click="$router.push({ name: 'login' })">
                Import/Add New Account
              </li>
              <li @click="$store.dispatch('wallet/exportPrivKey')">
                Export Private Key
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </template>
  </header>
</template>
<script>
import Avatar from "../components/Avatar.vue";
import Address from "../components/Address.vue";

export default {
  components: { Avatar, Address },
  data: () => ({ dropdownOpened: false }),
};
</script>

<style lang="scss">
header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 40px;
  width: 100%;
  background: rgb(229, 229, 229);

  .logo-lg {
    width: 70px;
    margin: 0 auto;
  }

  .logo-sm {
    width: 36px;
  }

  .dropdown {
    position: relative;

    .list {
      position: absolute;
      padding: 0;
      margin: 0;
      list-style: none;
      width: 140px;
      right: 0;
      top: 40px;
      background: var(--primary-light);
      box-shadow: 0 4px 15px rgb(0 0 0 / 15%);
      border-radius: 4px;
      display: none;
      z-index: 1;

      &.show {
        display: block;
      }

      li {
        cursor: pointer;

        small {
          padding-left: 8px;
          margin: 4px 0;
          display: inline-block;
          font-weight: bold;
          font-size: 12px;
        }

        ul {
          padding: 0;
          margin: 0;
          li {
            list-style: none;
            padding: 8px;

            &:hover,
            &.selected {
              background: rgb(229, 229, 229);
            }
          }
        }
      }
    }
  }
}
</style>

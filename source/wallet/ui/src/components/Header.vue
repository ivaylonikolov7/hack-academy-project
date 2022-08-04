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
    </template>
  </header>
</template>

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
}
</style>

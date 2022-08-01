<script>
import { RouterView } from 'vue-router';

export default {
  created() {
    console.log(this.$store.state.accounts);
  },
};
</script>

<template>
  <RouterView />
</template>

<style lang="scss">
.app-container {
  background: #fff;
  padding: 80px 40px;
  border: 1px solid #f0f0f2;
  border-radius: 4px;
  box-shadow: 0 4px 8px rgb(0 0 0 / 10%);
  min-height: 60%;
  max-height: 700px;
  display: flex;
  align-items: center;
  justify-content: center;
  max-width: 360px;
  word-break: break-word;
}
</style>

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

<style scoped>
</style>

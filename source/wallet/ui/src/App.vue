<script>
import { RouterView } from "vue-router";
import Header from "./components/Header.vue";

export default {
  components: { Header },
  created() {
    console.log(this.$store.state.accounts);
  },
};
</script>

<template>
  <div class="app-container">
    <Header />
    <RouterView class="main-content" />
  </div>
</template>

<style lang="scss">
.app-container {
  background: #fff;
  border: 1px solid #f0f0f2;
  border-radius: 4px;
  box-shadow: 0 4px 8px rgb(0 0 0 / 10%);
  min-height: 60%;
  height: 600px;
  display: flex;
  align-items: center;
  flex-direction: column;
  max-width: 360px;
  word-break: break-word;
  overflow: hidden;

  .main-content {
    padding: 80px 40px;
    margin: auto;
    width: 336px;
  }
}
</style>

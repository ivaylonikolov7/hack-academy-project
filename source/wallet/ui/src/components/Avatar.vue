<template>
  <div class="avatar" v-html="avatar"></div>
</template>

<script>
import { createAvatar } from "@dicebear/avatars";
import * as style from "@dicebear/avatars-bottts-sprites";

export default {
  props: {
    address: {
      type: String,
      required: true,
    },
  },
  data: () => ({ avatar: null }),
  created() {
    this.avatar = createAvatar(style, { seed: this.address });
  },
};
</script>
<style lang="scss">
.avatar {
  width: 36px;
  height: 36px;
  border: 1px solid var(--primary-color);
  border-radius: 50%;
  background-color: var(--secondary-color);
  cursor: pointer;
}
</style>

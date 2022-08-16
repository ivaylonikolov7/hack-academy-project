<template>
  <span
    class="address"
    :class="{ copied }"
    @click="copyAddress"
    :title="address"
  >
    {{ formattedAddress }}
    <div v-if="copy" @click.stop="copyAddress" class="copy">
      <i class="fas fa-copy copy"></i>
    </div>
  </span>
</template>

<script>
export default {
  props: {
    address: {
      type: String,
      required: true,
    },
    copy: {
      type: Boolean,
    },
  },
  data: () => ({ copied: false }),
  computed: {
    formattedAddress() {
      const b = this.address.substr(0, 5);
      const e = this.address.substr(
        this.address.length - 4,
        this.address.length
      );
      return `${b}...${e}`;
    },
  },
  methods: {
    copyAddress() {
      const el = document.createElement("textarea");
      el.value = this.address;
      document.body.appendChild(el);
      el.select();
      document.execCommand("copy");
      document.body.removeChild(el);
      if (this.copy) {
        this.copied = true;
        setTimeout(() => {
          this.copied = false;
        }, 1000);
      }
    },
  },
};
</script>

<style lang="scss">
.address {
  position: relative;

  &.copied:after {
    content: "Copied";
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 100%;
    height: 100%;
    background: inherit;
  }

  .copy {
    cursor: pointer;
    display: inline-block;
  }
}
</style>

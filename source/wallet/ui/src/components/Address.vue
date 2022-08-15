<template>
  <span class="address">{{ formattedAddress }}</span>
</template>

<script>
export default {
  props: {
    address: {
      type: String,
      required: true,
    },
  },
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
};
</script>

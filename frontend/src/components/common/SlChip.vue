<script setup>
defineProps({
  styleClass: String,
});
</script>

<template>
  <div
    class="inline-flex justify-center items-center py-2 px-3 rounded-full border border-light-gray-500"
    :class="styleClass"
  >
    <div class="leading-none max-w-full flex-initial"><slot></slot></div>
  </div>
</template>

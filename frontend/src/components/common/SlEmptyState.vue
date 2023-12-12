<script setup>
import SlIconEmptyState from '@/components/common/icons/SlIconEmptyState.vue';

defineProps({
  imageStyleClass: String,
});
</script>

<template>
  <div class="flex flex-col justify-center items-center gap-7 w-full h-full">
    <SlIconEmptyState :class="imageStyleClass" />
    <div class="text-primary-500 text-lg md:text-xl font-semibold">
      <slot>No data</slot>
    </div>
  </div>
</template>

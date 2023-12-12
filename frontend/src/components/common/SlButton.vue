<script setup>
import SlIconLoading from '@/components/common/icons/SlIconLoading.vue';

defineProps({
  label: String,
  type: { type: String, default: 'button' },
  loading: Boolean,
  disabled: Boolean,
  styleClass: String,
});

defineEmits({
  handleClick: null,
});
</script>

<template>
  <button
    :type="type"
    :class="styleClass ?? 'btn-primary'"
    :disabled="disabled || loading"
    class="flex flex-row justify-center items-center gap-2"
    @click="(e) => $emit('handleClick', e)"
  >
    <SlIconLoading class="text-primary-900 h-5 w-5 mr-2" v-if="loading" />
    <slot v-if="!loading" name="icon"></slot>
    {{ label }}
  </button>
</template>

<style scoped>
.btn-primary {
  @apply text-white bg-primary-500 hover:bg-primary-800 focus:bg-primary-800 focus:ring-3 focus:ring-primary-400 rounded-lg disabled:bg-primary-700 disabled:opacity-70;
}

.btn-outlined {
  @apply border border-primary-500 text-primary-500 focus:outline-none focus:ring-3 focus:ring-primary-400 rounded-lg disabled:opacity-70;
}

.btn-link {
  @apply text-primary-500 hover:text-primary-500 focus:text-primary-800 rounded-lg disabled:text-primary-400 disabled:opacity-70;
}
</style>

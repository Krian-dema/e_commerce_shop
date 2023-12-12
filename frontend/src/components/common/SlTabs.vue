<script setup>
const props = defineProps({
  options: {
    type: Array,
    required: true,
  },
  activeTab: Object,
});

const emit = defineEmits({
  selectTab: null,
});

const handleSelectTab = (selectedOption) => {
  if (props.activeTab?.id === selectedOption?.id) return;

  emit('selectTab', selectedOption);
};
</script>

<template>
  <ul class="flex flex-wrap justify-center items-center gap-6 w-full max-w-max">
    <li
      v-for="option in options"
      :key="option.id"
      class="flex-1 font-semibold text-gray-800 w-28 h-full min-w-max hover:cursor-pointer"
      @click="() => handleSelectTab(option)"
    >
      <div class="mx-auto pb-4 max-w-max" :class="{ active: option.id === activeTab?.id }">
        <slot name="item" :item="option"> </slot>
      </div>
    </li>
  </ul>
</template>

<style scoped>
.active {
  @apply border-b-3 border-primary-500;

  padding-bottom: 0.8rem;
}
</style>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  toggleId: {
    type: String,
    required: true,
  },
  value: Boolean,
  error: String,
});

defineEmits({
  changeValue: null,
});

const hasError = computed(() => !!props.error);
</script>

<template>
  <div>
    <label class="relative inline-flex items-center cursor-pointer">
      <input
        :id="toggleId"
        type="checkbox"
        class="hidden peer"
        :checked="value"
        @input="$emit('changeValue', $event.target.checked)"
      />
      <div
        class="flex-shrink-0 bg-gray-200 w-16 h-8 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:mt-1 after:left-[0.25rem] after:bg-white after:border-gray-300 after:border after:rounded-full after:w-7 after:h-6 after:transition-all peer-checked:bg-primary-500"
      ></div>
      <span v-if="$slots.default" class="ml-4"><slot></slot></span>
    </label>
    <div v-if="hasError" class="text-xs text-red-500 mt-1">
      {{ error }}
    </div>
  </div>
</template>
